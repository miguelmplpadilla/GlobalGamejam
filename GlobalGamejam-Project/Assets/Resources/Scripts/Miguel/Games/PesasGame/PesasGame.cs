using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PesasGame : MonoBehaviour
{
    public float maxPesas = 50;
    public float currentPesas = 0;
    
    private Image imagePlayerPesas;
    public Image imageBarraFill;
    
    public Sprite spriteLevantar;
    public Sprite spriteAgachar;

    private bool currentAnimation = true;
    private bool gameEnded = false;

    private void Awake()
    {
        imagePlayerPesas = GetComponent<Image>();
    }

    private void Update()
    {
        imageBarraFill.DOFillAmount(currentPesas / maxPesas, 0.2f);

        if (currentPesas >= maxPesas && !gameEnded)
        {
            StopCoroutine("RestarVariable");
            NewSceneController.instance.EndGame();
            gameEnded = true;
        }

        if (gameEnded) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            imagePlayerPesas.sprite = currentAnimation ? spriteLevantar : spriteAgachar;
            currentAnimation = !currentAnimation;
            currentPesas += currentPesas > maxPesas / 2 ? 1f : 2;
            StopCoroutine("RestarVariable");
            StartCoroutine("RestarVariable");
        }
    }

    private IEnumerator RestarVariable()
    {
        yield return new WaitForSeconds(1);

        while (currentPesas > 0)
        {
            currentPesas--;
            yield return null;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
