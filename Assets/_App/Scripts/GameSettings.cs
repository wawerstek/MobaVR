using System;
using BNG;
using UnityEngine;

namespace MobaVR
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField] private GameObject m_View;
        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private bool m_UseVRUISystem = false;
        [SerializeField] private ClassicGameSession m_GameSession;

        public GameObject _Call;
        
        private VRUISystem m_VRUISystem;

        private void Awake()
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
            /*
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                m_VRUISystem = FindObjectOfType<VRUISystem>();
                m_VRUISystem.enabled = m_UseVRUISystem;
            }
            */

            //LocomotionVREmulator l;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_View.SetActive(!m_View.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                m_VRUISystem = FindObjectOfType<VRUISystem>();
                if (m_VRUISystem != null)
                {
                    m_VRUISystem.enabled = !m_VRUISystem.enabled;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (m_Canvas != null)
                {
                    m_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
            }
        }

        [ContextMenu("SwitchTeam")]
        public void SwitchTeam()
        {
            m_GameSession.SwitchTeam();
        }
        
        public void ClearCal()
        {
            _Call.GetComponent<Calibr>().calibr = false;
        }
    }
}