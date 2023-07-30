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
        [SerializeField] private Scene m_SkyLandMap;
        [SerializeField] private Scene m_SkyLandProps;
        [SerializeField] private Scene m_MobaMap;
        [SerializeField] private Scene m_LichMap;
        [SerializeField] private Scene m_TavernMap;
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
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                m_CanLoad = true;
                SceneManager.SetActiveScene(scene);
            };

            SceneManager.sceneUnloaded += scene =>
            {
                if (!string.IsNullOrEmpty(m_CurrentMap))
                {
                    SceneManager.LoadSceneAsync(m_CurrentMap, LoadSceneMode.Additive);
                }
            };
        }

        private void LoadSceneAsync(string sceneName)
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
            photonView.RPC(nameof(RpcShowTavernMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowTavernMap()
        {
            LoadSceneAsync(m_TavernMap.name);
        }

        [ContextMenu("Show Sky Land")]
        public void ShowSkyLandMap()
        {
            photonView.RPC(nameof(RpcShowSkyLandMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowSkyLandMap()
        {
            LoadSceneAsync(m_SkyLandMap.name);
        }

        [ContextMenu("Show Sky Land Props")]
        public void ShowSkyLandWithPropMap()
        {
            photonView.RPC(nameof(RpcShowSkyLandWithPropMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowSkyLandWithPropMap()
        {
            LoadSceneAsync(m_SkyLandProps.name);
        }

        [ContextMenu("Show Moba")]
        public void ShowMobaMap()
        {
            photonView.RPC(nameof(RpcShowMobaMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowMobaMap()
        {
            LoadSceneAsync(m_MobaMap.name);
        }
        
        [ContextMenu("Show Lich")]
        public void ShowLichMap()
        {
            photonView.RPC(nameof(RpcShowLichMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowLichMap()
        {
            m_Settings.SetNight();
            LoadSceneAsync(m_LichMap.name);
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