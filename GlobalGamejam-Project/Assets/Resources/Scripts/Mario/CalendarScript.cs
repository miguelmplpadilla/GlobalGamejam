using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarScript : MonoBehaviour
{
    TextMeshProUGUI txtEdadAnterior;
    TextMeshProUGUI txtEdadSiguiente;

    Animator animator;

    public int edadAnterior;
    public int edadSiguiente;


    private void Awake()
    {
        txtEdadAnterior = GameObject.Find("TXT_Calendar_Top").GetComponent<TextMeshProUGUI>();
        txtEdadSiguiente = GameObject.Find("TXT_Calendar_Bot").GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        animator.SetBool("Next", false);
        txtEdadAnterior.text = "" + edadAnterior;
        txtEdadSiguiente.text = "" + edadSiguiente;

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
