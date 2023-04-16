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
    public class MagicShield : Shield
    {
        [SerializeField] private Renderer m_AuraRenderer;
        [SerializeField] private Renderer m_BackgroundRenderer;
        [SerializeField] private ParticleSystem m_Particle = new();
        [SerializeField] private float m_DelayRestoreHp = 10f;
        [SerializeField] private float m_SpeedRotation = 10f;

        private float m_CurrentScale = 0f;
        private float m_MaxScale = 0f;
        private TweenerCore<Vector3, Vector3, VectorOptions> m_TweenScale;

        protected override void Reset()
        {
            base.Reset();
            m_CurrentScale = 0f;
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

        protected override void Awake()
        {
            base.Awake();
            m_MaxScale = m_Shield.transform.lossyScale.x;
        }

        private void Update()
        {
            if (m_IsAvailable)
            {
                m_Shield.transform.Rotate(Vector3.forward * m_SpeedRotation * Time.deltaTime, Space.Self);
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
            damage = 1f;
            base.RpcHit(damage);
        }

        [ContextMenu("Die")]
        public override void Die()
        {
            base.Die();
        }
    }
}