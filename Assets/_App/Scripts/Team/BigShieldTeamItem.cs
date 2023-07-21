using AmazingAssets.AdvancedDissolve;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class BigShieldTeamItem : ThemeTeamItem
    {
        [Header("Renderer")]
        [SerializeField] private Renderer m_Renderer;
        [SerializeField] private PhotonView m_PhotonView;

        [SerializeField] private Material[] m_BlueMaterials;
        [SerializeField] private Material[] m_RedMaterials;
        
        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);
            Material[] materials = m_RedMaterials;
            switch (teamType)
            {
                case TeamType.RED:
                    materials = m_RedMaterials;
                    break;

                case TeamType.BLUE:
                    materials = m_BlueMaterials;
                    break;
            }

            if (m_Renderer.materials is { Length: > 1 })
            {
                m_Renderer.materials = materials;
            }
            else
            {
                m_Renderer.material = materials[0];
            }
        }
    }
}