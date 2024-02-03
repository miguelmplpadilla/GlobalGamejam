using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDetector : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public GameObject objectToDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        IBeginDragHandler beginHandler = objectToDrag.GetComponent<IBeginDragHandler>();
        beginHandler.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        IDragHandler dragHandler = objectToDrag.GetComponent<IDragHandler>();
        dragHandler.OnDrag(eventData);
    }
}
