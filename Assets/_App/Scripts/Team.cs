using System.Collections.Generic;
using MobaVR.Utils;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MobaVR
{
    public class Team : MonoBehaviourPunCallbacks, ITeam
    {
        [SerializeField] private string m_TeamName = "Team";
        [SerializeField] private TeamType m_TeamType = TeamType.RED;
        [SerializeField] private int m_Score = 0;
        [SerializeField] private int m_Kills = 0;
        [SerializeField] private List<PlayerVR> m_Players = new();

        public string Name => m_TeamName;
        public TeamType TeamType => m_TeamType;
        public List<PlayerVR> Players => m_Players;
        public int Score
        {
            get => m_Score;
            set => m_Score = value;
        }
        public int Kills
        {
            get => m_Kills;
            set => m_Kills = value;
        }

        public void RemovePlayer(PlayerVR player)
        {
            photonView.RPC(nameof(RpcRemovePlayer), RpcTarget.AllBuffered, player.photonView.ViewID);
        }

        [PunRPC]
        private void RpcRemovePlayer(int idPlayer)
        {
            if (PhotonViewExtension.TryGetComponent(idPlayer, out PlayerVR player))
            {
                player.OnDestroyPlayer -= OnDestroyPlayer;
                m_Players.Remove(player);
            }
        }

        public void AddPlayer(PlayerVR player)
        {
            photonView.RPC(nameof(RpcAddPlayer), RpcTarget.AllBuffered, player.photonView.ViewID);
        }

        [PunRPC]
        private void RpcAddPlayer(int idPlayer)
        {
            if (PhotonViewExtension.TryGetComponent(idPlayer, out PlayerVR player))
            {
                if (!m_Players.Contains(player))
                {
                    player.OnDestroyPlayer += OnDestroyPlayer;
                    m_Players.Add(player);
                }
            }
        }

        private void OnDestroyPlayer(PlayerVR playerVR)
        {
            if (playerVR != null)
            {
                playerVR.OnDestroyPlayer -= OnDestroyPlayer;
                m_Players.Remove(playerVR);
            }
        }

        private void OnDestroyPlayer()
        {
            
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            m_Players.RemoveAll(player => player == null);
        }
    }
}