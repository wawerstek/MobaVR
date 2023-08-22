using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderMenu : MonoBehaviour
{

    public AudioClip hoverSound;
    public AudioClip clickSound;

    public Button[] characterButtons;

    public Vector3[] originalScales;


    public GameObject MenuClass;
    public GameObject MenuName;
    public GameObject MenuHands;
    public GameObject MenuGoTir;

    public Text HandsLeftRight;
   

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
    

    }



    public void NextClick()
    {
        
        PlaySound(clickSound);
        MenuClass.SetActive(false);
        MenuName.SetActive(true);
        MenuHands.SetActive(false);
        MenuGoTir.SetActive(false);
       
    }


    public void NextClickName()
    {
        //Тут можно добавить функцию присвоения имени игроку
        PlaySound(clickSound);
        MenuClass.SetActive(false);
        MenuName.SetActive(false);
        MenuHands.SetActive(true);
        MenuGoTir.SetActive(false);
    }

    public void NextClickHands()
    {
        
        PlaySound(clickSound);
        MenuClass.SetActive(false);
        MenuName.SetActive(false);
        MenuHands.SetActive(false);
        MenuGoTir.SetActive(true);
    }  
    
    
    public void RessetMenu()
    {
        
        PlaySound(clickSound);
        MenuClass.SetActive(true);
        MenuName.SetActive(false);
        MenuHands.SetActive(false);
        MenuGoTir.SetActive(false);
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


    public void LeftHands()
    {

        PlaySound(clickSound);
        HandsLeftRight.text = "Ты Левша";

    }   
    
    public void RightHands()
    {

        PlaySound(clickSound);
        HandsLeftRight.text = "Ты Правша";

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
