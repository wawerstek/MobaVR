using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{
    public abstract class SpellStateSO : ScriptableObject, ISpellState
    {
        [SerializeField] protected int m_Priority = 0;

        [SerializeField] private List<SpellStateSO> m_BlockingSpells = new ();

        protected SpellHandler m_SpellsHandler;
        protected PlayerVR m_PlayerVR;
        
        protected bool m_IsPerformed = false;
        protected bool m_IsValidInput = false;
        protected bool m_IsLiving = false;

        public bool IsPerformed => m_IsPerformed;
        public bool IsValidInput => m_IsValidInput;
        public bool IsLiving => m_IsLiving;
        public int Priority => m_Priority;

        public Action OnStarted;
        public Action OnPerformed;
        public Action OnCompleted;

        /*
        public virtual void Init(SpellSwitcher stateMachine, PlayerVR playerVR)
        {
            m_SpellStateMachine = stateMachine;
            m_PlayerVR = playerVR;
        }
        */
        
        public virtual void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            m_SpellsHandler = spellHandler;
            m_PlayerVR = playerVR;
        }
        
        protected bool CanCast => m_PlayerVR.WizardPlayer.PlayerState.StateSo.CanCast && m_PlayerVR.WizardPlayer.IsLife;
        
        #region Input

        public abstract void CheckInput();
        public abstract bool IsInProgress();
        public abstract bool IsPressed();

        #endregion

        #region Spell State

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

        #endregion
    }
}