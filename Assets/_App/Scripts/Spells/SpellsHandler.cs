using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    public class SpellsHandler : MonoBehaviour
    {
        [SerializeField] private WizardPlayer m_Player;
        
        [Tooltip("Priority list")]
        [SerializeField] private List<SpellHandler> m_SpellHandlers = new();

        [ContextMenu("FindSpellHandlers")]
        private void FindSpellHandlers()
        {
            m_SpellHandlers.AddRange(GetComponentsInChildren<SpellHandler>());            
        }

        private void OnEnable()
        {
            if (m_Player != null)
            {
                m_Player.OnDie += Reset;
                m_Player.OnReborn += Reset;
            }
        }
        
        private void OnDisable()
        {
            if (m_Player != null)
            {
                m_Player.OnDie -= Reset;
                m_Player.OnReborn -= Reset;
            }
        }


        private void Reset()
        {
            foreach (SpellHandler spellHandler in m_SpellHandlers)
            {
                foreach (SpellMap spellMap in spellHandler.Spells)
                {
                    spellMap.SpellBehaviour.Reset();
                }
            }
        }

        private void Awake()
        {
            foreach (SpellHandler spellHandler in m_SpellHandlers)
            {
                spellHandler.Init();
            }
        }
    }
}