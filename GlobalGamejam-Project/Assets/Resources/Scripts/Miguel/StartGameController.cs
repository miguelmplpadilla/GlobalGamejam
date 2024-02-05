using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour
{
    public Image foreground;

    private void Start()
    {
        AudioManagerController.instance.PlaySfx("MenuMusica", true);
        FadeOutForeGround();
    }

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

    private async void FadeOutForeGround(Action callback = null)
    {
        foreground.transform.localScale = Vector3.one;
        Color colorForeground = foreground.color;
        colorForeground.a = 1;

        foreground.color = colorForeground;
        
        colorForeground.a = 0;

        await foreground.DOColor(colorForeground, 1).AsyncWaitForCompletion();
        
        callback?.Invoke();
        
        foreground.transform.localScale = Vector3.zero;
    }
}
