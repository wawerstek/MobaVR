using System;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    public abstract class ModeState<T> : ScriptableObject, IState where T : GameMode<T>
    {
        [SerializeField] protected PlayerStateSO m_PlayerState;

        protected T m_Mode;
        protected BaseStateMachine<T> m_StateMachine;

        public virtual void Init(T mode, BaseStateMachine<T> stateMachine)
        {
            m_Mode = mode;
            m_StateMachine = stateMachine;
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