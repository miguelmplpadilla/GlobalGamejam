using System;
using TMPro;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    private TextMeshProUGUI logText;
    public static ConsoleController instance;

    private void Awake()
    {
        instance = this;
        
        logText = GetComponent<TextMeshProUGUI>();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogCallback;
    }

    void LogCallback(string logString, string stackTrace, LogType type)
    {
        logText.text += logString + "\n\n";
    }

    public void ClearConsole()
    {
        logText.text = "";
    }
}
