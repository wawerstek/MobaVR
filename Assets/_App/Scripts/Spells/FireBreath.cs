using UnityEngine;

namespace MobaVR
{
    public class FireBreath : MonoBehaviour
    {
        [SerializeField] private float m_Damage = 2f;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHit hitEnemy))
            {
                //hitEnemy.RpcHit(m_Damage);
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out IHit hitEnemy))
            {
                hitEnemy.RpcHit(m_Damage);
            }
        }
    }
}