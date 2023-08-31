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


        public void SetEmptyType()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            photonView.RPC(nameof(RpcSetDieBehaviour), RpcTarget.All, OnDieBehaviourType.NONE);
        }

        public void SetAnimType()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            photonView.RPC(nameof(RpcSetDieBehaviour), RpcTarget.All, OnDieBehaviourType.ANIM);
        }

        public void SetRagDollType()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            photonView.RPC(nameof(RpcSetDieBehaviour), RpcTarget.All, OnDieBehaviourType.RAGDOLL);
        }

        [PunRPC]
        public void RpcSetDieBehaviour(OnDieBehaviourType dieBehaviourType)
        {
            SkinRagdollSwitcher[] ragdollSwitchers = FindObjectsOfType<SkinRagdollSwitcher>();
            foreach (SkinRagdollSwitcher skinRagdollSwitcher in ragdollSwitchers)
            {
                skinRagdollSwitcher.SetDieBehaviour(dieBehaviourType);
            }
        }
    }
}