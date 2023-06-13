using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MobaVR
{
    public class PlayerNetworkInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_PlayerInfo;


        private void OnEnable()
        {
            string text = $"IsMasterClient: {PhotonNetwork.IsMasterClient}; \n" +
                          $"UserId: {PhotonNetwork.LocalPlayer.UserId}; \n" +
                          $"IsLocal: {PhotonNetwork.LocalPlayer.IsLocal}; \n" +
                          $"ActionNumber: {PhotonNetwork.LocalPlayer.ActorNumber}; \n" +
                          $"Players: {PhotonNetwork.CountOfPlayersOnMaster}";

            m_PlayerInfo.text = text;
        }
    }
}