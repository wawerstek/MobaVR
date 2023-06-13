using System;
using BNG;
using UnityEngine;

namespace MobaVR
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField] private GameObject m_View;
        [SerializeField] private bool m_UseVRUISystem = false;
        [SerializeField] private ClassicGameSession m_GameSession;

        private VRUISystem m_VRUISystem;

        private void Start()
        {
            m_VRUISystem = FindObjectOfType<VRUISystem>();

            /*
            if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                m_VRUISystem.enabled = false;
            }
            else
            {
                m_VRUISystem.enabled = true;
            }
            */
            
            //m_VRUISystem.enabled = m_UseVRUISystem;
        }

        private void OnEnable()
        {
            //m_VRUISystem = FindObjectOfType<VRUISystem>();
            //m_VRUISystem.enabled = m_UseVRUISystem;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_View.SetActive(!m_View.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                if (m_VRUISystem != null)
                {
                    m_VRUISystem.enabled = !m_VRUISystem.enabled;
                }
            }
        }

        [ContextMenu("SwitchTeam")]
        public void SwitchTeam()
        {
            m_GameSession.SwitchTeam();
        }
    }
}