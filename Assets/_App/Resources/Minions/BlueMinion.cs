using System;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using MobaVR;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace MobaVR
{
    public class BlueMinion : MonoBehaviour, IHit
    {
        [SerializeField] private Transform m_DestinationPoint;
        [SerializeField] private NavMeshAgent m_NavMeshAgent;
        [SerializeField] private Animator m_Animator;
        [SerializeField] private Transform m_Root;
        [SerializeField] private Collider m_BodyCollider;
        [SerializeField] private Rigidbody m_PelvisRigidbody;
        [SerializeField] private Rigidbody m_Rigidbody;

        [SerializeField] private float m_Health = 3.0f;
        [SerializeField] private float m_AttackRange = 1.0f;

        private bool m_IsLife = true;
        [SerializeField] private float m_CurrentHp;

        private Rigidbody[] m_ChildRigidbodies;
        private Collider[] m_ChildColliders;
        private Renderer[] m_MeshRenderers;

        private void OnValidate()
        {
            if (m_Animator == null)
            {
                TryGetComponent(out m_Animator);
            }

            if (m_NavMeshAgent == null)
            {
                TryGetComponent(out m_NavMeshAgent);
            }
        }

        private void Awake()
        {
            m_CurrentHp = m_Health;
            if (m_NavMeshAgent != null)
            {
                m_NavMeshAgent.stoppingDistance = m_AttackRange;
            }

            m_ChildColliders = m_Root.GetComponentsInChildren<Collider>();
            m_ChildRigidbodies = m_Root.GetComponentsInChildren<Rigidbody>();
            m_MeshRenderers = GetComponentsInChildren<Renderer>();

            //ToggleRagDolls(false);

            //Destroy(gameObject, 10f);
        }

        private void Update()
        {
            if (m_DestinationPoint == null)
            {
                return;
            }

            m_NavMeshAgent.destination = m_DestinationPoint.position;
            //float velocity = m_NavMeshAgent.velocity.magnitude / m_NavMeshAgent.speed;
            float velocity = m_NavMeshAgent.velocity.magnitude;
            m_Animator.SetFloat("speed", velocity);

            if (m_NavMeshAgent.isStopped)
            {
                //TODO
                m_Animator.SetTrigger("attack");
            }

            float distance = Vector3.Distance(m_DestinationPoint.position, transform.position);
            if (distance <= m_AttackRange)
            {
                m_Animator.SetTrigger("attack");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.transform.name);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.transform.name);
        }

        private void ToggleRagDolls(bool useRagDoll)
        {
            m_BodyCollider.isTrigger = useRagDoll;
            m_Animator.enabled = !useRagDoll;

            foreach (Rigidbody childRigidbody in m_ChildRigidbodies)
            {
                childRigidbody.isKinematic = !useRagDoll;
            }

            foreach (Collider childCollider in m_ChildColliders)
            {
                childCollider.enabled = useRagDoll;
            }
        }

        private void Dissolve()
        {
            float clip = 0f;
            DOTween
                .To(()=> clip, x=> clip = x, 1f, 2f)
                .OnUpdate(() =>
                {
                    foreach (Renderer renderer in m_MeshRenderers)
                    {
                        Dissolve(renderer.material, clip);
                    }
                })
                .OnComplete(() =>
                {
                    Destroy(gameObject, 2f);
                });
        }
        
        private void Dissolve(Material material, float clip)
        {
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
        }

        public void SetDestination(Transform destination)
        {
            m_DestinationPoint = destination;
        }

        public void Hit(float damage)
        {
            if (m_IsLife)
            {
                m_CurrentHp -= damage;
                if (m_CurrentHp <= 0)
                {
                    Die();
                }
            }
        }

        public void Die()
        {
            if (m_IsLife)
            {
                ToggleRagDolls(true);

                m_NavMeshAgent.enabled = false;
                m_Animator.enabled = false;

                //Destroy(gameObject, 10f);
                m_IsLife = false;

                Invoke(nameof(Dissolve), 10f);
            }
        }

        public float m_KForce = 10f;
        
        public void Explode(float explosionForce, Vector3 position, float radius, float modifier)
        {
            if (!m_IsLife)
            {
                
                foreach (var childRigidbody in m_ChildRigidbodies)
                {
                    childRigidbody.AddExplosionForce(explosionForce * m_KForce,
                                                     position,
                                                     radius,
                                                     modifier);
                }

                /*
                m_Rigidbody.AddExplosionForce(explosionForce * 10f,
                                              position,
                                              radius,
                                              modifier);
                */
            }
        }
    }
}