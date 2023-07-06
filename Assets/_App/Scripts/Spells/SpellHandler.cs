using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    public class SpellHandler : MonoBehaviour
    {
        [SerializeField] private PlayerVR m_PlayerVR;
        [SerializeField] private List<SpellMap> m_Spells;
        
        [SerializeField] [ReadOnly] private SpellBehaviour m_ActiveSpell;

        public List<SpellMap> Spells => m_Spells;
        public List<SpellBehaviour> SpellBehaviours => m_Spells.Select(map => map.SpellBehaviour).Distinct().ToList();
        public SpellBehaviour ActiveSpell => m_ActiveSpell;
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
            foreach (SpellMap spellMap in m_Spells)
            {
                spellMap.SpellBehaviour.Init(this, m_PlayerVR);
            }
        }

        private void Update()
        {
        
        }

        public void SetActiveSpell(SpellBehaviour spellBehaviour)
        {
            if (m_ActiveSpell != null)
            {
                m_ActiveSpell.Exit();
            }

            m_ActiveSpell = spellBehaviour;
            m_ActiveSpell.Enter();
        }
    }
}