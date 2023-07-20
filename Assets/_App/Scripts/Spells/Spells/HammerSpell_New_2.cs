using System.Collections.Generic;
using BNG;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class HammerSpell_New_2 : ThrowableSpell
    {
        [SerializeField] private Throwable m_Throwable;

        [Space]
        [Header("Magic")]
        [SerializeField] private GameObject m_VfxParent;
        [SerializeField] private GameObject m_Ball;
        [SerializeField] private GameObject m_Trail;

        [SerializeField] private GameObject m_CreateFx;
        [SerializeField] private GameObject m_ProjectileFx;
        [SerializeField] private GameObject m_ExplosionFx;
        [SerializeField] private GameObject m_FailFx;

        [Space]
        [Header("Rising on Enable")]
        [SerializeField] private bool m_IsRisingOnStart = false;
        [SerializeField] private float m_DurationRisingOnStart = 2f;
        [SerializeField] private float m_MaxScaleOnStart = 0.2f;

        [Space]
        [Header("Destroy Time")]
        [SerializeField] private float m_DestroyExplosion = 4.0f;
        [SerializeField] private float m_DestroyChildren = 2.0f;

        [Header("Components")]
        [SerializeField] private Grabbable m_Grabbable;
        [SerializeField] private List<Collider> m_CollisionColliders;
        [SerializeField] private List<Collider> m_TriggerColliders;

        public Grabbable Grabbable => m_Grabbable;

        protected override void OnEnable()
        {
            //base.OnEnable();
            Invoke(nameof(RpcDestroy), m_DestroyLifeTime);

            m_CreateFx.SetActive(true);
            m_ProjectileFx.SetActive(false);
            m_ExplosionFx.SetActive(false);
            m_FailFx.SetActive(false);

            if (!m_IsRisingOnStart)
            {
                m_Ball.transform.localScale = Vector3.zero;
                m_Ball.transform.DOScale(1f, 1f);
            }
            else
            {
                m_Ball.transform.localScale = Vector3.one;
                m_VfxParent.transform.localScale = Vector3.zero;
                m_VfxParent.transform.DOScale(m_MaxScaleOnStart, m_DurationRisingOnStart);
            }

            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.AddListener(OnThrown);
                m_Throwable.OnRedirected.AddListener(OnRedirected);
                m_Throwable.OnThrowChecked.AddListener(OnThrowChecked);
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.RemoveListener(OnThrown);
                m_Throwable.OnRedirected.RemoveListener(OnRedirected);
                m_Throwable.OnThrowChecked.RemoveListener(OnThrowChecked);
            }
        }

        protected override void HandleCollision(Transform interactable)
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RpcDestroy), RpcTarget.All);
            }
        }

        [PunRPC]
        private void RpcDestroy()
        {
            Destroy(m_Ball.gameObject);
            m_ExplosionFx.SetActive(true);
            m_ExplosionFx.transform.parent = null;

            OnDestroySpell?.Invoke();

            Destroy(m_ExplosionFx, m_DestroyExplosion);
            //m_Trail.transform.DetachChildren();
            m_Trail.transform.parent = null;
            Destroy(m_Trail.gameObject, m_DestroyChildren);

            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        [PunRPC]
        private void RpcQuickDestroy()
        {
            m_ExplosionFx.SetActive(true);
            m_ExplosionFx.transform.parent = null;
            Destroy(m_ExplosionFx, m_DestroyExplosion);
            //Destroy(gameObject);

            OnDestroySpell?.Invoke();

            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }

        public override void Init(WizardPlayer wizardPlayer, TeamType teamType)
        {
            base.Init(wizardPlayer, teamType);

            //photonView.RPC(nameof(RpcSwitchGravity), RpcTarget.All, wizardPlayer.GravityFireballType);
            photonView.RPC(nameof(RpcSetPhysics), 
                           RpcTarget.All,
                           wizardPlayer.GravityFireballType,
                           wizardPlayer.ThrowForce, 
                           wizardPlayer.UseAim);
        }

        //TODO: add destroy public method

        //[PunRPC]
        private void RpcSwitchGravity(GravityType gravityType)
        {
            if (m_Throwable != null)
            {
                m_Throwable.PhysicsHandler.GravityType = gravityType;
            }
        }

        //[PunRPC]
        private void RpcSetPhysics(GravityType gravityType, float force, bool useAim)
        {
            if (m_Throwable != null)
            {
                m_Throwable.PhysicsHandler.InitPhysics(gravityType, force, useAim);
            }
        }

        protected override float CalculateDamage()
        {
            return m_DefaultDamage;
        }

        public override void Throw()
        {
            m_IsThrown = true;
            m_Throwable.Throw();
        }

        public override void ThrowByDirection(Vector3 direction)
        {
            m_IsThrown = true;
            m_Throwable.ThrowByDirection(direction);
        }

        //[PunRPC]
        private void RpcThrowByDirection(Vector3 direction)
        {
            if (m_Grabbable != null)
            {
                m_Grabbable.DropItem(true, true);
            }

            m_IsThrown = true;
            m_Ball.transform.parent = m_Trail.transform;
            m_ProjectileFx.SetActive(true);
        }

        private void OnThrown()
        {
            m_IsThrown = true;

            foreach (Collider collisionCollider in m_CollisionColliders)
            {
                collisionCollider.enabled = false;
            }

            m_Ball.transform.parent = m_Trail.transform;
            m_CreateFx.SetActive(false);
            m_ProjectileFx.SetActive(true);
        }

        private void OnThrowChecked(bool isGoodThrow)
        {
            if (!isGoodThrow)
            {
                photonView.RPC(nameof(RpcQuickDestroy), RpcTarget.All);
                StopAllCoroutines();
            }
        }

        private void OnRedirected(Vector3 arg0)
        {
            m_IsThrown = true;
            m_Ball.transform.parent = m_Trail.transform;
            m_ProjectileFx.SetActive(true);
        }
    }
}