using System;
using AmazingAssets.AdvancedDissolve;
using UnityEngine;

namespace MobaVR
{
    public class ThemeTeamItem : TeamItem
    {
        [Header("Team Theme")]
        [SerializeField] protected FireballTeamThemeSO m_RedTeamThemeSo;
        [SerializeField] protected FireballTeamThemeSO m_BlueTeamThemeSo;

        protected virtual void OnValidate()
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
    }
}