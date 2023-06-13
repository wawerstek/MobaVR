using System.Collections.Generic;
using MobaVR.Utils;
using UnityEngine;

namespace MobaVR
{
    public class BigFireballTeamItem : TeamItem
    {
        [Header("Team Theme")]
        [SerializeField] private FireballTeamThemeSO m_RedTeamThemeSo;
        [SerializeField] private FireballTeamThemeSO m_BlueTeamThemeSo;

        [Header("Elements")]
        [SerializeField] private ParticleSystemRenderer m_BallRenderer;
        [SerializeField] private ParticleSystem m_BallParticle;
        [SerializeField] private ParticleSystem m_GlowBallParticle;
        [SerializeField] private ParticleSystem m_CastingParticle;
        [SerializeField] private ParticleSystem m_TrailParticle;
        [SerializeField] private ParticleSystem m_ExplosionParticle;
        [SerializeField] private ParticleSystem m_GlowExplosionParticle;
        [SerializeField] private TrailRenderer m_TrailRenderer;

        [Header("Other")]
        [SerializeField] private List<ParticleSystem> m_MainParticles = new();
        [SerializeField] private List<ParticleSystem> m_AdditiveParticles = new();
        [SerializeField] private List<ParticleSystem> m_TransparentParticles = new();
        [SerializeField] private List<ParticleSystem> m_GradientParticles = new();


        private void Awake()
        {
            SetTeam(m_TeamType);
        }

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);

            FireballTeamThemeSO fireballTheme = m_RedTeamThemeSo;
            switch (teamType)
            {
                case TeamType.RED:
                    fireballTheme = m_RedTeamThemeSo;
                    break;
                case TeamType.BLUE:
                    fireballTheme = m_BlueTeamThemeSo;
                    break;
            }

            if (m_BallRenderer != null)
            {
                m_BallRenderer.material = fireballTheme.Material;
            }

            if (m_BallParticle != null)
            {
                ParticleSystem.MainModule ballMain = m_BallParticle.main;
                ballMain.startColor = fireballTheme.MainColor;
            }

            if (m_GlowBallParticle != null)
            {
                ParticleSystem.MainModule ballMain = m_GlowBallParticle.main;
                ballMain.startColor = fireballTheme.MainColor;
            }

            if (m_CastingParticle != null)
            {
                ParticleSystem.MainModule castingMain = m_CastingParticle.main;
                castingMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, fireballTheme.MainColor);
            }

            if (m_CastingParticle != null)
            {
                m_CastingParticle.SetColorOverLifeTime(fireballTheme.Gradient);
            }

            if (m_TrailParticle != null)
            {
                m_TrailParticle.SetColorOverLifeTime(fireballTheme.Gradient);
            }

            if (m_ExplosionParticle != null)
            {
                m_ExplosionParticle.SetColorOverLifeTime(fireballTheme.Gradient);
            }

            if (m_GlowExplosionParticle != null)
            {
                ParticleSystem.MainModule ballMain = m_GlowExplosionParticle.main;
                ballMain.startColor = fireballTheme.TransparentColor;
            }
            
            // Trail

            if (m_TrailRenderer != null)
            {
                //m_TrailRenderer.colorGradient = fireballTheme.Gradient;
                m_TrailRenderer.startColor = fireballTheme.MainColor;
                m_TrailRenderer.endColor = fireballTheme.MainColor;
            }

            // Other

            foreach (ParticleSystem mainParticle in m_MainParticles)
            {
                ParticleSystem.MainModule mainModule = mainParticle.main;
                mainModule.startColor = fireballTheme.MainColor;
            }
            
            foreach (ParticleSystem mainParticle in m_AdditiveParticles)
            {
                ParticleSystem.MainModule mainModule = mainParticle.main;
                mainModule.startColor = fireballTheme.SubColor;
            }
            
            foreach (ParticleSystem mainParticle in m_TransparentParticles)
            {
                ParticleSystem.MainModule mainModule = mainParticle.main;
                mainModule.startColor = fireballTheme.TransparentColor;
            }
        }
    }
}