using System;
using UnityEngine;

namespace MobaVR
{
    public class DieCollider : MonoBehaviour
    {
        [SerializeField] private WizardPlayer m_WizardPlayer;

        public WizardPlayer WizardPlayer => m_WizardPlayer;

        public bool TryDie(HitData hitData)
        {
            if (m_WizardPlayer != null
                && m_WizardPlayer.photonView.IsMine
                && m_WizardPlayer.IsLife
                && m_WizardPlayer.TryGetComponent(out NetworkDamageable networkDamageable))
            {
                networkDamageable.Hit(hitData);
                return true;
            }

            return false;
        }
    }
}