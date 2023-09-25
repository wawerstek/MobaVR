using System;
using Michsky.MUIP;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace MobaVR
{
    public class ManaView : MonoBehaviourPun
    {
        
        public Image healthImage; // Это для индикатора здоровья
        private float currentHealth;
        
        
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
            //m_SpellBehaviour.OnCompleted += OnCompleted;
            m_SpellBehaviour.OnPerformed += OnPerformed;
            m_CooldownTime = m_SpellBehaviour.CooldownTime;

            if (photonView.IsMine)
            {
                m_ProgressBar.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            m_SpellBehaviour.OnCompleted -= OnCompleted;

            if (photonView.IsMine)
            {
                m_ProgressBar.gameObject.SetActive(false);
            }
        }

        private void OnPerformed()
        {
            m_CurrentTime = 0f;
            m_ProgressBar.currentPercent = 0f;
            m_ProgressBar.UpdateUI();
            
            UpdateHealthImage();//обновляем картинку
            
            
            m_IsCompleted = false;
        }
        
        private void OnCompleted()
        {
            /*
            if (!m_SpellBehaviour.IsPerformed())
            {
                return;
            }
            */
            
            m_CurrentTime = 0f;
            m_ProgressBar.currentPercent = 0f;
            m_ProgressBar.UpdateUI();
            UpdateHealthImage();//обновляем картинку
            m_IsCompleted = true;
        }

        
        
        private void UpdateHealthImage()
        {
            if (healthImage != null) 
            {
                healthImage.fillAmount = m_ProgressBar.currentPercent / 100.0f;
            }
        }

        
        
        
        
        
        
        private void Update()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (m_SpellBehaviour.IsAvailable || !m_SpellBehaviour.UseCooldown)
            {
                m_ProgressBar.currentPercent = 100f;
                m_ProgressBar.UpdateUI();
                UpdateHealthImage();//обновляем картинку
                
            }
            else
            {
                m_CurrentTime = m_SpellBehaviour.CurrentTime;
                m_ProgressBar.currentPercent = m_CurrentTime / m_CooldownTime * 100f;
                m_ProgressBar.UpdateUI();
                UpdateHealthImage();//обновляем картинку
            }
            
            /*
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
            */
        }
    }
}