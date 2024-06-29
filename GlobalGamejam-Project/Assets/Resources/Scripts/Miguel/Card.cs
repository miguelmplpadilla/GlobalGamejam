using System;
using UnityEngine;

[Serializable]
public class Card
{
    public KeyDecision keys;
    public string textCard = "";

    public int tipeCard = 0;

    public PlayAudio audio;
}

[Serializable]
public class KeyDecision
{
    public string key = "";
    public string leftDecisionText = "";
    public string rightDecisionText = "";
}

[Serializable]
public class PlayAudio
{
    public int typeSound = 0;
    public string soundName = "";
    public bool loop = false;
}
