using System;
using System.Diagnostics;
using BNG;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class LeverSpikeTrap : MonoBehaviourPun
    {
        [Header("Trap")]
        [SerializeField] private ManualTraps m_Traps;
        [SerializeField] private Lever m_Lever;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private float m_Cooldown = 10f;

        [Header("Render")]
        [SerializeField] private Renderer m_LeverRenderer;
        [SerializeField] private Material m_EnableMaterial;
        [SerializeField] private Material m_DisableMaterial;

        private bool m_IsAvailable = true;

        public float Cooldown => m_Cooldown;
        public bool IsAvailable => m_IsAvailable;

        private void OnEnable()
        {
            m_Lever.onLeverDown.AddListener(Activate);
            m_Lever.onLeverUp.AddListener(Activate);
        }
        
        private void OnDisable()
        {
            m_Lever.onLeverDown.RemoveListener(Activate);
            m_Lever.onLeverUp.RemoveListener(Activate);
        }

        private void Reset()
        {
            CancelInvoke(nameof(Reset));

            m_Collider.enabled = true;
            m_Lever.enabled = true;
            m_LeverRenderer.material = m_EnableMaterial;
            
            m_IsAvailable = true;
        }

        public void Activate()
        {
            if (!m_IsAvailable)
            {
                return;
            }
            
            photonView.RPC(nameof(RpcActivateTrap), RpcTarget.All);
        }

        [PunRPC]
        private void RpcActivateTrap()
        {
            if (!m_IsAvailable)
            {
                return;
            }

            m_IsAvailable = false;
            m_Traps.Activate();

            m_Collider.enabled = false;
            //m_Lever.enabled = false;
            m_LeverRenderer.material = m_DisableMaterial;

            Invoke(nameof(Reset), m_Cooldown);
        }
    }
}