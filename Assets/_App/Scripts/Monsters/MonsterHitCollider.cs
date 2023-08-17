using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class MonsterHitCollider : Damageable
    {
        [SerializeField] private NetworkDamageable m_ParentDamageable;
        [SerializeField] private Collider m_Collider;

        private void InitComponents()
        {
            if (m_Collider == null)
            {
                m_Collider = GetComponent<Collider>();
            }

            if (m_ParentDamageable == null)
            {
                m_ParentDamageable = GetComponentInParent<NetworkDamageable>();
            }
        }
        
        private void OnValidate()
        {
            InitComponents();
        }

        private void Awake()
        {
            InitComponents();
        }

        public override void Hit(HitData hitData)
        {
            if (m_ParentDamageable != null)
            {
                m_ParentDamageable.Hit(hitData);
            }
        }

        public override void Die()
        {
        }

        public override void Reborn()
        {
        }
    }
}