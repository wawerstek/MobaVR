using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class Tower : MonoBehaviourPun
    {
        [SerializeField] private MeshRenderer m_Renderer;
        [SerializeField] private float m_MaxHp = 1000;
        [SerializeField] private float m_CurrentHp;

        public UnityEvent OnDie;
        public UnityEvent OnHit;
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
            m_Renderer.enabled = true;
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
                m_Renderer.enabled = false;
                OnDie?.Invoke();
            }
        }
    }
}