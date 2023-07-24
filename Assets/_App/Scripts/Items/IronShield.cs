using System;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    public class IronShield : Shield
    {
        [SerializeField] private MeshRenderer m_Renderer;

        private float m_CurrentClip = 0f;
        private TweenerCore<float, float, FloatOptions> m_TweenDissolve;

        protected override void Reset()
        {
            base.Reset();
            m_CurrentClip = 1f;
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
            m_Renderer.enabled = true;
            m_Collider.enabled = false;

            float endClip = isShow ? 0f : 1f;

            if (m_TweenDissolve != null)
            {
                m_TweenDissolve.Kill();
            }

            if (m_View != null)
            {
                m_View.gameObject.SetActive(isShow);
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