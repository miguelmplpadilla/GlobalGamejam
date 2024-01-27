using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door instance;
    public GameObject doorOpen;
    public GameObject doorClosed;
    public bool isOpen;
    public bool isChecking;
    void Awake()
    {
        instance = this;
    }
    
    public async void OpenDoor(int delay)
    {
        await Task.Delay(delay);
        isOpen = true;
        doorOpen.SetActive(true);
        doorClosed.SetActive(false);

        int nextAction = Random.Range(2000, 5000);
        CheckDoor(750);
        CloseDoor(nextAction);
    }
    public async void CloseDoor(int delay)
    {
        await Task.Delay(delay);
        Debug.Log("Closed");
        isChecking = false;
        isOpen = false;
        doorOpen.SetActive(false);
        doorClosed.SetActive(true);

        int nextAction = Random.Range(2500, 6500);
        OpenDoor(nextAction);
    }

    public void CloseDoorFirst(int delay)
    {
        Debug.Log("Closed");
        isChecking = false;
        isOpen = false;
        doorOpen.SetActive(false);
        doorClosed.SetActive(true);

        OpenDoor(delay);
    }

    public async void CheckDoor(int delay)
    {
        await Task.Delay(delay);
        isChecking = true;
    }

}
