using System;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class BaseDamageable : MonoBehaviour, IDamageable
    {
        public UnityAction<HitData> OnHit;
        public UnityAction OnDie;
        public UnityAction OnReborn;

        protected virtual void Awake()
        {
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
    }
}