using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XNode;

public class NewSceneController : MonoBehaviour
{
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

    public Image imageDiapositiva;
    
    public Image imageForeground;
    
    public GameObject animationPanel;

    public TextMeshProUGUI textCard;
    public TextMeshProUGUI textLeft;
    public TextMeshProUGUI textRight;

    private Color originalColorPanel;

    private int panelPaintColor = 0;

    private bool returningCenter = false;

    private Passage currentPassage;

    private PassageNode leftKey;
    private PassageNode rightKey;

    private PassageNode nextPassageKey;

    private bool leftRight;

    private GameObject camera;
    private GameObject audioDiapositiva;

    public string[] idiomas;
    public string languageName;
    
    private HistoryCreator historyCreator;
    private PassageNode firstPassageNode;

    private void Awake()
    {
        CultureInfo culture = CultureInfo.CurrentCulture;

        languageName = culture.TwoLetterISOLanguageName;
        
        if (PlayerPrefs.HasKey("CurrentIdiom"))
            languageName = PlayerPrefs.GetString("CurrentIdiom");
        
        //historyCreator = UnityEngine.Resources.Load<HistoryCreator>("HistoryCreator/Historias/" + currentHistoria);
        
        currentHistoria = CurrentHistoryController.instance.currectHistory.name;
        historyCreator = CurrentHistoryController.instance.currectHistory;
        
        instance = this;
        
        rtParent = cardObj.transform.parent.GetComponent<RectTransform>();
        
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

        StoryNode storyNode = null;
        
        for (int i = 0; i < historyCreator.nodes.Count; i++)
        {
            if (historyCreator.nodes[i].name.Equals("Story"))
            {
                storyNode = historyCreator.nodes[i] as StoryNode;
                break;
            }
        }

        int idStartPassage = -1;
        PassageNode passageNodeStart = storyNode.nodeTest;

        if (passageNodeStart == null && PlayerPrefs.HasKey("PassageSaved_" + currentHistoria))
        {
            idStartPassage = PlayerPrefs.GetInt("PassageSaved_"+currentHistoria);
        }
        else if (passageNodeStart == null)
        {
            idStartPassage = -1;
            passageNodeStart = firstPassageNode;
        }

        if (idStartPassage != -1)
        {
            for (int i = 0; i < historyCreator.nodes.Count; i++)
            {
                PassageNode node = historyCreator.nodes[i] as PassageNode;
                
                if (node == null) continue;
                
                int idNode = GetNodeId(historyCreator.nodes[i]);
                
                Debug.Log(idNode);
                
                if (idNode == idStartPassage) passageNodeStart = node;
            }
        }

        Debug.Log(passageNodeStart.name);
        
        SetScene(passageNodeStart);
    }

    private int GetNodeId(Node node)
    {
        for (int i = 0; i < historyCreator.nodesCreated.Count; i++)
            if (historyCreator.nodesCreated[i].Equals(node)) return i;

        return -1;
    }

    private void Update()
    {
        ChangeColorPanel();
    }

    public void SetScene(PassageNode passage)
    {
        SaveData(passage);
        
        PlayAudio(passage.audio);
        
        if (passage is DecisionNode)
        {
            SetCard(passage as DecisionNode);
        } else if (passage is CalendarNode)
        {
            SetCalendar(passage as CalendarNode);
        } else if (passage is AnimationNode)
        {
            SetAnimation(passage as AnimationNode);
        } else if (passage is DiapositiveNode)
        {
            SetDiapositiva(passage as DiapositiveNode);
        } else if (passage is GameNode)
        {
            SetGame(passage as GameNode);
        } else if (passage is EndStoryNode)
        {
            EndHistoria();
            VolverMenuInicio();
        }
    }

    private async void PlayAudio(DecisionNode.PlayAudio audio)
    {
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
    }

    private void SaveData(PassageNode passage)
    {
        int idNode = GetNodeId(passage);
        
        PlayerPrefs.SetInt("PassageSaved_"+currentHistoria, idNode);
        
        string recorrido = "";
        if (PlayerPrefs.HasKey("RecorridoTomado_" + currentHistoria))
            recorrido = PlayerPrefs.GetString("RecorridoTomado_" + currentHistoria);
        else return;

        string[] recorridoSplit = recorrido.Split("\n");

        for (int i = 0; i < recorridoSplit.Length; i++)
            Debug.Log(recorridoSplit[i]);

        if (recorridoSplit[recorridoSplit.Length - 1].Equals("- " + idNode)) return;

        recorrido += "- " + passage.name + "\n";
        
        PlayerPrefs.SetString("RecorridoTomado_"+currentHistoria, recorrido);
    }

