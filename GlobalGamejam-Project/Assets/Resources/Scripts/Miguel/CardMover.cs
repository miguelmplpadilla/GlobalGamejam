using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SceneController.instance.OnPointerDown(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        SceneController.instance.OnDrag(eventData);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        SceneController.instance.OnPointerUp(eventData);
    }
}
