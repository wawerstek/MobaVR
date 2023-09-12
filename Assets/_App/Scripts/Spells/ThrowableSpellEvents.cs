using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class ThrowableSpellEvents : MonoBehaviourPun
    {
        private Spell m_Spell;
        private Throwable m_Throwable;

        public UnityEvent OnInitSpell;
        public UnityEvent OnThrowSpell;
        public UnityEvent OnHitSpell;
        public UnityEvent OnDestroySpell;

        private void OnEnable()
        {
            if (m_Spell != null)
            {
                m_Spell.OnRpcInitSpell += Spell_OnInitSpell;
                m_Spell.OnDestroySpell += Spell_OnDestroySpell;
                m_Spell.OnHit += Spell_OnHitSpell;
            }

            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.AddListener(Throwable_OnThrown);
            }
        }

        private void OnDisable()
        {
            if (m_Spell != null)
            {
                m_Spell.OnRpcInitSpell -= Spell_OnInitSpell;
                m_Spell.OnDestroySpell -= Spell_OnDestroySpell;
                m_Spell.OnHit -= Spell_OnHitSpell;
            }
            
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.RemoveListener(Throwable_OnThrown);
            }
        }

        private void Awake()
        {
            m_Spell = GetComponent<Spell>();
            m_Throwable = GetComponent<Throwable>();
        }

        private void Spell_OnInitSpell()
        {
            OnInitSpell?.Invoke();
        }

        private void Spell_OnHitSpell()
        {
            OnHitSpell?.Invoke();
        }

        private void Spell_OnDestroySpell()
        {
            OnDestroySpell?.Invoke();
        }

        private void Throwable_OnThrown()
        {
            OnThrowSpell?.Invoke();
        }
    }
}