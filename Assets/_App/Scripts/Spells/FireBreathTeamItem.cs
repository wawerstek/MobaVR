using UnityEngine;

namespace MobaVR
{
    public class FireBreathTeamItem : TeamItem
    {
        [Header("Team Theme")]
        [SerializeField] private FireballTeamThemeSO m_RedTeamThemeSo;
        [SerializeField] private FireballTeamThemeSO m_BlueTeamThemeSo;

        [SerializeField] private Material m_RedMaterial;
        [SerializeField] private Material m_BlueMaterial;

        [SerializeField] private ParticleSystem m_ParticleSystem;

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);

            FireballTeamThemeSO fireballTheme = m_RedTeamThemeSo;
            Material material = m_RedMaterial;
            switch (teamType)
            {
                case TeamType.RED:
                    fireballTheme = m_RedTeamThemeSo;
                    material = m_RedMaterial;
                    break;
                case TeamType.BLUE:
                    fireballTheme = m_BlueTeamThemeSo;
                    material = m_BlueMaterial;
                    break;
            }

            if (m_ParticleSystem != null)
            {
                ParticleSystem.MainModule main = m_ParticleSystem.main;
                main.startColor = fireballTheme.MainColor;

                var particleRenderer = m_ParticleSystem.GetComponent<ParticleSystemRenderer>();
                if (m_RedMaterial != null && m_BlueMaterial != null)
                {
                    particleRenderer.material = material;
                }
            }
        }
    }
}