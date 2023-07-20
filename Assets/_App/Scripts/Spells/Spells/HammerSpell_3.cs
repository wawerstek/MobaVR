using BNG;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class HammerSpell_3 : ThrowableSpell
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
        [Header("Rising on Thrown")]
        [SerializeField] private float m_DurationRisingOnThrow = 1.5f;
        [SerializeField] private float m_MaxScaleOnThrow = 1.2f;

        [Space]
        [Header("Rising on Fly")]
        [SerializeField] private float m_DurationRisingOnFly = 3f;
        [SerializeField] private float m_MaxScaleOnFly = 5f;

        [Space]
        [Header("Destroy Time")]
        [SerializeField] private float m_DestroyExplosion = 4.0f;
        [SerializeField] private float m_DestroyChildren = 2.0f;

        [Space]
        [Header("Explosion Wave")]
        [SerializeField] private bool m_UseExplosionWave = false;
        [SerializeField] private LayerMask m_ExplosionLayers;
        [SerializeField] private float m_ExplosionCollisionRadius = 10f;
        [SerializeField] private float m_ExplosionForce = 400f;
        [SerializeField] private float m_ExplosionForceRadius = 4f;
        [SerializeField] private float m_ExplosionModifier = 2f;

        [Header("Components")]
        [SerializeField] private Grabbable m_Grabbable;
        [SerializeField] private SphereCollider m_CollisionCollider;
        [SerializeField] private SphereCollider m_TriggerCollider;

        private float m_InitColliderRadius = 0.1f;
        private float m_InitTriggerRadius = 0.12f;
        private float m_ColliderScale = 2f;

        private float m_ScaleFactor = 1f;

        public Grabbable Grabbable => m_Grabbable;

        protected override void OnEnable()
        {
            //base.OnEnable();
            Invoke(nameof(RpcDestroyBall), m_DestroyLifeTime);

            m_CreateFx.SetActive(true);
            m_ProjectileFx.SetActive(false);
            m_ExplosionFx.SetActive(false);
            m_FailFx.SetActive(false);

            //TODO: Check??
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

        private void Explode(Transform interactable)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position,
                                                         m_ExplosionCollisionRadius,
                                                         m_ExplosionLayers,
                                                         QueryTriggerInteraction.Collide);
            foreach (Collider enemy in colliders)
            {
                if (enemy.TryGetComponent(out Rigidbody rigidbody))
                {
                    if (!rigidbody.isKinematic)
                    {
                        rigidbody.AddExplosionForce(m_ExplosionForce,
                                                    transform.position,
                                                    m_ExplosionForceRadius,
                                                    m_ExplosionModifier);
                    }
                }
            }
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            if (m_IsThrown && !collision.transform.CompareTag("Player"))
            {
                //Explode(collision.transform);
            }
        }

        protected override void HandleCollision(Transform interactable)
        {
            //Explode(interactable);

            if (photonView.IsMine)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position,
                                                             //m_ExplosionCollisionRadius,
                                                             m_ExplosionCollisionRadius + m_TriggerCollider.radius,
                                                             m_ExplosionLayers,
                                                             QueryTriggerInteraction.Collide);
                foreach (Collider enemy in colliders)
                {
                    //if (enemy.CompareTag("Enemy") && enemy.TryGetComponent(out IHit hitEnemy))
                    if (enemy.TryGetComponent(out IHit hitEnemy))
                    {
                        hitEnemy.RpcHit(CalculateDamage());
                        hitEnemy.Explode(m_ExplosionForce,
                                         transform.position,
                                         m_ExplosionForceRadius,
                                         m_ExplosionModifier);
                    }
                }

                photonView.RPC(nameof(RpcDestroyBall), RpcTarget.All);
            }
        }

        [PunRPC]
        private void RpcDestroyBall()
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
        private void RpcFailDestroyBall()
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

        private void UpdateColliderRadius(TweenerCore<Vector3, Vector3, VectorOptions> ballScale)
        {
            float value = m_VfxParent.transform.localScale.x;
            m_CollisionCollider.radius = value / 2f;
            m_TriggerCollider.radius = value / 1.65f;
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
            /*
            float damage = m_DefaultDamage * m_ScaleFactor;
            if (m_VfxParent.transform.localScale.x > 1f)
            {
                damage *= m_VfxParent.transform.localScale.x;
            }
            */

            float scaleFactor = m_ScaleFactor + m_VfxParent.transform.localScale.x;
            float damage = m_DefaultDamage * scaleFactor;
            return damage;
        }

        public override void Throw()
        {
            if (!m_IsThrown)
            {
                m_ScaleFactor = 2.5f;
            }

            m_IsThrown = true;
            m_Throwable.Throw();
        }

        public override void ThrowByDirection(Vector3 direction)
        {
            if (!m_IsThrown)
            {
                m_ScaleFactor = 1f;
            }

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
            if (!m_IsThrown)
            {
                m_ScaleFactor = 2.5f;
            }

            m_IsThrown = true;

            m_Ball.transform.parent = m_Trail.transform;
            m_CreateFx.SetActive(false);
            m_ProjectileFx.SetActive(true);

            TweenerCore<Vector3, Vector3, VectorOptions> ballScale =
                m_VfxParent.transform
                           //.DOScale(m_VfxParent.transform.localScale.x * 4f, 4f);
                           //.DOScale(m_VfxParent.transform.localScale.x * 6f, 1.2f);
                           .DOScale(m_MaxScaleOnThrow, m_DurationRisingOnThrow);


            ballScale.onUpdate = () => { UpdateColliderRadius(ballScale); };

            ballScale.onComplete = () =>
            {
                m_VfxParent.transform
                           .DOScale(m_MaxScaleOnFly, m_DurationRisingOnFly)
                           .onUpdate = () => { UpdateColliderRadius(ballScale); };
            };
        }

        private void OnThrowChecked(bool isGoodThrow)
        {
            if (!isGoodThrow)
            {
                photonView.RPC(nameof(RpcFailDestroyBall), RpcTarget.All);
                StopAllCoroutines();
            }
        }

        private void OnRedirected(Vector3 arg0)
        {
            if (!m_IsThrown)
            {
                m_ScaleFactor = 1f;
            }

            m_IsThrown = true;
            m_Ball.transform.parent = m_Trail.transform;
            m_ProjectileFx.SetActive(true);
        }
    }
}