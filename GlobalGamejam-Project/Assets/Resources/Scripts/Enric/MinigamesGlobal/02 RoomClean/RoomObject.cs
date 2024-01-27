using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomObject : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        Color imageColor = image.color;
        imageColor.a = 0;
        image.DOColor(imageColor, 1f).OnComplete(() => {
            ObjectsList.instance.RemoveObject(gameObject);
            Destroy(gameObject);
        });
    }
}
