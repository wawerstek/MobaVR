using System;
using CloudFine.ThrowLab;
using CloudFine.ThrowLab.Oculus;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class PhysicsHandler_1 : MonoBehaviourPun
    {
        [SerializeField] private GravityType m_GravityType;
        [SerializeField] private ThrowConfiguration m_CustomThrowSettings;

        [Header("Components")]
        [SerializeField] private BigFireBall m_FireBall;
        [SerializeField] private Throwable m_Throwable;
        [SerializeField] private ThrowHandle m_ThrowHandle;
        [SerializeField] private ThrowLabOVRGrabbable m_ThrowLabOvrGrabbable;
        [SerializeField] private GravityFireball m_GravityFireball;
        [SerializeField] private Rigidbody m_Rigidbody;

        private void OnValidate()
        {
            if (m_FireBall == null)
            {
                TryGetComponent(out m_FireBall);
            }

            if (m_ThrowHandle == null)
            {
                TryGetComponent(out m_ThrowHandle);
            }

            if (m_ThrowLabOvrGrabbable == null)
            {
                TryGetComponent(out m_ThrowLabOvrGrabbable);
            }

            if (m_GravityFireball == null)
            {
                TryGetComponent(out m_GravityFireball);
            }

            if (m_Rigidbody == null)
            {
                TryGetComponent(out m_Rigidbody);
            }
        }

        public GravityType GravityType
        {
            get => m_GravityType;
            set
            {
                m_GravityType = value;
                switch (m_GravityType)
                {
                    case GravityType.NO_GRAVITY:
                        m_ThrowHandle.enabled = false;
                        m_GravityFireball.enabled = false;
                        if (m_FireBall != null)
                        {
                            m_FireBall.UseCustomGravity = false;
                            m_FireBall.GravityDelay = 1.2f;
                        }

                        else
                        {
                            if (m_Throwable != null)
                            {
                                m_Throwable.UseCustomGravity = false;
                                m_Throwable.GravityDelay = 1.2f;
                            }
                        }

                        break;
                    case GravityType.REAL_GRAVITY:
                        m_ThrowHandle.enabled = false;
                        m_GravityFireball.enabled = false;
                        if (m_FireBall != null)
                        {
                            m_FireBall.UseCustomGravity = false;
                            m_FireBall.GravityDelay = 0f;
                        }
                        else
                        {
                            if (m_Throwable != null)
                            {
                                m_Throwable.UseCustomGravity = false;
                                m_Throwable.GravityDelay = 0f;
                            }
                        }


                        break;
                    case GravityType.CUSTOM_GRAVITY:
                        m_ThrowHandle.enabled = false;
                        m_GravityFireball.enabled = true;
                        if (m_FireBall != null)
                        {
                            m_FireBall.UseCustomGravity = true;
                        }
                        else
                        {
                            if (m_Throwable != null)
                            {
                                m_Throwable.UseCustomGravity = true;
                            }
                        }

                        break;
                    case GravityType.CUSTOM_THROW:
                        m_ThrowHandle.enabled = true;
                        m_GravityFireball.enabled = false;
                        if (m_FireBall != null)
                        {
                            m_FireBall.UseCustomGravity = false;
                            m_FireBall.GravityDelay = 0f;
                        }
                        else
                        {
                            if (m_Throwable != null)
                            {
                                m_Throwable.UseCustomGravity = false;
                                m_Throwable.GravityDelay = 0f;
                            }
                        }

                        break;
                }
            }
        }

        public void Throw()
        {
            switch (m_GravityType)
            {
                case GravityType.NO_GRAVITY:
                    m_Rigidbody.useGravity = false;
                    break;
                case GravityType.REAL_GRAVITY:
                    m_Rigidbody.useGravity = false;
                    break;
                case GravityType.CUSTOM_GRAVITY:
                    m_Rigidbody.useGravity = false;
                    break;
                case GravityType.CUSTOM_THROW:
                    m_Rigidbody.useGravity = false;
                    break;
            }
        }

        public void InitPhysics(GravityType gravityType)
        {
            GravityType = gravityType;
        }

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
    }
}