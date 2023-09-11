using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class ParticleTrigger : MonoBehaviour
    {
        [SerializeField] private float m_Speed;
        [SerializeField] private bool m_IsDestroyOnTrigger = true;
        [SerializeField] private float m_DestroyTime = 4f;
        [SerializeField] private bool m_IsUpdateEveryFrame = true;
        [SerializeField] private Rigidbody m_Rigidbody;

        private TeamType m_TeamType;
        private ParticleSystem m_ParticleSystem;
        private Transform m_Point;

        public Action OnInitTrigger;
        public Action OnDestroyTrigger;
        public Action<Collider> OnParticleTriggerEnter;
        public Action<Collision> OnParticleCollisionEnter;

        private bool m_IsShoot = false;

        private void OnDisable()
        {
            OnDestroyTrigger?.Invoke();
        }

        public void Init(ParticleSystem particles, Transform point, TeamType teamType)
        {
            gameObject.SetActive(true);
            m_TeamType = teamType;
            m_ParticleSystem = particles;
            m_Point = point;
            OnInitTrigger?.Invoke();

            string nameLayer = teamType == TeamType.RED
                ? "RedTeam_DetectOnlyEnemyCollision"
                : "BlueTeam_DetectOnlyEnemyCollision";
            gameObject.layer = LayerMask.NameToLayer(nameLayer);
        }

        public void Shoot()
        {
            m_IsShoot = true;
            if (m_Rigidbody)
            {
                m_Rigidbody.velocity = m_Point.transform.forward * m_Speed;
            }

            float lifeTime = m_ParticleSystem.main.startLifetime.constant;
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnParticleTriggerEnter?.Invoke(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnParticleCollisionEnter?.Invoke(collision);
        }

        private void Update()
        {
            if (m_IsShoot && m_IsUpdateEveryFrame)
            {
                if (m_Rigidbody)
                {
                    m_Rigidbody.velocity = m_Point.transform.forward * m_Speed;
                }
            }
        }
    }
}