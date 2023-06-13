using Photon.Pun;
using UnityEngine;

namespace MobaVR.Utils
{
    public static class PhotonViewExtension
    {
        public static bool TryGetComponent<T>(this PhotonView photonView, int viewId, out T outComponent) where T : Component
        {
            PhotonView playerPhotonView = PhotonView.Find(viewId);
            if (playerPhotonView != null)
            {
                return playerPhotonView.TryGetComponent(out outComponent);
            }

            outComponent = null;
            return false;
        }
        
        public static bool TryGetComponent<T>(int viewId, out T outComponent) where T : Component
        {
            PhotonView playerPhotonView = PhotonView.Find(viewId);
            if (playerPhotonView != null)
            {
                return playerPhotonView.TryGetComponent(out outComponent);
            }

            outComponent = null;
            return false;
        }
    }
}