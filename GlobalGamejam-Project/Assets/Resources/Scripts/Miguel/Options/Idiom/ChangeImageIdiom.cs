using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.Miguel;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageIdiom : MonoBehaviour
{
    public string id = "";

    private Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        ChangeImage();
    }

    public void ChangeImage()
    {
        ImageUIIdiom imageUIIdiom = 
            UnityEngine.Resources.Load<ImageUIIdiom>("ScriptableObjects/ImageUIIdiomSO");
            
        string idiom = "es";
        if (PlayerPrefs.HasKey("CurrentIdiom")) 
            idiom = PlayerPrefs.GetString("CurrentIdiom");

        foreach (var imageObj in imageUIIdiom.images)
        {
            if (!imageObj.id.Equals(id)) continue;

            switch (idiom)
            {
                case "es": image.sprite = imageObj.spriteES;
                    break;
                case "en": image.sprite = imageObj.spriteEN;
                    break;
            }
            
            return;
        }

        image.sprite = null;
    }
}
