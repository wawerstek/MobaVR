using System;
using BNG;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class HammerSpell : MonoBehaviourPun
    {
        [SerializeField] private float m_Damage = 10f;
        private bool m_IsDamaged = false;
        private Grabbable m_Grabbable;


        private void Awake()
        {
            TryGetComponent(out m_Grabbable);
        }

        public void Throw()
        {
            photonView.RPC(nameof(RpcThrow), RpcTarget.AllBuffered);            
        }

        [PunRPC]
        private void RpcThrow()
        {
            m_Grabbable.enabled = false;
            transform.parent = null;
        }
        
        public void Show(bool isShow)
        {
            photonView.RPC(nameof(RpcShow), RpcTarget.AllBuffered, isShow);
        }
        
        [PunRPC]
        private void RpcShow(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == transform || m_IsDamaged)
            {
                return;
            }

            if ((other.CompareTag("Player") || other.CompareTag("RemotePlayer"))
                && other.transform.TryGetComponent(out WizardPlayer wizardPlayer))
            {
                wizardPlayer.Hit(m_Damage);
                m_IsDamaged = true;
                Hide();
            }

            if (other.CompareTag("Enemy") && other.transform.TryGetComponent(out IHit iHit))
            {
                iHit.RpcHit(m_Damage);
                m_IsDamaged = true;
                Hide();
            }

            if (other.CompareTag("Item"))
            {
                Shield shield = other.GetComponentInParent<Shield>();
                if (shield != null)
                {
                    shield.Hit(1f);
                    m_IsDamaged = true;
                    Hide();
                }
            }
            
            //TODO: refactoring
            //Hide();
        }

        private void Hide()
        {
            if (PhotonNetwork.IsMasterClient && photonView != null)
            {
                PhotonNetwork.Destroy(gameObject);
            }

        }
    }
}