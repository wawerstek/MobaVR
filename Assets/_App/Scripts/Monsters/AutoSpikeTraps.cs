using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class AutoSpikeTraps : MonoBehaviourPun
    {
        [SerializeField] private List<Trap> m_Traps = new();
        [SerializeField] private float m_Delay = 2f;
        [SerializeField] private float m_Cooldown = 10f;

        private bool m_IsAvailable = true;

        public float Cooldown => m_Cooldown;
        public bool IsAvailable => m_IsAvailable;

        private void Reset()
        {
            CancelInvoke(nameof(Reset));
            CancelInvoke(nameof(Deactivate));

            m_IsAvailable = true;
        }

        public void Activate()
        {
            if (!m_IsAvailable)
            {
                return;
            }

            /*
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            */

            photonView.RPC(nameof(RpcActivateTrap), RpcTarget.All);
        }

        [PunRPC]
        private void RpcActivateTrap()
        {
            if (!m_IsAvailable)
            {
                return;
            }

            m_IsAvailable = false;

            foreach (Trap trap in m_Traps)
            {
                trap.Activate();
            }

            Invoke(nameof(Reset), m_Cooldown);
            Invoke(nameof(Deactivate), m_Delay);
        }

        public void Deactivate()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            photonView.RPC(nameof(RpcDeactivateTrap), RpcTarget.All);
        }

        [PunRPC]
        private void RpcDeactivateTrap()
        {
            foreach (Trap trap in m_Traps)
            {
                trap.Deactivate();
            }

            m_IsAvailable = false;
        }
    }
}