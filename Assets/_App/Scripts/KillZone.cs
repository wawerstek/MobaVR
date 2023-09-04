using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class KillZone : MonoBehaviourPun
    {
        [SerializeField] private TeamType m_TeamType;
        [SerializeField] private float m_Damage = 1000f;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DieCollider dieCollider))
            {
                if (dieCollider.WizardPlayer.TeamType == m_TeamType)
                {
                    return;
                }
                
                HitData hitData = new HitData()
                {
                    TeamType = m_TeamType,
                    Amount = m_Damage,
                    PhotonView = photonView
                };
                
                dieCollider.TryDie(hitData);
            }
        }
    }
}