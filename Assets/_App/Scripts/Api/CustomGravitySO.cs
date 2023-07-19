using System;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    [CreateAssetMenu(fileName = "CustomGravity", menuName = "MobaVR API/Create custom gravity")]
    public class CustomGravitySO : ScriptableObject
    {
        [SerializeField] private float m_Gravity;
        [SerializeField] private float m_TerminalVelocity = 40.0f;

        public float Gravity => m_Gravity;
        public float TerminalVelocity => m_TerminalVelocity;
    }
}