using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class TeamLayerPhysics : MonoBehaviourPun
    {
        [SerializeField] private SingleUnityLayer m_BlueLayerMask;
        [SerializeField] private SingleUnityLayer m_RedLayerMask;

        private Spell m_Spell;

        private void OnEnable()
        {
            if (m_Spell != null)
            {
                m_Spell.OnRpcInitSpell += OnInitSpell;
            }
        }

        private void OnDisable()
        {
            if (m_Spell != null)
            {
                m_Spell.OnRpcInitSpell -= OnInitSpell;
            }
        }

        private void Awake()
        {
            m_Spell = GetComponent<Spell>();
        }

        private void OnInitSpell()
        {
            if (m_Spell.Owner != null)
            {
                gameObject.layer = m_Spell.Owner.TeamType == TeamType.RED ? m_RedLayerMask.LayerIndex : m_BlueLayerMask.LayerIndex;
            }
        }
    }
}