using System;
using System.Collections.Generic;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    [RequireComponent(typeof(PhotonView))]
    public class HitPlayerInfo : MonoBehaviourPun
    {
        [SerializeField] private float m_HitCooldown = 20f;
        private WizardPlayer m_Wizard;

        private void OnEnable()
        {
            if (m_Wizard != null)
            {
                m_Wizard.OnPlayerHit += OnPlayerHit;
                m_Wizard.OnPlayerDie += OnPlayerDie;
            }
        }

        private void OnDisable()
        {
            if (m_Wizard != null)
            {
                m_Wizard.OnPlayerHit += OnPlayerHit;
                m_Wizard.OnPlayerDie += OnPlayerDie;
            }
        }

        private void Awake()
        {
            m_Wizard = GetComponent<WizardPlayer>();
        }

        private void OnPlayerHit(HitData hitData)
        {
        }

        private void OnPlayerDie(HitData hitData)
        {
        }
    }
}