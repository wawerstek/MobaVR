using System;
using Michsky.MUIP;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class ManaView : MonoBehaviourPun
    {
        [SerializeField] private SpellBehaviour m_SpellBehaviour;
        [SerializeField] private float m_CooldownTime = 30f;
        [SerializeField] private ProgressBar m_ProgressBar;

        private float m_CurrentTime = 0f;
        private bool m_IsCompleted = true;
        
        private void Start()
        {
            if (photonView.IsMine && m_SpellBehaviour.isActiveAndEnabled)
            {
                m_ProgressBar.gameObject.SetActive(true);
                m_ProgressBar.currentPercent = 0;
                m_ProgressBar.UpdateUI();
            }
            else
            {
                m_ProgressBar.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            m_SpellBehaviour.OnCompleted += OnCompleted;
            m_SpellBehaviour.OnPerformed += OnPerformed;
            m_CooldownTime = m_SpellBehaviour.CooldownTime;
        }

        private void OnDisable()
        {
            m_SpellBehaviour.OnCompleted -= OnCompleted;
        }

        private void OnPerformed()
        {
            m_CurrentTime = 0f;
            m_ProgressBar.currentPercent = 0f;
            m_ProgressBar.UpdateUI();
            m_IsCompleted = false;
        }
        
        private void OnCompleted()
        {
            m_CurrentTime = 0f;
            m_ProgressBar.currentPercent = 0f;
            m_ProgressBar.UpdateUI();
            m_IsCompleted = true;
        }

        private void Update()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            if ((m_SpellBehaviour.IsAvailable && !m_SpellBehaviour.IsPerformed()) || !m_SpellBehaviour.UseCooldown)
            {
                m_ProgressBar.currentPercent = 100;
                m_ProgressBar.UpdateUI();
            }
            else
            {
                if (m_IsCompleted)
                {
                    m_CurrentTime += Time.deltaTime;
                    m_ProgressBar.currentPercent = m_CurrentTime / m_CooldownTime * 100f;
                    m_ProgressBar.UpdateUI();
                }
            }
        }
    }
}