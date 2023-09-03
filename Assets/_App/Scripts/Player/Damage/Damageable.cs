using UnityEngine;

namespace MobaVR
{
    public abstract class Damageable : MonoBehaviour, IDamageable
    {
        public abstract void Hit(HitData hitData);
        public abstract void Die();
        public abstract void Reborn();
    }
}