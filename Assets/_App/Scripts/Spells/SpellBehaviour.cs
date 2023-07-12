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
        [Tooltip("If blocking spell is performed, player can't use this spell.")]
        [SerializeField] private List<SpellBehaviour> m_BlockingSpells = new();
        [SerializeField] [ReadOnly] protected bool m_IsPerformed = false;

        protected SpellHandler m_SpellsHandler;
        protected PlayerVR m_PlayerVR;

        public bool IsPerformed => m_IsPerformed;

        public Action OnStarted;
        public Action OnPerformed;
        public Action OnCompleted;

        #region Spell

        public virtual void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            m_SpellsHandler = spellHandler;
            m_PlayerVR = playerVR;
        }

        public virtual bool CanCast()
        {
            return m_PlayerVR.WizardPlayer.PlayerState.StateSo.CanCast && m_PlayerVR.WizardPlayer.IsLife;
        }

        public virtual bool HasBlockingSpells()
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

        #endregion


        #region Spell States

        public abstract bool IsInProgress();

        public abstract void SpellEnter();
        public abstract void SpellUpdate();
        public abstract void SpellExit();

        #endregion
    }
}