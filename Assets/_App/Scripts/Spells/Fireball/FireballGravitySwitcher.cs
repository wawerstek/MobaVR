using System;
using CloudFine.ThrowLab;
using CloudFine.ThrowLab.Oculus;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class FireballGravitySwitcher : MonoBehaviourPun
    {
        [SerializeField] private BigFireballType m_GravityType;
        [SerializeField] private ThrowConfiguration m_CustomThrowSettings;
        
        [Header("Components")]
        [SerializeField] private BigFireBall m_FireBall;
        [SerializeField] private Throwable m_Throwable;
        [SerializeField] private ThrowHandle m_ThrowHandle;
        [SerializeField] private ThrowLabOVRGrabbable m_ThrowLabOvrGrabbable;
        [SerializeField] private GravityFireball m_GravityFireball;

        public BigFireballType GravityType
        {
            get => m_GravityType;
            set
            {
                m_GravityType = value;
                switch (m_GravityType)
                {
                    case BigFireballType.NO_GRAVITY:
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
                    case BigFireballType.REAL_GRAVITY:
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
                    case BigFireballType.CUSTOM_GRAVITY:
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
                    case BigFireballType.CUSTOM_THROW:
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

        public void SetPhysics(BigFireballType gravityType, float force, bool useAim)
        {
            GravityType = gravityType;
            m_ThrowLabOvrGrabbable.ThrowForceMultiplier = force;
            if (photonView.IsMine)
            {
                m_CustomThrowSettings.scaleMultiplier = force;
                m_CustomThrowSettings.assistEnabled = useAim;
            }
        }

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
        }
    }
}