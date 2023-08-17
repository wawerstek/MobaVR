using UnityEngine;

namespace MobaVR
{
    public interface IExploding
    {
        //public void RpcHit(float damage);
        //public void Die();

        public void Explode(float explosionForce,
                            Vector3 position,
                            float radius,
                            float modifier);
    }
}