using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class BirthLevel : MonoBehaviour, IPointerClickHandler
{
    public static BirthLevel instance;
    [SerializeField] private bool isTimerOn = false;

    [SerializeField] private float totalForce;
    [SerializeField] private PositionConstraint cameraConstraint;
    public Baby baby;

    void Awake()
    {
        totalForce = 0;
        instance = this;
        isTimerOn = false;
    }
    void Start()
    {
        StartTimer(1);
    }
    private void Click()
    {
        if (!isTimerOn) return;
        totalForce++;
    }

    private async void StartTimer(int delay)
    {
        await Task.Delay(delay * 1000);
        TimerBirth.instance.StartTimer(10);
        isTimerOn = true;
    }

    public void timerEnded()
    {
        isTimerOn = false;
        baby.ThrowBaby(totalForce);
        cameraConstraint.constraintActive = true;
    }
    // Update is called once per frame
    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Click();
    }
}
