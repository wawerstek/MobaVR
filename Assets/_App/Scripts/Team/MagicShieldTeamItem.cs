using MobaVR.Utils;
using UnityEngine;

namespace MobaVR
{
    public class MagicShieldTeamItem : ThemeTeamItem
    {
        [Header("Renderer")]
        [SerializeField] private Renderer m_AuraRenderer;
        [SerializeField] private Renderer m_BackgroundRenderer;
        [SerializeField] private ParticleSystem m_Particle;

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

            m_AuraRenderer.material.color = fireballTheme.MainColor;
            m_AuraRenderer.material.SetColor("_EmissionColor", fireballTheme.MainColor);

            Color transparentColor = new Color(fireballTheme.MainColor.r,
                                               fireballTheme.MainColor.g,
                                               fireballTheme.MainColor.b,
                                               m_BackgroundRenderer.material.color.a);
            m_BackgroundRenderer.material.color = transparentColor;
            
            m_Particle.SetTrailColor(fireballTheme.Gradient);
        }
    }
}