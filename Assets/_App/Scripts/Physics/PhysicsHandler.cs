using BNG;
using CloudFine.ThrowLab;
using CloudFine.ThrowLab.Oculus;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class PhysicsHandler : MonoBehaviourPun
    {
        [SerializeField] private GravityType m_GravityType;
        [SerializeField] private ThrowConfiguration m_CustomThrowSettings;
        [SerializeField] private Grabbable m_Grabbable;
        [SerializeField] private bool m_IsAutoInit = true;
        private BasePhysics m_Physics;

        private void OnValidate()
        {
            if (m_Grabbable == null)
            {
                TryGetComponent(out m_Grabbable);
            }
        }

        public GravityType GravityType
        {
            get => m_GravityType;
            set
            {
                m_GravityType = value;
                InitPhysics(m_GravityType);
            }
        }

        private void Awake()
        {
            if (m_IsAutoInit)
            {
                InitPhysics(m_GravityType);
            }
        }

        public void InitPhysics(GravityType gravityType)
        {
            //GravityType = gravityType;
            /*
            if (m_GravityType != gravityType && m_Physics != null)
            {
                Destroy(m_Physics);
            }
            */
            
            if (m_Physics != null)
            {
                Destroy(m_Physics);
            }

            m_GravityType = gravityType;

            switch (m_GravityType)
            {
                case GravityType.NO_GRAVITY:
                    m_Physics = gameObject.AddComponent<NoGravity>();
                    break;
                case GravityType.REAL_GRAVITY:
                    m_Physics = gameObject.AddComponent<RealGravity>();
                    break;
                case GravityType.CUSTOM_GRAVITY:
                    m_Physics = gameObject.AddComponent<CustomGravity>();
                    break;
                case GravityType.CURVE_GRAVITY:
                    m_Physics = gameObject.AddComponent<GravityCurve>();
                    break;
                case GravityType.CUSTOM_THROW:
                    m_Physics = gameObject.AddComponent<GrabbableThrowHandler>();
                    break;
                default:
                    m_Physics = gameObject.AddComponent<RealGravity>();
                    break;
            }
        }

        public void InitPhysics(GravityType gravityType, float force, bool useAim)
        {
            InitPhysics(gravityType);

            m_Grabbable.ThrowForceMultiplier = force;
            if (photonView.IsMine)
            {
                //m_CustomThrowSettings.scaleMultiplier = force;
                //m_CustomThrowSettings.assistEnabled = useAim;
            }
            
        }

        /*
        public void InitPhysics(GravityType gravityType, float force, bool useAim)
        {
            GravityType = gravityType;
            m_ThrowLabOvrGrabbable.ThrowForceMultiplier = force;
            if (photonView.IsMine)
            {
                m_CustomThrowSettings.scaleMultiplier = force;
                m_CustomThrowSettings.assistEnabled = useAim;
            }
        }
        */
    }
}