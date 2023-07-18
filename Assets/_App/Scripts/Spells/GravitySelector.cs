using System;
using MobaVR.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace MobaVR
{
    public class GravitySelector : MonoBehaviour
    {
        [SerializeField] private ClassicGameSession m_GameSession;

        private void Awake()
        {
            if (m_GameSession == null)
            {
                m_GameSession = FindObjectOfType<ClassicGameSession>();
                
                   
            }
        }

        public void SetAim(bool useAim)
        {
            m_GameSession.LocalPlayer.WizardPlayer.UseAim = useAim;
        }
        
        public void SetForce(float force)
        {
            m_GameSession.LocalPlayer.WizardPlayer.ThrowForce = force;
        }
        
        public void SetGravityType(GravityType gravityType)
        {
            m_GameSession.LocalPlayer.WizardPlayer.GravityFireballType = gravityType;
        }

        public void SetNoGravity()
        {
            SetGravityType(GravityType.NO_GRAVITY);
            SetAim(false);
        }

        public void SetRealGravity()
        {
            SetGravityType(GravityType.REAL_GRAVITY);
            SetAim(false);
        }

        public void SetCustomGravity()
        {
            SetGravityType(GravityType.CUSTOM_GRAVITY);
            SetAim(false);
        }
        
        public void SetCustomThrow()
        {
            SetGravityType(GravityType.CUSTOM_THROW);
            SetAim(false);
        }
        
        public void SetCustomThrowAim()
        {
            SetGravityType(GravityType.CUSTOM_THROW);
            SetAim(true);
        }
    }
}