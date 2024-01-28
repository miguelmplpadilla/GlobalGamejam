using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marcador : MonoBehaviour
{
    public List<GameObject> balls;
    private  Sequence seq;
    private Image image;

    public Color aciertoColor;
    public Color falloColor;
    public Color originalColor;
    void Start()
    {
        image = GetComponent<Image>();
        balls = new List<GameObject>();
        seq = DOTween.Sequence();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Click();
    }
    private void Click()
    {
        if (!ExamGame.instance.isGameOn) return;
        if(balls.Count > 0)
        {
            foreach(GameObject ball in balls)
            {
                Acierto();
                ball.GetComponent<BallExam>().Succeed();
            }
            balls.Clear();
        }
        else
        {
            Fallo();
        }
    }

    private void Fallo()
    {
        ExamGame.instance.Fallo();
        seq.Kill();
        seq = DOTween.Sequence();
        image.color = falloColor;
        seq.Append(image.DOColor(originalColor, 0.5f));
        seq.Play();
    }

    private void Acierto()
    {
        seq.Kill();
        seq = DOTween.Sequence();
        image.color = aciertoColor;
        seq.Append(image.DOColor(originalColor, 0.25f));
        seq.Play();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        balls.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (balls.Contains(collision.gameObject))
        {
            balls.Remove(collision.gameObject);
            collision.gameObject.GetComponent<BallExam>().Fallo();
        }
    }


}
