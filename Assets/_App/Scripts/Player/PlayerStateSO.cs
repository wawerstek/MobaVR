using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobaVR
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerState", menuName = "MobaVR API/Create player state")]
    public class PlayerStateSO : ScriptableObject
    {
        [SerializeField] private PlayerState m_PlayerState = PlayerState.PLAY_PVP;
        [SerializeField] private bool m_IsInPlayMode = false;
        [SerializeField] private bool m_CanCast = false;
        [SerializeField] private bool m_CanGetDamage = false;
        [SerializeField] private bool m_CanGetDamageFromFriendPlayers = false;
        [SerializeField] private bool m_CanGetDamageFromEnemyPlayers = true;
        [SerializeField] private bool m_IsLife = false;

        public PlayerState State => m_PlayerState;
        public bool IsInPlayMode => m_IsInPlayMode;
        public bool CanCast => m_CanCast;
        public bool CanGetDamage => m_CanGetDamage;
        public bool IsLife => m_IsLife;
        public bool CanGetDamageFromFriendPlayers => m_CanGetDamageFromFriendPlayers;
        public bool CanGetDamageFromEnemyPlayers => m_CanGetDamageFromEnemyPlayers;
    }
}