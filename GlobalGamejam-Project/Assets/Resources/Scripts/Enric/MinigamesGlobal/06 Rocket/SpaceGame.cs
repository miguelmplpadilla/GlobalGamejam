using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGame : MonoBehaviour
{
    public static SpaceGame instance;
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
        TimerSpace.instance.StartTimer(40);
        AsteroidSpawner.instance.StartSpawning();
    }

    // Update is called once per frame
    public void EndTimer()
    {
        MinigamesHandler.instance.EndMinigame();
    }

    public void Crash()
    {
        TimerSpace.instance.timerStoped = true;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        MinigamesHandler.instance.EndMinigame();
    }
}
