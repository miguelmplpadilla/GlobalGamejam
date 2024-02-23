using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class RotateController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Vector3 mPrevPos = Vector3.zero;
    private Vector3 mPosDelta = Vector3.zero;

    public float rotationSpeed = 2;

    public GameObject childRotate;
    public GameObject parentRotate;

    public List<RaycastHit> hitsDrag = new List<RaycastHit>();

    public GameObject objectToDrag;

    private void Update()
    {
        mPrevPos = Input.mousePosition;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        hitsDrag = Physics.RaycastAll(ray).ToList();
        
        hitsDrag.Sort((x, y) => x.distance.CompareTo(y.distance));

        foreach (RaycastHit hit in hitsDrag)
        {
            Debug.Log("Objeto impactado: " + hit.collider.gameObject.name+" Distancia: "+hit.distance);
        }

        StartCoroutine("WaitForInteractChild");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(hitsDrag.Count);
        if (hitsDrag.Count == 0) return;
        
        if (objectToDrag != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
            
            objectToDrag.transform.position = new Vector3(mousePosition.x, mousePosition.y,
                objectToDrag.transform.position.z);
            return;
        }
        
        StopCoroutine("WaitForInteractChild");
        RotateObject();
    }

    private void RotateObject()
    {
        if (!hitsDrag[0].collider.gameObject.Equals(parentRotate)) return;
        
        mPosDelta = Input.mousePosition - mPrevPos;
        parentRotate.transform.Rotate(parentRotate.transform.up,
            -Vector3.Dot(mPosDelta, Camera.main.transform.right) * rotationSpeed, Space.World);
        parentRotate.transform.Rotate(Camera.main.transform.right,
            Vector3.Dot(mPosDelta, Camera.main.transform.up) * rotationSpeed, Space.World);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine("WaitForInteractChild");
        if (objectToDrag != null)
        {
            objectToDrag.transform.SetParent(null);
            objectToDrag = null;
            return;
        }
        
        childRotate.transform.SetParent(null);
        parentRotate.transform.rotation = Quaternion.identity;
        childRotate.transform.SetParent(parentRotate.transform);
    }

    private IEnumerator WaitForInteractChild()
    {
        yield return new WaitForSeconds(1.5f);
        
        if (hitsDrag.Count < 2) yield break;

        objectToDrag = hitsDrag[1].collider.gameObject;
        objectToDrag.transform.SetParent(null);
        
        Debug.Log("Drag object: "+objectToDrag);
    }
}
