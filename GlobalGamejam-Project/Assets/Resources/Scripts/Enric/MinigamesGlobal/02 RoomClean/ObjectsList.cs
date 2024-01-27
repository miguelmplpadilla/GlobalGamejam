using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsList : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> objects;
    public static ObjectsList instance;

    private void Awake()
    {
        instance = this;
    }

    public void RemoveObject(GameObject gameOb)
    {
        if(objects.Contains(gameOb))
        {
            objects.Remove(gameOb);
            if (objects.Count <= 0) Debug.Log("Game Ended");
        }
    }
}
