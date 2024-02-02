using System;
using System.Collections.Generic;

namespace Resources.Scripts.Miguel
{
    [Serializable]
    public class TextUIIdiom
    {
        public string id = "";
        public string text = "";
    }

    [Serializable]
    public class TextsUI
    {
        public List<TextUIIdiom> texts;
    }
}