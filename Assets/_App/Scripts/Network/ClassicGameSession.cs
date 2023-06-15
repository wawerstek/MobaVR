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

        private void Start()
        {
            InitPlayer();
            InitMode();
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

        public void SetRedTeam()
        {
            SetTeam(TeamType.RED);
        }

        public void SetBlueTeam()
        {
            SetTeam(TeamType.BLUE);
        }

        #endregion

        #region Region Mode

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

        public void StartPvPMode()
        {
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
            m_ClassicMode.CompleteMode();
        }

        public void DeactivatePvPMode()
        {
            m_ClassicMode.DeactivateMode();
        }

        public void StartPvEMode()
        {
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
            m_LichMode.StopGame();
        }

        public void SetMaster()
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
    }
}