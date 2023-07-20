using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace MobaVR
{
    public class Skin : TeamItem, ISkin
    {
        [Header("Armature")]
        [SerializeField] private Transform m_Armature;
        [SerializeField] [ReadOnly] private float m_ArmatureScale = 0.54f;

        [Header("Team")]
        [SerializeField] private List<SkinItem> m_TeamRenderers = new();

        [Header("Renderers")]
        [SerializeField] private List<Renderer> m_Renderers = new();
        [SerializeField] private List<Renderer> m_LegRenderers = new();
        [SerializeField] private List<Renderer> m_FaceRenderers = new();
        [SerializeField] private List<Renderer> m_BodyRenderers = new();
        [SerializeField] private List<Renderer> m_HiddenVrRenderers = new();

        [Space]
        [Header("Events")]
        public UnityEvent OnActivated;
        public UnityEvent OnDeactivated;
        
        private string[] m_LegNames = new[]
        {
            "leg",
            "shoe",
            "boot"
        };

        private string[] m_FaceNames = new[]
        {
            "face",
            "eye",
            "head",
            "hair",
            "neck",
            "teeth",
        };

        private void OnValidate()
        {
            //FindArmature();
            //FindTeamRenderers();
            //FindAllRenderers();
            //FindLegs();
            //FindFace();
        }

        #region Find Renderers
        
        [ContextMenu("FindArmature")]
        private void FindArmature()
        {
            if (m_Armature == null)
            {
                m_Armature = transform.Find("Armature");
            }
        }

        [ContextMenu("FindAllRenderers")]
        private void FindAllRenderers()
        {
            if (m_Renderers.Count == 0)
            {
                m_Renderers.AddRange(GetComponentsInChildren<Renderer>(true));
            }
        }

        [ContextMenu("FindTeamRenderers")]
        private void FindTeamRenderers()
        {
            if (m_TeamRenderers.Count == 0)
            {
                m_TeamRenderers.AddRange(GetComponentsInChildren<SkinItem>(true));
            }
        }


        [ContextMenu("FindFace")]
        private void FindFace()
        {
            if (m_TeamRenderers.Count != 0 && m_FaceRenderers.Count == 0)
            {
                m_FaceRenderers.AddRange(m_Renderers.Where(meshRenderer =>
                {
                    foreach (string legName in m_FaceNames)
                    {
                        if (meshRenderer.name.Contains(legName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    return false;
                }));
            }

            Transform face = transform.Find("Body/Base/Head");
            if (face != null)
            {
                m_FaceRenderers.AddRange(face.GetComponentsInChildren<Renderer>(true));
            }
            
            Transform customization = transform.Find("Customization");
            if (customization != null)
            {
                m_FaceRenderers.AddRange(customization.GetComponentsInChildren<Renderer>(true));
            }
        }

        [ContextMenu("FindLegs")]
        private void FindLegs()
        {
            if (m_TeamRenderers.Count != 0 && m_LegRenderers.Count == 0)
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

        #endregion

        #region Visibility

        [ContextMenu("ScaleArmature")]
        public void ScaleArmature()
        {
            if (m_Armature != null)
            {
                m_Armature.localScale = new Vector3(m_ArmatureScale, m_ArmatureScale, m_ArmatureScale);
            }
        }

        [ContextMenu("SetVisibilityLegs")]
        public void SetVisibilityLegs(bool isVisible = false)
        {
            foreach (Renderer meshRenderer in m_LegRenderers)
            {
                meshRenderer.gameObject.SetActive(isVisible);
            }
        }

        [ContextMenu("SetVisibilityFace")]
        public void SetVisibilityFace(bool isVisible = false)
        {
            foreach (Renderer meshRenderer in m_FaceRenderers)
            {
                meshRenderer.gameObject.SetActive(isVisible);
            }
        }

        [ContextMenu("SetVisibility")]
        public void SetVisibilityVR(bool isVisible = false)
        {
            foreach (Renderer meshRenderer in m_HiddenVrRenderers)
            {
                meshRenderer.gameObject.SetActive(isVisible);
            }
        }

        [ContextMenu("SetVisibilityBody")]
        public void SetVisibilityBody(bool isVisible = false)
        {
            foreach (Renderer meshRenderer in m_BodyRenderers)
            {
                meshRenderer.gameObject.SetActive(isVisible);
            }
        }

        #endregion

        #region Skin

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);

            foreach (SkinItem skinItem in m_TeamRenderers)
            {
                skinItem.SetTeam(teamType);
            }
        }

        public void ActivateSkin(TeamType teamType)
        {
            gameObject.SetActive(true);
            SetTeam(teamType);
            
            OnActivated?.Invoke();
        }

        public void DeactivateSkin()
        {
            gameObject.SetActive(false);
            
            OnDeactivated?.Invoke();
        }

        #endregion
    }
}