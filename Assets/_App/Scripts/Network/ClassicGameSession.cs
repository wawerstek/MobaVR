using System;
using MetaConference;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobaVR
{
    public class ClassicGameSession : BaseGameSession
    {
        [Header("Network")]
        [SerializeField] private NetworkSession m_NetworkSession;

        [Header("Modes")]
        [FormerlySerializedAs("m_Mode")]
        [SerializeField] private ClassicMode m_ClassicMode;
        [SerializeField] private LichGame m_LichMode;
        [SerializeField] private Environment m_Environment;
        [SerializeField] private ZoneManager m_ZoneManager;

        [Header("Player")]
        [SerializeField] private BasePlayerSpawner<PlayerVR> m_PlayerSpawner;
        [SerializeField] private Team m_RedTeam;
        [SerializeField] private Team m_BlueTeam;


        private bool m_IsPvPMode = true;
        //private PlayerVR m_LocalPlayer;

        //public PlayerVR LocalPlayer => m_LocalPlayer;
        public Team RedTeam => m_RedTeam;
        public Team BlueTeam => m_BlueTeam;
        public bool IsPvPMode => m_IsPvPMode;
        public ZoneManager ZoneManager => m_ZoneManager;

        private void Awake()
        {
            //TODO: 
            PhotonCustomHitData.Register();
            #if !UNITY_EDITOR
                CustomComposites.Init();
            #endif
        }

        private void Start()
        {
            //InitPlayer();
            //InitMode();
            Invoke(nameof(InitPlayer), 2f);
            //Invoke(nameof(InitMode), 3f);
        }

        #region Player and Team

        private void InitPlayer()
        {
            //if (!photonView.IsMine)
            if (m_LocalPlayer != null)
            {
                return;
            }

            Team team = m_BlueTeam.Players.Count > m_RedTeam.Players.Count ? m_RedTeam : m_BlueTeam;
            m_LocalPlayer = m_PlayerSpawner.SpawnPlayer(team);
            m_Player = m_LocalPlayer.gameObject;
            team.AddPlayer(m_LocalPlayer);
        }

        public void SwitchTeam()
        {
            if (m_LocalPlayer == null)
            {
                return;
            }

            if (m_LocalPlayer.Team.TeamType == TeamType.RED)
            {
                m_RedTeam.RemovePlayer(m_LocalPlayer);
                m_BlueTeam.AddPlayer(m_LocalPlayer);

                m_LocalPlayer.SetTeam(m_BlueTeam);
            }
            else
            {
                m_BlueTeam.RemovePlayer(m_LocalPlayer);
                m_RedTeam.AddPlayer(m_LocalPlayer);

                m_LocalPlayer.SetTeam(m_RedTeam);
            }

            //m_LocalPlayer.ChangeTeamOnClick();
        }

        public void SetTeam(TeamType teamType)
        {
            if (m_LocalPlayer == null)
            {
                return;
            }

            if (teamType == TeamType.RED)
            {
                m_BlueTeam.RemovePlayer(m_LocalPlayer);
                m_RedTeam.AddPlayer(m_LocalPlayer);

                m_LocalPlayer.SetTeam(m_RedTeam);
            }
            else
            {
                m_RedTeam.RemovePlayer(m_LocalPlayer);
                m_BlueTeam.AddPlayer(m_LocalPlayer);

                m_LocalPlayer.SetTeam(m_BlueTeam);
            }
        }

        [ContextMenu("Set Red Team")]
        public void SetRedTeam()
        {
            SetTeam(TeamType.RED);
        }


        [ContextMenu("Set Blue Team")]
        public void SetBlueTeam()
        {
            SetTeam(TeamType.BLUE);
        }

        public void SwitchRole(string idClass)
        {
            if (m_LocalPlayer.TryGetComponent(out ClassSwitcher classSwitcher))
            {
                classSwitcher.SetRole(idClass);
            }
        }

        #endregion

        #region Base Mode

        private void InitMode()
        {
            //m_ClassicMode.InitMode();
            m_ClassicMode.DeactivateMode();
        }

        public void StartMode()
        {
            m_ClassicMode.StartMode();
        }

        #endregion

        #region PvP Mode

        public void StartPvPMode()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (!m_IsPvPMode)
            {
                m_LichMode.StopGame();
                m_Environment.ShowDefaultPvPMap();
            }

            m_IsPvPMode = true;
            m_ClassicMode.StartMode();
        }

        public void CompletePvPMode()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            m_ClassicMode.CompleteMode();
        }

        public void DeactivatePvPMode()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            m_ClassicMode.DeactivateMode();
        }

        #endregion

        #region PvE Mode

        public void StartPvEMode()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (m_IsPvPMode)
            {
                m_ClassicMode.DeactivateMode();
                m_Environment.ShowDefaultPvEMap();
            }

            m_IsPvPMode = false;
            m_LichMode.StartGame();
        }

        public void CompletePvEMode()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            m_LichMode.StopGame();
        }

        private void ResetModes()
        {
            m_LichMode.StopGame();
            m_ClassicMode.DeactivateMode();
            m_Environment.ShowDefaultPvPMap();
            m_IsPvPMode = true;
        }

        #endregion

        #region Photon

        public void SetMaster()
        {
            //ResetModes();
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            if (PhotonNetwork.IsMasterClient)
            {
                ResetModes();
            }

            /*
            if (m_LocalPlayer.Team.TeamType == TeamType.RED)
            {
                m_RedTeam.RemovePlayer(m_LocalPlayer);
            }
            else
            {
                m_BlueTeam.RemovePlayer(m_LocalPlayer);
            }
            */
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            if (otherPlayer.IsMasterClient)
            {
                //ResetModes();
            }
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            ResetModes();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            //InitPlayer();
            //InitMode();
        }


        public override void OnConnected()
        {
            base.OnConnected();
        }

        #endregion
    }
}