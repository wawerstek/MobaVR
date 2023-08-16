using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class FireBreath : Spell
    {
        [SerializeField] private float m_Damage = 5f;
        [SerializeField] private ParticleSystem m_ParticleSystem;

        [Header("Balls")]
        [SerializeField] private bool m_UseParticleTriggers;
        [SerializeField] private ParticleTrigger m_ParticleTriggerPrefab;
        [SerializeField] private float m_RepeatingTime = 0.2f;

        private void Awake()
        {
            RpcShow(false);
            ParticleSystem.TriggerModule particleSystemTrigger = m_ParticleSystem.trigger;
            particleSystemTrigger.enabled = photonView.IsMine;
        }

        public void Init()
        {
            
        }

        public void Show(bool isShow)
        {
            photonView.RPC(nameof(RpcShow), RpcTarget.All, isShow);

            if (photonView.IsMine)
            {
                if (m_UseParticleTriggers)
                {
                    SpawnParticleTriggers(isShow);
                }
                else
                {
                    InitColliders();
                }
            }
        }

        private void InitColliders()
        {
            WizardPlayer[] wizardPlayers = FindObjectsOfType<WizardPlayer>();
            List<WizardPlayer> filterPlayer = new();
            foreach (WizardPlayer wizardPlayer in wizardPlayers)
            {
            }
        }

        private void SpawnParticleTriggers(bool isShow)
        {
            if (isShow)
            {
                InvokeRepeating(nameof(CreateSphere), 0, m_RepeatingTime);
            }
            else
            {
                CancelInvoke(nameof(CreateSphere));
            }
        }

        private void CreateSphere()
        {
            ParticleTrigger particleTrigger = Instantiate(m_ParticleTriggerPrefab,
                                                          transform.position,
                                                          transform.rotation);

            particleTrigger.OnParticleTriggerEnter += OnParticleTriggerEnter;
            particleTrigger.OnParticleCollisionEnter += OnParticleCollisionEnter;
            particleTrigger.OnDestroyTrigger += () =>
            {
                particleTrigger.OnParticleTriggerEnter -= OnParticleTriggerEnter;
                particleTrigger.OnParticleCollisionEnter -= OnParticleCollisionEnter;
            };

            particleTrigger.Init(m_ParticleSystem, transform);
            particleTrigger.Shoot();
        }

        private void OnParticleCollisionEnter(Collision collision)
        {
        }

        private void OnParticleTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (other.transform == transform)
            {
                return;
            }
            
            HitData hitData = new HitData()
            {
                Amount = m_Damage,
                Player = PhotonNetwork.LocalPlayer,
                PhotonOwner = photonView,
                PlayerVR = Owner.PlayerVR,
                TeamType = TeamType
            };

            /*
            if (other.CompareTag("RemotePlayer")
                || other.CompareTag("LifeCollider")
                || other.CompareTag("Item")
                || other.CompareTag("Enemy"))
            {
                //TODO
            }
            */
            
            if (other.transform.TryGetComponent(out Damageable damageable))
            {
                if (photonView.IsMine)
                {
                    //wizardPlayer.Hit(this, CalculateDamage());
                    damageable.Hit(hitData);
                }
            }

            if (other.CompareTag("Item"))
            {
                Shield shield = other.GetComponentInParent<Shield>();
                if (shield != null)
                {
                    if (shield.TeamType == m_TeamType)
                    {
                        //return;
                    }
                    
                    /*
                    if (photonView.IsMine)
                    {
                        shield.Hit(m_Damage);
                    }
                    */
                }

                if (other.TryGetComponent(out BigShield bigShield))
                {
                    if (bigShield.Team.TeamType != m_TeamType)
                    {
                        //HandleCollision(other.transform);
                    }
                }
            }

            if (other.CompareTag("Enemy") && other.TryGetComponent(out IHit hitEnemy))
            {
                if (photonView.IsMine)
                {
                    hitEnemy.RpcHit(m_Damage);
                }
            }
        }

        [PunRPC]
        public virtual void RpcShow(bool isShow)
        {
            if (isShow)
            {
                m_ParticleSystem.Play();
            }
            else
            {
                m_ParticleSystem.Stop();
            }
        }

        /*
        private void OnParticleTrigger()
        {
            Debug.Log($"{name}: OnParticleTrigger");
            List<ParticleSystem.Particle> enter = new();
            int numEnter = m_ParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            Debug.Log($"{name}: OnParticleTrigger: {numEnter}");
        }

        private int colCollision = 0;

        private void OnParticleCollision(GameObject other)
        {
            colCollision++;
            Debug.Log("PARTICLE OnParticleCollision " + other.name + colCollision);
        }
                */

        /*
        private void OnTriggerStay(Collider other)
        {
            OnTriggerStay(other);
        }
        */
    }
}