using System;
using UnityEngine;

namespace MobaVR
{
    public class HitCollider : MonoBehaviour
    {
        [SerializeField] private WizardPlayer m_WizardPlayer;

        public Action OnHit;
        
        public WizardPlayer WizardPlayer => m_WizardPlayer;
        
        private void OnValidate()
        {
            if (m_WizardPlayer == null)
            {
                m_WizardPlayer = GetComponentInParent<WizardPlayer>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit?.Invoke();            
        }
    }
}