using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobaVR
{
    public class SkinIkTargets : MonoBehaviour
    {
        [SerializeField] private Skin m_Skin;

        [Header("Head")]
        [SerializeField] private bool m_IsOverrideHeadPosition = true;
        [SerializeField] private Transform m_HeadVrTarget;
        [SerializeField] private VrTargetTranformSO m_HeadTransformSO;

        [Header("Left Hand")]
        [SerializeField] private bool m_IsOverrideLeftHandPosition = true;
        [SerializeField] private Transform m_LeftHandVrTarget;
        [SerializeField] private VrTargetTranformSO m_LeftHandTransformSO;

        [Header("Right Hand")]
        [SerializeField] private bool m_IsOverrideRightHandPosition = true;
        [SerializeField] private Transform m_RightHandVrTarget;
        [SerializeField] private VrTargetTranformSO m_RightHandTransformSO;

        private void OnValidate()
        {
            if (m_Skin == null)
            {
                TryGetComponent(out m_Skin);
            }
        }

        private void OnEnable()
        {
            if (m_Skin != null)
            {
                m_Skin.OnActivated.AddListener(UpdateTargets);
            }
        }

        private void OnDisable()
        {
            if (m_Skin != null)
            {
                m_Skin.OnActivated.RemoveListener(UpdateTargets);
            }
        }

        public void UpdateTargets()
        {
            if (m_IsOverrideHeadPosition)
            {
                m_HeadVrTarget.localPosition = m_HeadTransformSO.Position;
                m_HeadVrTarget.localRotation = Quaternion.Euler(m_HeadTransformSO.Rotation);
            }

            if (m_IsOverrideLeftHandPosition)
            {
                m_LeftHandVrTarget.localPosition = m_LeftHandTransformSO.Position;
                m_LeftHandVrTarget.localRotation = Quaternion.Euler(m_LeftHandTransformSO.Rotation);
            }

            if (m_IsOverrideRightHandPosition)
            {
                m_RightHandVrTarget.localPosition = m_RightHandTransformSO.Position;
                m_RightHandVrTarget.localRotation = Quaternion.Euler(m_RightHandTransformSO.Rotation);
            }
        }
    }
}