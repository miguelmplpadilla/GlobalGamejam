using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateDragController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
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

        foreach (var hit in hitsDrag) Debug.Log(hit.collider.name);

        StartCoroutine("WaitForInteractChild");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (hitsDrag.Count == 0) return;
        
        if (objectToDrag != null)
        {
            objectToDrag.SendMessage("Move");
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
            objectToDrag.SendMessage("EndMove");
            objectToDrag = null;
            return;
        }
        
        childRotate.transform.SetParent(null);
        parentRotate.transform.rotation = Quaternion.identity;
        childRotate.transform.SetParent(parentRotate.transform);
    }

    private IEnumerator WaitForInteractChild()
    {
        yield return new WaitForSeconds(0.5f);

        if (hitsDrag.Count < 2)
        {
            if (hitsDrag.Count == 1)
            {
                if (hitsDrag[0].collider.CompareTag("DisassembleDrag"))
                {
                    objectToDrag = hitsDrag[0].collider.gameObject;
                    objectToDrag.SendMessage("StartMove");
                    
                    Debug.Log(objectToDrag);
                }
            }
            
            yield break;
        }

        objectToDrag = hitsDrag[1].collider.gameObject;
        
        objectToDrag.SendMessage("StartMove");
    }
}
