using System;

[Serializable]
public class Card
{
    public KeyDecision keys;
    public string textCard = "";

    public int tipeCard = 0;
}

[Serializable]
public class KeyDecision
{
    public string key = "";
    public string leftDecisionText = "";
    public string rightDecisionText = "";
}
