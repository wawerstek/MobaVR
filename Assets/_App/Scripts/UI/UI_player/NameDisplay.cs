using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Text.RegularExpressions; // Для регулярных выражений

public class NameDisplay : MonoBehaviourPunCallbacks, IPunObservable
{
    public TMP_Text playerNameText;
    private string playerName;

    private void Update()
    {
        if (photonView.IsMine)
        {
            playerNameText.text = StripNumberFromName(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            playerNameText.text = StripNumberFromName(playerName);
        }
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
        if (stream.IsWriting)
        {
            stream.SendNext(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            playerName = (string)stream.ReceiveNext();
        }
    }
}