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
        [SerializeField] private SessionSettings m_SessionSettings;
        [SerializeField] private StateMachineSwitcher m_StateMachineSwitcher;
        [SerializeField] private Environment m_Environment;
        [SerializeField] private ScenesEnvironment m_SceneEnvironment;

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

            if (m_SceneEnvironment == null)
            {
                m_SceneEnvironment = FindObjectOfType<ScenesEnvironment>();
            }

            if (m_StateMachineSwitcher == null)
            {
                m_StateMachineSwitcher = FindObjectOfType<StateMachineSwitcher>();
            }

            if (m_SessionSettings == null)
            {
                m_SessionSettings = FindObjectOfType<SessionSettings>();
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

        public void SetPhysicsCollision(bool isIgnore)
        {
            Physics.IgnoreLayerCollision(10, 21, isIgnore);
            Physics.IgnoreLayerCollision(21, 10, isIgnore);
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

        public void ActivateRagdolls()
        {
            if (m_SessionSettings == null)
            {
                return;
            }

            m_SessionSettings.ActivateRagdolls();
        }

        public void DeactivateRagdolls()
        {
            if (m_SessionSettings == null)
            {
                return;
            }

            m_SessionSettings.DeactivateRagdolls();
        }

        public void SetEmptyType()
        {
            if (m_SessionSettings == null)
            {
                return;
            }

            m_SessionSettings.SetEmptyType();
        }

        public void SetAnimType()
        {
            if (m_SessionSettings == null)
            {
                return;
            }

            m_SessionSettings.SetAnimType();
        }

        public void SetRagDollType()
        {
            if (m_SessionSettings == null)
            {
                return;
            }

            m_SessionSettings.SetRagDollType();
        }

        public void ClearCal()
        {
            //_Call.GetComponent<Calibration>().calibr = false;
            //_Call.GetComponent<Calibration>().calibr = false;

            if (_Call.TryGetComponent(out CalibrationPol calibraion))
            {
                calibraion.calibr = false;
                calibraion.calibr = false;
            }
        }

        #region Game Mode

        public void StartMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.StartMode();
            }
        }

        public void CompleteMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.CompleteMode();
            }
        }

        public void DeactivateMode()
        {
            if (m_GameSession != null)
            {
                m_GameSession.DeactivateMode();
            }
        }

        #endregion

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

        public void SetClassMode()
        {
            if (m_StateMachineSwitcher == null)
            {
                m_StateMachineSwitcher = FindObjectOfType<StateMachineSwitcher>();
            }

            if (m_StateMachineSwitcher != null)
            {
                m_StateMachineSwitcher.SetClassMode();
            }
        }

        public void SetTimerMode()
        {
            if (m_StateMachineSwitcher == null)
            {
                m_StateMachineSwitcher = FindObjectOfType<StateMachineSwitcher>();
            }

            if (m_StateMachineSwitcher != null)
            {
                m_StateMachineSwitcher.SetTimerMode();
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
            m_SceneEnvironment.ShowTavernMap();
        }

        public void ShowSkyLandMap()
        {
            m_SceneEnvironment.ShowSkyLandMap();
        }

        public void ShowSkyLandWithPropMap()
        {
            m_SceneEnvironment.ShowSkyLandWithPropMap();
        }

        public void ShowMobaMap()
        {
            m_SceneEnvironment.ShowMobaMap();
        }

        public void ShowLichMap()
        {
            m_SceneEnvironment.ShowLichMap();
        }

        public void ShowNecropolisMap()
        {
            m_SceneEnvironment.ShowNecropolisMap();
        }

        public void ShowTowerDemoMap()
        {
            m_SceneEnvironment.ShowTowerDemoMap();
        }

        public void ShowTowerMap()
        {
            m_SceneEnvironment.ShowTowerMap();
        }

        public void ShowDungeonMap()
        {
            m_SceneEnvironment.ShowDungeonMap();
        }

        public void ShowDefaultPvPMap()
        {
            m_SceneEnvironment.ShowSkyLandMap();
        }

        public void ShowDefaultPvEMap()
        {
            m_SceneEnvironment.ShowLichMap();
        }

        ////
        ///
        ///
        public void Local_ShowTavernMap()
        {
            m_Environment.ShowTavernMap();
        }

        public void Local_ShowSkyLandMap()
        {
            m_Environment.ShowSkyLandMap();
        }

        public void Local_ShowSkyLandWithPropMap()
        {
            m_Environment.ShowSkyLandWithPropMap();
        }

        public void Local_ShowMobaMap()
        {
            m_Environment.ShowMobaMap();
        }

        public void Local_ShowLichMap()
        {
            m_Environment.ShowLichMap();
        }

        public void Local_ShowDefaultPvPMap()
        {
            m_Environment.ShowSkyLandMap();
        }

        public void Local_ShowDefaultPvEMap()
        {
            m_Environment.ShowLichMap();
        }
    }
}