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

        private GameStatistics m_GameStatistics;

        [SerializeField] [ReadOnly] private List<HitDataTime> m_Hits = new();
        private List<PlayerVR> m_HitPlayers = new List<PlayerVR>();
        private PlayerVR m_LastHitPlayer;
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
            m_GameStatistics = FindObjectOfType<GameStatistics>();
            Reset();
        }

        private void OnPlayerHit(HitData hitData)
        {
            if (hitData.PlayerVR == null)
            {
                return;
            }

            m_LastHitPlayer = hitData.PlayerVR;

            if (!m_HitPlayers.Contains(hitData.PlayerVR))
            {
                m_HitPlayers.Add(hitData.PlayerVR);
            }
        }

        private void OnPlayerDie(HitData hitData)
        {
            m_Killer = hitData.PlayerVR != null ? hitData.PlayerVR : m_LastHitPlayer;

            if (m_Killer != null)
            {
                m_Wizard.PlayerVR.DieView.SetDieInfo(m_Killer.photonView.Owner.NickName);
            }
        }

        private void OnPlayerReborn()
        {
            Reset();
        }

        private void Reset()
        {
            m_HitPlayers.Clear();
            m_Killer = null;
            m_LastHitPlayer = null;
            m_CurrentTime = 0f;
        }
    }
}