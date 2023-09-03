using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class LichSpell : MonsterSpell
    {
        [Header("VFX")]
        [SerializeField] private ParticleSystem m_CastVfx;
        [SerializeField] private ParticleSystem m_ShootVfx;
        [SerializeField] private float m_ShootDelay = 5f;
        [SerializeField] private float m_ShootDuration = 5f;
        
        [Header("Spell")]
        [SerializeField] private float m_Damage = 50f;
        [SerializeField] private float m_Cooldown = 20f;
        [SerializeField] private Collider m_Collider;

        private bool m_IsAvailable = true;
        private List<WizardPlayer> m_HitPlayers = new();

        public Action OnReset;
        public Action OnCast;
        public Action OnShoot;
        public Action<Collider> OnHit;

        public bool IsAvailable => m_IsAvailable;
        public float Damage => m_Damage;
        public float Cooldown => m_Cooldown;

        private void Awake()
        {
            RpcReset();
        }

        public void Reset()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcReset), RpcTarget.All);
            }
        }

        [PunRPC]
        public void RpcReset()
        {
            CancelInvoke(nameof(Shoot));
            CancelInvoke(nameof(Reset));

            m_HitPlayers.Clear();
            
            m_Collider.enabled = false;
            m_CastVfx.Stop();
            m_ShootVfx.Stop();
            m_IsAvailable = true;
            
            OnReset?.Invoke();
        }

        [ContextMenu("Activate")]
        public override void Activate()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcActivate), RpcTarget.All);
            }
        }

        [PunRPC]
        public void RpcActivate()
        {
            OnCast?.Invoke();
            
            m_Collider.enabled = false;
            Cast();
        }

        public void Cast()
        {
            m_CastVfx.Play();
            m_Collider.enabled = false;

            Invoke(nameof(Shoot), m_ShootDelay);
        }

        public void Shoot()
        {
            OnShoot?.Invoke();
            
            m_Collider.enabled = true;
            m_ShootVfx.Play();

            CancelInvoke(nameof(Shoot));
            Invoke(nameof(Reset), m_ShootDuration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            /*
            if (other.TryGetComponent(out Damageable damageable))
            {
                HitData hitData = new HitData()
                {
                    Action = HitActionType.Damage,
                    TeamType = TeamType.OTHER,
                    PhotonOwner = photonView,
                    PhotonView = photonView,
                    Amount = m_Damage,
                };
                
                OnHit?.Invoke(other);
                damageable.Hit(hitData);
            }
            */

            if (other.CompareTag("LifeCollider") && other.TryGetComponent(out HitCollider hitCollider))
            {
                if (m_HitPlayers.Contains(hitCollider.WizardPlayer))
                {
                    return;
                }
                
                HitData hitData = new HitData()
                {
                    Action = HitActionType.Damage,
                    TeamType = TeamType.OTHER,
                    PhotonOwner = photonView,
                    PhotonView = photonView,
                    Amount = m_Damage,
                };
                
                OnHit?.Invoke(other);
                m_HitPlayers.Add(hitCollider.WizardPlayer);
                
                hitCollider.Hit(hitData);
            }
        }
    }
}