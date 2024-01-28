using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour
{
    public Image foreground;

    public void StartGame()
    {
        FadeInForeGround(() =>
        {
            SceneManager.LoadScene("GameScene");
        });
    }

    public void EndGame()
    {
        Application.Quit();
    }

    private async void FadeInForeGround(Action callback = null)
    {
        foreground.transform.localScale = Vector3.one;
        Color colorForeground = foreground.color;
        colorForeground.a = 1;

        await foreground.DOColor(colorForeground, 1).AsyncWaitForCompletion();
        
        callback?.Invoke();
    }
}
