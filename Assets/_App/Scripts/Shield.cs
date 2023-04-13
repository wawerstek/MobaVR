using System;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using UnityEngine;

namespace MobaVR
{
    public class Shield : MonoBehaviour, IHit
    {
        [SerializeField] private float m_Health = 1f;
        [SerializeField] private float m_CooldownTime = 5f;

        [SerializeField] private GameObject m_Shield;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private MeshRenderer m_Renderer;

        private float m_CurrentHealth = 1f;

        public bool IsLife => m_CurrentHealth > 0f;

        private void Awake()
        {
            m_CurrentHealth = m_Health;
        }

        void Start()
        {
        }

        private void Dissolve()
        {
            float clip = 0f;
            DOTween
                .To(() => clip, x => clip = x, 1f, 1f)
                .OnUpdate(() =>
                {
                    AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(
                        m_Renderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
                })
                .OnComplete(() =>
                {
                    //m_Shield.SetActive(false);
                    //m_Renderer.enabled = false;
                    //Destroy(gameObject, 2f);
                });
        }

        private void Reset()
        {
            
        }

        private void Show(bool isShow)
        {
            float clip, endClip;
            if (!isShow)
            {
                clip = 0f;
                endClip = 1f;
            }
            else
            {
                clip = 1f;
                endClip = 0f;
            }
            DOTween
                .To(() => clip, x => clip = x, endClip, 1f)
                .OnUpdate(() =>
                {
                    AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(
                        m_Renderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
                })
                .OnComplete(() =>
                {
                    //TODO
                });
        }

        [ContextMenu("Show")]
        public void Show()
        {
            Show(true);
        }

        [ContextMenu("Hide")]
        public void Hide()
        {
            Show(false);
        }

        [ContextMenu("Hit")]
        public void Hit()
        {
            Hit(0.5f);
        }

        public void Hit(float damage)
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
                m_Collider.enabled = false;
                Dissolve();
                Invoke(nameof(Reset), m_CooldownTime);                
            }
        }

        public void Explode(float explosionForce, Vector3 position, float radius, float modifier)
        {
        }
    }
}