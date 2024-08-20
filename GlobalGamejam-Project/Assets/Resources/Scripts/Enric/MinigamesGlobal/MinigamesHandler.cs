using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamesHandler : MonoBehaviour
{
    public static MinigamesHandler instance;

    public GameObject defaultGame;
    private GameObject currentGame;

    public Transform minigameParent;
    private void Awake()
    {
        instance = this;
    }

    public void StartMinigame(GameObject gameToSpawn)
    {
        if (gameToSpawn == null) gameToSpawn = defaultGame;
        
        currentGame = Instantiate(gameToSpawn, minigameParent);
    }

    public void EndMinigame()
    {
        NewSceneController.instance.EndGame();
    }

    public void EndMinigame(int final)
    {
        NewSceneController.instance.EndGame(final);
    }
    public void DestroyGame()
    {
        Destroy(currentGame);
        currentGame = null;
    }
}
