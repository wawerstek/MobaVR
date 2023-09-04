using UnityEngine;
using UnityEngine.UI;

public class FontManager : MonoBehaviour
{

    public bool UserFont;// выбор шрифта не автоматически, нужно использовать, если вдруг нужны где-то другие не стандартные шрифты
    public Font engFont; // Шрифт для английского языка
    public Font rusFont; // Шрифт для русского языка
    public Font chnFont; // Шрифт для китайского языка

    private Text textComponent;

    private void Start()
    {
        textComponent = GetComponent<Text>();

        // Подписываемся на событие изменения языка
        LanguageManager.Instance.LanguageChanged += UpdateFont;
        
        // Начальная установка шрифта
        UpdateFont();
    }

    private void UpdateFont()
    {
        //если у нас не стандартный шрифт
        if (UserFont)
        {
            // Проверяем текущий язык и применяем соответствующий шрифт
            switch (LanguageManager.Instance.currentLanguage)
            {
                case LanguageManager.Language.Eng:
                    textComponent.font = engFont;
                    break;
                case LanguageManager.Language.Rus:
                    textComponent.font = rusFont;
                    break;
                case LanguageManager.Language.Chn:
                    textComponent.font = chnFont;
                    break;
                default:
                    break;
            }
        }
        else if (!UserFont)
        {
            // Проверяем текущий язык и применяем соответствующий шрифт из FontData
            switch (LanguageManager.Instance.currentLanguage)
            {
                case LanguageManager.Language.Eng:
                    textComponent.font = FontData.Instance.engFont;
                    break;
                case LanguageManager.Language.Rus:
                    textComponent.font = FontData.Instance.rusFont;
                    break;
                case LanguageManager.Language.Chn:
                    textComponent.font = FontData.Instance.chnFont;
                    break;
                default:
                    break;
            }
        }
        
        
        
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        LanguageManager.Instance.LanguageChanged -= UpdateFont;
    }
}