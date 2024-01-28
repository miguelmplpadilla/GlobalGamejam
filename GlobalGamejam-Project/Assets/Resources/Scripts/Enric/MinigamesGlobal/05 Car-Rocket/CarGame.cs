using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGame : MonoBehaviour
{
    public static CarGame instance;
    public float carVel = 300f;
    public float roadVel = 250f;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(StartCars());
    }

    IEnumerator StartCars()
    {
        yield return new WaitForSeconds(4);
        TimerCar.instance.StartTimer(40);
        CarSpawner.instance.StartSpawning();
    }

    // Update is called once per frame
    public void EndTimer()
    {
        MinigamesHandler.instance.EndMinigame(1);
    }

    public void Crash()
    {
        TimerCar.instance.timerStoped = true;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);
        MinigamesHandler.instance.EndMinigame(2);
    }
}
