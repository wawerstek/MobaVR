using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    public class SpellHandler : MonoBehaviour
    {
        [SerializeField] private PlayerVR m_PlayerVR;
        [SerializeField] private List<SpellStateSO> m_Spells;
        
        [SerializeField] [ReadOnly] private SpellStateSO m_ActiveSpell;

        public List<SpellStateSO> Spells => m_Spells;
        public SpellStateSO ActiveSpell => m_ActiveSpell;
        public bool HasActiveSpell => m_ActiveSpell != null;

        private void OnValidate()
        {
            if (m_PlayerVR == null)
            {
                TryGetComponent(out m_PlayerVR);

                if (m_PlayerVR == null)
                {
                    m_PlayerVR = GetComponentInParent<PlayerVR>();
                }
            }
        }

        private void Awake()
        {
            foreach (SpellStateSO spellStateSo in m_Spells)
            {
                spellStateSo.Init(this, m_PlayerVR);
            }
        }

        private void Update()
        {
            foreach (SpellStateSO spellStateSo in m_Spells)
            {
                if (spellStateSo.IsPerformed)
                {
                    //m_ActiveSpell = spellStateSo;
                }
            }
        }

        public void SetActiveSpell(SpellStateSO spellStateSo)
        {
            if (m_ActiveSpell != null)
            {
                m_ActiveSpell.Exit();
            }

            m_ActiveSpell = spellStateSo;
            m_ActiveSpell.Enter();
        }
    }
}