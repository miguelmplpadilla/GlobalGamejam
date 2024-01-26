using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public bool isThrown;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.centerOfMass = new Vector2(.5f, 0);
        isThrown = false;
    }

    // Update is called once per frame
    public void ThrowBaby(float totalForce)
    {
        Vector2 direction = new Vector2(1f, 0.25f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction.normalized * totalForce, ForceMode2D.Impulse);
        rb.angularVelocity = totalForce;
        isThrown = true;
    }

    private void Update()
    {
        if(isThrown && rb.velocity.magnitude <= 0.5f)
        {
            Debug.Log("Game Ended");
        }
    }
}
