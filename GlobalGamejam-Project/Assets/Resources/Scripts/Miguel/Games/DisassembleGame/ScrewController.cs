using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ScrewController : MonoBehaviour
{
    private GameObject endPosition;
    
    public float distanceMove = 0.2f;
    public float speedMove = 0.4f;

    private bool canDrag = false;
    private bool canMove = true;

    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private Vector3 originalRotationWorld;

    public Axis axisToSeparate;

    public GameObject[] allScrewPositions;

    public enum Axis
    {
        X, Y, Z
    }

    private void Awake()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localEulerAngles;
        originalRotationWorld = transform.eulerAngles;
    }

    private void Start()
    {
        endPosition = GameObject.Find("PositionEndScrew");
        allScrewPositions = GameObject.FindGameObjectsWithTag("ScrewPosition");
    }

    public async void StartMove()
    {
        Sequence sequenceInitialMove = DOTween.Sequence();
        sequenceInitialMove.Pause();

        await StartAnimationMove(distanceMove);
        
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
        
        GameObject objToMove = GetHoleNearest();
        
        if (objToMove != null)
        {
            transform.SetParent(objToMove.transform);
            
            transform.DOLocalRotate(originalRotation, speedMove);
            await transform.DOLocalMove(Vector3.zero, speedMove)
                .AsyncWaitForCompletion();
            
            canMove = true;

            return;
        }
        
        transform.SetParent(endPosition.transform);
        
        Vector3 posZero = Vector3.zero;
        posZero.z = transform.localPosition.z;
        await transform.DOLocalMove(posZero, speedMove)
            .AsyncWaitForCompletion();

        canMove = true;
    }

    private GameObject GetHoleNearest()
    {
        GameObject objToMove = null;
        float distanceNearest = 10000000;

        foreach (var obj in allScrewPositions)
        {
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            if (distance < distanceNearest && obj.transform.childCount == 0)
            {
                objToMove = obj;
                distanceNearest = distance;
            }
        }

        if (distanceNearest > 1) return null;

        return objToMove;
    }

    private async Task StartAnimationMove(float distance)
    {
        GameObject originalParent = transform.parent.gameObject;

        Tween move = null;
        
        if (!originalParent.Equals(endPosition))
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
