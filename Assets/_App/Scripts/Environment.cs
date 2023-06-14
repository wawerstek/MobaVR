using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class Environment : MonoBehaviourPun
    {
        [SerializeField] private Settings m_Settings;
        
        [Header("SkyLand")]
        [SerializeField] private GameObject m_SkyLandMap;
        [SerializeField] private GameObject m_SkyLandProps;
        [Header("Moba")]
        [SerializeField] private GameObject m_MobaMap;
        [Header("Lich")]
        [SerializeField] private GameObject m_LichMap;
        [Header("Tavern")]
        [SerializeField] private GameObject m_TavernMap;

        private GameObject m_CurrentMap = null;

        public UnityEvent OnShow;
        public UnityEvent OnHide;

        public void DeactivateCurrentMap()
        {
            if (m_CurrentMap != null)
            {
                m_CurrentMap.SetActive(false);
            }
        }

        public void DeactivateAll()
        {
            m_TavernMap.SetActive(false);
            m_SkyLandMap.SetActive(false);
            m_SkyLandProps.SetActive(false);
            m_MobaMap.SetActive(false);
            m_LichMap.SetActive(false);
        }

        public void ShowTavernMap()
        {
            photonView.RPC(nameof(RpcShowTavernMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowTavernMap()
        {
            DeactivateAll();
            m_TavernMap.SetActive(true);
        }

        public void ShowSkyLandMap()
        {
            photonView.RPC(nameof(RpcShowSkyLandMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowSkyLandMap()
        {
            DeactivateAll();
            m_SkyLandMap.SetActive(true);
            m_SkyLandProps.SetActive(false);
        }

        public void ShowSkyLandWithPropMap()
        {
            photonView.RPC(nameof(RpcShowSkyLandWithPropMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowSkyLandWithPropMap()
        {
            DeactivateAll();
            m_SkyLandMap.SetActive(true);
            m_SkyLandProps.SetActive(true);
        }

        public void ShowMobaMap()
        {
            photonView.RPC(nameof(RpcShowMobaMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowMobaMap()
        {
            DeactivateAll();
            m_MobaMap.SetActive(true);
        }
        
        public void ShowLichMap()
        {
            photonView.RPC(nameof(RpcShowLichMap), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcShowLichMap()
        {
            DeactivateAll();
            m_Settings.SetNight();
            m_LichMap.SetActive(true);
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