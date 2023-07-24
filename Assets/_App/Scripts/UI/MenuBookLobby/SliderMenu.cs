using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderMenu : MonoBehaviour
{
    public GameObject descriptionPanel;
    public Text descriptionText;

    public AudioClip hoverSound;
    public AudioClip clickSound;

    public Button[] characterButtons;

    public string[] descriptions;

    public Vector3[] originalScales;

    private void Start()
    {
        // Находим все кнопки и сохраняем их оригинальные размеры
        //characterButtons = GetComponentsInChildren<Button>();

        originalScales = new Vector3[characterButtons.Length];
        for (int i = 0; i < characterButtons.Length; i++)
        {
            originalScales[i] = characterButtons[i].transform.localScale;
        }
    }

    public void OnCharacterButtonEnter(int buttonIndex)
    {
        // Увеличиваем размер кнопки при наведении
        characterButtons[buttonIndex].transform.localScale = originalScales[buttonIndex] * 1.2f;

        // Воспроизводим звук наведения
        PlaySound(hoverSound);
    }




    public void OnCharacterButtonExit(int buttonIndex)
    {
        // Возвращаем кнопке оригинальный размер
        characterButtons[buttonIndex].transform.localScale = originalScales[buttonIndex];
    }

    public void OnCharacterButtonClick(int buttonIndex)
    {
        // Воспроизводим звук нажатия
        PlaySound(clickSound);

        // Скрываем все описания персонажей и показываем описание выбранного персонажа
        for (int i = 0; i < characterButtons.Length; i++)
        {
            if (i == buttonIndex)
            {
                characterButtons[i].transform.localScale = originalScales[i] * 0.8f;
                descriptionPanel.SetActive(true);
                descriptionText.text = GetCharacterDescription(buttonIndex);
            }
            else
            {
                characterButtons[i].transform.localScale = originalScales[i];
            }
        }
    }



    public void NextClick()
    {
        // Воспроизводим звук нажатия
        PlaySound(clickSound);

      
    }


    public void Red_team_Click()
    {
        // Воспроизводим звук нажатия
        PlaySound(clickSound);


    }

    public void Blue_team_Click()
    {
        // Воспроизводим звук нажатия
        PlaySound(clickSound);


    }



    private string GetCharacterDescription(int buttonIndex)
    {
        // Возвращает описание персонажа по индексу кнопки (здесь может быть ваша логика описания)
       //descriptions[] = {
       //     "Рыцарь: сильный и храбрый воин.",
       //     "Друид: мудрый и могущественный заклинатель.",
       //     "Бог огня: сущность огня и страсти."
       // };

        if (buttonIndex >= 0 && buttonIndex < descriptions.Length)
        {
            return descriptions[buttonIndex];
        }

        return "Описание персонажа недоступно.";
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
