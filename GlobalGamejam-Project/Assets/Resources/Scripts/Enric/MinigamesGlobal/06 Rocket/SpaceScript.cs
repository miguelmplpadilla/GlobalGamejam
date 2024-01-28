using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        transform.Translate(Vector2.left * SpaceGame.instance.roadVel * Time.deltaTime);
    }
}
