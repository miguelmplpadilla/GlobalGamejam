using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GPGSManager : MonoBehaviour
{
    public static GPGSManager instance;
    
    private PlayGamesClientConfiguration clientConfiguration;

    public Logro[] logrosList;
    public Dictionary<string, string> logros = new Dictionary<string, string>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        instance = this;
        
        for (int i = 0; i < logrosList.Length; i++)
            logros.Add(logrosList[i].nombreLogro, logrosList[i].keyLogro);
    }

    public void Start()
    {
        ConfigureGPGS();
        SignIntoGPGS(SignInInteractivity.CanPromptOnce, clientConfiguration);
    }
    
    internal void ConfigureGPGS()
    {
        clientConfiguration = new PlayGamesClientConfiguration.Builder().Build();
    }

    internal void SignIntoGPGS(SignInInteractivity interactivity, PlayGamesClientConfiguration configuration)
    {
        configuration = clientConfiguration;
        PlayGamesPlatform.InitializeInstance(configuration);
        PlayGamesPlatform.Activate();
        
        PlayGamesPlatform.Instance.Authenticate(interactivity, (code) =>
        {
            //statusText.text = "Autentificando...";

            if (code == SignInStatus.Success)
            {
                //statusText.text = "Autentificado correctamente";
                //descriptionText.text = "Hola " + Social.localUser.userName + " tu ID es " + Social.localUser.id;
            }
            else
            {
                //statusText.text = "Fallo en la autentificacion";
                //descriptionText.text = "Fallo en la autentificacion, la razon del fallo es " + code;
            }
        });
    }
    
    public void DoGrantAchievement(string achievement)
    {
        Debug.Log("Desbloqueando logro ...");
        
        PlayGamesPlatform.Instance.ReportProgress(GPGSManager.instance.logros[achievement], 100, (status) =>
        {
        });
    }
    
    public void ShowAchivementsUI()
    {
        Social.ShowAchievementsUI();
    }

    [Serializable]
    public class Logro
    {
        public string nombreLogro;
        public string keyLogro;
    }
}
