using System;
using DamageNumbersPro;
using UnityEngine;

namespace MobaVR
{
    public class PlayerDamageNumber : MonoBehaviour
    {
        [SerializeField] private DamageNumber m_DamageNumber;
        [SerializeField] private Color m_DefaultColor = new Color(0.82f, 0.37f, 0.36f);

        private WizardPlayer m_WizardPlayer;
        
        private void OnEnable()
        {
            if (m_WizardPlayer != null)
            {
                m_WizardPlayer.OnPlayerHit += OnHit;
            }
        }

        private void OnDisable()
        {
            if (m_WizardPlayer != null)
            {
                m_WizardPlayer.OnPlayerHit -= OnHit;
            }
        }

        private void Awake()
        {
            m_WizardPlayer = GetComponentInParent<WizardPlayer>();
        }

        public void OnHit(HitData hitData)
        {
            if (hitData.Action != HitActionType.Damage)
            {
                return;
            }

            DamageNumber damageNumber = Instantiate(m_DamageNumber);
            damageNumber.faceCameraView = true;
            damageNumber.transform.position = transform.position;
            damageNumber.transform.localScale = Vector3.one;
            damageNumber.number = hitData.Amount;
            damageNumber.numberSettings.customColor = true;

            damageNumber.numberSettings.color = m_DefaultColor;
            damageNumber.transform.localScale = Vector3.one;
        }
    }
}