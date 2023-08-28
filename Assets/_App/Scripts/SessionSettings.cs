using Photon.Pun;

namespace MobaVR
{
    public class SessionSettings : MonoBehaviourPun
    {
        public void ActivateRagdolls()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            photonView.RPC(nameof(RpcSetRagdolls), RpcTarget.All, true);
        }

        public void DeactivateRagdolls()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            photonView.RPC(nameof(RpcSetRagdolls), RpcTarget.All, false);
        }

        [PunRPC]
        private void RpcSetRagdolls(bool useRagdoll)
        {
            SkinRagdollSwitcher[] ragdollSwitchers = FindObjectsOfType<SkinRagdollSwitcher>();
            foreach (SkinRagdollSwitcher skinRagdollSwitcher in ragdollSwitchers)
            {
                skinRagdollSwitcher.SetRagdoll(useRagdoll);
            }
        }
    }
}