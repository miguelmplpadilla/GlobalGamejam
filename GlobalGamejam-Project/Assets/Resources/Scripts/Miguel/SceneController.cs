using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public bool isTesting = false;
    public string startingPassageName = "";

    private string currentHistoria = "";
    
    public static SceneController instance;

    public GameObject cardObj;
    public GameObject canvasCard;
    public GameObject canvasCalendar;
    
    private RectTransform rtParent;
    
    private Vector2 initialPosition;
    private Vector2 originalPositionPanel;

    public Image imagePanelLeft;
    public Image imagePanelRight;

    public Image imageCard;
    public Image imageDiapositiva;
    
    public Image imageForeground;
    
    public GameObject animationPanel;

    public TextMeshProUGUI textCard;
    public TextMeshProUGUI textLeft;
    public TextMeshProUGUI textRight;

    private Color originalColorPanel;

    private int panelPaintColor = 0;

    private bool returningCenter = false;

    private Story story;
    private Passage currentPassage;

    private TextAsset jsonStory;

    private string leftKey;
    private string rightKey;

    private string nextPassageKey;

    private bool leftRight;

    private GameObject camera;

    public string[] idiomas;
    public string languageName;

    private void Awake()
    {
        CultureInfo culture = CultureInfo.CurrentCulture;

        languageName = culture.TwoLetterISOLanguageName;
        
        if (PlayerPrefs.HasKey("CurrentIdiom"))
            languageName = PlayerPrefs.GetString("CurrentIdiom");

        if (PlayerPrefs.HasKey("CurrentHistoria"))
            currentHistoria = PlayerPrefs.GetString("CurrentHistoria");
        else
            currentHistoria = "Historia1";
        
        jsonStory = UnityEngine.Resources.Load<TextAsset>("JSON/" + languageName + "/" + currentHistoria + "/" + currentHistoria);

        if (idiomas.Contains(languageName))
            jsonStory = UnityEngine.Resources.Load<TextAsset>("JSON/" + languageName + "/" + currentHistoria + "/" + currentHistoria);
        else
            jsonStory = UnityEngine.Resources.Load<TextAsset>("JSON/en/" + currentHistoria + "/" + currentHistoria);
        
        instance = this;
        
        rtParent = cardObj.transform.parent.GetComponent<RectTransform>();

        story = new Story();
        JsonUtility.FromJsonOverwrite(jsonStory.text, story);
        
        SceneManager.LoadScene("Minigames", LoadSceneMode.Additive);
        
        if (PlayerPrefs.HasKey("MusicPlayed_"+currentHistoria)) 
            PlayAudio(2, PlayerPrefs.GetString("MusicPlayed_"+currentHistoria), true);
    }

    private void Start()
    {
        camera = GameObject.Find("MainCamera");
        originalPositionPanel = rtParent.anchoredPosition;
        originalColorPanel = imagePanelLeft.color;
        
        //GPGSManager.instance.DoGrantAchievement("Inicio"+currentHistoria);

        string startPassage = startingPassageName;

        if (!isTesting && PlayerPrefs.HasKey("PassageSaved_"+currentHistoria))
            startPassage = PlayerPrefs.GetString("PassageSaved_"+currentHistoria);
        else if (!isTesting)
            startPassage = story.passages[0].name;
        
        SetScene(GetPassage(startPassage));
    }

    private void Update()
    {
        ChangeColorPanel();
    }

    public void SetScene(Passage passage)
    {
        SaveData(passage);
        
        string[] info = passage.text.Split("\n\n");

        Card card = new Card();

        Debug.Log(passage.text);
    
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
            case 5: SetCalendar(card, passage);
                break;
            case 6:
                EndHistoria();
                VolverMenuInicio();
                break;
        }

        Debug.Log(card.audio.soundName);
        
        PlayAudio(card.audio.typeSound, card.audio.soundName, card.audio.loop);
    }

    private void PlayAudio(int typeAudio, string audioName, bool loop)
    {
        Debug.Log("Play Audio: "+audioName);
        if (audioName.Equals("")) return;
        
        switch (typeAudio)
        {
            case 1:
                AudioManagerController.instance.PlaySfx(audioName, loop);
                break;
            case 2:
                AudioManagerController.instance.PlayMusic(audioName);
                PlayerPrefs.SetString("MusicPlayed_"+currentHistoria, audioName);
                break;
        }
    }

    private void SaveData(Passage passage)
    {
        string recorrido = "";
        if (PlayerPrefs.HasKey("RecorridoTomado_" + currentHistoria))
            recorrido = PlayerPrefs.GetString("RecorridoTomado_" + currentHistoria);

        string[] recorridoSplit = recorrido.Split("\n");

        for (int i = 0; i < recorridoSplit.Length; i++)
            Debug.Log(recorridoSplit[i]);

        if (recorridoSplit[recorridoSplit.Length - 1].Equals("- " + passage.name)) return;

        recorrido += "- " + passage.name + "\n";
        
        PlayerPrefs.SetString("RecorridoTomado_"+currentHistoria, recorrido);
        PlayerPrefs.SetString("PassageSaved_"+currentHistoria, passage.name);
    }

    private void EndHistoria()
    {
        DateTime now = DateTime.Now;
        
        string recorridoTxt = "RecorridoTomado_" + SystemInfo.deviceName + "_" + currentHistoria + "_" +
                              now.ToString("yyyy-MM-dd_HH:mm:ss");
        
        FirebaseStorageController.instance.UploadFile(
            Encoding.UTF8.GetBytes(PlayerPrefs.GetString("RecorridoTomado_" + currentHistoria)), recorridoTxt);
                
        PlayerPrefs.DeleteKey("RecorridoTomado_"+currentHistoria);
        PlayerPrefs.SetString("PassageSaved_"+currentHistoria, story.passages[0].name);
        
        PlayerPrefs.DeleteKey("MusicPlayed_"+currentHistoria);
        
        //GPGSManager.instance.DoGrantAchievement("Fin"+currentHistoria);
    }

    public void VolverMenuInicio()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void CerrarJuegoMenuInicio()
    {
        FadeInForeground(() =>
        {
            VolverMenuInicio();
        });
    }

    private async void SetCalendar(Card card, Passage passage)
    {
        if (passage.links.Count > 0) 
            nextPassageKey = passage.links[0].name;
        else
            nextPassageKey = story.passages[0].name;
        
        canvasCalendar.SetActive(true);
        CalendarScript.instance.
            SetTextCalendar(card.keys.leftDecisionText, card.keys.rightDecisionText);
        
        FadeOutForeground(() =>
        {
            CalendarScript.instance.StartCalendar();
        });

        await Task.Delay(2500);
        
        FadeInForeground(() =>
        {
            canvasCalendar.SetActive(false);
            SetNext();
        });
    }

    private void SetCard(Card card, Passage passage)
    {
        //imageCard.sprite = GetSprite(card.keys.key);
        
        textCard.text = card.textCard.Replace("/n", "\n");
        textLeft.text = card.keys.leftDecisionText;
        textRight.text = card.keys.rightDecisionText;
        
        leftKey = passage.links[0].name;
        rightKey = passage.links[1].name;
        
        FadeOutForeground();
        
        cardObj.transform.parent.parent.localScale = Vector3.one;
    }

    private void SetDiapositiva(Card card, Passage passage)
    {
        FadeOutForeground();
        
        imageDiapositiva.transform.parent.gameObject.SetActive(true);
        
        Sprite spriteDiapositiva = GetSprite(card.keys.key);
        imageDiapositiva.sprite = spriteDiapositiva;

        if (passage.links.Count > 0) 
            nextPassageKey = passage.links[0].name;
        else
            nextPassageKey = story.passages[0].name;
    }

    private Sprite GetSprite(string nameSprite)
    {
        Sprite[] sprites = UnityEngine.Resources.LoadAll<Sprite>("Sprites/Diapositivas");

        foreach (var sprite in sprites)
            if (sprite.name.Equals(nameSprite)) return sprite;
        
        return GetSprite("Kojima_Quieres");
    }

    public void HideDiapositiva()
    {
        Debug.Log("HideDiapositiva");
        FadeInForeground(() =>
        {
            imageDiapositiva.transform.parent.gameObject.SetActive(false);
            SetNext();
        });
    }

    public void HideAnimation(Action callback = null)
    {
        FadeInForeground(() =>
        {
            animationPanel.transform.parent.gameObject.SetActive(false);
        
            callback?.Invoke();
        });
    }

    public void EndAnimation()
    {
        HideAnimation(() =>
        {
            Destroy(animationPanel.transform.GetChild(0).gameObject);
            SetScene(GetPassage(nextPassageKey));
        });
    }

    public void EndGame(int typeDecision = 0)
    {
        Debug.Log(typeDecision);
        Passage passage = null; 

        switch (typeDecision)
        {
            case 0: passage = GetPassage(nextPassageKey);
                break;
            case 1: passage = GetPassage(leftKey);
                break;
            case 2: passage = GetPassage(rightKey);
                break;
        }
        
        FadeInForeground(() =>
        {
            canvasCard.SetActive(true);
            camera.SetActive(true);
            MinigamesHandler.instance.DestroyGame();
            SetScene(passage);
        });
    }

    private async void FadeInForeground(Action callback = null)
    {
        imageForeground.transform.parent.gameObject.SetActive(true);
        
        Color colorForeground = imageForeground.color;
        colorForeground.a = 0;

        imageForeground.color = colorForeground;

        colorForeground.a = 1;
        
        imageForeground.DOKill();

        await imageForeground.DOColor(colorForeground, 1).AsyncWaitForCompletion();
        
        callback?.Invoke();
    }
    
    private async void FadeOutForeground(Action callback = null)
    {
        imageForeground.transform.parent.gameObject.SetActive(true);
        
        Color colorForeground = imageForeground.color;
        colorForeground.a = 1;

        imageForeground.color = colorForeground;
        
        colorForeground.a = 0;
        await imageForeground.DOColor(colorForeground, 1).AsyncWaitForCompletion();
        
        callback?.Invoke();
        
        imageForeground.transform.parent.gameObject.SetActive(false);
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

    private void SetAnimation(Card card, Passage passage)
    {
        if (passage.links.Count > 0) 
            nextPassageKey = passage.links[0].name;
        else
            nextPassageKey = story.passages[0].name;
        
        animationPanel.transform.parent.gameObject.SetActive(true);

        Instantiate(GetPrefab(card.keys.key), animationPanel.transform).transform.localPosition = Vector3.zero;
        
        FadeOutForeground();
    }

    private GameObject GetPrefab(string namePrefab)
    {
        GameObject[] prefabs = UnityEngine.Resources.LoadAll<GameObject>("Prefabs/Animations");

        foreach (var prefab in prefabs)
            if (prefab.name.Equals(namePrefab))
                return prefab;

        return GetPrefab("PitiAnimation");
    }

    private void SetGame(Card card, Passage passage)
    {
        if (passage.links.Count > 0)
        {
            nextPassageKey = passage.links[0].name;

            if (passage.links.Count > 1)
            {
                leftKey = passage.links[0].name;
                rightKey = passage.links[1].name;
            }
        }
        else
            nextPassageKey = story.passages[0].name;

        if (!card.keys.key.Equals(""))
        {
            MinigamesHandler.instance.StartMinigame(card.keys.key);
            canvasCard.SetActive(false);
            camera.SetActive(false);
        }
        
        FadeOutForeground(() =>
        {
            if (card.keys.key.Equals(""))
                EndGame();
        });
    }

    public Passage GetPassage(string cardName)
    {
        foreach (var passage in story.passages)
            if (cardName.Equals(passage.name)) return passage;
        
        return null;
    }

    private void FadeInOutForegorund(Action callbackMidle = null, Action callbackEnd = null)
    {
        FadeInForeground(() =>
        {
            callbackMidle?.Invoke();
            FadeOutForeground(callbackEnd);
        });
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

        if (decision != 0)
        {
            FadeInForeground(() =>
            {
                cardObj.transform.parent.parent.localScale = Vector3.zero;
                SetScene(GetPassage(decision == 1 ? leftKey : rightKey));
            });
        }
        
        cardObj.transform.parent.DOScale(1, 0.2f);
        cardObj.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        await rtParent.DOAnchorPos(originalPositionPanel, 0.2f).AsyncWaitForCompletion();
        
        returningCenter = false;
    }

    private void ChangeColorPanel()
    {
        imagePanelLeft.DOColor(originalColorPanel, 0.3f);
        imagePanelRight.DOColor(originalColorPanel, 0.3f);
        
        if (panelPaintColor == 0) return;

        imagePanelLeft.DOKill();
        imagePanelRight.DOKill();
        
        imagePanelLeft.DOColor(panelPaintColor == 1 ? new Color32(27, 27, 27, 255) : originalColorPanel, 0.2f);
        imagePanelRight.DOColor(panelPaintColor == 2 ? new Color32(27, 27, 27, 255) : originalColorPanel, 0.2f);
    }
}