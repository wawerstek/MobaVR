using System;
using System.Collections;
using BNG;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class HammerSpell_Okd : ThrowableSpell
    {
        [Space]
        [Header("Magic")]
        //[SerializeField] private GameObject m_Hammer;
        //[SerializeField] private GameObject m_Trail;

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
        [SerializeField] private float m_ExplosionCollisionRadius = 10f;
        [SerializeField] private float m_ExplosionForce = 400f;
        [SerializeField] private float m_ExplosionForceRadius = 4f;
        [SerializeField] private float m_ExplosionModifier = 2f;

        [Header("Components")]
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Grabbable m_Grabbable;
        [SerializeField] private Collider m_CollisionCollider;
        [SerializeField] private Collider m_TriggerCollider;

        private bool m_IsFirstThrown = true;


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
            base.OnEnable();
            //TODO:
        }

        protected override float CalculateDamage()
        {
            return m_DefaultDamage;
        }

        /*
        public override void Throw()
        {
            photonView.RPC(nameof(RpcThrow), RpcTarget.AllBuffered);
        }

        public override void ThrowByDirection(Vector3 direction)
        {
            photonView.RPC(nameof(RpcThrowByDirection), RpcTarget.All, direction);
        }
        */

        [PunRPC]
        private void RpcThrow()
        {
            m_Grabbable.enabled = false;
            transform.parent = null;
            
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
            if (photonView.IsMine)
            {
                if (interactable.transform == transform)
                {
                    return;
                }

                Collider[] colliders = Physics.OverlapSphere(transform.position,
                                                             //m_ExplosionCollisionRadius,
                                                             m_ExplosionCollisionRadius,
                                                             m_ExplosionLayers,
                                                             QueryTriggerInteraction.Collide);
                foreach (Collider enemy in colliders)
                {
                    if (enemy.TryGetComponent(out IHit hitEnemy))
                    {
                        hitEnemy.RpcHit(CalculateDamage());
                        //TODO: работает только один раз
                        hitEnemy.Explode(m_ExplosionForce,
                                         transform.position,
                                         m_ExplosionForceRadius,
                                         m_ExplosionModifier);
                    }
                }

                photonView.RPC(nameof(RpcDestroyHammer), RpcTarget.All);
                Hide();

                /*
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
                */
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

        [PunRPC]
        private void RpcDestroyHammer()
        {
            //Destroy(m_Ball.gameObject);
            //m_ExplosionFx.SetActive(true);
            //m_ExplosionFx.transform.parent = null;

            OnDestroySpell?.Invoke();

            //Destroy(m_ExplosionFx, m_DestroyExplosion);
            //m_Trail.transform.parent = null;
            //Destroy(m_Trail.gameObject, m_DestroyChildren);

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
            //m_ExplosionFx.SetActive(true);
            //m_ExplosionFx.transform.parent = null;
            //Destroy(m_ExplosionFx, m_DestroyExplosion);
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
                if (photonView.IsMine)
                {
                    photonView.RPC(nameof(RpcFailDestroyBall), RpcTarget.All);
                    StopAllCoroutines();
                }
            }
            else
            {
                //OnThrown?.Invoke();
                m_Rigidbody.useGravity = true;
                m_Rigidbody.isKinematic = false;
            }
        }
    }
}