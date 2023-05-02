using AmazingAssets.AdvancedDissolve;
using UnityEngine;

namespace MobaVR
{
    public class IronShieldTeamItem : ThemeTeamItem
    {
        [Header("Renderer")]
        [SerializeField] private Renderer m_Renderer;

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
                color);
        }
    }
}