using BNG;
using CloudFine.ThrowLab;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MobaVR
{
    public class ThrowGrabbable : Grabbable
    {
        [SerializeField] private bool m_UseThrowHandle = true;
        
        private ThrowHandle _handle
        {
            get
            {
                if (m_handle == null)
                {
                    m_handle = GetComponent<ThrowHandle>();
                    if (m_handle == null)
                    {
                        m_handle = gameObject.AddComponent<ThrowHandle>();
                    }
                }
                return m_handle;
            }
        }
        private ThrowHandle m_handle;

        public override Vector3 GetGrabberWithGrabPointOffset(Grabber grabber, Transform grabPoint)
        {
            return base.GetGrabberWithGrabPointOffset(grabber, grabPoint);
        }

        public override void GrabItem(Grabber grabbedBy)
        {
            base.GrabItem(grabbedBy);

            if (m_UseThrowHandle && _handle.isActiveAndEnabled)
            {
                _handle.OnAttach(grabbedBy.gameObject, grabbedBy.gameObject);

                TrackedDevice trackedDevice = grabbedBy.GetComponentInParent<TrackedDevice>();
                //_handle.OnAttach(trackedDevice.gameObject, grabbedBy.gameObject);
            }
        }

        public override void GrabRemoteItem(Grabber grabbedBy)
        {
            base.GrabRemoteItem(grabbedBy);
        }

        public override void DropItem(Grabber droppedBy, bool resetVelocity, bool resetParent)
        {
            base.DropItem(droppedBy, resetVelocity, resetParent);
        }

        public override void Release(Vector3 velocity, Vector3 angularVelocity)
        {
            base.Release(velocity, angularVelocity);
            if (m_UseThrowHandle && _handle.isActiveAndEnabled)
            {
                _handle.OnDetach();
            }
        }

        /*
        public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            base.GrabBegin(hand, grabPoint);
            _handle.OnAttach(hand.gameObject, hand.gameObject);
        }

        public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
        {
            base.GrabEnd(linearVelocity, angularVelocity);
            _handle.OnDetach();
        }
  */

        public override void Update()
        {
            base.Update();
        }

        public override void UpdateVelocityPhysics()
        {
            base.UpdateVelocityPhysics();
        }
    }
}