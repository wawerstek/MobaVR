using System;
using UnityEngine;
using UnityEngine.UI;

namespace MobaVR
{
    public class DamageIndicator : BaseDamageIndicator
    {
        [SerializeField] private Animator m_Animator;
        [SerializeField] private bool m_IsOverrideColor;
        [SerializeField] private Image m_DamageImage;
        [SerializeField] private Color m_BloodColor;

        private readonly int m_ShowAnimId = Animator.StringToHash("damage");
        
        private void Awake()
        {
            if (m_IsOverrideColor)
            {
                m_DamageImage.color = m_BloodColor;
            }

            m_DamageImage.enabled = false;
        }

        private void ResetTriggers()
        {
            m_Animator.ResetTrigger(m_ShowAnimId);
        }

        public override void Show()
        {
            m_DamageImage.enabled = true;
            m_Animator.SetTrigger(m_ShowAnimId);
            Invoke(nameof(ResetTriggers), 0.4f);
        }

        public override void Hide()
        {
        }
    }
}