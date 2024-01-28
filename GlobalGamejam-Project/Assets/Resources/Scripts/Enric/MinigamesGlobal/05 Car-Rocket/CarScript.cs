using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame

    void Update()
    {
        transform.Translate(Vector2.up * CarGame.instance.carVel * Time.deltaTime);
    }
}
