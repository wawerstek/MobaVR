using System;
using UnityEngine;

namespace MobaVR
{
    public class SkinTeamItem : TeamItem
    {
        [Header("Theme")]
        [SerializeField] private Material m_RedMaterial;
        [SerializeField] private Material m_BlueMaterial;
        
        [Header("Renderer")]
        [SerializeField] private SkinnedMeshRenderer m_Renderer;

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);
            switch (teamType)
            {
                case TeamType.RED:
                    m_Renderer.material = m_RedMaterial;
                    break;
                case TeamType.BLUE:
                    m_Renderer.material = m_BlueMaterial;
                    break;
            }
        }
    }
}