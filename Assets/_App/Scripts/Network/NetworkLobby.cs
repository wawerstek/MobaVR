using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class NetworkLobby : MonoBehaviourPunCallbacks
    {
        [Header("Photon")]
        [SerializeField] private string m_SceneName = "Room";
        [SerializeField] private byte m_MaxPlayersPerRoom = 12;
        [SerializeField] private string m_RoomName = "MobaVR";
        [SerializeField] private string m_GameVersion = "1";

        private bool m_IsConnecting = false;

        public UnityEvent OnRoomConnected;
        public UnityEvent OnRoomDisconnected;

        #region Photon

        private void JoinRoom()
        {
            //PhotonNetwork.JoinRandomRoom();

            PhotonNetwork.JoinOrCreateRoom(m_RoomName,
                                           new RoomOptions()
                                           {
                                               MaxPlayers = m_MaxPlayersPerRoom,
                                           },
                                           TypedLobby.Default);
        }

        public void Connect(string username = null)
        {
            m_IsConnecting = true;

            if (PhotonNetwork.IsConnected)
            {
                JoinRoom();
            }
            else
            {
                if (username != null)
                {
                    PhotonNetwork.NickName = username;
                }

                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.OfflineMode = false;
                PhotonNetwork.GameVersion = m_GameVersion;
                //PhotonNetwork.UseRpcMonoBehaviourCache = true;

                PhotonNetwork.ConnectUsingSettings();
            }

            OnRoomConnected?.Invoke();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            if (m_IsConnecting)
            {
                Debug.Log(
                    $"{name}: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
                JoinRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.Log(
                $"{name}: Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(m_RoomName,
                                     new RoomOptions()
                                     {
                                         MaxPlayers = m_MaxPlayersPerRoom,
                                     });
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.LogError($"{name}: Launcher:Disconnected");

            m_IsConnecting = false;
            OnRoomDisconnected?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log(
                $"{name}: Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
            //if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(m_SceneName);
            }
        }

        #endregion
    }
}