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
    public AudioClip clickSound;

    public void OnButtonClick(string buttonId)
    {
        foreach (TextData textData in textElements)
        {
            if (textData.id == buttonId)
            {
                // Воспроизводим звук нажатия
                PlaySound(clickSound);


                titleText.text = textData.title; // заголовок
                outputText.text = textData.text; // Текст

                //отправляем значение ИД персонажа в скрипт, который визуально меняет его скин на книге
                zonaBook.targetID = textData.id;
                zonaBook.UpdateTargetID(textData.id);
                break;
            }
        }
    }


    private void PlaySound(AudioClip sound)
    {
        // Воспроизведение звука
        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
        }
    }


}


[System.Serializable]
public class TextData
{
    public string id;
    public string title;
    public string text;
}