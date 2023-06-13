using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TransformShitLocal : MonoBehaviourPun
{
    //перенос щитов в родительские объекты локального игрока. в сетевом они остаются там, где были.

    public GameObject LeftShit;
    public GameObject RightShit;

    private void Start()
    {
        if (!photonView.IsMine) return;

        FindAndTransferObjects();
    }

    private void FindAndTransferObjects()
    {
        //ищем объект рук, они  в локальном игроке должны быть
        GameObject leftShitLocalObj = GameObject.Find("LeftShitLocal");
        GameObject rightShitLocalObj = GameObject.Find("RightShitLocal");

        if (leftShitLocalObj != null)
        {
            TransferObject(LeftShit, leftShitLocalObj);
        }

        if (rightShitLocalObj != null)
        {
            TransferObject(RightShit, rightShitLocalObj);
        }
    }

    //перемещаем туда щиты, чтобы они привязывались к руке
    private void TransferObject(GameObject obj, GameObject parentObj)
    {
        obj.transform.parent = parentObj.transform;
        obj.transform.localPosition = Vector3.zero;
    }
}
