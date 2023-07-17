using System.Collections;
using System.Collections;
using BNG;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class BigFireBall : ThrowableSpell
    {
        [SerializeField] private FireballGravitySwitcher m_GravitySwitcher;

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
        [Header("Validate cast")]
        [SerializeField] private float m_ThrowCheckDelay = 0.2f;
        [SerializeField] private float m_SecondForce = 400f;
        [SerializeField] private float m_ThrowMinDistance = 1.0f;

        [Space]
        [Header("Explosion Wave")]
        [SerializeField] private LayerMask m_ExplosionLayers;
        [SerializeField] private bool m_UseCustomGravity = false;
        [SerializeField] private float m_GravityDelay = 0.5f;
        [SerializeField] private float m_ExplosionCollisionRadius = 10f;
        [SerializeField] private float m_ExplosionForce = 400f;
        [SerializeField] private float m_ExplosionForceRadius = 4f;
        [SerializeField] private float m_ExplosionModifier = 2f;

        [Header("Components")]
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Grabbable m_Grabbable;
        [SerializeField] private SphereCollider m_CollisionCollider;
        [SerializeField] private SphereCollider m_TriggerCollider;

        private float m_InitColliderRadius = 0.1f;
        private float m_InitTriggerRadius = 0.12f;
        private float m_ColliderScale = 2f;

        private float m_ScaleFactor = 1f;
        private bool m_IsFirstThrown = true;

        public Grabbable Grabbable => m_Grabbable;
        public bool UseCustomGravity
        {
            get => m_UseCustomGravity;
            set => m_UseCustomGravity = value;
        }
        public float GravityDelay
        {
            get => m_GravityDelay;
            set => m_GravityDelay = value;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (m_Rigidbody == null)
            {
                TryGetComponent(out m_Rigidbody);
            }

            if (m_Grabbable == null)
            {
                TryGetComponent(out m_Grabbable);
            }
        }

        protected override void Start()
        {
            base.Start();
            if (photonView.IsMine)
            {
                m_Rigidbody.useGravity = true;
                m_Grabbable.enabled = true;
                m_Grabbable.CanBeDropped = true;

                m_Rigidbody.WakeUp();
                m_Rigidbody.sleepThreshold = 0.0f;
            }
        }

        protected override void OnEnable()
        {
            //base.OnEnable();
            Invoke(nameof(RpcDestroyBall), m_DestroyLifeTime);

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

        protected override void InteractBall(Transform interactable)
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
                        //hitEnemy.Die();
                        //hitEnemy.RpcHit(1f);
                        hitEnemy.RpcHit(CalculateDamage());

                        //TODO: работает только один раз
                        hitEnemy.Explode(m_ExplosionForce,
                                         transform.position,
                                         m_ExplosionForceRadius,
                                         m_ExplosionModifier);
                    }

                    //Расскоментить, рабочий вариант
                    /*
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
                    */

                    /*
                    if (enemy.TryGetComponent(out Animator animator))
                    {
                        animator.enabled = false;
                        //Destroy(animator.gameObject, 5f);
                    }

                    var anim = enemy.GetComponentInParent<Animator>();
                    if (anim != null)
                    {
                        anim.enabled = false;
                        //Destroy(anim.gameObject, 5f);
                    }

                    if (enemy.TryGetComponent(out Rigidbody rigidbody))
                    {
                        rigidbody.AddExplosionForce(m_ExplosionForce, 
                                                    transform.position, 
                                                    m_ExplosionForceRadius,
                                                    m_ExplosionModifier);
                    }
                    */
                }

                photonView.RPC(nameof(RpcDestroyBall), RpcTarget.All);
                //DestroyBall();
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
                //Destroy(gameObject);
            }

            //PhotonNetwork.Destroy(gameObject);
        }

        private IEnumerator CheckThrow()
        {
            m_Grabbable.enabled = false;
            m_Rigidbody.WakeUp();

            Vector3 startPosition = transform.position;
            yield return new WaitForSeconds(m_ThrowCheckDelay);
            Vector3 endPosition = transform.position;
            float distance = Vector3.Distance(startPosition, endPosition);
            if (distance < m_ThrowMinDistance)
            {
                //DestroyBall();
                //FailDestroyBall();
                if (photonView.IsMine)
                {
                    photonView.RPC(nameof(RpcFailDestroyBall), RpcTarget.All);
                    StopAllCoroutines();
                }
            }
            else
            {
                OnThrown?.Invoke();

                if (m_UseCustomGravity)
                {
                    //m_Rigidbody.isKinematic = false;
                    m_Rigidbody.useGravity = false;
                }
                else
                {
                    //m_Rigidbody.isKinematic = false;
                    if (m_GravityDelay > 0f)
                    {
                        m_Rigidbody.useGravity = false;
                        Invoke(nameof(ActivateGravity), m_GravityDelay);
                    }
                    else
                    {
                        m_Rigidbody.useGravity = true;
                    }
                }

                m_Rigidbody.isKinematic = false;

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
        }

        private void ActivateGravity()
        {
            m_Rigidbody.useGravity = true;
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
            photonView.RPC(nameof(RpcSetPhysics), RpcTarget.All, wizardPlayer.GravityFireballType,
                           wizardPlayer.ThrowForce, wizardPlayer.UseAim);
        }

        //TODO: add destroy public method

        [PunRPC]
        private void RpcSwitchGravity(BigFireballType gravityType)
        {
            if (m_GravitySwitcher != null)
            {
                m_GravitySwitcher.GravityType = gravityType;
            }
        }

        [PunRPC]
        private void RpcSetPhysics(BigFireballType gravityType, float force, bool useAim)
        {
            if (m_GravitySwitcher != null)
            {
                m_GravitySwitcher.SetPhysics(gravityType, force, useAim);
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

            m_Grabbable.enabled = false;
            m_Rigidbody.WakeUp();
            photonView.RPC(nameof(RpcThrow), RpcTarget.All);
        }

        public override void ThrowByDirection(Vector3 direction)
        {
            if (!m_IsThrown)
            {
                m_ScaleFactor = 1f;
            }

            photonView.RPC(nameof(RpcThrowByDirection), RpcTarget.All, direction);
        }

        [PunRPC]
        private void RpcThrow()
        {
            m_Grabbable.enabled = false;
            m_Rigidbody.WakeUp();
            m_IsThrown = true;
            m_IsFirstThrown = false;
            StartCoroutine(CheckThrow());
        }

        [PunRPC]
        private void RpcThrowByDirection(Vector3 direction)
        {
            if (m_Grabbable != null)
            {
                m_Grabbable.DropItem(true, true);
            }

            m_IsThrown = true;
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.useGravity = true;
            if (m_IsFirstThrown)
            {
                m_Rigidbody.AddForce(direction * m_Force);
                m_IsFirstThrown = false;
            }
            else
            {
                m_Rigidbody.AddForce(direction * m_SecondForce);
            }

            m_Ball.transform.parent = m_Trail.transform;
            m_ProjectileFx.SetActive(true);
        }
    }
}