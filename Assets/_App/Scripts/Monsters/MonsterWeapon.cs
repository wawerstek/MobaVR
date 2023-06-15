using System;
using UnityEngine;

namespace MobaVR
{
    public class MonsterWeapon : MonoBehaviour
    {
        [SerializeField] private Collider m_Collider;
        
        public Action<Collider> OnTrigger;

        private void OnValidate()
        {
            if (m_Collider == null)
            {
                TryGetComponent(out m_Collider);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTrigger?.Invoke(other);
        }

        public void SetEnabled(bool isEnabled)
        {
            m_Collider.enabled = isEnabled;
        }
    }
}