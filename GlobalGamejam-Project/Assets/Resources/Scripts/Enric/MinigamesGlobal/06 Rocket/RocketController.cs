using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RocketController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private bool gameEnded;
    private float clickOffset;
    private float originalPos;

    public GameObject prefabParticle;
    public GameObject explosionPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameEnded) return;
        clickOffset = eventData.position.y - transform.position.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameEnded) return;
        float desiredPos = Mathf.Clamp(eventData.position.y - clickOffset, originalPos - 270, originalPos + 280);
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
        SpaceGame.instance.carVel = 0f;
        SpaceGame.instance.roadVel = 0f;
        gameEnded = true;

        transform.GetChild(0).gameObject.SetActive(false);
        
        GameObject explosionInstantiated = Instantiate(prefabParticle, transform.parent);
        explosionInstantiated.transform.position = explosionPosition.transform.position;
        
        SpaceGame.instance.Crash();
    }
}
