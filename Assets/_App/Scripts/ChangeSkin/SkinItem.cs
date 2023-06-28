using System;
using UnityEngine;

namespace MobaVR
{
    public class SkinItem : TeamItem
    {
        [Header("Theme")]
        [SerializeField] private Material[] m_RedMaterial;
        [SerializeField] private Material[] m_BlueMaterial;

        [Header("Renderer")]
        [SerializeField] private Renderer m_Renderer;

        public Renderer Renderer => m_Renderer;

        private void OnValidate()
        {
            if (m_Renderer == null)
            {
                m_Renderer = GetComponent<Renderer>();
            }
        }

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);
            Material[] materials = m_RedMaterial;
            switch (teamType)
            {
                case TeamType.RED:
                    materials = m_RedMaterial;
                    break;

                case TeamType.BLUE:
                    materials = m_BlueMaterial;
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