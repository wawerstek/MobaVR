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
        [SerializeField] private ParticleSystem m_CollisionParticleSystem;

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

        public override void Init(WizardPlayer wizardPlayer, TeamType teamType)
        {
            base.Init(wizardPlayer, teamType);
        }

        [PunRPC]
        public override void RpcInit(TeamType teamType, int idOwner)
        {
            base.RpcInit(teamType, idOwner);
            
            // TODO: Выполняется только один раз. Плохо
            // Добавить для заклинаний, щитов возможности смены команды. Сейчас это делается только для смены визуала, а на сам спелл не влияет
            /*
            ParticleSystem.CollisionModule collisionModule = m_CollisionParticleSystem.collision;
            if (teamType == TeamType.RED)
            {
                collisionModule.collidesWith = LayerMask.GetMask(new[]
                {
                    "Default",
                    //"RedTeam_DetectOnlyEnemyCollision"
                    "BlueTeam_DetectOnlyEnemyCollision"
                });
            }
            else
            {
                collisionModule.collidesWith = LayerMask.GetMask(new[]
                {
                    "Default",
                    //"BlueTeam_DetectOnlyEnemyCollision"
                    "RedTeam_DetectOnlyEnemyCollision"
                });
            }
            */
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

            if (m_Owner != null)
            {
                particleTrigger.Init(m_ParticleSystem, transform, m_Owner.TeamType);
            }
            else
            {
                particleTrigger.Init(m_ParticleSystem, transform, m_TeamType);
            }

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
            
            /*
            if (other.CompareTag("RemotePlayer")
                || other.CompareTag("LifeCollider")
                || other.CompareTag("Item")
                || other.CompareTag("Enemy"))
            {
                //TODO
            }
            */
            
            HitData hitData = new HitData()
            {
                Amount = m_Damage,
                Player = PhotonNetwork.LocalPlayer,
                PhotonOwner = photonView,
                PhotonView = photonView,
                PlayerVR = Owner.PlayerVR,
                TeamType = TeamType
            };

            if (other.transform.TryGetComponent(out Damageable damageable))
            {
                if (photonView.IsMine)
                {
                    damageable.Hit(hitData);
                }
            }

            if (other.CompareTag("Item") && other.TryGetComponent(out BigShield bigShield))
            {
                if (bigShield.Team.TeamType != m_TeamType)
                {
                    //HandleCollision(other.transform);
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