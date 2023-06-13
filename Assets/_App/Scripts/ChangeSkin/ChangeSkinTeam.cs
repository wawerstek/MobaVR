using UnityEngine;

namespace MobaVR
{
    public class ChangeSkinTeam : TeamItem
    {
        [Header("Theme")]
        [SerializeField] private Material[] m_RedMaterial;
        [SerializeField] private Material[] m_BlueMaterial;

        [Header("Renderer")]
        private SkinnedMeshRenderer m_Renderer;

        private void Awake()
        {
            //меняется материал на том объекте, на котором лежит скрипт
            m_Renderer = GetComponent<SkinnedMeshRenderer>();
        }



        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);

            switch (teamType)
            {
                case TeamType.RED:
                    if (m_RedMaterial.Length > 0)
                    {
                        m_Renderer.materials = m_RedMaterial;
                    }
                    break;
                case TeamType.BLUE:
                    if (m_BlueMaterial.Length > 0)
                    {
                        m_Renderer.materials = m_BlueMaterial;
                    }
                    break;
            }

        }

    }
}