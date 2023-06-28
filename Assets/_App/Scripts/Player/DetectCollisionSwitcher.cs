using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{
    [RequireComponent(typeof(CharacterController))]
    public class DetectCollisionSwitcher : MonoBehaviour
    {
        [SerializeField] private CharacterController m_CharacterController;
        [SerializeField] private bool m_UseDetectCollision;

        private void OnValidate()
        {
            if (m_CharacterController == null)
            {
                TryGetComponent(out m_CharacterController);
            }
        }

        private void Awake()
        {
            if (m_CharacterController != null)
            {
                m_CharacterController.detectCollisions = m_UseDetectCollision;
                m_CharacterController.isTrigger = false;
            }
        }
    }
}