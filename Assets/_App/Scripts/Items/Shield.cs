using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class Shield : MonoBehaviourPunCallbacks, IHit
    {
        [Header("Values")]
        [SerializeField] protected float m_Health = 1f;
        [SerializeField] protected float m_CooldownTime = 5f;
        [SerializeField] protected float m_ScaleDuration = 0.8f;

        [Header("Components")]
        [SerializeField] protected TimeView m_TimeView;
        [SerializeField] protected TeamItem m_TeamItem;
        [SerializeField] protected GameObject m_Shield;
        [SerializeField] protected Collider m_Collider;

        protected float m_CurrentCooldownTime = 0f;
        protected bool m_Use = false;
        protected bool m_IsAvailable = true;
        protected float m_CurrentHealth = 1f;

        public bool IsLife => m_CurrentHealth > 0f;

        protected virtual void Awake()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            m_IsAvailable = true;
            m_CurrentHealth = m_Health;

            m_Collider.enabled = false;
            m_Shield.SetActive(false);
        }

        public virtual void Show(bool isShow)
        {
            m_Use = isShow;
            photonView.RPC(nameof(RpcShow), RpcTarget.All, isShow);
        }

        [PunRPC]
        public virtual void RpcShow(bool isShow)
        {
            //if (isShow && !m_IsAvailable)
            if (!m_IsAvailable)
            {
                if (photonView.IsMine && m_TimeView != null)
                {
                    m_TimeView.SetTime(m_CurrentCooldownTime);
                    m_TimeView.Show(true);
                }
                return;
            }
        }


        [ContextMenu("Hit")]
        public virtual void Hit()
        {
            photonView.RPC(nameof(RpcHit), RpcTarget.All, 1f);
        }

        public virtual void Hit(Fireball fireball, float damage)
        {
            if ((fireball.Team.IsRed && m_TeamItem.IsRed)
                || (!fireball.Team.IsRed && !m_TeamItem.IsRed))
            {
                return;
            }

            photonView.RPC(nameof(RpcHit), RpcTarget.All, damage);
        }

        [PunRPC]
        public virtual void RpcHit(float damage)
        {
            if (IsLife)
            {
                m_CurrentHealth -= damage;
                if (m_CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }

        [ContextMenu("Die")]
        public virtual void Die()
        {
            if (!IsLife)
            {
                RpcShow(false);

                m_Collider.enabled = false;
                m_IsAvailable = false;

                //Invoke(nameof(Reset), m_CooldownTime);
                m_CurrentCooldownTime = m_CooldownTime;
                DOTween
                    .To(() => m_CurrentCooldownTime, x => m_CurrentCooldownTime = x, 0f, m_CooldownTime)
                    .OnUpdate(() =>
                    {
                        
                    })
                    .OnComplete(() =>
                    {
                        Reset();
                    });
            }
        }

        public virtual void Explode(float explosionForce, Vector3 position, float radius, float modifier)
        {
        }
    }
}