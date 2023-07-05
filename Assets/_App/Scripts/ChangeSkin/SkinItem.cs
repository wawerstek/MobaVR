using System;
using UnityEditor;
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

        [ContextMenu("Convert Blue to Red: NHance")]
        private void SwitchBlueToRedMaterials_NHance()
        {
            for (var i = 0; i < m_BlueMaterial.Length; i++)
            {
                Material material = m_BlueMaterial[i];
                //string path = AssetDatabase.GetAssetPath(material);
                //if (path.Contains("_Bl"))
                //{
                //    string newPath = path.Replace("_Bl", "_Rd");
                //    Material redMaterial = AssetDatabase.LoadAssetAtPath<Material>(newPath);
                //    if (redMaterial != null)
                //    {
                //        m_RedMaterial[i] = redMaterial;
                //    }
                //}
            }
        }

        [ContextMenu("FindMaterials")]
        private void FindMaterials()
        {
            FindBlueMaterials();
            FindRedMaterials();
        }

        [ContextMenu("FindBlueMaterials")]
        private void FindBlueMaterials()
        {
            if (m_Renderer != null)
            {
                m_BlueMaterial = m_Renderer.sharedMaterials;
            }
        }


        [ContextMenu("FindRedMaterials")]
        private void FindRedMaterials()
        {
            if (m_Renderer != null)
            {
                m_RedMaterial = m_Renderer.sharedMaterials;
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