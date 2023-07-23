using System;
using UnityEngine;

namespace MobaVR
{
    public class ClassStats : MonoBehaviour
    {
        [SerializeField] private string m_ClassId = "Wizard";
        [SerializeField] private WizardPlayer m_WizardPlayer;
        [SerializeField] private ClassStatsSO m_ClassStatsSo;
        [SerializeField] private SpellsHandler m_SpellsHandler;
        [SerializeField] private Skin m_Skin;

        public string ClassId => m_ClassId;
        public WizardPlayer WizardPlayer => m_WizardPlayer;
        public ClassStatsSO ClassStatsSo => m_ClassStatsSo;
        public SpellsHandler SpellsHandler => m_SpellsHandler;
        public Skin Skin => m_Skin;

        private void OnValidate()
        {
            if (m_WizardPlayer == null)
            {
                m_WizardPlayer = GetComponentInParent<WizardPlayer>();
            }
            
            if (m_SpellsHandler == null)
            {
                TryGetComponent(out m_SpellsHandler);
            }
        }

        private void OnEnable()
        {
            if (m_ClassStatsSo)
            {
                m_WizardPlayer.Stats = m_ClassStatsSo;
            }
        }
    }
}