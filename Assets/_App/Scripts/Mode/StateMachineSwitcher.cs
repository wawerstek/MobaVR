using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class StateMachineSwitcher : MonoBehaviourPun
    {
        [SerializeField] private ClassicMode m_ClassicMode;
        [SerializeField] private StateMachine m_ClassicStateMachine;
        [SerializeField] private StateMachine m_TimerStateMachine;

        public void SetClassMode()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcSetClassMode), RpcTarget.All);
            }
        }

        [PunRPC]
        private void RpcSetClassMode()
        {
            if (m_ClassicMode != null)
            {
                m_ClassicMode.SetStateMachine(m_ClassicStateMachine);
            }
        }

        public void SetTimerMode()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcSetTimerMode), RpcTarget.All);
            }
        }

        [PunRPC]
        private void RpcSetTimerMode()
        {
            if (m_ClassicMode != null)
            {
                m_ClassicMode.SetStateMachine(m_TimerStateMachine);
            }
        }
    }
}