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

public class NewSceneController : MonoBehaviour
{
    public bool isTesting = false;
    public string startingPassageName = "";

    private string currentHistoria = "";
    
    public static NewSceneController instance;

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

    private PassageNode leftKey;
    private PassageNode rightKey;

    private PassageNode nextPassageKey;

    private bool leftRight;

    private GameObject camera;
    private GameObject audioDiapositiva;

    public string[] idiomas;
    public string languageName;
    
    public HistoryCreator historyCreator;
    private PassageNode firstPassageNode;

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
            PlayAudio(new DecisionNode.PlayAudio
            {
                soundName = PlayerPrefs.GetString("MusicPlayed_"+currentHistoria),
                loop = true,
                typeSound = DecisionNode.PlayAudio.TypeSound.MUSIC
            });

        for (int i = 0; i < historyCreator.nodes.Count; i++)
        {
            if (historyCreator.nodes[i].name.Equals("Story"))
            {
                firstPassageNode = (historyCreator.nodes[i] as StoryNode).startPassage;
                break;
            }
        }
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
        
        SetScene(firstPassageNode);
    }

    private void Update()
    {
        ChangeColorPanel();
    }

    public void SetScene(PassageNode passage)
    {
        //SaveData(passage);
        
        if (firstPassageNode is DecisionNode)
        {
            SetCard(passage as DecisionNode);
        } else if (firstPassageNode is CalendarNode)
        {
            SetCalendar(passage as CalendarNode);
        } else if (firstPassageNode is AnimationNode)
        {
            SetAnimation(passage as AnimationNode);
        } else if (firstPassageNode is DiapositiveNode)
        {
            SetDiapositiva(passage as DiapositiveNode);
        } else if (firstPassageNode is GameNode)
        {
            SetGame(passage as GameNode);
        } else if (firstPassageNode is EndStoryNode)
        {
            EndHistoria();
            VolverMenuInicio();
        } else if (firstPassageNode is PlayAudioNode)
        {
            PlayAudioNode playAudioNode = passage as PlayAudioNode;
            PlayAudio(playAudioNode.audio, playAudioNode.exitPassage1);
        }
    }

    private async void PlayAudio(DecisionNode.PlayAudio audio, PassageNode nexPassage = null)
    {
        Debug.Log("Play Audio: "+audio.soundName);
        if (audio.soundName.Equals("")) return;
        
        switch (audio.typeSound)
        {
            case DecisionNode.PlayAudio.TypeSound.SFX:
                audioDiapositiva = await AudioManagerController.instance.PlaySfx(audio.soundName,
                    audio.loop, isAwaited: false);
                break;
            case DecisionNode.PlayAudio.TypeSound.MUSIC:
                AudioManagerController.instance.PlayMusic(audio.soundName);
                PlayerPrefs.SetString("MusicPlayed_"+currentHistoria, audio.soundName);
                break;
        }

        if (nexPassage != null)
        {
            nextPassageKey = nexPassage;
            SetNext();
        }
    }

    private void SaveData(Passage passage)
    {
        string recorrido = "";
        if (PlayerPrefs.HasKey("RecorridoTomado_" + currentHistoria))
            recorrido = PlayerPrefs.GetString("RecorridoTomado_" + currentHistoria);
        else return;

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

    private async void SetCalendar(CalendarNode calendarNode)
    {
        nextPassageKey = calendarNode.exitPassage1 != null ? calendarNode.exitPassage1 : firstPassageNode;
        
        canvasCalendar.SetActive(true);
        CalendarScript.instance.SetTextCalendar(calendarNode);
        
        FadeOutForeground(() =>
        { CalendarScript.instance.StartCalendar(); });

        await Task.Delay(2500);
        
        FadeInForeground(() =>
        {
            canvasCalendar.SetActive(false);
            SetNext();
        });
    }

    private void SetCard(DecisionNode decisionNode)
    {
        //imageCard.sprite = GetSprite(card.keys.key);
        
        textCard.text = decisionNode.card.textos.textoES;
        textLeft.text = decisionNode.card.decisiones.decisionIzquierda;
        textRight.text = decisionNode.card.decisiones.decisionDerecha;
        
        leftKey = decisionNode.exitPassage1;
        rightKey = decisionNode.exitPassage2;
        
        FadeOutForeground();
        
        cardObj.transform.parent.parent.localScale = Vector3.one;
    }

    private void SetDiapositiva(DiapositiveNode diapositiveNode)
    {
        FadeOutForeground();
        
        imageDiapositiva.transform.parent.gameObject.SetActive(true);
        
        Sprite spriteDiapositiva = GetSprite(diapositiveNode.id);
        imageDiapositiva.sprite = spriteDiapositiva;

        nextPassageKey = diapositiveNode.exitPassage1 != null ? diapositiveNode.exitPassage1 : firstPassageNode;
    }

    private Sprite GetSprite(string nameSprite)
    {
        Sprite[] sprites = UnityEngine.Resources.LoadAll<Sprite>("Sprites/Diapositivas");

        Debug.Log("Idioma Get Sprite: "+languageName);

        foreach (var sprite in sprites)
            if (sprite.name.Equals(nameSprite+"_"+languageName.ToUpper())) return sprite;
        
        foreach (var sprite in sprites)
            if (sprite.name.Equals(nameSprite)) return sprite;
        
        foreach (var sprite in sprites)
            if (sprite.name.Equals(nameSprite+"_EN")) return sprite;
        
        return GetSprite("Kojima_Quieres");
    }

    public void HideDiapositiva()
    {
        FadeInForeground(() =>
        {
            if (audioDiapositiva != null) Destroy(audioDiapositiva);
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
            SetScene(nextPassageKey);
        });
    }

    public void EndGame(int typeDecision = 0)
    {
        Debug.Log(typeDecision);
        PassageNode passage = null; 

        switch (typeDecision)
        {
            case 0: passage = nextPassageKey;
                break;
            case 1: passage = leftKey;
                break;
            case 2: passage = rightKey;
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
        SetScene(nextPassageKey);
    }

    private void SetAnimation(AnimationNode animationNode)
    {
        nextPassageKey = animationNode.exitPassage1 != null ? animationNode.exitPassage1 : firstPassageNode;
        
        animationPanel.transform.parent.gameObject.SetActive(true);

        Instantiate(GetPrefab(animationNode.id), animationPanel.transform).transform.localPosition = Vector3.zero;
        
        FadeOutForeground();
    }

    private GameObject GetPrefab(string namePrefab)
    {
        GameObject[] prefabs = UnityEngine.Resources.LoadAll<GameObject>("Prefabs/Animations");

        foreach (var prefab in prefabs)
            if (prefab.name.Equals(namePrefab))
                return prefab;
        
        foreach (var prefab in prefabs)
            if (prefab.name.Equals(namePrefab+"_"+languageName.ToUpper()))
                return prefab;
        
        foreach (var prefab in prefabs)
            if (prefab.name.Equals(namePrefab+"_EN"))
                return prefab;

        return GetPrefab("PitiAnimation");
    }

    private void SetGame(GameNode gameNode)
    {
        if (gameNode.exitPassage1 != null || gameNode.exitPassage2 != null)
        {
            nextPassageKey = gameNode.exitPassage1;

            if (gameNode.exitPassage1 != null && gameNode.exitPassage2 != null)
            {
                leftKey = gameNode.exitPassage1;
                rightKey = gameNode.exitPassage2;
            }
        }
        else
            nextPassageKey = firstPassageNode;

        MinigamesHandler.instance.StartMinigame(gameNode.id);
        canvasCard.SetActive(false);
        camera.SetActive(false);
        
        FadeOutForeground(() =>
        {
            if (gameNode.exitPassage1 == null && gameNode.exitPassage2 == null)
                EndGame();
        });
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
                SetScene(decision == 1 ? leftKey : rightKey);
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