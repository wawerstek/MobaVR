using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class AttackMagicShield : Shield
    {
        [SerializeField] private WizardPlayer m_WizardPlayer;
        [SerializeField] private Renderer m_AuraRenderer;
        [SerializeField] private Renderer m_BackgroundRenderer;
        [SerializeField] private ParticleSystem m_Particle;
        [SerializeField] private float m_DelayRestoreHp = 10f;
        [SerializeField] private float m_SpeedRotation = 10f;
        [SerializeField] private float m_Damage = 10f;
        [SerializeField] private float m_MaxDistance = 100f;
        [SerializeField] private float m_TimeDistance = 10f;

        private bool m_IsThrown = false;
        private float m_CurrentScale = 0f;
        private float m_MaxScale = 0f;
        private TweenerCore<Vector3, Vector3, VectorOptions> m_TweenScale;
        private TweenerCore<Vector3, Vector3, VectorOptions> m_TweenMove;
        private Transform m_Parent;
        private Vector3 m_InitPosition;
        private Quaternion m_InitRotation;

        protected override void Reset()
        {
            base.Reset();

            if (m_IsThrown)
            {
                m_IsThrown = false;
                transform.parent = m_Parent;
                transform.localPosition = m_InitPosition;
                transform.localRotation = m_InitRotation;
            }

            m_CurrentScale = 0f;
            UpdateVisualState();
        }

        protected void RestoreHp()
        {
            if (m_CurrentHealth >= m_Health || m_Use)
            {
                return;
            }

            m_CurrentHealth++;
            Invoke(nameof(RestoreHp), m_DelayRestoreHp);
        }

        private void OnValidate()
        {
            if (m_WizardPlayer == null)
            {
                m_WizardPlayer = GetComponentInParent<WizardPlayer>();
            }
        }


        protected override void Awake()
        {
            base.Awake();
            m_MaxScale = m_Shield.transform.lossyScale.x;

            m_Parent = transform.parent;
            m_InitPosition = transform.localPosition;
            m_InitRotation = transform.localRotation;
        }

        protected void Update()
        {
            if (m_IsAvailable)
            {
                m_Shield.transform.Rotate(Vector3.forward * m_SpeedRotation * Time.deltaTime, Space.Self);
            }
        }

        protected override void UpdateVisualState()
        {
            base.UpdateVisualState();
            float auraAlpha = m_CurrentHealth * 0.33f;
            Color auraColor = m_AuraRenderer.material.color;
            auraColor.a = auraAlpha;
            m_AuraRenderer.material.color = auraColor;

            float backgroundAlpha = m_CurrentHealth * 0.2f;
            Color backgroundColor = m_AuraRenderer.material.color;
            backgroundColor.a = backgroundAlpha;
            m_BackgroundRenderer.material.color = backgroundColor;
        }

        public void Throw(Vector3 direction)
        {
            if (!m_IsThrown && m_IsAvailable && m_Shield.gameObject.activeSelf && m_CurrentScale >= m_MaxScale)
            {
                WaitBeforeReset();
                
                //m_IsAvailable = false;
                m_IsThrown = true;
                transform.parent = null;
                
                //Vector3 endValue = transform.position + direction * m_MaxDistance;
                Vector3 endValue = transform.position + (-transform.right) * m_MaxDistance;
                m_TweenMove = transform
                              .DOMove(endValue, m_TimeDistance)
                              .OnComplete(() =>
                              {
                                  m_CurrentHealth = 0f;
                                  RpcShow(false);

                                  m_Collider.enabled = false;
                                  m_IsAvailable = false;
                                  
                                  //Die();
                                  //WaitBeforeReset();
                              });
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_IsThrown)
            {
                if (other.transform.TryGetComponent(out PhotonView colliderPhoton))
                {
                    if (colliderPhoton == photonView)
                    {
                        return;
                    }
                }

                if (other.CompareTag("Player") && other.transform.TryGetComponent(out WizardPlayer wizardPlayer))
                {
                    if (wizardPlayer == m_WizardPlayer)
                    {
                        return;
                    }

                    wizardPlayer.Hit(m_Damage * m_CurrentHealth);
                }

                if (other.TryGetComponent(out IHit hit))
                {
                    hit.RpcHit(m_Damage * m_CurrentHealth);
                }

                if (other.CompareTag("Item"))
                {
                    Shield shield = other.GetComponentInParent<Shield>();
                    if (shield != null)
                    {
                        shield.Hit(1f);
                    }
                }
                
                //m_Particle.Play();
                
                /*
                if (!other.TryGetComponent(out Fireball fireball))
                {
                    if (m_TweenMove != null)
                    {
                        m_TweenMove.Kill();
                    }
                    
                    Hit(1000f);
                }
                */
            }
        }

        [PunRPC]
        public override void RpcShow(bool isShow)
        {
            base.RpcShow(isShow);
            if (!m_IsAvailable)
            {
                return;
            }

            if (m_IsThrown && !isShow)
            {
                //return;
            }

            m_Shield.SetActive(true);
            //m_Renderer.enabled = true;
            m_Collider.enabled = false;

            float endScale = isShow ? m_MaxScale : 0f;

            if (m_TweenScale != null)
            {
                m_TweenScale.Kill();
            }

            float duration = Math.Abs(endScale - m_CurrentScale) * m_ScaleDuration;

            m_TweenScale = m_Shield.transform
                                   .DOScale(endScale, duration)
                                   .OnUpdate(() =>
                                   {
                                       m_CurrentScale =
                                           m_Shield.transform.lossyScale.x;
                                   })
                                   .OnComplete(() =>
                                   {
                                       if (isShow)
                                       {
                                           m_Shield.SetActive(true);
                                           //m_Renderer.enabled = true;
                                           m_Collider.enabled = true;
                                       }
                                       else
                                       {
                                           m_Shield.SetActive(false);
                                           //m_Renderer.enabled = false;
                                           m_Collider.enabled = false;
                                       }
                                   });
        }

        [PunRPC]
        public override void RpcHit(float damage)
        {
            if (!m_IsThrown)
            {
                damage = 1f;
                base.RpcHit(damage);
            }
        }

        [ContextMenu("Die")]
        public override void Die()
        {
            base.Die();
        }
    }
}