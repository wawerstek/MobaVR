using System;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    [CreateAssetMenu(fileName = "ClassStats", menuName = "MobaVR API/Create class stats")]
    public class ClassStatsSO : ScriptableObject
    {
        [SerializeField] private float m_MaxHp = 100;
        [SerializeField] private float m_RegenHp = 0.5f;
        [SerializeField] private float m_RegenMp = 0.5f;

        public float MaxHp => m_MaxHp;
        public float RegenHp => m_RegenHp;
        public float RegenMp => m_RegenMp;
    }
}