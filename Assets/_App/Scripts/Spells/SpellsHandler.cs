using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    public class SpellsHandler : MonoBehaviour
    {
        [Tooltip("Priority list")]
        [SerializeField] private List<SpellHandler> m_SpellHandlers = new();

        [ContextMenu("FindSpellHandlers")]
        private void FindSpellHandlers()
        {
            m_SpellHandlers.AddRange(GetComponentsInChildren<SpellHandler>());            
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