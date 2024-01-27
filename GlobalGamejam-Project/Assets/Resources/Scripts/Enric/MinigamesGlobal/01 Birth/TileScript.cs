using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public TextMeshProUGUI number;
    public GameObject doctorPrefab;
    // Start is called before the first frame update
    public void setNumber(int i)
    {
        number.SetText(i.ToString());
    }

    public void SpawnDoctor()
    {
        float xPos = Random.Range(-4.3f, 4.7f);
        GameObject doctor = Instantiate(doctorPrefab, transform.TransformPoint(xPos, -2.21f, 0), Quaternion.identity, transform);
        if (Random.value < 0.5f)
        {
            doctor.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
