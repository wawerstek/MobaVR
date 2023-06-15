using UnityEngine;

namespace MobaVR
{
    public class MonsterPelvis : MonoBehaviour, IHit
    {
        [SerializeField] private Monster m_Monster;
        [SerializeField] private Collider m_Collider;

        public void SetEnabled(bool isEnable)
        {
            m_Collider.enabled = isEnable;
        }
        
        public void RpcHit(float damage)
        {
            m_Monster.RpcHit(damage);
        }

        public void Die()
        {
            //throw new System.NotImplementedException();
        }

        public void Explode(float explosionForce, Vector3 position, float radius, float modifier)
        {
            m_Monster.Explode(explosionForce, position, radius, modifier);
        }
    }
}