using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    public GameObject roadAll;
    public int roadNumber = 500;

    public float offset;

    void Start()
    {
        for (int i = 0; i < roadNumber; i++)
        {
            Instantiate(roadAll, transform.TransformPoint(offset * i, 0, 0), Quaternion.identity, transform);
        }
    }
}
