using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeSkinPlayerRemote : MonoBehaviourPun
{

    [Header("Номер скина")]
    public int NomerSkins;
    public int OldNomerSkins;
    PhotonView PV;


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




    // Start is called before the first frame update
    void Start()
    {
        NomerSkins = 0;
        PV = GetComponent<PhotonView>();
        PV.RPC("DisableAllSkinsExceptFirst", RpcTarget.AllBuffered);
    }



    [PunRPC]
    public void DisableAllSkinsExceptFirst()
    {
        for (int i = 1; i < Skins.Length; i++)
        {
            Skins[i].SetActive(false);
        }
    }




    public void ChangeSkin(int value)
    {
        //OldNomerSkins = NomerSkins;
        //NomerSkins = value;
        PV.RPC("RPC_ChangeSkin", RpcTarget.AllBuffered, value);
    }

    [PunRPC]
    public void RPC_ChangeSkin(int value)
    {
        Debug.Log("Скин сменился на " + NomerSkins);
        // Отключаем текущий скин
        Skins[NomerSkins].SetActive(false);

        NomerSkins = value;
        // Включаем новый скин локально
        Skins[NomerSkins].SetActive(true);
        ActiveFase();
    }

    //активируем растительность на лице, в зависимости от скина
    public void ActiveFase()
    {
        Debug.Log("Включилась борода ");

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





}
