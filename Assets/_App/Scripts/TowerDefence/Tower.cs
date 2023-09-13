using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class Tower : MonoBehaviourPun
    {
        [SerializeField] private MeshRenderer m_Renderer;
        [SerializeField] private bool m_UseRender = false;
        [SerializeField] private float m_MaxHp = 1000;
        [SerializeField] private float m_CurrentHp;

        public UnityEvent OnDie;
        public UnityEvent OnHit;
        public UnityEvent<float> OnChangeHealth;
        public UnityEvent OnRestore;
        
        public bool IsLife => m_CurrentHp > 0f;

        private void Awake()
        {
            Restore();
        }

        public void Restore()
        {
            OnRestore?.Invoke();
            m_CurrentHp = m_MaxHp;
            if (m_UseRender)
            {
                m_Renderer.enabled = true;
            }

            OnChangeHealth?.Invoke(m_CurrentHp);
        }

        public void Hit(HitData hitData)
        {
            if (hitData.TeamType == TeamType.OTHER)
            {
                m_CurrentHp -= hitData.Amount;
                OnHit?.Invoke();
            }

            if (m_CurrentHp <= 0)
            {
                m_CurrentHp = 0;
                if (m_UseRender)
                {
                    m_Renderer.enabled = false;
                }

                OnDie?.Invoke();
            }
            
            OnChangeHealth?.Invoke(m_CurrentHp);
        }
    }
}