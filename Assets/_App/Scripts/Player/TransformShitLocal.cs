using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TransformShitLocal : MonoBehaviourPun
{
    //������� ����� � ������������ ������� ���������� ������. � ������� ��� �������� ���, ��� ����.

    public GameObject LeftShit;
    public GameObject RightShit;

    private void Start()
    {
        if (!photonView.IsMine) return;

        FindAndTransferObjects();
    }

    private void FindAndTransferObjects()
    {
        //���� ������ ���, ���  � ��������� ������ ������ ����
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

    //���������� ���� ����, ����� ��� ������������� � ����
    private void TransferObject(GameObject obj, GameObject parentObj)
    {
        obj.transform.parent = parentObj.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
    }
}
