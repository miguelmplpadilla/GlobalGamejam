using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    public GameObject panelButtons;
    public GameObject panelOptions;
    
    public void OpenOptions()
    {
        panelButtons.transform.localScale = Vector3.zero;
        panelOptions.transform.localScale = Vector3.one;
    }

    public void CloseOptions()
    {
        panelButtons.transform.localScale = Vector3.one;
        panelOptions.transform.localScale = Vector3.zero;
    }
    
    //[MenuItem("Tools/Delete Player Prefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Player Prefs Deleted");
    }
}
