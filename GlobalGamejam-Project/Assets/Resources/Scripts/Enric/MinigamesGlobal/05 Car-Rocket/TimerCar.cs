using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerCar : MonoBehaviour
{
    public static TimerCar instance;
    public TextMeshProUGUI timerText;

    public bool timerStoped;

    public int time = 40;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        timerText.SetText(time.ToString());
    }

    public void StartTimer(int timer)
    {
        timerStoped = false;
        time = timer;
        timerText.SetText(time.ToString());
        StartCoroutine(nextTimer());
    }
    IEnumerator nextTimer()
    {
        yield return new WaitForSeconds(1);
        time--;
        if (time <= 0) endTimer();
        else if(!timerStoped)
        {
            timerText.SetText(time.ToString());
            StartCoroutine(nextTimer());
        }
    }

    private void endTimer()
    {
        CarGame.instance.EndTimer();
    }
}
