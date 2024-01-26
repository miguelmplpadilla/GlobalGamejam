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

    public void StartMinigame(int idLevel)
    {
        if(idLevel >= minigames.Length || idLevel < 0)
        {
            Debug.LogError("idLevel out of bounds: " + idLevel);
            return;
        }
        currentGame = Instantiate(minigames[idLevel], minigameParent);
    }

    public void EndMinigame(string nextDecision)
    {
        Destroy(currentGame);
        currentGame = null;
    }

}
