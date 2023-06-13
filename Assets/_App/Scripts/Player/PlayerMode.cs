using System;
using UnityEngine;

namespace MobaVR
{
    public class PlayerMode : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private PlayerStateSO m_InactiveState;
        [SerializeField] private PlayerStateSO m_ReadyState;
        [SerializeField] private PlayerStateSO m_GodState;
        [SerializeField] private PlayerStateSO m_PlayState;
        [SerializeField] private PlayerStateSO m_DieState;

        [Header("Current State")]
        [SerializeField] private PlayerState m_PlayerState;
        [SerializeField] private PlayerStateSO m_CurrentStateSo;

        public PlayerState State => m_PlayerState;
        public PlayerStateSO StateSo => m_CurrentStateSo;

        public void SetState(PlayerState playerState)
        {
            m_PlayerState = playerState;
            switch (playerState)
            {
                case PlayerState.INACTIVE:
                    SetState(m_InactiveState);
                    break;
                case PlayerState.READY:
                    SetState(m_ReadyState);
                    break;
                case PlayerState.GOD:
                    SetState(m_GodState);
                    break;
                case PlayerState.PLAY:
                    SetState(m_PlayState);
                    break;
                case PlayerState.DIE:
                    SetState(m_DieState);
                    break;
                default:
                    SetState(m_InactiveState);
                    break;
            }
        }

        public void SetState(PlayerStateSO playerStateSo)
        {
            m_CurrentStateSo = playerStateSo;
        }
    }
}