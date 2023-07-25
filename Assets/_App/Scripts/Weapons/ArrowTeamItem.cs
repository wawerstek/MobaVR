using MobaVR.Utils;
using UnityEngine;

namespace MobaVR
{
    public class ArrowTeamItem : ThemeTeamItem
    {
        [SerializeField] private ParticleSystem m_ExplosionParticle;
        [SerializeField] private TrailRenderer m_TrailRenderer;
        
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
            
            if (m_TrailRenderer != null)
            {
                //m_TrailRenderer.colorGradient = fireballTheme.Gradient;
                m_TrailRenderer.startColor = fireballTheme.MainColor;
                m_TrailRenderer.endColor = fireballTheme.MainColor;
            }
            
            if (m_ExplosionParticle != null)
            {
                m_ExplosionParticle.SetColorOverLifeTime(fireballTheme.Gradient);
            }
        }
    }
}