using UnityEngine;

public class AnimationExecutor : MonoBehaviour
{
    public void ExecuteFunction(string functionName)
    {
        GameObject.Find("SceneManager").SendMessage(functionName);
    }
    
    public void ExecuteEndAnimation() {
        SceneController.instance.EndAnimation();
    }
}
