using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    public abstract class SpellBehaviour : MonoBehaviour, ISpellState
    {
        protected const string TAG = nameof(SpellBehaviour);

        [Header("Blocking spells")]
        [Tooltip("If blocking spell is performed, then player can't use it spell.")]
        [SerializeField] private List<SpellBehaviour> m_BlockingSpells = new();

        protected SpellHandler m_SpellsHandler;
        protected PlayerVR m_PlayerVR;

        [SerializeField] [ReadOnly] protected bool m_IsPerformed = false;
        [SerializeField] [ReadOnly] protected bool m_IsValidInput = false;
        [SerializeField] [ReadOnly] protected bool m_IsLiving = false;

        public bool IsPerformed => m_IsPerformed;
        public bool IsValidInput => m_IsValidInput;
        public bool IsLiving => m_IsLiving;

        public Action OnStarted;
        public Action OnPerformed;
        public Action OnCompleted;

        public virtual void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            m_SpellsHandler = spellHandler;
            m_PlayerVR = playerVR;
        }

        public virtual bool CanCast()
        {
            return m_PlayerVR.WizardPlayer.PlayerState.StateSo.CanCast && m_PlayerVR.WizardPlayer.IsLife;
        }

        public bool HasBlockingSpells()
        {
            foreach (SpellBehaviour spellBehaviour in m_BlockingSpells)
            {
                if (spellBehaviour.IsPerformed)
                {
                    return true;
                }
            }

            return false;
        }


        #region Spell State

        public abstract bool IsInProgress();
        public abstract bool IsPressed();

        public abstract void SpellEnter();
        public abstract void SpellUpdate();
        public abstract void SpellExit();

        #endregion
    }
}