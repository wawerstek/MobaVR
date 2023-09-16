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

        private List<PlayerVR> m_HitPlayers;
        private PlayerVR m_Killer;
        private float m_CurrentTime = 0f;

        private void OnEnable()
        {
            if (m_Wizard != null)
            {
                m_Wizard.OnPlayerHit += OnPlayerHit;
                m_Wizard.OnPlayerDie += OnPlayerDie;
                m_Wizard.OnReborn += OnPlayerReborn;
            }
        }

        private void OnDisable()
        {
            if (m_Wizard != null)
            {
                m_Wizard.OnPlayerHit -= OnPlayerHit;
                m_Wizard.OnPlayerDie -= OnPlayerDie;
                m_Wizard.OnReborn -= OnPlayerReborn;
            }
        }

        private void Awake()
        {
            m_Wizard = GetComponent<WizardPlayer>();
            Reset();
        }

        private void OnPlayerHit(HitData hitData)
        {
        }

        private void OnPlayerDie(HitData hitData)
        {
        }

        private void OnPlayerReborn()
        {
        }

        private void Reset()
        {
            m_HitPlayers.Clear();
            m_Killer = null;
            m_CurrentTime = 0f;
        }
    }
}