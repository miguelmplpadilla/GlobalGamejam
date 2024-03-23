using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PartController : MonoBehaviour
{
    [NonSerialized] public GameObject parentRotate;
    public GameObject positionEnd;
    
    public float distanceMove = 0.2f;
    public float speedMove = 0.4f;

    private bool canDrag = false;
    private bool canMove = true;

    public bool isBroken = false;
    public bool isDisassembled = false;
    public bool isObjRepare = false;

    public GameObject objRepared;

    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private Vector3 originalRotationWorld;

    public Axis axisToSeparate;

    public GameObject parentObjectsToRemove;
    public List<ObjRemove> objectsToRemove = new List<ObjRemove>();

    private DisassembleController _disassembleController;

    [Serializable]
    public class ObjRemove
    {
        public enum ObjType
        {
            NORMAL, SCREW
        }

        public ObjType type;
        public GameObject obj;
    }

    public enum Axis
    {
        X, Y, Z
    }

    private void Awake()
    {
        parentRotate = transform.parent.gameObject;

        originalPosition = transform.localPosition;
        originalRotation = transform.localEulerAngles;
        originalRotationWorld = transform.eulerAngles;
    }

    private void Start()
    {
        _disassembleController = GameObject.Find("DisassembleManager").GetComponent<DisassembleController>();
        if (objRepared != null) objRepared.SendMessage("SetParentObj", parentRotate);
    }

    public async void StartMove()
    {
        if (positionEnd == null) return;
        
        Sequence sequenceInitialMove = DOTween.Sequence();
        sequenceInitialMove.Pause();

        await StartAnimationMove(distanceMove);

        if (!CheckCanMove())
        {
            await StartAnimationMove(-distanceMove);
            return;
        }
        
        transform.SetParent(null);

        canDrag = true;
    }

    public void Move()
    {
        if (!canDrag || !canMove) return;

        if (isBroken)
        {
            objRepared.SendMessage("GoToEnd");
            Destroy(gameObject);
            return;
        }
        
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        mousePosition.z = -1.5f;

        float distanceEnd = Vector3.Distance(mousePosition, positionEnd.transform.position);
        float distanceStart = Vector3.Distance(mousePosition, parentRotate.transform.position);

        if (distanceEnd < 0.4f || distanceStart < 0.4f)
        {
            DoMovePoint(mousePosition);
            return;
        }
        
        DoMoveNormal(mousePosition);
    }

    private void DoMoveNormal(Vector2 mousePosition)
    {
        transform.SetParent(null);
        
        transform.DOKill();
        
        transform.DORotate(originalRotationWorld, speedMove);
        transform.DOMove(new Vector3(mousePosition.x, mousePosition.y,
            -1.5f), speedMove);
    }

    private void DoMovePoint(Vector3 mousePosition)
    {
        float distanceEnd = Vector3.Distance(mousePosition, positionEnd.transform.position);
        float distanceStart = Vector3.Distance(mousePosition, parentRotate.transform.position);

        GameObject objToMove;

        isDisassembled = !(distanceEnd > distanceStart && !parentRotate.GetComponent<PartController>().isDisassembled);

        if (isDisassembled) objToMove = positionEnd;
        else objToMove = parentRotate;
        
        transform.SetParent(objToMove.transform);

        if (distanceEnd > distanceStart) transform.DOLocalRotate(originalRotation, speedMove);
        
        transform.DOLocalMove(distanceEnd < distanceStart ? Vector3.zero : originalPosition, speedMove);
    }

    public async void EndMove()
    {
        canMove = false;

        bool parentDisasembled = parentRotate.GetComponent<PartController>().isDisassembled;

        GameObject objToMove = parentDisasembled ? positionEnd : parentRotate;
        
        if (transform.parent == null)
        {
            isDisassembled = false;
            transform.SetParent(objToMove.transform);
            if (!parentDisasembled) transform.DOLocalRotate(originalRotation, speedMove);
            await transform.DOLocalMove(!parentDisasembled ? originalPosition : Vector3.zero, speedMove)
                .AsyncWaitForCompletion();
        }

        if (transform.parent.gameObject == parentRotate)
        {
            isDisassembled = false;
            await transform.DOScale(1.2f, 0.1f).AsyncWaitForCompletion();
            await transform.DOScale(1, 0.1f).AsyncWaitForCompletion();
        }

        if (_disassembleController.CheckDisassembled()) return;

        canMove = true;
    }

    public async void GoToEnd()
    {
        await transform.DOMove(positionEnd.transform.position, 0.5f).AsyncWaitForCompletion();

        canMove = true;
        canDrag = true;
    }

    private bool CheckCanMove()
    {
        foreach (var objRemove in objectsToRemove)
        {
            if (objRemove.type == ObjRemove.ObjType.NORMAL &&
                objRemove.obj.transform.parent.gameObject.Equals(parentObjectsToRemove)) return false;

            if (objRemove.type == ObjRemove.ObjType.SCREW &&
                objRemove.obj.transform.childCount > 0)
                return false;
        }

        return true;
    }

    public void SetParentObj(GameObject parent)
    {
        parentRotate = parent;
    }

    private async Task StartAnimationMove(float distance)
    {
        GameObject originalParent = transform.parent.gameObject;

        Tween move = null;
        
        if (originalParent.Equals(parentRotate))
            switch (axisToSeparate)
            {
                case Axis.X:
                    move = transform.DOLocalMoveX(transform.localPosition.x + distance, speedMove);
                    break;
                case Axis.Y:
                    move = transform.DOLocalMoveY(transform.localPosition.y + distance, speedMove);
                    break;
                case Axis.Z:
                    move = transform.DOLocalMoveZ(transform.localPosition.z + distance, speedMove);
                    break;
            }
        else
            move = transform.DOLocalMoveX(transform.localPosition.x + -1f, speedMove);

        await move.SetEase(Ease.OutBack).AsyncWaitForCompletion();
    }
}
