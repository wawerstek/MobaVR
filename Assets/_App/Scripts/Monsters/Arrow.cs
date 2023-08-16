using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class Arrow : MonoBehaviourPun
    {
        [SerializeField] private Renderer m_ArrowRenderer;
        [SerializeField] private ParticleSystem m_DestroyFx;
        [SerializeField] private float m_Speed;

        private bool m_IsActive = false;
        private Vector3 m_Direction = Vector3.zero;
        private float m_Damage = 1f;

        private void Start()
        {
            Invoke(nameof(Hide), 10f);
        }

        private void Update()
        {
            if (m_IsActive)
            {
                //transform.Translate(-transform.forward * m_Speed);
                //transform.Translate(m_Direction * m_Speed, Space.Self);
                //transform.Translate(-transform.forward * m_Speed, Space.Self);
                transform.Translate(0f, 0f, m_Speed * Time.deltaTime);
            }
        }
        
        protected virtual void OnCollisionEnter(Collision collision)
        {
            /*
            if (m_IsActive && !collision.transform.CompareTag("Player"))
            {
                Hide();
            }
            */
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (m_IsActive)
            {
                if (other.transform == transform)
                {
                    return;
                }
                
                /*
                if ((other.CompareTag("Player")||other.CompareTag("RemotePlayer"))
                    && other.transform.TryGetComponent(out WizardPlayer wizardPlayer))
                {
                    wizardPlayer.Hit(m_Damage);
                    //Hide();
                }
                */
                
                HitData hitData = new HitData()
                {
                    TeamType = TeamType.OTHER,
                    PhotonOwner = photonView,
                    Amount = m_Damage
                };
                
                if ((other.transform.TryGetComponent(out Damageable damageable)))
                {
                    damageable.Hit(hitData);
                    //Hide();
                }
                
                if (other.CompareTag("Enemy") && other.transform.TryGetComponent(out IHit iHit))
                {
                    iHit.RpcHit(m_Damage);
                    //Hide();
                }

                /*
                if (other.CompareTag("Item"))
                {
                    Shield shield = other.GetComponentInParent<Shield>();
                    if (shield != null)
                    {
                        shield.Hit(hitData);
                        //shield.Hit(1f);
                        //Hide();
                    }
                }
                */
                
                Hide();
            }
        }

        private void Hide()
        {
            if (m_IsActive)
            {
                m_IsActive = false;
                m_ArrowRenderer.enabled = false;
                m_DestroyFx.Play();
                
                //gameObject.SetActive(false);
                //if (photonView.IsMine)
                
                if (PhotonNetwork.IsMasterClient && photonView != null)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
                else
                {
                    //Destroy(gameObject, 2f);
                    //Destroy(gameObject);
                }
                
                
                //Destroy(gameObject, 2f);
            }
        }

        public void Init(Vector3 direction, float damage)
        {
            m_IsActive = true;
            
            m_Direction = direction;
            m_Damage = damage;
        }
    }
}