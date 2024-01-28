using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.left * SpaceGame.instance.carVel * Time.deltaTime);
    }
}
