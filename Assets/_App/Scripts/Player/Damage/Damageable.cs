using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class Damageable : MonoBehaviourPun, IDamageable
    {
        [SerializeField] private float m_CurrentHp = 100f;
        [SerializeField] private float m_MaxHp = 100f;

        public UnityAction<HitData> OnHit;
        public UnityAction OnDie;
        public UnityAction OnReborn;

        public float MaxHp
        {
            get => m_MaxHp;
            set => m_MaxHp = value;
        }
        public float CurrentHp => m_CurrentHp;
        public bool IsLife => m_CurrentHp > 0;

        protected virtual void Awake()
        {
            m_CurrentHp = m_MaxHp;
        }

        public virtual void Hit(HitData hitData)
        {
            OnHit?.Invoke(hitData);
        }
        
        public virtual void Die()
        {
            OnDie?.Invoke();
        }

        public void Reborn()
        {
            OnReborn?.Invoke();
        }

        #region Debug

        [ContextMenu("Hit")]
        private void Hit_Debug()
        {
            HitData hitData = new HitData()
            {
                Amount = 25f
            };
            
            Hit(hitData);
        }

        [ContextMenu("Kill")]
        private void Kill_Debug()
        {
            HitData hitData = new HitData()
            {
                Amount = 2000f
            };

            Hit(hitData);
        }

        #endregion
    }
}