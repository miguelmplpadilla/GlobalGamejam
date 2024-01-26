using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rtParent;
    
    private Vector2 initialPosition;
    private Vector2 originalPositionPanel;

    public Image imagePanelLeft;
    public Image imagePanelRight;

    public Image imageDiapositiva;

    public TextMeshProUGUI textCard;
    public TextMeshProUGUI textLeft;
    public TextMeshProUGUI textRight;

    private Color originalColorPanel;

    private int panelPaintColor = 0;

    private bool returningCenter = false;

    private Story story;
    private Passage currentPassage;

    public TextAsset jsonStory;

    public string leftKey;
    public string rightKey;

    public string nextPassageKey;

    private bool leftRight;

    private void Awake()
    {
        rtParent = transform.parent.GetComponent<RectTransform>();

        story = new Story();
        JsonUtility.FromJsonOverwrite(jsonStory.text, story);
    }

    private void Start()
    {
        originalPositionPanel = rtParent.anchoredPosition;
        originalColorPanel = imagePanelLeft.color;
        
        SetCard(story.passages[0]);
    }

    private void Update()
    {
        ChangeColorPanel();
    }

    private void SetCard(Passage passage)
    {
        string[] info = passage.text.Split("\n\n");

        Card card = new Card();
        
        JsonUtility.FromJsonOverwrite(info[0], card);

        if (!card.isCard)
        {
            imageDiapositiva.transform.localScale = Vector2.one;
            Debug.Log("Diapositiva Mostrada");
            return;
        }
        
        textCard.text = card.textCard;
        textLeft.text = card.keys.leftDecisionText;
        textRight.text = card.keys.rightDecisionText;

        Debug.Log(info[1].Replace(" ", "").Replace("\n", ""));
        
        string[] opciones = info[1].Replace(" ", "").Split("]][[");
        
        if (opciones.Length > 1)
        {
            leftKey = opciones[0].Replace("[[","").Replace("]]","");
            rightKey = opciones[1].Replace("[[","").Replace("]]","");;
            
            return;
        }
        
        nextPassageKey = opciones[0];
    }

    private Passage GetPassage(string cardName)
    {
        foreach (var passage in story.passages)
        {
            Debug.Log(passage.name);
            if (cardName.Equals(passage.name)) return passage;
        }
        
        return null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (returningCenter) return;
        
        transform.parent.DOScale(0.95f, 0.2f);
        initialPosition = eventData.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (returningCenter) return;
        
        float distance = Vector2.Distance(new Vector2(initialPosition.x, 0), new Vector2(eventData.position.x, 0));
        if (distance < 70) 
        {
            panelPaintColor = 0;
            transform.DORotate(new Vector3(0, 0, 0), 0.2f);
            rtParent.DOAnchorPos(originalPositionPanel, 0.2f);
            return;
        }
        
        leftRight = eventData.position.x - initialPosition.x < 0;

        panelPaintColor = leftRight ? 1 : 2;
        
        transform.DORotate(new Vector3(0, 0, leftRight ? 10 : -10), 0.3f);
        rtParent.DOAnchorPos(new Vector2(leftRight ? -100 : 100, originalPositionPanel.y), 0.3f);
    }
    
    public async void OnPointerUp(PointerEventData eventData)
    {
        panelPaintColor = 0;
        
        transform.parent.DOScale(1, 0.2f);
        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        await rtParent.DOAnchorPos(originalPositionPanel, 0.2f).AsyncWaitForCompletion();
        
        SetCard(GetPassage(leftRight ? leftKey : rightKey));

        returningCenter = false;
    }

    private void ChangeColorPanel()
    {
        imagePanelLeft.DOColor(originalColorPanel, 0.3f);
        imagePanelRight.DOColor(originalColorPanel, 0.3f);
        
        if (panelPaintColor == 0) return;
        
        imagePanelLeft.DOColor(panelPaintColor == 1 ? new Color32(27, 27, 27, 255) : originalColorPanel, 0.2f);
        imagePanelRight.DOColor(panelPaintColor == 2 ? new Color32(27, 27, 27, 255) : originalColorPanel, 0.2f);
    }
}
