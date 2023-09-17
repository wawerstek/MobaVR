using System;
using UnityEngine;

namespace MobaVR
{
    public class HealingEffect : MonoBehaviour
    {
        private ParticleSystem m_ParticleSystem;
        private WizardPlayer m_WizardPlayer;

        private void OnEnable()
        {
            if (m_WizardPlayer != null)
            {
                m_WizardPlayer.OnHeal += OnHeal;
            }
        }

        private void OnDisable()
        {
            if (m_WizardPlayer != null)
            {
                m_WizardPlayer.OnHeal -= OnHeal;
            }
        }

        private void Awake()
        {
            m_ParticleSystem = GetComponent<ParticleSystem>();
            m_WizardPlayer = GetComponentInParent<WizardPlayer>();
        }

        private void OnHeal(float obj)
        {
            m_ParticleSystem.Play();
        }
    }
}