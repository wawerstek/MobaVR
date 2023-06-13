using System;
using BNG;
using UnityEngine;

namespace MobaVR
{
    public class LocomotionVREmulator : VREmulator
    {
        [SerializeField] private SmoothLocomotion m_SmoothLocomotion;
        
        [Header("Move Settings")]
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_StrafeSpeed;
        [SerializeField] private float m_SprintSpeed;
        [SerializeField] private float m_JumpForce;

        private void Awake()
        {
            if ((Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                && EmulatorEnabled)
            {
                m_SmoothLocomotion.MovementSpeed = m_MoveSpeed;
                m_SmoothLocomotion.SprintSpeed = m_SprintSpeed;
                m_SmoothLocomotion.StrafeSpeed = m_StrafeSpeed;
                m_SmoothLocomotion.JumpForce = m_JumpForce;
            }
        }
    }
}