using UnityEngine;

namespace MobaVR
{
    public class ItemSkinTeam : TeamItem
    {
        [Header("Theme")]
        [SerializeField] private Material[] m_RedMaterial;
        [SerializeField] private Material[] m_BlueMaterial;

        [Header("Renderer")]
        private SkinnedMeshRenderer m_Renderer;

        private void Awake()
        {
            m_Renderer = GetComponent<SkinnedMeshRenderer>();
        }

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);

            switch (teamType)
            {
                case TeamType.RED:
                    m_Renderer.materials = m_RedMaterial;
                    break;

                case TeamType.BLUE:
                    m_Renderer.materials = m_BlueMaterial;
                    break;
            }
        }
    }
}