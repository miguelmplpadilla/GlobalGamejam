using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rtParent;
    
    public Vector2 initialPosition;
    private Vector2 originalPositionPanel;

    private void Awake()
    {
        rtParent = transform.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        originalPositionPanel = rtParent.anchoredPosition;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.DOScale(0.95f, 0.2f);
        initialPosition = eventData.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        bool leftRight = eventData.position.x - initialPosition.x < 0;
        transform.DORotate(new Vector3(0, 0, leftRight ? 10 : -10), 0.3f);
        rtParent.DOAnchorPos(new Vector2(leftRight ? -100 : 100, originalPositionPanel.y), 0.3f);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.parent.DOScale(1, 0.2f);
        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        rtParent.DOAnchorPos(originalPositionPanel, 0.2f);
    }
}
