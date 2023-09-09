using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using BNG;
using UnityEngine;

namespace MobaVR
{
    public class GrabbableOwner : MonoBehaviourPun, IPunOwnershipCallbacks
    {
        [SerializeField] private bool isCheckOnGrab = true;
        
        private Grabber leftGrabber;
        private GrabbablesInTrigger leftTriggerGrabber;
        private Grabber rightGrabber;
        private GrabbablesInTrigger rightGrabberTrigger;

        private double lastRequestTime = 0;
        private float requestInterval = 0.1f;
        private Dictionary<int, double> requestedGrabbables;

        private bool _syncLeftHoldingItem;
        private bool _syncRightHoldingItem;

        private bool _isLeftGrabbed = false;
        private bool _isRightGrabbed = false;

        #if PUN_2_OR_NEWER

        private void OnEnable()
        {
            
        }

        private void OnDestroy()
        {
            if (photonView.IsMine)
            {
                Unsubscribe();
            }
        }

        private void Start()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            InputVR inputVR = FindObjectOfType<InputVR>();

            leftGrabber = inputVR.LeftGrabber;
            leftTriggerGrabber = leftGrabber.GetComponent<GrabbablesInTrigger>();

            rightGrabber = inputVR.RightGrabber;
            rightGrabberTrigger = rightGrabber.GetComponent<GrabbablesInTrigger>();

            Subscribe();
            
            requestedGrabbables = new Dictionary<int, double>();
        }

        private void Update()
        {
            CheckGrabbablesTransfer();
        }

        private void Subscribe()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (leftGrabber != null)
            {
                leftGrabber.onGrabEvent.AddListener(OnLeftGrab);
                leftGrabber.onReleaseEvent.AddListener(OnLeftRelease);
            }
            
            if (rightGrabber != null)
            {
                rightGrabber.onGrabEvent.AddListener(OnRightGrab);
                rightGrabber.onReleaseEvent.AddListener(OnRightRelease);
            }
        }

        private void Unsubscribe()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (leftGrabber != null)
            {
                leftGrabber.onGrabEvent.RemoveListener(OnLeftGrab);
                leftGrabber.onReleaseEvent.RemoveListener(OnLeftRelease);
            }
            
            if (rightGrabber != null)
            {
                rightGrabber.onGrabEvent.RemoveListener(OnRightGrab);
                rightGrabber.onReleaseEvent.RemoveListener(OnRightRelease);
            }
        }

        private void OnLeftGrab(Grabbable grabbable)
        {
            _isLeftGrabbed = true;
        }
        
        private void OnLeftRelease(Grabbable grabbable)
        {
            _isLeftGrabbed = false;
        }

        private void OnRightGrab(Grabbable grabbable)
        {
            _isRightGrabbed = true;
        }
        
        private void OnRightRelease(Grabbable grabbable)
        {
            _isRightGrabbed = false;
        }

        private void CheckGrabbablesTransfer()
        {
            if (PhotonNetwork.Time - lastRequestTime < requestInterval)
            {
                return;
            }

            if (!isCheckOnGrab || _isLeftGrabbed)
            {
                RequestOwnerShipForNearbyGrabbables(leftTriggerGrabber);
            }

            if (!isCheckOnGrab || _isRightGrabbed)
            {
                RequestOwnerShipForNearbyGrabbables(rightGrabberTrigger);
            }
        }

        private void RequestOwnerShipForNearbyGrabbables(GrabbablesInTrigger grabbables)
        {
            if (grabbables == null)
            {
                return;
            }
            
            grabbables.RemoveNullKeys();

            // In Hand
            foreach (KeyValuePair<Collider, Grabbable> grab in grabbables.NearbyGrabbables)
            {
                PhotonView view = grab.Value.GetComponent<PhotonView>();
                if (view.OwnershipTransfer == OwnershipOption.Fixed)
                {
                    continue;
                }

                if (grab.Value.BeingHeld)
                {
                    continue;
                }

                if (view != null && RecentlyRequested(view) == false && !view.AmOwner)
                {
                    RequestGrabbableOwnership(view);
                }
            }

            // Remote Grabbables
            foreach (KeyValuePair<Collider, Grabbable> grab in grabbables.ValidRemoteGrabbables)
            {
                PhotonView view = grab.Value.GetComponent<PhotonView>();
                
                if (view.OwnershipTransfer != OwnershipOption.Fixed)
                {
                    continue;
                }
                
                if (grab.Value.BeingHeld)
                {
                    continue;
                }

                if (view != null && RecentlyRequested(view) == false && !view.AmOwner)
                {
                    RequestGrabbableOwnership(view);
                }
            }
        }

        public virtual bool RecentlyRequested(PhotonView view)
        {
            // Previously requested if in list and requested less than 3 seconds ago
            return requestedGrabbables != null 
                   && requestedGrabbables.ContainsKey(view.ViewID) 
                   && PhotonNetwork.Time - requestedGrabbables[view.ViewID] < 3f;
        }

        public virtual void RequestGrabbableOwnership(PhotonView view)
        {
            lastRequestTime = PhotonNetwork.Time;

            if (requestedGrabbables.ContainsKey(view.ViewID))
            {
                requestedGrabbables[view.ViewID] = lastRequestTime;
            }
            else
            {
                requestedGrabbables.Add(view.ViewID, lastRequestTime);
            }

            view.RequestOwnership();
        }


        // Handle Ownership Requests (Ex: Grabbable Ownership)
        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            bool amOwner = targetView.AmOwner || (targetView.Owner == null && PhotonNetwork.IsMasterClient);

            NetworkedGrabbable netGrabbable = targetView.gameObject.GetComponent<NetworkedGrabbable>();
            if (netGrabbable != null)
            {
                // Authorize transfer of ownership if we're not holding it
                if (amOwner && !netGrabbable.BeingHeld)
                {
                    targetView.TransferOwnership(requestingPlayer.ActorNumber);
                    return;
                }
            }
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player requestingPlayer)
        {
            // Debug.Log("OnOwnershipTransfered to Player " + requestingPlayer);
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player requestingPlayer)
        {
            // Debug.Log("OnOwnershipTransferFailed for Player " + requestingPlayer);
        }

        #endif
    }
}