using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamesHandler : MonoBehaviour
{
    public static MinigamesHandler instance;

    public GameObject[] minigames;
    public GameObject currentGame;

    public Transform minigameParent;
    private void Awake()
    {
        instance = this;
    }

    public void StartMinigame(string levelName)
    {
        GameObject gameToSpawn = null;
        foreach(GameObject game in minigames)
        {
            if(game.name == levelName)
            {
                gameToSpawn = game;
            }
        }
        if (!gameToSpawn)
        {
            Debug.LogError("Level not found");
            return;
        }
        currentGame = Instantiate(gameToSpawn, minigameParent);
    }

    public void EndMinigame()
    {
        CardController.instance.EndGame();
    }
    public void DestroyGame()
    {
        Destroy(currentGame);
        currentGame = null;
    }
}
