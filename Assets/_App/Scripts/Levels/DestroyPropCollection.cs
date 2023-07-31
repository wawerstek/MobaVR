using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class DestroyPropCollection : MonoBehaviourPun
    {
        [SerializeField] private List<destruction> m_Destructions = new();

        private void OnValidate()
        {
            if (m_Destructions.Count == 0)
            {
                m_Destructions.AddRange(GetComponentsInChildren<destruction>());
            }
        }

        [ContextMenu("Restore")]
        public void Restore()
        {
            photonView.RPC(nameof(RpcRestore), RpcTarget.All);
        }

        [PunRPC]
        private void RpcRestore()
        {
            foreach (destruction destruction in m_Destructions)
            {
                destruction.Restore();
            }
        }
    }
}