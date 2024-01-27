using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public static CardController instance;

    public GameObject cardObj;
    public GameObject canvasCard;
    
    private RectTransform rtParent;
    
    private Vector2 initialPosition;
    private Vector2 originalPositionPanel;

    public Image imagePanelLeft;
    public Image imagePanelRight;

    public Image imageCard;
    public Image imageDiapositiva;
    public Image imageBackgroundDiapositiva;
    
    public Image imageForeground;
    
    public GameObject animationPanel;
    public Image imageBackgroundAnimation;

    public TextMeshProUGUI textCard;
    public TextMeshProUGUI textLeft;
    public TextMeshProUGUI textRight;

    private Color originalColorPanel;

    private int panelPaintColor = 0;

    private bool returningCenter = false;

    private Story story;
    private Passage currentPassage;

    public TextAsset jsonStory;

    private string leftKey;
    private string rightKey;

    private string nextPassageKey;

    private bool leftRight;

    private GameObject camera;

    private void Awake()
    {
        instance = this;
        
        rtParent = cardObj.transform.parent.GetComponent<RectTransform>();

        story = new Story();
        JsonUtility.FromJsonOverwrite(jsonStory.text, story);

        //Debug.Log(JsonUtility.ToJson(new Card()));
        
        SceneManager.LoadScene("Minigames", LoadSceneMode.Additive);
    }

    private void Start()
    {
        camera = GameObject.Find("MainCamera");
        originalPositionPanel = rtParent.anchoredPosition;
        originalColorPanel = imagePanelLeft.color;
        
        SetScene(story.passages[0]);
    }

    private void Update()
    {
        ChangeColorPanel();
    }

    public void SetScene(Passage passage)
    {
        string[] info = passage.text.Split("\n\n");

        Card card = new Card();
        
        JsonUtility.FromJsonOverwrite(info[0].Replace("\n", "").Replace("\t", ""), card);

        switch (card.tipeCard)
        {
            case 1: SetCard(card, passage);
                break;
            case 2: SetDiapositiva(card, passage);
                break;
            case 3: SetAnimation(card, passage);
                break;
            case 4: SetGame(card, passage);
                break;
        }
    }

    private void SetCard(Card card, Passage passage)
    {
        imageCard.sprite = GetSprite(card.keys.key);
        
        textCard.text = card.textCard;
        textLeft.text = card.keys.leftDecisionText;
        textRight.text = card.keys.rightDecisionText;
        
        leftKey = passage.links[0].name;
        rightKey = passage.links[1].name;
    }

    private void SetDiapositiva(Card card, Passage passage)
    {
        imageDiapositiva.transform.parent.gameObject.SetActive(true);
        
        Sprite spriteDiapositiva = GetSprite(card.keys.key);
        imageDiapositiva.sprite = spriteDiapositiva;

        Color colorBackground = imageBackgroundDiapositiva.color;
        colorBackground.a = 1;

        imageBackgroundDiapositiva.DOColor(colorBackground, 0.2f);
        imageDiapositiva.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        
        nextPassageKey = passage.links[0].name;
    }

    private Sprite GetSprite(string nameSprite)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Diapositivas");

        foreach (var sprite in sprites)
            if (sprite.name.Equals(nameSprite)) return sprite;
        
        return GetSprite("Kojima_Quieres");
    }

    public async void HideDiapositiva()
    {
        Color colorBackground = imageBackgroundDiapositiva.color;
        colorBackground.a = 0;
        
        imageBackgroundDiapositiva.DOColor(colorBackground, 0.2f);
        await imageDiapositiva.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        
        imageDiapositiva.transform.parent.gameObject.SetActive(false);
    }

    public async void HideAnimation(Action callback = null)
    {
        Color colorBackground = imageBackgroundAnimation.color;
        colorBackground.a = 0;
        
        imageBackgroundAnimation.DOColor(colorBackground, 0.2f);
        await animationPanel.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        
        animationPanel.transform.parent.gameObject.SetActive(false);
        
        callback?.Invoke();
    }

    public void EndAnimation()
    {
        SetScene(GetPassage(nextPassageKey));
        HideAnimation(() =>
        {
            Destroy(animationPanel.transform.GetChild(0).gameObject);
        });
    }

    public void EndGame()
    {
        Debug.Log("EndGame");
        
        Passage passage = GetPassage(nextPassageKey);

        bool sceneSeted = passage.links.Count < 2;
        
        FadeInOutForegorund(callbackMidle: () =>
        {
            canvasCard.SetActive(true);
            camera.SetActive(true);
            MinigamesHandler.instance.DestroyGame();
            
            if (sceneSeted)
                SetScene(passage);
        });
        
        if (!sceneSeted)
            SetScene(passage);
    }

    private void FadeImage(GameObject imageObj, float time, float alpha, Action callback = null, bool onlyObj = false)
    {
        Image image = imageObj.GetComponent<Image>();
        
        Color colorImage = image.color;
        colorImage.a = alpha;
        
        image.DOColor(colorImage, time).OnComplete(() => { callback?.Invoke(); });

        if (onlyObj) return;

        if (imageObj.transform.childCount == 0) return;

        for (int i = 0; i < imageObj.transform.childCount; i++)
            FadeImage(imageObj.transform.GetChild(i).gameObject, time, alpha);
    }

    public void SetNext()
    {
        SetScene(GetPassage(nextPassageKey));
    }

     async void SetAnimation(Card card, Passage passage)
    {
        nextPassageKey = passage.links[0].name;
        
        animationPanel.transform.parent.gameObject.SetActive(true);

        Color colorBackground = imageBackgroundAnimation.color;
        colorBackground.a = 1;

        await imageBackgroundAnimation.DOColor(colorBackground, 0.2f).AsyncWaitForCompletion();

        Instantiate(GetPrefab(card.keys.key), animationPanel.transform);
        
        animationPanel.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
    }

    private GameObject GetPrefab(string namePrefab)
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("PrefabsMiguel");

        foreach (var prefab in prefabs)
            if (prefab.name.Equals(namePrefab))
                return prefab;

        return GetPrefab("PitiAnimation");
    }

    private void SetGame(Card card, Passage passage)
    {
        nextPassageKey = passage.links[0].name;
        
        FadeInOutForegorund(callbackMidle: () =>
        {
            canvasCard.SetActive(false);
            camera.SetActive(false);
            MinigamesHandler.instance.StartMinigame(card.keys.key);
        });
    }

    public Passage GetPassage(string cardName)
    {
        foreach (var passage in story.passages)
            if (cardName.Equals(passage.name)) return passage;
        
        return null;
    }

    private async void FadeInOutForegorund(Action callbackMidle = null, Action callbackEnd = null)
    {
        imageForeground.transform.parent.gameObject.SetActive(true);
        
        Color colorForeground = imageForeground.color;
        colorForeground.a = 0;

        imageForeground.color = colorForeground;

        colorForeground.a = 1;

        await imageForeground.DOColor(colorForeground, 1).AsyncWaitForCompletion();
        
        callbackMidle?.Invoke();

        imageForeground.DOKill();
        
        colorForeground.a = 1;
        imageForeground.color = colorForeground;
        
        colorForeground.a = 0;
        await imageForeground.DOColor(colorForeground, 1).AsyncWaitForCompletion();
        
        callbackEnd?.Invoke();
        
        imageForeground.transform.parent.gameObject.SetActive(false);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (returningCenter) return;
        
        cardObj.transform.parent.DOScale(0.95f, 0.2f);
        initialPosition = eventData.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (returningCenter) return;
        
        float distance = Vector2.Distance(new Vector2(initialPosition.x, 0), new Vector2(eventData.position.x, 0));
        
        if (distance < 70) 
        {
            panelPaintColor = 0;
            cardObj.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
            rtParent.DOAnchorPos(originalPositionPanel, 0.2f);
            return;
        }
        
        leftRight = eventData.position.x - initialPosition.x < 0;

        panelPaintColor = leftRight ? 1 : 2;
        
        cardObj.transform.DORotate(new Vector3(0, 0, leftRight ? 10 : -10), 0.3f);
        rtParent.DOAnchorPos(new Vector2(leftRight ? -100 : 100, originalPositionPanel.y), 0.3f);
    }
    
    public async void OnPointerUp(PointerEventData eventData)
    {
        int decision = panelPaintColor;
        panelPaintColor = 0;

        cardObj.transform.parent.DOKill();
        cardObj.transform.DOKill();
        rtParent.DOKill();
        
        cardObj.transform.parent.DOScale(1, 0.2f);
        cardObj.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        await rtParent.DOAnchorPos(originalPositionPanel, 0.2f).AsyncWaitForCompletion();
        
        if (decision != 0) SetScene(GetPassage(decision == 1 ? leftKey : rightKey));

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