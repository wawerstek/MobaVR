using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class RespawnZone : MonoBehaviourPun
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out RebornCollider rebornCollider))
            {
                rebornCollider.TryReborn();
            }
        }
    }
}