using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class BirthLevel : Level, IPointerClickHandler
{
    public static BirthLevel instance;
    [SerializeField] private bool isTimerOn = false;

    [SerializeField] private float totalForce;
    [SerializeField] private PositionConstraint cameraConstraint;
    [SerializeField] private GameObject tutorialText;
    [SerializeField] private GameObject totalForceObject;
    [SerializeField] private TextMeshProUGUI totalForceText;
    public Baby baby;

    void Awake()
    {
        totalForce = 0;
        instance = this;
        isTimerOn = false;
    }
    void Start()
    {
        totalForceObject.SetActive(false);
        tutorialText.SetActive(true);
        StartTimer(3);
    }
    private void Click()
    {
        if (!isTimerOn) return;
        totalForceObject.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-10,10));
        totalForce = totalForce + Random.Range(0.73f, 1.42f);
        totalForceText.SetText("X" + totalForce.ToString("0.00"));
    }

    private async void StartTimer(int delay)
    {
        await Task.Delay(delay * 1000);
        tutorialText.SetActive(false);
        totalForceObject.SetActive(true);
        TimerBirth.instance.StartTimer(10);
        isTimerOn = true;
    }

    public void timerEnded()
    {
        isTimerOn = false;
        baby.ThrowBaby(totalForce);
        totalForceObject.SetActive(false);
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
