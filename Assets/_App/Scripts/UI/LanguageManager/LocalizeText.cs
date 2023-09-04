using UnityEngine;
using UnityEngine.UI;

public class LocalizeText : MonoBehaviour
{
    [TextArea(3, 5)]
    [SerializeField]
    private string textEng;

    [TextArea(3, 5)]
    [SerializeField]
    private string textRus;

    [TextArea(3, 5)]
    [SerializeField]
    private string textChn;

    private Text textField;

    private void Start()
    {
        textField = GetComponent<Text>();
        UpdateText();

        LanguageManager.Instance.LanguageChanged += UpdateText;
    }

    private void OnDestroy()
    {
        LanguageManager.Instance.LanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        switch (LanguageManager.Instance.currentLanguage)
        {
            case LanguageManager.Language.Eng:
                textField.text = textEng;
                break;
            case LanguageManager.Language.Rus:
                textField.text = textRus;
                break;
            case LanguageManager.Language.Chn:
                textField.text = textChn;
                break;
        }
    }
}