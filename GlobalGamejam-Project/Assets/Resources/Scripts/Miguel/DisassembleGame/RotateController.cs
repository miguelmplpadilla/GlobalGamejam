using UnityEngine;
using UnityEngine.EventSystems;

public class RotateController : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    private Vector3 mPrevPos = Vector3.zero;
    private Vector3 mPosDelta = Vector3.zero;

    public float rotationSpeed = 2;

    public GameObject childRotate;
    public GameObject parentRotate;

    private void Update()
    {
        mPrevPos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        mPosDelta = Input.mousePosition - mPrevPos;
        parentRotate.transform.Rotate(parentRotate.transform.up,
            -Vector3.Dot(mPosDelta, Camera.main.transform.right) * rotationSpeed, Space.World);
        parentRotate.transform.Rotate(Camera.main.transform.right,
            Vector3.Dot(mPosDelta, Camera.main.transform.up) * rotationSpeed, Space.World);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        childRotate.transform.SetParent(null);
        parentRotate.transform.rotation = Quaternion.identity;
        childRotate.transform.SetParent(parentRotate.transform);
    }
}
