using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class ArrowSpellEvents : MonoBehaviourPun
    {
        private ArrowSpell m_Spell;

        public UnityEvent OnInitSpell;
        public UnityEvent OnHitSpell;
        public UnityEvent OnDestroySpell;
        public UnityEvent OnThrowSpell;

        private void OnEnable()
        {
            if (m_Spell != null)
            {
                m_Spell.OnRpcInitSpell += Spell_OnInitSpell;
                m_Spell.OnDestroySpell += Spell_OnDestroySpell;
                m_Spell.OnHit += Spell_OnHitSpell;
                m_Spell.OnThrown += Spell_OnThrownSpell;
            }
        }

        private void OnDisable()
        {
            if (m_Spell != null)
            {
                m_Spell.OnRpcInitSpell -= Spell_OnInitSpell;
                m_Spell.OnDestroySpell -= Spell_OnDestroySpell;
                m_Spell.OnHit -= Spell_OnHitSpell;
                m_Spell.OnThrown -= Spell_OnThrownSpell;
            }
        }

        private void Awake()
        {
            m_Spell = GetComponent<ArrowSpell>();
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

        private void Spell_OnThrownSpell()
        {
            OnThrowSpell?.Invoke();
        }
    }
}