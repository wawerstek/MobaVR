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
    public class AnimalSkin : TeamItem, ISkin
    {
        public string ID;

        [Header("Armature")]
        [SerializeField] private Transform m_Armature;
        [SerializeField] [ReadOnly] private float m_ArmatureScale = 0.54f;

        [Header("Team")]
        [SerializeField] private List<SkinItem> m_TeamRenderers = new();
        [SerializeField] private List<Renderer> m_HiddenVrRenderers = new();

        [Space]
        [Header("Events")]
        public UnityEvent OnActivated;
        public UnityEvent OnDeactivated;
        public UnityEvent OnDie;

        public Transform Armature => m_Armature;

        #region Find Renderers

        [ContextMenu("FindArmature")]
        private void FindArmature()
        {
            if (m_Armature == null)
            {
                m_Armature = transform.Find("Armature");
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

        [ContextMenu("SetVisibility")]
        public void SetVisibilityVR(bool isVisible = false)
        {
            foreach (Renderer meshRenderer in m_HiddenVrRenderers)
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

        public void SetDieSkin()
        {
            gameObject.SetActive(false);
            OnDie?.Invoke();
        }

        #endregion
    }
}