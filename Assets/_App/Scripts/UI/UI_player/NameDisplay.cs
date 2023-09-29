using System;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Text.RegularExpressions;
using MobaVR; // Для регулярных выражений

public class NameDisplay : MonoBehaviourPunCallbacks
                         , IPunObservable
{
    public TextMeshProUGUI playerNameText;
    private string playerName;
    private PlayerVR playerVR;

    public override void OnEnable()
    {
        base.OnEnable();
        if (playerVR != null)
        {
            playerVR.OnNickNameChange += OnNicknameChange;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (playerVR != null)
        {
            playerVR.OnNickNameChange -= OnNicknameChange;
        }
    }

    private void Awake()
    {
        playerVR = GetComponentInParent<PlayerVR>();
    }
    
    private void OnNicknameChange(string nickName)
    {
        playerNameText.text = nickName;
    }

    public void SetName(string nickName)
    {
        nickName = StripNumberFromName(nickName);
        PhotonNetwork.LocalPlayer.NickName = nickName;
        
        photonView.RPC(nameof(RpcSetNickname), RpcTarget.AllBuffered, nickName);
    }

    [PunRPC]
    private void RpcSetNickname(string nickName)
    {
        if (playerVR != null)
        {
            playerVR.SetNickName(nickName);
        }
        
        playerNameText.text = nickName;
    }
    
    private void Update()
    {
        /*
        if (photonView.IsMine)
        {
            playerNameText.text = StripNumberFromName(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            playerNameText.text = StripNumberFromName(playerName);
        }
        */
    }

    private string StripNumberFromName(string fullName)
    {
        // Проверка на соответствие регулярному выражению (имя + 4-значное число)
        if (!Regex.IsMatch(fullName, @".+\d{4}$"))
        {
            return ""; // Если не соответствует, вернуть пустую строку
        }

        // Если соответствует, убираем последние 4 символа (наши рандомные числа)
        return fullName.Substring(0, fullName.Length - 4);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if (stream.IsWriting)
        {
            stream.SendNext(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            playerName = (string)stream.ReceiveNext();
        }
        */
    }
}