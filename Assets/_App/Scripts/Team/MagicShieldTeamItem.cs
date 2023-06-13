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

            Color color = fireballTheme.SubColor;

            m_AuraRenderer.material.color = color;
            m_AuraRenderer.material.SetColor("_EmissionColor", color);

            Color transparentColor = new Color(color.r,
                                               color.g,
                                               color.b,
                                               m_BackgroundRenderer.material.color.a);
            m_BackgroundRenderer.material.color = transparentColor;
            
            m_Particle.SetTrailColor(fireballTheme.Gradient);
        }
    }
}