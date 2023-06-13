using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MobaVR
{
    public class PlayerStateRenderer : MonoBehaviour
    {
        [SerializeField] private WizardPlayer m_Player;
        [SerializeField] private List<Renderer> m_Renderers = new();
        
        private List<Color> m_InitColors = new();

        private void OnEnable()
        {
            m_Player.OnReborn += OnReborn;
            m_Player.OnDie += OnDie;
        }

        private void OnDisable()
        {
            m_Player.OnReborn -= OnReborn;
            m_Player.OnDie -= OnDie;
        }

        private void Awake()
        {
            foreach (Renderer playerRenderer in m_Renderers)
            {
                m_InitColors.Add(playerRenderer.material.color);
            }
            
            //Дикий костыль
            if (m_Player.photonView.IsMine)
            {
                InputVR inputVR = FindObjectOfType<InputVR>();
                if (inputVR != null)
                {
                    TeamItem[] teamItems = inputVR.GetComponentsInChildren<TeamItem>();
                    foreach (TeamItem teamItem in teamItems)
                    {
                        Renderer[] renderers = teamItem.GetComponentsInChildren<Renderer>();
                        foreach (var teamItemRenderer in renderers)
                        {
                            m_Renderers.Add(teamItemRenderer);
                            m_InitColors.Add(teamItemRenderer.material.color);
                        }
                    }
                }
            }
        }

        private void OnDie()
        {
            SetVisibility(false);
        }

        private void OnReborn()
        {
            SetVisibility(true);
        }

        private void SetVisibility(bool isVisible)
        {
            for (var i = 0; i < m_Renderers.Count; i++)
            {
                Renderer playerRenderer = m_Renderers[i];
                if (playerRenderer.gameObject.activeSelf)
                {
                    playerRenderer.material.color = isVisible ? m_InitColors[i] : Color.black;
                }
            }
        }
    }
}