using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimerBirth : MonoBehaviour
{
    public static TimerBirth instance;
    public TextMeshProUGUI timerText;
    public GameObject titleText;
    public int time = 10;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        timerText.SetText(time.ToString());
        timerText.enabled = false;
        titleText.SetActive(false);
    }

    public void StartTimer(int timer)
    {
        titleText.SetActive(true);
        timerText.enabled = true;
        time = timer;
        timerText.SetText(time.ToString());
        nextTimer();
    }
    private async void nextTimer()
    {
        await Task.Delay(1000);
        time--;
        if (time <= 0) endTimer();
        else
        {
            timerText.SetText(time.ToString());
            nextTimer();
        }
    }

    private void endTimer()
    {
        titleText.SetActive(false);
        timerText.enabled = false;
        BirthLevel.instance.timerEnded();
    }

    
}
