using System.Collections;
using UnityEngine;

namespace MobaVR
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private float m_Damage = 100f;
        [SerializeField] private float m_Delay;

        [Header("Explosion")]
        [SerializeField] private Transform m_ExplosionPoint;
        [SerializeField] private float m_ExplosionForce = 100f;
        [SerializeField] private float m_ExplosionRadius = 4f;
        [SerializeField] private float m_ExplosionModifier = 1f;

        private void Start()
        {
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            m_Collider.enabled = false;
            yield return new WaitForSeconds(m_Delay);
            m_Animator.SetTrigger("open");
            m_Collider.enabled = true;
            yield return new WaitForSeconds(0.5f);
            m_Collider.enabled = false;
            yield return new WaitForSeconds(1f);
            m_Animator.SetTrigger("close");
            StartCoroutine(Attack());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent(out Damageable damageable))
                {
                    HitData hitData = new HitData()
                    {
                        Amount = m_Damage,
                        TeamType = TeamType.OTHER,
                    };
                    damageable.Hit(hitData);
                }

                if (other.TryGetComponent(out IExploding hit))
                {
                    hit.Explode(m_ExplosionForce, m_ExplosionPoint.position, m_ExplosionRadius, m_ExplosionModifier);
                }
            }
        }
    }
}