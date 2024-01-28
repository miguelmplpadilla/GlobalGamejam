using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TutorialRoom : MonoBehaviour
{
    public bool clicked;

    private void Start()
    {
        clicked = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Click();
    }

    private void Click()
    {
        if (clicked) return;
        clicked = true;
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.DOFade(0, 2f);
    }
}
