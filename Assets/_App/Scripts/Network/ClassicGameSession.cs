using MetaConference;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MobaVR
{
    public class ClassicGameSession : MonoBehaviourPunCallbacks
    {
        [Header("Network")]
        [SerializeField] private NetworkSession m_NetworkSession;

        [Header("Game")]
        [SerializeField] private ClassicMode m_Mode;

        [Header("Player")]
        [SerializeField] private BasePlayerSpawner<PlayerVR> m_PlayerSpawner;
        [SerializeField] private Team m_RedTeam;
        [SerializeField] private Team m_BlueTeam;

        private PlayerVR m_LocalPlayer;

        public PlayerVR LocalPlayer => m_LocalPlayer;
        public Team RedTeam => m_RedTeam;
        public Team BlueTeam => m_BlueTeam;

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
        }

        #endregion

        #region Region Mode

        private void InitMode()
        {
            //m_Mode.InitMode();
            m_Mode.DeactivateMode();
        }

        public void StartMode()
        {
            m_Mode.StartMode();
        }

        #endregion
    }
}