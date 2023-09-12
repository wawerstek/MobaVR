using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class Shield : MonoBehaviourPunCallbacks
        //, IHit
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
        [SerializeField] protected ShieldView m_View;

        protected float m_CurrentCooldownTime = 0f;
        protected bool m_Use = false;
        protected bool m_IsAvailable = true;
        [SerializeField] [ReadOnly] protected float m_CurrentHealth = 1f;

        public UnityEvent OnShow;
        public UnityEvent OnHide;
        public UnityEvent OnHit;
        public UnityEvent OnDestroy;

        public bool IsLife => m_CurrentHealth > 0f;
        public TeamType TeamType => m_TeamItem != null ? m_TeamItem.TeamType : TeamType.RED;

        protected virtual void Awake()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            m_IsAvailable = true;
            m_CurrentHealth = m_Health;

            if (m_View != null)
            {
                m_View.gameObject.SetActive(false);
                m_View.RpcSetDefenceValue(m_CurrentHealth);
            }

            m_Collider.enabled = false;
            m_Shield.SetActive(false);
        }

        protected virtual void UpdateVisualState()
        {
        }

        public virtual void Show(bool isShow)
        {
            m_Use = isShow;
            photonView.RPC(nameof(RpcShow), RpcTarget.All, isShow);
        }

        [PunRPC]
        public virtual void RpcShow(bool isShow)
        {
            if (isShow)
            {
                OnShow?.Invoke();
            }
            else
            {
                OnHide?.Invoke();
            }
            
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
        protected virtual void Hit()
        {
            photonView.RPC(nameof(RpcHit), RpcTarget.All, 1f);
        }

        public virtual void Hit(HitData hitData)
        {
            if (hitData.TeamType == TeamType)
            {
                return;
            }
            
            RpcHit(hitData.Amount);
        }

        protected virtual void Hit(float damage)
        {
            photonView.RPC(nameof(RpcHit), RpcTarget.All, damage);
        }

        protected virtual void Hit(ThrowableSpell fireball, float damage)
        {
            if ((fireball.Team.IsRed && m_TeamItem.IsRed)
                || (!fireball.Team.IsRed && !m_TeamItem.IsRed))
            {
                return;
            }

            if (fireball is SmallFireBall)
            {
                //TODO: shield bug
                //return;
            }

            photonView.RPC(nameof(RpcHit), RpcTarget.All, damage);
        }

        [PunRPC]
        protected virtual void RpcHit(float damage)
        {
            if (IsLife)
            {
                m_CurrentHealth -= damage;
                if (m_View != null)
                {
                    m_View.RpcSetDefenceValue(m_CurrentHealth);
                }

                UpdateVisualState();
                if (m_CurrentHealth <= 0)
                {
                    OnDestroy?.Invoke();
                    Die();
                }
                else
                {
                    OnHit?.Invoke();
                }
            }
        }

        [ContextMenu("Die")]
        protected virtual void Die()
        {
            if (!IsLife)
            {
                RpcShow(false);

                m_Collider.enabled = false;
                m_IsAvailable = false;

                //Invoke(nameof(Reset), m_CooldownTime);
                WaitBeforeReset();
            }
        }

        protected void WaitBeforeReset()
        {
            m_CurrentCooldownTime = m_CooldownTime;
            DOTween
                .To(() => m_CurrentCooldownTime, x => m_CurrentCooldownTime = x, 0f, m_CooldownTime)
                .OnUpdate(() => { })
                .OnComplete(() => { Reset(); });
        }

        public virtual void Explode(float explosionForce, Vector3 position, float radius, float modifier)
        {
        }
    }
}