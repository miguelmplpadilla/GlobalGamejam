using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationExecutor : MonoBehaviour
{
    public void ExecuteFunction(string functionName)
    {
        GameObject.Find("CardManager").SendMessage(functionName);
    }
}