    private void EndHistoria()
    {
        DateTime now = DateTime.Now;
        
        string recorridoTxt = "RecorridoTomado_" + SystemInfo.deviceName + "_" + currentHistoria + "_" +
                              now.ToString("yyyy-MM-dd_HH:mm:ss");
        
        FirebaseStorageController.instance.UploadFile(
            Encoding.UTF8.GetBytes(PlayerPrefs.GetString("RecorridoTomado_" + currentHistoria)), recorridoTxt);
                
        PlayerPrefs.DeleteKey("RecorridoTomado_"+currentHistoria);
        PlayerPrefs.DeleteKey("PassageSaved_"+currentHistoria);
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
        nextPassageKey = calendarNode.decisionIzquierda != null ? calendarNode.decisionIzquierda : firstPassageNode;
        
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
        
        textCard.text = GetText(decisionNode);
        textLeft.text = GetDecision(decisionNode, false);
        textRight.text = GetDecision(decisionNode, true);;
        
        leftKey = decisionNode.decisionIzquierda;
        rightKey = decisionNode.decisionDerecha;
        
        FadeOutForeground();
        
        cardObj.transform.parent.parent.localScale = Vector3.one;
    }

    private string GetDecision(DecisionNode decisionNode, bool direction)
    {
        switch (languageName)
        {
            case "es":
                return direction
                    ? decisionNode.card.decisionesEs.decisionDerecha
                    : decisionNode.card.decisionesEs.decisionIzquierda;
            
            case "en":
                return direction
                    ? decisionNode.card.decisionesEn.decisionDerecha
                    : decisionNode.card.decisionesEn.decisionIzquierda;
        }

        return "";
    }

    private string GetText(DecisionNode decisionNode)
    {
        switch (languageName)
        {
            case "es":
                return decisionNode.card.textos.textoES;
            
            case "en":
                return decisionNode.card.textos.textoEN;
        }

        return null;
    }

    private void SetDiapositiva(DiapositiveNode diapositiveNode)
    {
        FadeOutForeground();
        
        imageDiapositiva.transform.parent.gameObject.SetActive(true);

        Sprite diapositiva = diapositiveNode.diapositivaEN;

        if (languageName.Equals("es") && diapositiveNode.diapositivaES != null)
            diapositiva = diapositiveNode.diapositivaES;

        imageDiapositiva.sprite = diapositiva != null
            ? diapositiva
            : GetSprite("Kojima_Quieres");

        nextPassageKey = diapositiveNode.decisionIzquierda != null ? diapositiveNode.decisionIzquierda : firstPassageNode;
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
        nextPassageKey = animationNode.decisionIzquierda != null ? animationNode.decisionIzquierda : firstPassageNode;
        
        animationPanel.transform.parent.gameObject.SetActive(true);
        
        GameObject animation = animationNode.animacionEN;

        if (languageName.Equals("es") && animationNode.animacionES != null)
            animation = animationNode.animacionES;

        Instantiate(animation != null ? animation : GetPrefab("PitiAnimation"),
            animationPanel.transform).transform.localPosition = Vector3.zero;
        
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
        if (gameNode.decisionIzquierda != null || gameNode.decisionDerecha != null)
        {
            if (gameNode.decisionIzquierda != null && gameNode.decisionDerecha != null)
            {
                leftKey = gameNode.decisionIzquierda;
                rightKey = gameNode.decisionDerecha;
            }
            else
            {
                nextPassageKey = gameNode.decisionIzquierda;

                if (gameNode.prefabGame == null)
                {
                    EndGame();
                    return;
                }
            }
        }
        else
            nextPassageKey = firstPassageNode;

        MinigamesHandler.instance.StartMinigame(gameNode.prefabGame);
        canvasCard.SetActive(false);
        camera.SetActive(false);
        
        FadeOutForeground(() =>
        {
            if (gameNode.decisionIzquierda == null && gameNode.decisionDerecha == null)
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