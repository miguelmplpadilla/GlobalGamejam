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
    
    IEnumerator OpenDoor(float delay)
    {
       yield return new WaitForSeconds(delay);
        isOpen = true;
        doorOpen.SetActive(true);
        doorClosed.SetActive(false);

        float nextAction = Random.Range(2, 5);
        StartCoroutine(CheckDoor(0.750f));
        StartCoroutine(CloseDoor(nextAction));
    }
    IEnumerator CloseDoor(float delay)
    {
        yield return new WaitForSeconds(delay);
        isChecking = false;
        isOpen = false;
        doorOpen.SetActive(false);
        doorClosed.SetActive(true);

        float nextAction = Random.Range(2.500f, 6.500f);
        StartCoroutine(OpenDoor(nextAction));
    }

    public void CloseDoorFirst(float delay)
    {
        isChecking = false;
        isOpen = false;
        doorOpen.SetActive(false);
        doorClosed.SetActive(true);

        StartCoroutine(OpenDoor(delay));
    }

    IEnumerator CheckDoor(float delay)
    {
        yield return new WaitForSeconds(delay);
        isChecking = true;
    }

}
