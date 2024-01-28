using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame

    void Update()
    {
        transform.Translate(Vector2.left * CarGame.instance.roadVel * Time.deltaTime);
    }
}
