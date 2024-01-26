using System;

[Serializable]
public class Card
{
    public KeyDecision keys;
    public string textCard = "";

    public bool isCard = true;
}

[Serializable]
public class KeyDecision
{
    public string leftDecisionText = "";
    public string rightDecisionText = "";
}
