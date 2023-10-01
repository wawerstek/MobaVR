using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    public abstract class BaseContentAdminView<T> : MonoBehaviour where T : BaseAdminPlayerView
    {
        [SerializeField] protected T m_PlayerView;
        [SerializeField] protected Transform m_ContentPoint;

        [SerializeField] [ReadOnly] protected List<T> m_PlayerViews = new();

        public virtual void RemovePlayer(PlayerVR playerVR)
        {
            T playerInfoView = m_PlayerViews.Find(view => view.PlayerVR == playerVR);
            if (playerInfoView != null)
            {
                m_PlayerViews.Remove(playerInfoView);
                Destroy(playerInfoView.gameObject);
            }
        }

        public virtual void AddPlayer(PlayerVR playerVR)
        {
            T playerInfo = Instantiate(m_PlayerView, m_ContentPoint);
            playerInfo.PlayerVR = playerVR;
            m_PlayerViews.Add(playerInfo);
        }

        public void UpdatePlayers()
        {
            for (int i = m_PlayerViews.Count - 1; i >= 0; i--)
            {
                Destroy(m_PlayerViews[i].gameObject);
            }
            
            m_PlayerViews.Clear();

            PlayerVR[] players = FindObjectsOfType<PlayerVR>();
            foreach (PlayerVR playerVR in players)
            {
                AddPlayer(playerVR);
            }
        }
    }
}