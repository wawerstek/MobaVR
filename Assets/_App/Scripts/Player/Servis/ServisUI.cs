using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using MobaVR;


public class ServisUI : MonoBehaviourPunCallbacks
{
    public Text statusLegs; // ������ �� ��������� ��������� ��� ����������� ������� ���
    private bool legsEnabled = true; // ����, �����������, �������� �� ����

    public Text statusBody; // ������ �� ��������� ��������� ��� ����������� ������� ���� ����������
    private bool bodyEnabled = true; // ����, �����������, �������� �� ����


    public BaseGameSession _GameSession;
    [SerializeField] private GameObject _PlayerVR;

    [SerializeField] private Off_legs[] offLegsArray;

    private void Start()
    {
        if (photonView.IsMine)
        {
            HideLegs();
        }
    }


    // ���� ������ �� ������ "����"
    public void Off_Legs()
    {
        // ���������� ������� ������� ��� ���������� ��� ���� �������
        photonView.RPC("DisableLegs", RpcTarget.All);
        //photonView.RPC(nameof(SetVisibleLegs), RpcTarget.All, false);
    }

    public void ShowLegs()
    {
        photonView.RPC(nameof(SetVisibleLegs), RpcTarget.All, true);
    }

    public void HideLegs()
    {
        photonView.RPC(nameof(SetVisibleLegs), RpcTarget.All, false);
    }

    public void On_Legs()
    {
        photonView.RPC(nameof(SetVisibleLegs), RpcTarget.All, true);
    }

    [PunRPC]
    private void SetVisibleLegs(bool isVisible)
    {
        offLegsArray = FindObjectsOfType<Off_legs>();

        foreach (Off_legs offLegs in offLegsArray)
        {
            offLegs.ToggleObjects(isVisible);
        }
    }

    [PunRPC]
    private void DisableLegs()
    {
        // ������� ��� ������� � ����������� Off_legs
        offLegsArray = FindObjectsOfType<Off_legs>();

        foreach (Off_legs offLegs in offLegsArray)
        {
            offLegs.ToggleObjects(legsEnabled); // �������� ��� ��������� ����
        }

        // �������� ��������� ����� � ������ �������
        legsEnabled = !legsEnabled;
        statusLegs.text = legsEnabled ? "���������" : "��������";
    }

    // ���� ������ �� ������ "����"
    public void Off_Body()
    {
        // ���������� ������� ������� ��� ���������� ��� ���� �������
        photonView.RPC("DisableBody", RpcTarget.All);
    }

    public void ShowBody()
    {
        photonView.RPC(nameof(SetVisibleBody), RpcTarget.All, true);
    }

    public void HideBody()
    {
        photonView.RPC(nameof(SetVisibleBody), RpcTarget.All, false);
    }

    [PunRPC]
    private void SetVisibleBody(bool isVisible)
    {
        Off_body[] offBodies = FindObjectsOfType<Off_body>();

        foreach (Off_body offLegs in offBodies)
        {
            offLegs.DisableAllObjects(isVisible);
        }
    }

    [PunRPC]
    private void DisableBody()
    {
        // ������� ������ � ����������� Off_body
        Off_body offBodies = FindObjectOfType<Off_body>();

        offBodies.DisableAllObjects(bodyEnabled); // �������� ��� ��������� ����

        // �������� ��������� ����� � ������ �������
        bodyEnabled = !bodyEnabled;
        statusBody.text = bodyEnabled ? "���������" : "��������";
    }

    // ���� ������ �� ������ "����� �������"
    public void ChangeTeam()
    {
        //����� ����� ������ Teammate � ��������� ������� � ��
        _PlayerVR = _GameSession.Player;
        _PlayerVR.GetComponent<PlayerVR>().ChangeTeamOnClick();
    }
}