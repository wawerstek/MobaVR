using System;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerState", menuName = "MobaVR API/Create player state")]
    public class PlayerStateSO : ScriptableObject
    {
        [SerializeField] private PlayerState m_PlayerState = PlayerState.PLAY;
        [SerializeField] private bool m_IsInPlayMode = false;
        [SerializeField] private bool m_CanCast = false;
        [SerializeField] private bool m_CanGetDamage = false;
        [SerializeField] private bool m_IsLife = false;

        public PlayerState State => m_PlayerState;
        public bool IsInPlayMode => m_IsInPlayMode;
        public bool CanCast => m_CanCast;
        public bool CanGetDamage => m_CanGetDamage;
        public bool IsLife => m_IsLife;
    }
}