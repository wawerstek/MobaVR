using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    //public class HitCollider : MonoBehaviour
    public class HitCollider : Damageable
    {
        [SerializeField] private WizardPlayer m_WizardPlayer;
        [SerializeField] private NetworkDamageable m_ParentDamageable;
        [SerializeField] private Collider m_Collider;

        public Action OnHit;

        public Collider Collider => m_Collider;

        public WizardPlayer WizardPlayer => m_WizardPlayer;

        private void InitComponents()
        {
            if (m_WizardPlayer == null)
            {
                m_WizardPlayer = GetComponentInParent<WizardPlayer>();
            }

            if (m_Collider == null)
            {
                m_Collider = GetComponent<Collider>();
            }

            if (m_ParentDamageable == null)
            {
                m_ParentDamageable = GetComponentInParent<NetworkDamageable>();
            }
        }
        
        private void OnValidate()
        {
            InitComponents();
        }

        private void Awake()
        {
            InitComponents();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit?.Invoke();
        }

        public void SetEnabledCollider(bool isEnabled)
        {
            m_Collider.enabled = isEnabled;
        }

        public override void Hit(HitData hitData)
        {
            if (m_ParentDamageable != null)
            {
                m_ParentDamageable.Hit(hitData);
            }
        }

        public override void Die()
        {
        }

        public override void Reborn()
        {
        }

        #region Debug

        [ContextMenu("Hit")]
        private void Hit_Debug()
        {
            HitData hitData = new HitData()
            {
                Action = HitActionType.Damage,
                Player = PhotonNetwork.LocalPlayer,
                PhotonOwner = m_WizardPlayer.photonView,
                PlayerVR = m_WizardPlayer.PlayerVR,
                Amount = 50f,
            };

            Hit(hitData);
        }

        #endregion
    }
}