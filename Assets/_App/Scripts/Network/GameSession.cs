using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using MetaConference;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MobaVR
{
    public class GameSession : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public const byte CustomManualInstantiationEventCode = 101;

        [Header("Network")]
        [SerializeField] private NetworkSession m_NetworkSession;
        [SerializeField] private InputVR m_InputVR;

        [Space]
        [Header("Players")]
        [SerializeField] private PlayerVR m_RedPlayer;
        [SerializeField] private PlayerVR m_BluePlayer;
        [SerializeField] private Transform m_Position1;
        [SerializeField] private Transform m_Position2;

        private Player m_LocalPlayer;

        //public Player LocalPlayer => m_LocalPlayer;
        public Player LocalPlayer => photonView.Owner;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            InitPlayer();
        }

        private void InitPlayer()
        {
            if (m_LocalPlayer != null)
            {
                return;
            }

            /*
            string prefabInputName = $"Players/InputVR_1";
            GameObject input = PhotonNetwork.Instantiate(prefabInputName, Vector3.zero, Quaternion.identity);
            if (input.TryGetComponent(out InputVR inputVR))
            {
                string prefabName = $"Players/PlayerVR_1";
                GameObject localPlayer = PhotonNetwork.Instantiate(prefabName, m_InitPosition, Quaternion.identity);
                localPlayer.name += "_" + Random.Range(1, 1000);
                if (localPlayer.TryGetComponent(out PlayerVR playerVR))
                {
                    playerVR.SetLocalPlayer(inputVR);
                }
            }
            */

            string prefabName = $"Players/{m_BluePlayer.name}";
            Vector3 position;
            TeamType teamType = PhotonNetwork.CurrentRoom.PlayerCount % 2 == 1 ? TeamType.RED : TeamType.BLUE;

            //GameObject localPlayer = PhotonNetwork.Instantiate(prefabName, position, Quaternion.identity);
            
            GameObject localPlayer = PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);
            localPlayer.name += "_" + Random.Range(1, 1000);
            if (localPlayer.TryGetComponent(out PlayerVR playerVR))
            {
                playerVR.SetTeam(teamType);
                playerVR.SetLocalPlayer(m_InputVR);
            }

            /*
            PlayerVR localPlayer = Instantiate(m_BluePlayer, Vector3.zero, Quaternion.identity);
            PhotonView playerPhotonView = localPlayer.GetComponent<PhotonView>();
            localPlayer.name += "_" + Random.Range(1, 1000);

            //localPlayer.SetTeam(teamType);
            localPlayer.SetLocalPlayer(m_InputVR);

            if (PhotonNetwork.AllocateViewID(playerPhotonView))
            {
                object[] data = new object[]
                {
                    teamType, 
                    localPlayer.transform.position,
                    localPlayer.transform.rotation,
                    playerPhotonView.ViewID
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    //Receivers = ReceiverGroup.Others,
                    Receivers = ReceiverGroup.All,
                    CachingOption = EventCaching.AddToRoomCache
                };

                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };

                PhotonNetwork.RaiseEvent(CustomManualInstantiationEventCode, data, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewId.");
                Destroy(localPlayer);
            }
            */
        }

        public override void OnEnable()
        {
            //PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            //PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == CustomManualInstantiationEventCode)
            {
                object[] data = (object[])photonEvent.CustomData;

                PlayerVR player = Instantiate(m_BluePlayer, Vector3.zero, Quaternion.identity);
                PhotonView playerPhotonView = player.GetComponent<PhotonView>();
                playerPhotonView.ViewID = (int)data[3];
                player.SetTeam((TeamType)data[0]);
            }
        }
    }
}