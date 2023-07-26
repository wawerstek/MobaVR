using Photon.Pun;
using UnityEngine;

namespace BNG
{
    public class NetworkFollowTransform : MonoBehaviourPun
    {
        public Transform FollowTarget;
        public bool MatchRotation = true;

        private void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (FollowTarget)
            {
                transform.position = FollowTarget.position;

                if (MatchRotation)
                {
                    transform.rotation = FollowTarget.rotation;
                }
            }
        }
    }
}