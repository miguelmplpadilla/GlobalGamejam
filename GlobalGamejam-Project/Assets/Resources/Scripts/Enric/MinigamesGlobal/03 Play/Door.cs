using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door instance;
    public GameObject doorOpen;
    public GameObject doorClosed;
    void Awake()
    {
        instance = this;
    }
    
    public void OpenDoor()
    {
        doorOpen.SetActive(true);
        doorClosed.SetActive(false);
    }
    public void CloseDoor()
    {
        doorOpen.SetActive(false);
        doorClosed.SetActive(true);
    }
}
