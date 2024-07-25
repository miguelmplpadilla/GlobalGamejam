using UnityEngine;

public class CurrentHistoryController : MonoBehaviour
{
    public HistoryCreator currectHistory;

    public static CurrentHistoryController instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        instance = this;
    }
}
