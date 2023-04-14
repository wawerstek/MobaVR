using System;
using AmazingAssets.AdvancedDissolve;
using UnityEngine;

namespace MobaVR
{
    public class ShieldTeamItem : TeamItem
    {
        [Header("Team Theme")]
        [SerializeField] private FireballTeamThemeSO m_RedTeamThemeSo;
        [SerializeField] private FireballTeamThemeSO m_BlueTeamThemeSo;

        [Header("Renderer")]
        [SerializeField] private Renderer m_Renderer;

        private void OnValidate()
        {
            if (m_RedTeamThemeSo == null)
            {
                m_RedTeamThemeSo = Resources.Load<FireballTeamThemeSO>("Api/BigFireball_Red");
            }
            
            if (m_BlueTeamThemeSo == null)
            {
                m_BlueTeamThemeSo = Resources.Load<FireballTeamThemeSO>("Api/BigFireball_Blue");
            }
        }

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);
            Color color = Color.black;
            switch (teamType)
            {
                case TeamType.RED:
                    color = m_RedTeamThemeSo.MainColor;
                    break;
                case TeamType.BLUE:
                    color = m_BlueTeamThemeSo.MainColor;
                    break;
            }

            AdvancedDissolveProperties.Edge.Base.UpdateLocalProperty(
                m_Renderer.material,
                AdvancedDissolveProperties.Edge.Base.Property.Color,
                m_RedTeamThemeSo.MainColor);
        }
    }
}