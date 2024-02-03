using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.Miguel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdiomController : MonoBehaviour
{
    public int index = 0;
    public Idiom[] idioms;

    private Image imageIdiom;

    [Serializable]
    public class Idiom
    {
        public TextUIIdiom textUIIdiom;
        public Sprite imageIdiom;
    }

    private void Awake()
    {
        imageIdiom = GetComponent<Image>();
    }

    private void Start()
    {
        ChangeIdiomFromPlayerPrefs();
    }

    public void ChangeIdiomLeft()
    {
        index--;
        if (index < 0) index = idioms.Length - 1;
        
        ChangeIdiom();
    }
    
    public void ChangeIdiomRight()
    {
        index++;
        if (index == idioms.Length) index = 0;
        
        ChangeIdiom();
    }

    private void ChangeIdiom()
    {
        PlayerPrefs.SetString("CurrentIdiom", idioms[index].textUIIdiom.id);

        imageIdiom.sprite = idioms[index].imageIdiom;

        foreach (var changeTextIdiom in FindObjectsOfType<ChangeTextIdiom>())
            changeTextIdiom.ChangeText();
        
        foreach (var changeImageIdiom in FindObjectsOfType<ChangeImageIdiom>())
            changeImageIdiom.ChangeImage();
    }
    
    private void ChangeIdiomFromPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("CurrentIdiom"))
        {
            ChangeIdiom();
            return;
        }
        
        for (int i = 0; i < idioms.Length; i++)
        {
            if (!idioms[i].textUIIdiom.id
                    .Equals(PlayerPrefs.GetString("CurrentIdiom"))) continue;
            
            index = i;
            ChangeIdiom();
            return;
        }
    }
}
