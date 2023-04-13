using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public abstract class Fireball : MonoBehaviourPunCallbacks
    {
        [Space]
        [Header("Components")]
        [SerializeField] protected TeamItem m_TeamItem;
        [SerializeField] protected float m_DefaultDamage = 1f;
        [SerializeField] protected float m_Force = 4000f;
        [SerializeField] protected float m_DestroyLifeTime = 20.0f;

        protected bool m_IsThrown = false;
        [HideInInspector] public WizardPlayer Owner;

        public TeamItem Team => m_TeamItem;
        
        protected virtual void OnValidate()
        {
            if (m_TeamItem == null)
            {
                TryGetComponent(out m_TeamItem);
            }
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
            Destroy(gameObject, m_DestroyLifeTime);
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
                return;
            }
            
            if (m_IsThrown && !collision.transform.CompareTag("Player"))
            {
                InteractBall(collision.transform);
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (m_IsThrown && other.CompareTag("Player"))
            {
                if (other.transform.TryGetComponent(out PhotonView colliderPhoton))
                {
                    if (colliderPhoton == photonView)
                    {
                        return;
                    }
                }

                if (other.transform.TryGetComponent(out WizardPlayer wizardPlayer))
                {
                    if (wizardPlayer == Owner)
                    {
                        return;
                    }
                    //if (wizardPlayer.IsRedTeam == Owner.IsRedTeam)
                    //{
                    //    wizardPlayer.Hit(0f);
                    //}
                    //else
                    wizardPlayer.RpcHit(this, CalculateDamage());
                    //wizardPlayer.Hit(this, CalculateDamage());
                }

                InteractBall(other.transform);
            }
        }

        public void Init(TeamType teamType)
        {
            //m_TeamItem.SetTeam(teamType);
            photonView.RPC(nameof(RpcInit), RpcTarget.All, teamType);
        }

        [PunRPC]
        public virtual void RpcInit(TeamType teamType)
        {
            m_TeamItem.SetTeam(teamType);
        }

        protected abstract float CalculateDamage();
        protected abstract void InteractBall(Transform interactable);
        public abstract void Throw();
        public abstract void ThrowByDirection(Vector3 direction);
    }
}