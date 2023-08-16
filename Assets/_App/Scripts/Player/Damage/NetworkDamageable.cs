using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    [RequireComponent(typeof(PhotonView))]
    public class NetworkDamageable : Damageable
    {
        private PhotonView m_PhotonView;

        public UnityEvent<HitData> OnHit;
        public UnityEvent OnDie;
        public UnityEvent OnReborn;

        private void Awake()
        {
            TryGetComponent(out m_PhotonView);
        }

        public override void Hit(HitData hitData)
        {
            if (m_PhotonView != null)
            {
                m_PhotonView.RPC(nameof(RpcHit), RpcTarget.AllBuffered, hitData);
            }
        }

        [PunRPC]
        protected virtual void RpcHit(HitData hitData)
        {
            OnHit?.Invoke(hitData);
        }

        public override void Die()
        {
            if (m_PhotonView != null)
            {
                m_PhotonView.RPC(nameof(RpcDie), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        protected virtual void RpcDie()
        {
            OnDie?.Invoke();
        }

        public override void Reborn()
        {
            if (m_PhotonView != null)
            {
                m_PhotonView.RPC(nameof(RpcReborn), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        protected void RpcReborn()
        {
            OnReborn?.Invoke();
        }
        
        #region Debug

        [ContextMenu("Hit")]
        private void Hit_Debug()
        {
            HitData hitData = new HitData()
            {
                Action = HitActionType.Damage,
                Player = PhotonNetwork.LocalPlayer,
                PhotonOwner = m_PhotonView,
                Amount = 50f,
            };

            Hit(hitData);
        }

        #endregion
    }
}