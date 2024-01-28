using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExamGame : MonoBehaviour
{
    public static ExamGame instance;
    public float velocity = 3;

    public Image persons;
    public Image marcador;
    public TextMeshProUGUI tutorial;

    public float aciertos;
    public float fallos;
    public bool isGameOn;

    public Image examResult;
    public TextMeshProUGUI examResultNote;

    public void EndGame()
    {
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(1);
        examResult.DOFade(1, 2);
        yield return new WaitForSeconds(3.5f);
        float note = aciertos / (aciertos + fallos);
        note *= 10;
        Debug.Log(note);
        examResultNote.SetText(note.ToString("0.00"));
        examResultNote.DOFade(1, 2);
        yield return new WaitForSeconds(4f);
        MinigamesHandler.instance.EndMinigame( note >= 5 ? 1 : 2 );

    }

    void Start()
    {
        isGameOn = false;
        aciertos = 0;
        fallos = 0;
        Color col = marcador.color;
        col.a = 0;
        marcador.color = col;

        Color colT = tutorial.color;
        colT.a = 0;
        tutorial.color = colT;

        StartCoroutine(Fades());
    }

    // Update is called once per frame
    void Awake()
    {
        instance = this;
    }

    IEnumerator Fades()
    {
        yield return new WaitForSeconds(3);
        persons.DOFade(0.7f, 2f);
        marcador.DOFade(1, 2f);
        tutorial.DOFade(1, 2f);
        yield return new WaitForSeconds(3);
        BallSpawner.instance.StartSpawning();
        isGameOn = true;
    }

    public void Acierto()
    {
        aciertos++;
    }
    public void Fallo()
    {
        fallos++;
    }
}
