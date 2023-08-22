using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBookLobby : MonoBehaviour
{
    public Text titleText;
    public Text outputText;
    public TextData[] textElements;
    public ZonaBook zonaBook;
    public SaveInfoClass _SaveInfoClass;

    public GameObject VideoKurok;


    public void OnButtonClick(string buttonId)
    {
        VideoKurok.SetActive(false);
        foreach (TextData textData in textElements)
        {
            if (textData.id == buttonId)
            {

                titleText.text = textData.title; // заголовок
                outputText.text = textData.text; // Текст

                //отправляем значение ИД персонажа в скрипт, который визуально меняет его скин на книге
                zonaBook.targetID = textData.id;
                zonaBook.UpdateTargetID(textData.id);
                //сохраняем данные о выбранном классе игрока
                _SaveInfoClass.targetID = textData.id;
                break;
            }
        }
    }


}


[System.Serializable]
public class TextData
{
    public string id;
    public string title;
    [TextArea(10, 10)]
    public string text;
}