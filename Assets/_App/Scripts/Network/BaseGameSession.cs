using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class BaseGameSession : MonoBehaviourPunCallbacks
    {
        protected GameObject m_Player;
        protected PlayerVR m_LocalPlayer;
        
        public PlayerVR LocalPlayer => m_LocalPlayer;
        public GameObject Player => m_Player;
    }
}