using System;
using System.Windows.Forms;
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
        [SerializeField] private Environment m_Environment;

        public GameObject _Call;

        private VRUISystem m_VRUISystem;

        private void Awake()
        {
            if (m_GameSession == null)
            {
                m_GameSession = FindObjectOfType<ClassicGameSession>();
            }
            
            if (m_Environment == null)
            {
                m_Environment = FindObjectOfType<Environment>();
            }
            
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

            if (Input.GetKeyDown(KeyCode.M))
            {
                m_View.SetActive(!m_View.activeSelf);

                if (m_Canvas != null)
                {
                    m_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }

                m_VRUISystem = FindObjectOfType<VRUISystem>();
                if (m_VRUISystem != null)
                {
                    //m_VRUISystem.enabled = !m_View.activeSelf;
                    m_VRUISystem.enabled = !m_View.activeSelf;
                }
            }
        }

        [UnityEngine.ContextMenu("SwitchTeam")]
        public void SwitchTeam()
        {
            if (m_GameSession != null)
            {
                m_GameSession.SwitchTeam();
            }
        }

        public void SetRedTeam()
        {
            if (m_GameSession != null)
            {
                m_GameSession.SetRedTeam();
            }
        }

        public void SetBlueTeam()
        {
            if (m_GameSession != null)
            {
                m_GameSession.SetBlueTeam();
            }
        }

        public void ClearCal()
        {
            _Call.GetComponent<Calibration>().calibr = false;
        }
        
        #region PvP Mode

        public void StartPvPMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.StartPvPMode();
            }
        }

        public void CompletePvPMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.CompletePvPMode();
            }
        }

        public void DeactivatePvPMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.DeactivatePvPMode();
            }
        }

        #endregion

        #region PvE Mode

        public void StartPvEMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.StartPvEMode();
            }
        }

        public void CompletePvEMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.CompletePvEMode();
            }
        }
        
        public void SetMasterClient()
        {
            if (m_GameSession != null)
            {
                m_GameSession.SetMaster();
            }
        }

        #endregion
        
        
        ////
        ///
        ///
        public void ShowTavernMap()
        {
            m_Environment.ShowTavernMap();
        }
        
        public void ShowSkyLandMap()
        {
            m_Environment.ShowSkyLandMap();
        }
        
        public void ShowSkyLandWithPropMap()
        {
            m_Environment.ShowSkyLandWithPropMap();
        }
        
        public void ShowMobaMap()
        {
            m_Environment.ShowMobaMap();
        }
        
        public void ShowLichMap()
        {
            m_Environment.ShowLichMap();
        }
        
        public void ShowDefaultPvPMap()
        {
            m_Environment.ShowSkyLandMap();
        }
        
        public void ShowDefaultPvEMap()
        {
            m_Environment.ShowLichMap();
        }
    }
}