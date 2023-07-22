using System;
using MobaVR.Base;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    /// <summary>
    /// Базовый класс для большого и маленького шаров
    /// Обработка всех столкновений пока в это классе.
    /// TODO: обрабатывать OnTriggerEnter в отдельных сущностях
    ///
    /// На игроках и щите должен висеть IsTrigger, чтобы прошла проверка
    /// </summary>
    public abstract class ThrowableSpell : Spell
        //, IThrowable
    {
        [Space]
        [Header("Components")]
        [SerializeField] protected float m_DefaultDamage = 1f;
        [SerializeField] protected float m_Force = 4000f;

        protected bool m_IsThrown = false;

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
            //Use RpcDestroy for Network
            //Destroy(gameObject, m_DestroyLifeTime);
        }

        private void OnDestroy()
        {
            if (photonView.IsMine)
            {
                //PhotonNetwork.Destroy(gameObject);
            }
        }

        protected virtual void Update()
        {
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            //if (!collision.transform.CompareTag("Player"))
            //if (m_IsThrown)
            if (!photonView.IsMine)
            {
                //return;
            }

            Debug.Log($"ThrowableSpell: Collision: " + collision.gameObject.name);

            if (m_IsThrown && !collision.transform.CompareTag("RemotePlayer"))
            {
                HandleCollision(collision.transform);
            }
        }

        //TODO
        protected void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                //return;
            }

            if (other.transform.TryGetComponent(out PhotonView colliderPhoton))
            {
                if (colliderPhoton == photonView)
                {
                    return;
                }
            }

            if (m_IsThrown)
            {
                if (other.CompareTag("RemotePlayer") && other.transform.TryGetComponent(out WizardPlayer wizardPlayer))
                {
                    if (wizardPlayer == Owner)
                    {
                        return;
                    }

                    if (photonView.IsMine)
                    {
                        wizardPlayer.Hit(this, CalculateDamage());
                    }

                    HandleCollision(other.transform);
                }

                if (other.CompareTag("LifeCollider") && other.transform.TryGetComponent(out HitCollider damagePlayer))
                {
                    if (damagePlayer.WizardPlayer == Owner)
                    {
                        return;
                    }

                    if (photonView.IsMine)
                    {
                        damagePlayer.WizardPlayer.Hit(this, CalculateDamage());
                    }

                    HandleCollision(other.transform);
                }

                if (other.CompareTag("Item"))
                {
                    Shield shield = other.GetComponentInParent<Shield>();
                    if (shield != null)
                    {
                        if (photonView.IsMine)
                        {
                            shield.Hit(this, CalculateDamage());
                        }

                        HandleCollision(other.transform);
                    }

                    //TODO: Перенести логику в класс БигШит
                    if (other.TryGetComponent(out BigShield bigShield))
                    {
                        if (bigShield.Team.TeamType != m_TeamType)
                        {
                            HandleCollision(other.transform);
                        }
                    }
                }

                //InteractBall(other.transform);
            }
        }

        public virtual void Init(WizardPlayer wizardPlayer, TeamType teamType)
        {
            m_Owner = wizardPlayer;
            m_TeamType = teamType;
            photonView.RPC(nameof(RpcInit), RpcTarget.All, teamType);
        }

        /*
        public virtual void Init(TeamType teamType)
        {
            //m_TeamItem.SetTeam(teamType);
            photonView.RPC(nameof(RpcInit), RpcTarget.All, teamType);
        }
        */

        [PunRPC]
        public virtual void RpcInit(TeamType teamType)
        {
            m_TeamItem.SetTeam(teamType);
        }

        protected abstract float CalculateDamage();

        protected abstract void HandleCollision(Transform interactable);
        //public abstract void Throw();
        //public abstract void ThrowByDirection(Vector3 direction);
    }
}