using System;
using UnityEngine;

namespace MobaVR
{
    public class RebornCollider : MonoBehaviour
    {
        [SerializeField] private WizardPlayer m_WizardPlayer;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private float m_Cooldown;

        private bool m_CanReborn = true;

        public bool CanReborn => m_CanReborn;

        private void OnEnable()
        {
            if (m_WizardPlayer != null)
            {
                m_WizardPlayer.OnDie += OnDie;
            }
        }
        
        private void OnDisable()
        {
            if (m_WizardPlayer != null)
            {
                m_WizardPlayer.OnDie -= OnDie;
            }
        }

        private void OnDie()
        {
            m_Collider.enabled = false;
            m_CanReborn = false;
            Invoke(nameof(ResetReborn), m_Cooldown);
        }

        public bool TryReborn()
        {
            if (m_WizardPlayer != null 
                && m_WizardPlayer.photonView.IsMine 
                && !m_WizardPlayer.IsLife
                && m_CanReborn)
            {
                m_Collider.enabled = false;
                m_CanReborn = false;
                //Invoke(nameof(ResetReborn), m_Cooldown);

                m_WizardPlayer.Reborn();
                return true;
            }

            return false;
        }

        public void ResetReborn()
        {
            m_Collider.enabled = true;
            m_CanReborn = true;
        }

        /*
        private void OnTriggerEnter(Collider other)
        {
            TryReborn();
        }
        */
    }
}