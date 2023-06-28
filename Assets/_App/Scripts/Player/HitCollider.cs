using System;
using UnityEngine;

namespace MobaVR
{
    public class HitCollider : MonoBehaviour
    {
        [SerializeField] private WizardPlayer m_WizardPlayer;
        [SerializeField] private Collider m_Collider;

        public Action OnHit;

        public WizardPlayer WizardPlayer => m_WizardPlayer;

        private void OnValidate()
        {
            if (m_WizardPlayer == null)
            {
                m_WizardPlayer = GetComponentInParent<WizardPlayer>();
            }

            if (m_Collider == null)
            {
                m_Collider = GetComponent<Collider>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit?.Invoke();
        }

        public void SetEnabledCollider(bool isEnabled)
        {
            m_Collider.enabled = isEnabled;
        }
    }
}