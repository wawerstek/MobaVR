using System;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MobaVR
{
    public class ScenesEnvironment : MonoBehaviourPun
    {
        [SerializeField] private Settings m_Settings;
        
        [Header("Maps")]
        [SerializeField] private string m_SkyLandMap;
        [SerializeField] private string m_SkyLandProps;
        [SerializeField] private string m_MobaMap;
        [SerializeField] private string m_LichMap;
        [SerializeField] private string m_TavernMap;

        [Header("Default Map")]
        [SerializeField] private bool m_IsLoadDefaultMap;
        [SerializeField] private string m_DefaultMap;
        /*
        [SerializeField] private SceneAsset m_SkyLandMap;
        [SerializeField] private SceneAsset m_SkyLandProps;
        [SerializeField] private SceneAsset m_MobaMap;
        [SerializeField] private SceneAsset m_LichMap;
        [SerializeField] private SceneAsset m_TavernMap;
        */
        private string m_CurrentMap;
        private bool m_CanLoad = true;
        
        public UnityEvent OnShow;
        public UnityEvent OnHide;

        private void Awake()
        {
            ClearScenes();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            //SceneManager.sceneUnloaded += SceneManagerOnsceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
            //SceneManager.sceneUnloaded -= SceneManagerOnsceneUnloaded;
        }

        private void SceneManagerOnsceneUnloaded(Scene arg0)
        {
            if (!string.IsNullOrEmpty(m_CurrentMap))
            {
                SceneManager.LoadSceneAsync(m_CurrentMap, LoadSceneMode.Additive);
            }
        }

        private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.name.Equals(m_DefaultMap))
            {
                m_CanLoad = true;
                SceneManager.SetActiveScene(scene);
            }
        }

        private void ClearScenes()
        {
            if (SceneManager.sceneCount > 0)
            {
                for (int i = SceneManager.sceneCount - 1; i >= 1; i--)
                {
                    //SceneManager.UnloadSceneAsync(i);
                    //SceneManager.UnloadSceneAsync(1);
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                }
            }
        }

        private void Start()
        {
            if (m_IsLoadDefaultMap)
            {
                LoadSceneAsync(m_DefaultMap);
            }
        }

        private void Update()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (!PhotonNetwork.AutomaticallySyncScene)
                {
                    photonView.RPC(nameof(RpcShowTavernMap), RpcTarget.All);
                }

                else
                {
                    RpcShowTavernMap();
                }
                //LoadScene(m_SkyLandMap);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (!PhotonNetwork.AutomaticallySyncScene)
                {
                    photonView.RPC(nameof(RpcShowSkyLandMap), RpcTarget.All);
                }

                else
                {
                    RpcShowSkyLandMap();
                }

                //LoadScene(m_TavernMap);
            }
        }

        private void LoadScene(string sceneName)
        {
            if (!m_CanLoad)
            {
                return;
            }

            /*
            if (!string.IsNullOrEmpty(m_CurrentMap))
            {
                SceneManager.UnloadSceneAsync(m_CurrentMap);
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneName);
            }
            */
            
            //PhotonNetwork.scene

            SceneManager.LoadSceneAsync(sceneName);

            m_CurrentMap = sceneName;
        }

        private void LoadSceneAsync(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
            m_CurrentMap = sceneName;
        }
        
        private void LoadSceneAsync1(string sceneName)
        {
            if (!m_CanLoad)
            {
                return;
            }

            m_CanLoad = false;

            if (!string.IsNullOrEmpty(m_CurrentMap))
            {
                SceneManager.UnloadSceneAsync(m_CurrentMap);
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }

            m_CurrentMap = sceneName;
        }
        
        [ContextMenu("Show Tavern")]
        public void ShowTavernMap()
        {
            if (!PhotonNetwork.AutomaticallySyncScene)
            {
                photonView.RPC(nameof(RpcShowTavernMap), RpcTarget.All);
            }
            else
            {
                RpcShowTavernMap();
            }
        }
        
        [PunRPC]
        public void RpcShowTavernMap()
        {
            //LoadSceneAsync(m_TavernMap.name);
            LoadSceneAsync(m_TavernMap);
        }

        [ContextMenu("Show Sky Land")]
        public void ShowSkyLandMap()
        {
            if (!PhotonNetwork.AutomaticallySyncScene)
            {
                photonView.RPC(nameof(RpcShowSkyLandMap), RpcTarget.All);
            }
            else
            {
                RpcShowSkyLandMap();
            }
        }
        
        [PunRPC]
        public void RpcShowSkyLandMap()
        {
            //LoadSceneAsync(m_SkyLandMap.name);
            LoadSceneAsync(m_SkyLandMap);
        }

        [ContextMenu("Show Sky Land Props")]
        public void ShowSkyLandWithPropMap()
        {
            if (!PhotonNetwork.AutomaticallySyncScene)
            {
                photonView.RPC(nameof(RpcShowSkyLandWithPropMap), RpcTarget.All);
            }
            else
            {
                RpcShowSkyLandWithPropMap();
            }
        }
        
        [PunRPC]
        public void RpcShowSkyLandWithPropMap()
        {
            //LoadSceneAsync(m_SkyLandProps.name);
            LoadSceneAsync(m_SkyLandProps);
        }

        [ContextMenu("Show Moba")]
        public void ShowMobaMap()
        {
            if (!PhotonNetwork.AutomaticallySyncScene)
            {
                photonView.RPC(nameof(RpcShowMobaMap), RpcTarget.All);
            }
            else
            {
                RpcShowMobaMap();
            }
        }
        
        [PunRPC]
        public void RpcShowMobaMap()
        {
            //LoadSceneAsync(m_MobaMap.name);
            LoadSceneAsync(m_MobaMap);
        }
        
        [ContextMenu("Show Lich")]
        public void ShowLichMap()
        {
            if (!PhotonNetwork.AutomaticallySyncScene)
            {
                photonView.RPC(nameof(RpcShowLichMap), RpcTarget.All);
            }
            else
            {
                RpcShowLichMap();
            }
        }
        
        [PunRPC]
        public void RpcShowLichMap()
        {
            m_Settings.SetNight();
            //LoadSceneAsync(m_LichMap.name);
            LoadSceneAsync(m_LichMap);
        }
        
        public void ShowDefaultPvPMap()
        {
            ShowSkyLandMap();
        }
        
        [PunRPC]
        public void RpcShowDefaultPvPMap()
        {
            ShowSkyLandMap();
        }

        public void ShowDefaultPvEMap()
        {
            ShowLichMap();
        }
        
        [PunRPC]
        public void RpcShowDefaultPvEMap()
        {
            ShowLichMap();
        }
    }
}