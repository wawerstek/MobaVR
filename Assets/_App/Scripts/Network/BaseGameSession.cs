using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class BaseGameSession : MonoBehaviourPunCallbacks
    {
        protected GameObject m_Player;
        protected PlayerVR m_LocalPlayer;
        
        public Action<PlayerVR> OnAddPlayer;
        public Action<PlayerVR> OnRemovePlayer;
        
        public PlayerVR LocalPlayer => m_LocalPlayer;
        public GameObject Player => m_Player;
    }
}