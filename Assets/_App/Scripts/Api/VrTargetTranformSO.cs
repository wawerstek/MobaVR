using System;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    [CreateAssetMenu(fileName = "VRTargetTransform", menuName = "MobaVR API/Create VR Target Transform")]
    public class VrTargetTranformSO : ScriptableObject
    {
        [Header("Head")]
        [SerializeField] private Vector3 m_Position;
        [SerializeField] private Vector3 m_Rotation;

        public Vector3 Position => m_Position;
        public Vector3 Rotation => m_Rotation;
    }
}