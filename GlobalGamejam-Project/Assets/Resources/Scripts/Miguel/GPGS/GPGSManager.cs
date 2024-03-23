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
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            gameObject.SetActive(false);
            return;
        }
        
        for (int i = 0; i < logrosList.Length; i++)
            logros.Add(logrosList[i].nombreLogro, logrosList[i].keyLogro);
    }

    public async void Start()
    {
        await Task.Delay(1000);
        
        ConsoleController.instance.ClearConsole();
        
        ConfigureGPGS();
        SignIntoGPGS(SignInInteractivity.CanPromptOnce, clientConfiguration);
    }
    
    internal void ConfigureGPGS()
    {
        clientConfiguration = new PlayGamesClientConfiguration.Builder().Build();
    }

    internal async void SignIntoGPGS(SignInInteractivity interactivity, PlayGamesClientConfiguration configuration)
    {
        Debug.Log("Autentificando...");
        
        configuration = clientConfiguration;
        PlayGamesPlatform.InitializeInstance(configuration);
        PlayGamesPlatform.Activate();

        await Task.Delay(1000);
        
        PlayGamesPlatform.Instance.Authenticate(interactivity, (code) =>
        {
            if (code == SignInStatus.Success)
            {
                GameObject.Find("IniciarSesionBoton").transform.localScale = Vector3.zero;
                GameObject.Find("MostrarLogrosBoton").transform.localScale = Vector3.one;
                
                Debug.Log("Autentificado correctamente");
                Debug.Log("Hola " + Social.localUser.userName + " tu ID es " + Social.localUser.id);
                return;
            }
            
            GameObject.Find("IniciarSesionBoton").transform.localScale = Vector3.one;
            
            Debug.Log("Fallo en la autentificacion");
            Debug.Log("Fallo en la autentificacion, la razon del fallo es " + code);
        });
    }
    
    public void LogIn() 
    {
        SignIntoGPGS(SignInInteractivity.CanPromptOnce, clientConfiguration);
    }
    
    
    public void DoGrantAchievement(string achievement)
    {
        Debug.Log("Desbloqueando logro ...");
        
        PlayGamesPlatform.Instance.ReportProgress(logros[achievement], 100, (status) =>
        {
            if (status)
                Debug.Log("Logro desbloqueado");
            else
                Debug.Log("Fallo en el desbloqueo");
        });
    }
    
    public void ShowAchivementsUI()
    {
        Debug.Log("Mostrando logros en pantalla ...");
        Social.ShowAchievementsUI();
    }

    [Serializable]
    public class Logro
    {
        public string nombreLogro;
        public string keyLogro;
    }
}
