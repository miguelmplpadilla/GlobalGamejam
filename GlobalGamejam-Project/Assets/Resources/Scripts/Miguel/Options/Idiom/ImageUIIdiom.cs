using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Resources.Scripts.Miguel
{
    [CreateAssetMenu(fileName = "ImageUIIdiomSO", menuName = "ScriptableObjects/ImageUIIdiom", order = 1)]
    public class ImageUIIdiom : ScriptableObject
    {
        public List<ImageUI> images;
    }
    
    [Serializable]
    public class ImageUI
    {
        public string id = "";
        public Sprite spriteES;
        public Sprite spriteEN;
    }
}