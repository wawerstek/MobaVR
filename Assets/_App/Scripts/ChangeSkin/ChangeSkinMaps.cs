using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkinMaps : MonoBehaviour
{

    //если нажали на кнопку назад, то мы говорим скрипту ChangeSkinPlayer сделать скин -1
    public void Down()
    {
        // Находим объект с компонентом ChangeSkinPlayer
        GameObject player = GameObject.Find("ChangeSkinPlayer");
        ChangeSkinPlayer script = player.GetComponent<ChangeSkinPlayer>();

        //функция смены скина
        script.ChangeSkinDown();
    }

    //если нажали на кнопку вперёд, то мы говорим скрипту ChangeSkinPlayer сделать скин +1
    public void Next()
    {
        // Находим объект с компонентом ChangeSkinPlayer
        GameObject player = GameObject.Find("ChangeSkinPlayer");
        ChangeSkinPlayer script = player.GetComponent<ChangeSkinPlayer>();

        //функция смены скина
        script.ChangeSkinNext();
    }






}
