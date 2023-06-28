using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MobaVR;

public class ChangeSkinPlayer : MonoBehaviourPun
{
    //���������� ������ �����
    [Header("����� �����")]
    public int NomerSkins;


    [Header("����� ������")]
    [Tooltip("�����")]
    public GameObject TopKnot;

    [Tooltip("�������")]
    public GameObject Beard;

    [Tooltip("����������")]
    public GameObject Sideburns;

    [Tooltip("���")]
    public GameObject Neck;

    [Tooltip("��������")]
    public GameObject Hair;


    [Header("�����")]
    [Tooltip("�������� ������ ���������� ������")]
    public GameObject[] Skins;
    public GameObject[] OldSkin;

    //public GameSession _GameSession;
    //public ClassicGameSession _ClassicGameSession;
    public BaseGameSession _GameSession;

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


    //������� ����� �����
    public void ChangeSkinNext()
    {
        // ��������� ���������� ����
        Skins[NomerSkins].SetActive(false);

        // ����������� ����� ����� ��� ���������� �� 0, ���� �������� ������ �������
        NomerSkins = (NomerSkins + 1) % Skins.Length;

        // �������� ����� ����
        Skins[NomerSkins].SetActive(true);
        ActiveFase();

        //�������� ����� ���� �������
        _PlayerVR = _GameSession.Player;

        _PlayerVR.GetComponent<ChangeSkinPlayerRemote>().ChangeSkin(NomerSkins);

    }

    public void ChangeSkinDown()
    {
        // ��������� ������� ����
        Skins[NomerSkins].SetActive(false);

        // ��������� ����� ����� ��� ��������� � ����� �������, ���� ����� ���������� ������ 0
        NomerSkins = (NomerSkins - 1 + Skins.Length) % Skins.Length;

        // �������� ����� ���� ��������
        Skins[NomerSkins].SetActive(true);
        ActiveFase();

        //�������� ����� ���� �������
        _PlayerVR = _GameSession.Player;
        _PlayerVR.GetComponent<ChangeSkinPlayerRemote>().ChangeSkin(NomerSkins);

    }

    //������� ��������� �����, ���� ��� ����� ���� ���������
    public void SkinON()
    {
        // �������� ����� ���� ��������
        Skins[NomerSkins].SetActive(true);
        ActiveFase();

    }


    //���������� �������������� �� ����, � ����������� �� �����
    public void ActiveFase()
    {
        //����� ������� ������ �� ���� ������.
        TopKnot.SetActive(NomerSkins == 0);

        //������ ������� ������ �� ���� ������.
        //Beard.SetActive();

        //���������� ������� ������ �� ���� ������.
        Sideburns.SetActive(NomerSkins == 1);

        //��� ������� ������ �� ���� ������.
        Neck.SetActive(NomerSkins == 1 || NomerSkins == 2 || NomerSkins == 4 || NomerSkins == 5);

        //�������� ������� ������ �� ���� ������.
        Hair.SetActive(NomerSkins == 0 || NomerSkins == 3);

        if (NomerSkins > 5)
        {
            for (int i = 1; i < OldSkin.Length; i++)
            {
                OldSkin[i].SetActive(false);
            }
        }
    }

    ////������� ���� ������� �����, ����� ���� ���������
    //private void FindLocalPlayerVR()
    //{

    //    GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("RemotePlayer");

    //    foreach (GameObject playerObject in playerObjects)
    //    {
    //        PhotonView photonView = playerObject.GetComponent<PhotonView>();

    //        if (photonView != null && photonView.IsMine)
    //        {
    //            // ������ ��������� ������ PlayerVR
    //            _PlayerVR = playerObject;
    //            //Debug.Log("������ ��������� �����");
    //            break;
    //        }
    //    }
    //}

    [ContextMenu("ChangeTeam")]
    // ���� ������ �� ������ "����� �������"
    public void ChangeTeam()
    {
        //����� ����� ������ Teammate � ��������� ������� � ��
        _PlayerVR = _GameSession.Player;
        _PlayerVR.GetComponent<PlayerVR>().ChangeTeamOnClick();
    }

    }
