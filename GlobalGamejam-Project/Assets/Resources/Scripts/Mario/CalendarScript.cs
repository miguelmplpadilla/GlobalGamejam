using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarScript : MonoBehaviour
{
    public static CalendarScript instance;
    
    TextMeshProUGUI txtEdadAnterior;
    TextMeshProUGUI txtEdadSiguiente;

    Animator animator;


    private void Awake()
    {
        instance = this;
        txtEdadAnterior = GameObject.Find("TXT_Calendar_Top").GetComponent<TextMeshProUGUI>();
        txtEdadSiguiente = GameObject.Find("TXT_Calendar_Bot").GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    public void SetTextCalendar(string edadAnterior, string edadSiguiente)
    {
        txtEdadAnterior.text = "" + edadAnterior;
        txtEdadSiguiente.text = "" + edadSiguiente;
    }

    public void StartCalendar()
    {
        animator.SetBool("Next", false);
        Invoke(nameof(PasrPagina), 0.2f);
    }

    public void PasrPagina()
    {
        animator.SetBool("Next", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
