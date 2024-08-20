using UnityEngine;

public class DefaultGameController : MonoBehaviour
{
    public void EndGame(int endNum)
    {
        MinigamesHandler.instance.EndMinigame(endNum);
    }
}
