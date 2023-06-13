using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using MobaVR;


public class ServisUI : MonoBehaviourPunCallbacks
{
    public Text statusLegs; // Ссылка на текстовый компонент для отображения статуса ног
    private bool legsEnabled = true; // Флаг, указывающий, включены ли ноги

    public Text statusBody; // Ссылка на текстовый компонент для отображения статуса тела локального
    private bool bodyEnabled = true; // Флаг, указывающий, включено ли тело


    public GameSession _GameSession;
    [SerializeField] private GameObject _PlayerVR;

    [SerializeField] private Off_legs[] offLegsArray;


    // Если нажали на кнопку "ноги"
    public void Off_Legs()
    {
        // Отправляем сетевое событие для отключения ног всем игрокам
        photonView.RPC("DisableLegs", RpcTarget.All);

    }

    [PunRPC]
    private void DisableLegs()
    {
        // Находим все объекты с компонентом Off_legs
        offLegsArray = FindObjectsOfType<Off_legs>();

        foreach (Off_legs offLegs in offLegsArray)
        {
            offLegs.ToggleObjects(legsEnabled); // Включаем или выключаем ноги
        }

        // Изменяем состояние флага и текста статуса
        legsEnabled = !legsEnabled;
        statusLegs.text = legsEnabled ? "Выключены" : "Включены";
    }

    // Если нажали на кнопку "ТЕЛО"
    public void Off_Body()
    {
        // Отправляем сетевое событие для отключения ног всем игрокам
        photonView.RPC("DisableBody", RpcTarget.All);

    }

    [PunRPC]
    private void DisableBody()
    {
        // Находим объект с компонентом Off_body
        Off_body offBodies = FindObjectOfType<Off_body>();

        offBodies.DisableAllObjects(bodyEnabled); // Включаем или выключаем тело

        // Изменяем состояние флага и текста статуса
        bodyEnabled = !bodyEnabled;
        statusBody.text = bodyEnabled? "Выключено" : "Включено";
    }

    // Если нажали на кнопку "Смена команды"
    public void ChangeTeam()
    {
        //нужно найти скрипт Teammate и выполнить функцию в нём
        _PlayerVR = _GameSession.localPlayer;
        _PlayerVR.GetComponent<PlayerVR>().ChangeTeamOnClick();

    }





}
