using System;
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
        [SerializeField] private ParticleSystem m_CastingParticle;
        [SerializeField] private ParticleSystem m_TrailParticle;
        [SerializeField] private ParticleSystem m_ExplosionParticle;

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
            
            m_BallRenderer.material = fireballTheme.Material;
            ParticleSystem.MainModule ballMain = m_BallParticle.main;
            ballMain.startColor = fireballTheme.MainColor;
            
            ParticleSystem.MainModule castingMain = m_CastingParticle.main;
            castingMain.startColor = new ParticleSystem.MinMaxGradient(Color.white, fireballTheme.MainColor); 
            
            m_CastingParticle.SetColorOverLifeTime(fireballTheme.Gradient);
            m_TrailParticle.SetColorOverLifeTime(fireballTheme.Gradient);
            m_ExplosionParticle.SetColorOverLifeTime(fireballTheme.Gradient);
        }
    }
}