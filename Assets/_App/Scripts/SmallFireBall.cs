using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class SmallFireBall : Fireball
    {
        [Space]
        [Header("Magic")]
        [SerializeField] private GameObject m_Trail;
        [SerializeField] private GameObject m_ProjectileFx;
        [SerializeField] private GameObject m_ExplosionFx;
        
        [SerializeField] private float m_DestroyExplosion = 4.0f;
        [SerializeField] private float m_DestroyChildren = 2.0f;
        [SerializeField] private float m_Delay = 0.1f;

        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Collider m_Collider;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (m_Rigidbody == null)
            {
                TryGetComponent(out m_Rigidbody);
            }

            if (m_Collider == null)
            {
                TryGetComponent(out m_Collider);
                m_Collider.enabled = false;
            }
        }
        
        protected override void OnEnable()
        {
            //base.OnEnable();
            Invoke(nameof(RpcDestroyBall), m_DestroyLifeTime);

            m_Collider.enabled = false;
            
            m_ProjectileFx.SetActive(true);
            m_ExplosionFx.SetActive(false);
        }

        protected override void InteractBall(Transform interactable)
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RpcDestroyBall), RpcTarget.All);
                //DestroyBall();
            }
        }

        protected override float CalculateDamage()
        {
            return m_DefaultDamage;
        }

        [PunRPC]
        private void RpcDestroyBall()
        {
            m_Trail.transform.parent = null;
            Destroy(m_Trail.gameObject, m_DestroyChildren);
            
            m_ExplosionFx.SetActive(true);
            m_ExplosionFx.transform.parent = null;
            Destroy(m_ExplosionFx, m_DestroyExplosion);
            //Destroy(gameObject);
            
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            
            //PhotonNetwork.Destroy(gameObject);
        }
        
        private IEnumerator EnableCollider()
        {
            m_Collider.enabled = false;
            yield return new WaitForSeconds(m_Delay);
            m_IsThrown = true;
            m_Collider.enabled = true;
        }

        public override void Throw()
        {
            //TODO
        }

        public override void ThrowByDirection(Vector3 direction)
        {
            photonView.RPC(nameof(RpcThrowByDirection), RpcTarget.All, direction);
        }

        [PunRPC]
        private void RpcThrowByDirection(Vector3 direction)
        {
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.useGravity = false;
            m_Rigidbody.AddForce(direction * m_Force);
            
            m_ProjectileFx.SetActive(true);

            StartCoroutine(EnableCollider());
        }
    }
}