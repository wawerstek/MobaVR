using System.Collections;
using BNG;
using MobaVR.Base;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    //public class Throwable : MonoBehaviourPun, IThrowable
    public class Throwable_1 : GrabbableEvents, IThrowable
    {
        [Space]
        [Header("Forces")]
        [SerializeField] private float m_PrimaryForce = 4000f;
        [SerializeField] private float m_SecondForce = 4000f;

        [Space]
        [Header("Validate throw")]
        [SerializeField] private bool m_IsCheckBadThrow = true;
        [SerializeField] private float m_ThrowCheckDelay = 0.2f;
        [SerializeField] private float m_ThrowMinDistance = 1.0f;

        [Space]
        [Header("Physics")]
        [SerializeField] private PhysicsHandler m_PhysicsHandler;
        [SerializeField] private bool m_UseCustomGravity = false;
        [SerializeField] private float m_GravityDelay = 0.5f;

        [Header("Components")]
        [SerializeField] private PhotonView m_PhotonView;
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Grabbable m_Grabbable;

        private Grabber m_Grabber;
        private bool m_IsThrown = false;
        private bool m_IsFirstThrown = true;

        #region Events

        public UnityEvent<bool> OnThrowChecked;
        public UnityEvent OnThrown;
        public UnityEvent<Vector3> OnRedirected;
        public UnityEvent<Grabber> OnGrabbed;
        public UnityEvent<Grabber> OnReleased;

        #endregion

        #region Properties

        public bool IsThrown => m_IsThrown;
        public bool IsGrabbable => m_Grabbable != null && m_Grabbable.IsGrabbable();
        public Grabbable Grabbable => m_Grabbable;
        public Grabber Grabber => m_Grabber;
        public PhysicsHandler PhysicsHandler => m_PhysicsHandler;
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

        #endregion

        #region MonoBehaviour

        



        private void OnValidate()
        {
            if (m_Rigidbody == null)
            {
                TryGetComponent(out m_Rigidbody);
            }

            if (m_Grabbable == null)
            {
                TryGetComponent(out m_Grabbable);
            }

            if (m_PhysicsHandler == null)
            {
                TryGetComponent(out m_PhysicsHandler);
            }

            if (m_PhotonView == null)
            {
                TryGetComponent(out m_PhotonView);
            }
        }

        private void OnEnable()
        {
            //m_Rigidbody.WakeUp();
            //m_Rigidbody.sleepThreshold = 0.0f;
            if (m_Grabbable != null)
            {
                //m_Grabbable.GetPrimaryGrabber();
            }
        }

        private void Start()
        {
            if (m_PhotonView.IsMine)
            {
                //m_Rigidbody.useGravity = true;
                m_Grabbable.enabled = true;
                m_Grabbable.CanBeDropped = true;

                //m_Rigidbody.WakeUp();
                //m_Rigidbody.sleepThreshold = 0.0f;
            }
        }
        
        #endregion


        private IEnumerator Release()
        {
            m_Grabbable.enabled = false;

            if (m_IsCheckBadThrow)
            {
                Vector3 startPosition = transform.position;
                yield return new WaitForSeconds(m_ThrowCheckDelay);
                Vector3 endPosition = transform.position;
                float distance = Vector3.Distance(startPosition, endPosition);
                if (distance < m_ThrowMinDistance)
                {
                    if (m_PhotonView.IsMine)
                    {
                        OnThrowChecked?.Invoke(false);
                        StopAllCoroutines();
                        yield break;
                    }
                }
            }

            OnThrowChecked?.Invoke(true);
            OnThrown?.Invoke();

            if (m_PhysicsHandler != null)
            {
                //m_PhysicsHandler.ApplyPhysics();
            }

            m_Rigidbody.isKinematic = false;
        }

        public void Init(WizardPlayer wizardPlayer, TeamType teamType)
        {
            m_PhotonView.RPC(nameof(RpcSetPhysics),
                             RpcTarget.All,
                             wizardPlayer.GravityFireballType,
                             wizardPlayer.ThrowForce,
                             wizardPlayer.UseAim);
        }

        [PunRPC]
        private void RpcSwitchGravity(GravityType gravityType)
        {
            if (m_PhysicsHandler != null)
            {
                m_PhysicsHandler.GravityType = gravityType;
            }
        }

        [PunRPC]
        private void RpcSetPhysics(GravityType gravityType, float force, bool useAim)
        {
            if (m_PhysicsHandler != null)
            {
                m_PhysicsHandler.InitPhysics(gravityType, force, useAim);
            }
        }

        public void Grab(Grabber grabber)
        {
            m_Grabber = grabber;
            OnGrabbed?.Invoke(grabber);
        }

        public void Release(Grabber grabber)
        {
            m_Grabber = null;
            OnReleased.Invoke(grabber);
        }

        public void Throw()
        {
            m_Grabbable.enabled = false;
            m_Rigidbody.WakeUp();
            m_PhotonView.RPC(nameof(RpcThrow), RpcTarget.All);
        }

        public void ThrowByDirection(Vector3 direction)
        {
            m_PhotonView.RPC(nameof(RpcThrowByDirection), RpcTarget.All, direction);
        }

        [PunRPC]
        private void RpcThrow()
        {
            m_Grabbable.enabled = false;
            m_Rigidbody.WakeUp();
            m_IsThrown = true;
            m_IsFirstThrown = false;
            StartCoroutine(Release());
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
                m_Rigidbody.AddForce(direction * m_PrimaryForce);
                m_IsFirstThrown = false;
            }
            else
            {
                m_Rigidbody.AddForce(direction * m_SecondForce);
            }

            OnThrown?.Invoke();
            OnRedirected?.Invoke(direction);
        }
    }
}