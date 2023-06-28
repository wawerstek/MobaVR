using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MobaVR
{
    public class Skin : TeamItem, ISkin
    {
        [SerializeField] private List<SkinItem> m_Renderers = new();
        [SerializeField] private List<SkinItem> m_LegRenderers = new();

        private string[] m_LegNames = new[]
        {
            "leg",
            "shoe",
            "boot"
        };
        
        private void OnValidate()
        {
            if (m_Renderers.Count == 0)
            {
                m_Renderers.AddRange(GetComponentsInChildren<SkinItem>(true));
            }

            if (m_Renderers.Count != 0 && m_LegRenderers.Count == 0)
            {
                m_LegRenderers.AddRange(m_Renderers.Where(meshRenderer =>
                {
                    foreach (string legName in m_LegNames)
                    {
                        if (meshRenderer.name.Contains(legName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    return false;
                }));
            }
        }

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);
            
            foreach (SkinItem skinItem in m_Renderers)
            {
                skinItem.SetTeam(teamType);
            }
        }

        public void SetVisibilityLegs(bool isVisible)
        {
            foreach (SkinItem skinItem in m_LegRenderers)
            {
                skinItem.gameObject.SetActive(isVisible);
            }
        }

        public void ActivateSkin(TeamType teamType)
        {
            gameObject.SetActive(true);
            SetTeam(teamType);
        }

        public void DeactivateSkin()
        {
            gameObject.SetActive(false);
        }
    }
}