using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        CardController.instance.OnPointerDown(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        CardController.instance.OnDrag(eventData);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        CardController.instance.OnPointerUp(eventData);
    }
}
