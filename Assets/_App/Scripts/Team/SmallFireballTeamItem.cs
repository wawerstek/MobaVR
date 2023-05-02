using MobaVR.Utils;
using UnityEngine;

namespace MobaVR
{
    public class SmallFireballTeamItem : TeamItem
    {
        [Header("Team Theme")]
        [SerializeField] private FireballTeamThemeSO m_RedTeamThemeSo;
        [SerializeField] private FireballTeamThemeSO m_BlueTeamThemeSo;
        
        [Header("Elements")]
        [SerializeField] private ParticleSystem m_ProjectileParticle;
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
            
            
            ParticleSystem.MainModule castingMain = m_ProjectileParticle.main;
            castingMain.startColor = fireballTheme.MainColor; 
            
            m_TrailParticle.SetColorOverLifeTime(fireballTheme.Gradient);
            m_ExplosionParticle.SetColorOverLifeTime(fireballTheme.Gradient);
        }
    }
}