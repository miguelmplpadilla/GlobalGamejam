using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarBoController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private bool gameEnded;
    private float clickOffset;
    private float originalPos;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameEnded) return;
        clickOffset = eventData.position.y - transform.position.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameEnded) return;
        float desiredPos = Mathf.Clamp(eventData.position.y - clickOffset, originalPos - 250, originalPos + 250);
        transform.position = new Vector2(transform.position.x, desiredPos);
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position.y;
        gameEnded = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CarGame.instance.carVel = 0f;
        gameEnded = true;
        CarGame.instance.Crash();
    }
}
