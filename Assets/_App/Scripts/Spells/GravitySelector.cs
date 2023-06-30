using System;
using UnityEngine;

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
        
        public void SetGravityType(BigFireballType gravityType)
        {
            m_GameSession.LocalPlayer.WizardPlayer.GravityFireballType = gravityType;
        }

        public void SetNoGravity()
        {
            SetGravityType(BigFireballType.NO_GRAVITY);
            SetAim(false);
        }

        public void SetRealGravity()
        {
            SetGravityType(BigFireballType.REAL_GRAVITY);
            SetAim(false);
        }

        public void SetCustomGravity()
        {
            SetGravityType(BigFireballType.CUSTOM_GRAVITY);
            SetAim(false);
        }
        
        public void SetCustomThrow()
        {
            SetGravityType(BigFireballType.CUSTOM_THROW);
            SetAim(false);
        }
        
        public void SetCustomThrowAim()
        {
            SetGravityType(BigFireballType.CUSTOM_THROW);
            SetAim(true);
        }
    }
}