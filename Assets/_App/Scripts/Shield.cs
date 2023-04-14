using System;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class Shield : MonoBehaviourPunCallbacks, IHit
    {
        [SerializeField] private float m_Health = 1f;
        [SerializeField] private float m_CooldownTime = 5f;
        [SerializeField] private float m_ScaleDuration = 0.8f;

        [SerializeField] private GameObject m_Shield;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private MeshRenderer m_Renderer;
        [SerializeField] private ShieldTeamItem m_TeamItem;

        private bool m_IsAvailable = true; 
        private float m_CurrentHealth = 1f;
        private float m_CurrentClip = 0f;
        private TweenerCore<float, float, FloatOptions> m_TweenDissolve;

        
        public bool IsLife => m_CurrentHealth > 0f;

        private void Awake()
        {
            Reset();
        }

        private void Reset()
        {
            m_IsAvailable = true;
            m_CurrentClip = 1f;
            m_CurrentHealth = m_Health;
            m_Shield.SetActive(false);
        }

        public void Show(bool isShow)
        {
            photonView.RPC(nameof(RpcShow), RpcTarget.All, isShow);
        }

        [PunRPC]
        public void RpcShow(bool isShow)
        {
            //if (isShow && !m_IsAvailable)
            if (!m_IsAvailable)
            {
                return;
            }
            
            m_Shield.SetActive(true);
            m_Renderer.enabled = true;
            m_Collider.enabled = false;
            
            float endClip;
            if (isShow)
            {
                endClip = 0f;
            }
            else
            {
                endClip = 1f;
            }

            if (m_TweenDissolve != null)
            {
                m_TweenDissolve.Kill();
            }

            float duration = Math.Abs(endClip - m_CurrentClip) * m_ScaleDuration;
            
            m_TweenDissolve = DOTween
                .To(() => m_CurrentClip, x => m_CurrentClip = x, endClip, duration)
                .OnUpdate(() =>
                {
                    AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(
                        m_Renderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, m_CurrentClip);
                })
                .OnComplete(() =>
                {
                    if (isShow)
                    {
                        m_Shield.SetActive(true);
                        m_Renderer.enabled = true;
                        m_Collider.enabled = true;
                    }
                    else
                    {
                        m_Shield.SetActive(false);
                        m_Renderer.enabled = false;
                        m_Collider.enabled = false;
                    }
                });
        }

        [ContextMenu("Hit")]
        public void Hit()
        {
            //RpcHit(0.5f);
            photonView.RPC(nameof(RpcHit), RpcTarget.All, 1f);
        }
        
        public void Hit(Fireball fireball, float damage)
        {
            if ((fireball.Team.IsRed && m_TeamItem.IsRed)
                || (!fireball.Team.IsRed && !m_TeamItem.IsRed))
            {
                return;
            }

            photonView.RPC(nameof(RpcHit), RpcTarget.All, damage);
        }

        [PunRPC]
        public void RpcHit(float damage)
        {
            if (IsLife)
            {
                m_CurrentHealth -= damage;
                if (m_CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }

        [ContextMenu("Die")]
        public void Die()
        {
            if (!IsLife)
            {
                RpcShow(false);
                
                m_Collider.enabled = false;
                m_IsAvailable = false;
                
                Invoke(nameof(Reset), m_CooldownTime);                
            }
        }

        public void Explode(float explosionForce, Vector3 position, float radius, float modifier)
        {
        }
    }
}