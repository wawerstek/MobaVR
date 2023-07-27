using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;

namespace MobaVR
{
    public class LocomotionSwitcher : MonoBehaviour
    {
        [SerializeField] private bool m_IsRealLocomotion;
        
        [SerializeField] private CharacterController m_CharacterController;
        [SerializeField] private BNGPlayerController m_PlayerController;
        [SerializeField] private LocomotionManager m_LocomotionManager;
        [SerializeField] private PlayerRotation m_PlayerRotation;
        [SerializeField] private SmoothLocomotion m_SmoothLocomotion;
        
        private void Start()
        {
            SetLocomotion();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                m_IsRealLocomotion = !m_IsRealLocomotion;
                SetLocomotion();
            }
        }

        public void SetLocomotion()
        {
            if (m_IsRealLocomotion)
            {
                SetRealLocomotion();
            }
            else
            {
                SetInputLocomotion();
            }
        }

        public void SetRealLocomotion()
        {
            m_CharacterController.enabled = false;
            m_PlayerController.enabled = false;
            m_LocomotionManager.enabled = false;
            m_PlayerRotation.enabled = false;
            m_SmoothLocomotion.enabled = false;
        }

        public void SetInputLocomotion()
        {
            m_CharacterController.enabled = true;
            m_PlayerController.enabled = true;
            m_LocomotionManager.enabled = true;
            m_PlayerRotation.enabled = true;
            m_SmoothLocomotion.enabled = true;
        }
    }
}