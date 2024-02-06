using Resources.Scripts.Miguel;
using TMPro;
using UnityEngine;

public class ChangeTextIdiom : MonoBehaviour
{
    public string id = "";

    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        string currentHistoria = "Historia1";
        if (PlayerPrefs.HasKey("CurrentHistoria"))
            currentHistoria = PlayerPrefs.GetString("CurrentHistoria");
        
        string idiom = "es";
        if (PlayerPrefs.HasKey("CurrentIdiom")) 
            idiom = PlayerPrefs.GetString("CurrentIdiom");

        TextsUI texts = new TextsUI();

        JsonUtility.FromJsonOverwrite(
            UnityEngine.Resources
                .Load<TextAsset>("JSON/" + idiom + "/" + currentHistoria + "/TextsUI").text,
            texts);

        foreach (var text in texts.texts)
        {
            if (!text.id.Equals(id)) continue;
                
            textMesh.text = text.text;
            return;
        }

        textMesh.text = "Text Not Found";
    }
}
