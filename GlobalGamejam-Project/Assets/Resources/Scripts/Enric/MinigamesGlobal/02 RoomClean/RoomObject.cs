using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomObject : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Works");
        ObjectsList.instance.RemoveObject(gameObject);
        Destroy(gameObject);
    }
}
