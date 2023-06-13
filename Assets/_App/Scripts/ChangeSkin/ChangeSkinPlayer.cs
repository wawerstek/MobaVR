using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MobaVR;

public class ChangeSkinPlayer : MonoBehaviourPun
{
    //переменная номера скина
    [Header("Номер скина")]
    public int NomerSkins;


    [Header("Части головы")]
    [Tooltip("Хвост")]
    public GameObject TopKnot;

    [Tooltip("Бородка")]
    public GameObject Beard;

    [Tooltip("Бакенбарды")]
    public GameObject Sideburns;

    [Tooltip("Шея")]
    public GameObject Neck;

    [Tooltip("Причёска")]
    public GameObject Hair;


    [Header("Скины")]
    [Tooltip("Добавить нужное количество скинов")]
    public GameObject[] Skins;

    public GameSession _GameSession;

    [SerializeField] private GameObject _PlayerVR;


    // Start is called before the first frame update
    void Start()
    {
       // FindLocalPlayerVR();
        NomerSkins = 0;
        DisableAllSkinsExceptFirst();
    }

    public void DisableAllSkinsExceptFirst()
    {
        for (int i = 1; i < Skins.Length; i++)
        {
            Skins[i].SetActive(false);
        }
    }


    //функция смены скина
    public void ChangeSkinNext()
    {
        // Отключаем предыдущий скин
        Skins[NomerSkins].SetActive(false);

        // Увеличиваем номер скина или сбрасываем на 0, если превышен размер массива
        NomerSkins = (NomerSkins + 1) % Skins.Length;

        // Включаем новый скин
        Skins[NomerSkins].SetActive(true);
        ActiveFase();

        //включаем новый скин сетевой
        _PlayerVR = _GameSession.localPlayer;

        _PlayerVR.GetComponent<ChangeSkinPlayerRemote>().ChangeSkin(NomerSkins);

    }

    public void ChangeSkinDown()
    {
        // Отключаем текущий скин
        Skins[NomerSkins].SetActive(false);

        // Уменьшаем номер скина или переходим к концу массива, если номер становится меньше 0
        NomerSkins = (NomerSkins - 1 + Skins.Length) % Skins.Length;

        // Включаем новый скин локально
        Skins[NomerSkins].SetActive(true);
        ActiveFase();

        //включаем новый скин сетевой
        _PlayerVR = _GameSession.localPlayer;
        _PlayerVR.GetComponent<ChangeSkinPlayerRemote>().ChangeSkin(NomerSkins);

    }

    //функция включения скина, если все скины были выключены
    public void SkinON()
    {
        // Включаем новый скин локально
        Skins[NomerSkins].SetActive(true);
        ActiveFase();

    }


    //активируем растительность на лице, в зависимости от скина
    public void ActiveFase()
    {
        //Хвост активен только на этих скинах.
        TopKnot.SetActive(NomerSkins == 0);

        //Борода активна только на этих скинах.
        //Beard.SetActive();

        //Бакенбарды активны только на этих скинах.
        Sideburns.SetActive(NomerSkins == 1);

        //Шея активна только на этих скинах.
        Neck.SetActive(NomerSkins == 1 || NomerSkins == 2 || NomerSkins == 4 || NomerSkins == 5);

        //Причёска активна только на этих скинах.
        Hair.SetActive(NomerSkins == 0 || NomerSkins == 3);
    }

    ////находим нашу сетевую копию, среди всех остальных
    //private void FindLocalPlayerVR()
    //{

    //    GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("RemotePlayer");

    //    foreach (GameObject playerObject in playerObjects)
    //    {
    //        PhotonView photonView = playerObject.GetComponent<PhotonView>();

    //        if (photonView != null && photonView.IsMine)
    //        {
    //            // Найден локальный объект PlayerVR
    //            _PlayerVR = playerObject;
    //            //Debug.Log("Найден локальный игрок");
    //            break;
    //        }
    //    }
    //}


    // Если нажали на кнопку "Смена команды"
    public void ChangeTeam()
    {
        //нужно найти скрипт Teammate и выполнить функцию в нём
        _PlayerVR = _GameSession.localPlayer;
        _PlayerVR.GetComponent<PlayerVR>().ChangeTeamOnClick();
    }

    }
