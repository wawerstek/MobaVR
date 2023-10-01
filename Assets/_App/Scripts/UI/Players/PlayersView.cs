using System;
using System.Collections.Generic;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobaVR
{
    public class PlayersView : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerInfoView m_PlayerInfoView;
        [SerializeField] private Transform m_PlayersParent;

        [SerializeField] [ReadOnly] private List<PlayerInfoView> m_PlayerInfoViews = new();
        [SerializeField] [ReadOnly] private ClassicGameSession m_GameSession;

        private void OnDestroy()
        {
            if (m_GameSession != null)
            {
                m_GameSession.OnAddPlayer -= OnAddPlayer;
                m_GameSession.OnRemovePlayer -= OnRemovePlayer;
            }
        }

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void FindGameSession()
        {
            if (m_GameSession != null)
            {
                return;
            }

            if (m_GameSession == null)
            {
                m_GameSession = FindObjectOfType<ClassicGameSession>(true);
            }

            if (m_GameSession != null)
            {
                m_GameSession.OnAddPlayer += OnAddPlayer;
                m_GameSession.OnRemovePlayer += OnRemovePlayer;
            }
        }

        private void OnRemovePlayer(PlayerVR playerVR)
        {
            PlayerInfoView playerInfoView = m_PlayerInfoViews.Find(view => view.PlayerVR == playerVR);
            if (playerInfoView != null)
            {
                m_PlayerInfoViews.Remove(playerInfoView);
                Destroy(playerInfoView.gameObject);
            }
        }

        private void OnAddPlayer(PlayerVR playerVR)
        {
            PlayerInfoView playerInfo = Instantiate(m_PlayerInfoView, m_PlayersParent);
            playerInfo.PlayerVR = playerVR;
            m_PlayerInfoViews.Add(playerInfo);
        }

        public void UpdatePlayers()
        {
            for (int i = m_PlayerInfoViews.Count - 1; i >= 0; i--)
            {
                Destroy(m_PlayerInfoViews[i].gameObject);
            }
            
            m_PlayerInfoViews.Clear();

            PlayerVR[] players = FindObjectsOfType<PlayerVR>();
            foreach (PlayerVR playerVR in players)
            {
                OnAddPlayer(playerVR);
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            FindGameSession();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            FindGameSession();
        }
    }
}