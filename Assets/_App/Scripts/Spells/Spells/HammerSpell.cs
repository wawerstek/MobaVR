using System.Collections.Generic;
using BNG;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class HammerSpell : ThrowableSpell
    {
        [SerializeField] private Throwable m_Throwable;

        [Space]
        [Header("Magic")]
        [SerializeField] private GameObject m_HammerMesh;

        [SerializeField] private GameObject m_ProjectileFx;
        [SerializeField] private GameObject m_ExplosionFx;

        [Header("Destroy Time")]
        [SerializeField] private float m_DestroyExplosion = 4.0f;
        [SerializeField] private float m_DestroyChildren = 2.0f;

        [Header("Components")]
        [SerializeField] private Grabbable m_Grabbable;
        [SerializeField] private List<Collider> m_CollisionColliders;
        [SerializeField] private List<Collider> m_TriggerColliders;

        private Grabber m_Grabber;
        
        public Grabbable Grabbable => m_Grabbable;
        public Throwable Throwable => m_Throwable;

        protected override void OnEnable()
        {
            //base.OnEnable();
            //Invoke(nameof(RpcDestroy), m_DestroyLifeTime); //TODO

            m_HammerMesh.SetActive(false);
            m_ProjectileFx.SetActive(false);
            m_ExplosionFx.SetActive(false);

            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.AddListener(OnThrown);
                m_Throwable.OnRedirected.AddListener(OnRedirected);
                m_Throwable.OnThrowChecked.AddListener(OnThrowChecked);
                m_Throwable.OnGrabbed.AddListener(OnGrabbed);
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
                photonView.RPC(nameof(RpcDestroy), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RpcDestroy()
        {
            m_ExplosionFx.SetActive(true);
            m_ExplosionFx.transform.parent = null;

            OnDestroySpell?.Invoke();

            Destroy(m_ExplosionFx, m_DestroyExplosion);

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

        //REMOVE
        /*
        public override void Throw()
        {
            m_Throwable.Throw();
        }

        public override void ThrowByDirection(Vector3 direction)
        {
            m_Throwable.ThrowByDirection(direction);
        }
        */

        //[PunRPC]
        private void RpcThrowByDirection(Vector3 direction)
        {
            if (m_Grabbable != null)
            {
                m_Grabbable.DropItem(true, true);
            }

            m_IsThrown = true;
            m_ProjectileFx.SetActive(true);
        }

        private void DestroySpell()
        {
            if (gameObject.activeSelf)
            {
                photonView.RPC(nameof(RpcDestroy), RpcTarget.AllBuffered);
                StopAllCoroutines();
            }
        }

        private void ShowWeapon()
        {
            m_HammerMesh.SetActive(true);
            photonView.RPC(nameof(RpcShowWeapon), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void RpcShowWeapon()
        {
            m_HammerMesh.SetActive(true);
        }
        
        private void OnGrabbed(Grabber grabber)
        {
            m_Grabber = grabber;
            Invoke(nameof(ShowWeapon), 0.2f);
            
            foreach (Collider collisionCollider in m_CollisionColliders)
            {
                //collisionCollider.enabled = false;
            }
        }

        private void OnThrown()
        {
            m_IsThrown = true;

            foreach (Collider collisionCollider in m_CollisionColliders)
            {
                //collisionCollider.enabled = false;
            }

            m_ProjectileFx.SetActive(true);
            
            Invoke(nameof(DestroySpell), m_DestroyLifeTime);
        }

        private void OnThrowChecked(bool isGoodThrow)
        {
            if (!isGoodThrow)
            {
                photonView.RPC(nameof(RpcDestroy), RpcTarget.AllBuffered);
                StopAllCoroutines();
            }
        }

        private void OnRedirected(Vector3 arg0)
        {
            m_IsThrown = true;
            m_ProjectileFx.SetActive(true);
        }
    }
}