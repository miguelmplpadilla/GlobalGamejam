using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PartController : MonoBehaviour
{
    private GameObject parentRotate;
    public GameObject positionEnd;
    
    public float distanceMove = 0.2f;
    public float speedMove = 0.4f;

    private bool canDrag = false;
    private bool canMove = true;

    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private Vector3 originalRotationWorld;

    public Axis axisToSeparate;

    public GameObject parentObjectsToRemove;
    public List<GameObject> objectsToRemove = new List<GameObject>();

    public bool isDisassembled = false;

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

        transform.DORotate(originalRotationWorld, speedMove);
        
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

        transform.DOMove(new Vector3(mousePosition.x, mousePosition.y,
            transform.position.z), speedMove);
    }

    public async void EndMove()
    {
        canMove = false;
        
        float distanceEnd = Vector3.Distance(transform.position, positionEnd.transform.position);
        float distanceStart = Vector3.Distance(transform.position, parentRotate.transform.position);

        GameObject objToMove;

        isDisassembled = !(distanceEnd > distanceStart && !parentRotate.GetComponent<PartController>().isDisassembled);

        if (isDisassembled)
        {
            objToMove = positionEnd;
        }
        else objToMove = parentRotate;
        
        transform.SetParent(objToMove.transform);

        if (distanceEnd > distanceStart) transform.DOLocalRotate(originalRotation, speedMove);
        
        Vector3 posZero = Vector3.zero;
        posZero.z = transform.localPosition.z;
        await transform.DOLocalMove(distanceEnd < distanceStart ? posZero : originalPosition, speedMove)
            .AsyncWaitForCompletion();

        canMove = true;
    }

    private bool CheckCanMove()
    {
        foreach (var obj in objectsToRemove)
        {
            if (obj.transform.parent.gameObject.Equals(parentObjectsToRemove)) return false;
        }

        return true;
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
