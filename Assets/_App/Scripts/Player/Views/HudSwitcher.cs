using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class HudSwitcher : MonoBehaviourPun
    {
        [SerializeField] private List<GameObject> m_LocalHuds = new List<GameObject>();
        [SerializeField] private List<GameObject> m_RemoteViews = new List<GameObject>();
        
        private bool isShowLocalHud = false;
        private bool isShowRemoteHud = false;
        
        public void ShowLocalHud()
        {
            photonView.RPC(nameof(SetVisibilityLocalHud), RpcTarget.All, true);
        }

        public void HideLocalHud()
        {
            photonView.RPC(nameof(SetVisibilityLocalHud), RpcTarget.All, false);
        }

        public void SwitchLocalHud()
        {
            photonView.RPC(nameof(SetVisibilityLocalHud), RpcTarget.All, !isShowLocalHud);
        }
        
        [PunRPC]
        public void SetVisibilityLocalHud(bool isShow)
        {
            isShowLocalHud = isShow;
            
            foreach (var adaptiveHud in m_LocalHuds)
            {
                adaptiveHud.gameObject.SetActive(isShow);
            }
            
            /*
            var adaptiveHuds = GetComponentsInChildren<AdaptiveHUGorizontal>();
            foreach (var adaptiveHud in adaptiveHuds)
            {
                adaptiveHud.gameObject.SetActive(isShowLocalHud);
            }
            */
        }
        
        //-----------//
        
        public void ShowRemoteHud()
        {
            photonView.RPC(nameof(SetVisibilityRemoteHud), RpcTarget.All, true);
        }

        public void HideRemoteHud()
        {
            photonView.RPC(nameof(SetVisibilityRemoteHud), RpcTarget.All, false);
        }

        [PunRPC]
        public void SetVisibilityRemoteHud(bool isShow)
        {
            isShowRemoteHud = isShow;
            
            foreach (var adaptiveHud in m_RemoteViews)
            {
                adaptiveHud.gameObject.SetActive(isShow);
            }
            
            /*
            var playerViews = GetComponentsInChildren<PlayerView>();
            foreach (var playerView in playerViews)
            {
                playerView.gameObject.SetActive(isShowRemoteHud);
            }
            */
        }

        public void SwitchRemoteHud()
        {
            photonView.RPC(nameof(SetVisibilityRemoteHud), RpcTarget.All, !isShowRemoteHud);
        }
    }
}