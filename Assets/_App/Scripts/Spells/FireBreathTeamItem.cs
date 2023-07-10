using UnityEngine;

namespace MobaVR
{
    public class FireBreathTeamItem : TeamItem
    {
        [Header("Team Theme")]
        [SerializeField] private FireballTeamThemeSO m_RedTeamThemeSo;
        [SerializeField] private FireballTeamThemeSO m_BlueTeamThemeSo;

        
        [SerializeField] private ParticleSystem m_ParticleSystem;

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
            
            if (m_ParticleSystem != null)
            {
                ParticleSystem.MainModule main = m_ParticleSystem.main;
                main.startColor = fireballTheme.MainColor;
            }
        }
    }
}