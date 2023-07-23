using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class FireBreath : MonoBehaviourPun
    {
        [SerializeField] private float m_Damage = 2f;
        [SerializeField] private ParticleSystem m_ParticleSystem;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private TeamItem m_TeamItem;

        private void Awake()
        {
            RpcShow(false);
        }

        public void Show(bool isShow)
        {
            photonView.RPC(nameof(RpcShow), RpcTarget.All, isShow);
        }

        [PunRPC]
        public virtual void RpcShow(bool isShow)
        {
            //if (photonView.IsMine)
            {
                if (isShow)
                {
                    m_Collider.enabled = true;
                    m_ParticleSystem.Play();
                }
                else
                {
                    m_Collider.enabled = false;
                    m_ParticleSystem.Stop();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            
            if (other.TryGetComponent(out IHit hitEnemy))
            {
                hitEnemy.RpcHit(m_Damage);
                return;
            }

            if (other.TryGetComponent(out HitCollider hitCollider))
            {
                if (hitCollider.WizardPlayer.TeamType != m_TeamItem.TeamType)
                {
                    hitCollider.WizardPlayer.Hit(m_Damage);
                }
            }
        }
    }
}