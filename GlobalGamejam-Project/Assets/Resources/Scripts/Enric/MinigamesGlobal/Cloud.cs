using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.right * SpawnerClouds.instance.velocity * Time.deltaTime);
    }
}
