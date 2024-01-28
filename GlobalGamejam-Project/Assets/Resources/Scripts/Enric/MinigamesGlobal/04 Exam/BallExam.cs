using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallExam : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    public Color aciertoColor;
    public Color falloColor;

    void Update()
    {
        transform.Translate(Vector2.right * ExamGame.instance.velocity * Time.deltaTime);
    }

    public void Succeed()
    {
        ExamGame.instance.Acierto();
        GetComponent<Image>().color = aciertoColor;
    }
    public void Fallo()
    {
        ExamGame.instance.Fallo();
        GetComponent<Image>().color = falloColor;
    }
}

