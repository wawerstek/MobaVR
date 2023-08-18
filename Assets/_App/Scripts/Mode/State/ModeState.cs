using System;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    public abstract class ModeState : ScriptableObject, IState
    {
        [SerializeField] protected PlayerStateSO m_PlayerState;

        protected GameMode m_Mode;
        protected BaseStateMachine m_StateMachine;

        public virtual void Init(GameMode mode)
        {
            m_Mode = mode;
            m_StateMachine = mode.StateMachine;
        }

        #region Players

        protected void UpdatePlayers()
        {
            foreach (PlayerVR player in m_Mode.Players)
            {
                UpdatePlayer(player);
            }
        }

        protected virtual void UpdatePlayer(PlayerVR player)
        {
            player.SetState(m_PlayerState);
        }

        #endregion

        #region State

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

        #endregion
    }
}