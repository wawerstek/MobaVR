using System;
using MobaVR.Utils;
using UnityEditor;
using UnityEngine;

namespace MobaVR
{
    public class WeaponMagicItem : SkinItem
    {
        [Header("Team Theme")]
        [SerializeField] private FireballTeamThemeSO m_RedTeamThemeSo;
        [SerializeField] private FireballTeamThemeSO m_BlueTeamThemeSo;

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

        public Material GetMagicMaterial()
        {
            return Renderer.materials[^1];
        }

        public void UpdateMagicMaterial(float value)
        {
            
        }
    }
}