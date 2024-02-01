using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.Miguel;
using TMPro;
using UnityEngine;

public class IdiomController : MonoBehaviour
{
    public int index = 0;
    public TextsUI idioms;

    private TextMeshProUGUI textIdiom;

    private void Awake()
    {
        textIdiom = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ChangeIdiomFromPlayerPrefs();
    }

    public void ChangeIdiomLeft()
    {
        index--;
        if (index < 0) index = idioms.texts.Count - 1;
        
        ChangeIdiom();
    }
    
    public void ChangeIdiomRight()
    {
        index++;
        if (index == idioms.texts.Count) index = 0;
        
        ChangeIdiom();
    }

    private void ChangeIdiom()
    {
        PlayerPrefs.SetString("CurrentIdiom", idioms.texts[index].id);

        textIdiom.text = idioms.texts[index].text;
    }
    
    private void ChangeIdiomFromPlayerPrefs()
    {
        for (int i = 0; i < idioms.texts.Count; i++)
        {
            if (idioms.texts[i].id.Equals(PlayerPrefs.GetString("CurrentIdiom")))
            {
                index = i;
                ChangeIdiom();
                return;
            }
        }
    }
}
