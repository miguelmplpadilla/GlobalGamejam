using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyGame : MonoBehaviour
{
    public static StudyGame instance;
    public GameObject pantalla;
    public GameObject barraCarrega;
    public bool isPantallaOn;
    private bool gameEnded;
    private float progress;
    public float velCarrega = 0.05f;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        progress = 0.01f;
        barraCarrega.transform.localScale = new Vector3(progress, 1, 1);
        gameEnded = false;
        isPantallaOn = false;
        pantalla.SetActive(false);
        Door.instance.CloseDoorFirst(6.500f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Click();
        if(!gameEnded && isPantallaOn && Door.instance.isChecking)
        {
            gameEnded = true;
            Debug.Log("Perdido");
            MinigamesHandler.instance.EndMinigame(2);
        }
        if (isPantallaOn)
        {
            progress += velCarrega * Time.deltaTime;
            progress = Mathf.Clamp(progress, 0, 1);
            barraCarrega.transform.localScale = new Vector3(progress, 1, 1);
            if(!gameEnded && progress >= 1f)
            {
                gameEnded = true;
                Debug.Log("Ganado");
                MinigamesHandler.instance.EndMinigame(1);
            }
        }
    }

    private void Click()
    {
        isPantallaOn = !isPantallaOn;
        pantalla.SetActive(isPantallaOn);
    }
}
