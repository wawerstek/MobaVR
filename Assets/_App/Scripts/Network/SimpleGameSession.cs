using MetaConference;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobaVR
{
    public class SimpleGameSession : BaseGameSession
    {
        [Header("Network")]
        [SerializeField] private NetworkSession m_NetworkSession;

        [Header("Player")]
        [SerializeField] private BasePlayerSpawner<PlayerVR> m_PlayerSpawner;
        [SerializeField] private Team m_RedTeam;
        [SerializeField] private Team m_BlueTeam;


        //public PlayerVR LocalPlayer => m_LocalPlayer;
        public Team RedTeam => m_RedTeam;
        public Team BlueTeam => m_BlueTeam;

        private void Start()
        {
            Invoke(nameof(InitPlayer), 2f);
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

                m_LocalPlayer.SetTeamAndSync(m_BlueTeam);
            }
            else
            {
                m_BlueTeam.RemovePlayer(m_LocalPlayer);
                m_RedTeam.AddPlayer(m_LocalPlayer);

                m_LocalPlayer.SetTeamAndSync(m_RedTeam);
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

                m_LocalPlayer.SetTeamAndSync(m_RedTeam);
            }
            else
            {
                m_RedTeam.RemovePlayer(m_LocalPlayer);
                m_BlueTeam.AddPlayer(m_LocalPlayer);

                m_LocalPlayer.SetTeamAndSync(m_BlueTeam);
            }
        }

        [ContextMenu("SetRedTeam")]
        public void SetRedTeam()
        {
            SetTeam(TeamType.RED);
        }
        
        
        [ContextMenu("SetBlueTeam")]
        public void SetBlueTeam()
        {
            SetTeam(TeamType.BLUE);
        }

        #endregion

        public void SetMaster()
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
    }
}