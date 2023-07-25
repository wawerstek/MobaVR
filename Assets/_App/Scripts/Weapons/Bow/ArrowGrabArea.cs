using BNG;
using UnityEngine;

namespace MobaVR.Weapons.Bow
{
    public class ArrowGrabArea : MonoBehaviour
    {
        private Bow m_Bow;

        private void Start()
        {
            m_Bow = transform.parent.GetComponent<Bow>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Grabber grabber = other.GetComponent<Grabber>();
            if (grabber != null)
            {
                m_Bow.ClosestGrabber = grabber;
                if (!grabber.HoldingItem)
                {
                    m_Bow.CanGrabArrow = true;
                }
                else if (grabber.HoldingItem && grabber.HeldGrabbable != null)
                {
                    Arrow arrow = grabber.HeldGrabbable.GetComponent<Arrow>();
                    if (arrow != null && m_Bow.GrabbedArrow == null)
                    {
                        arrow.AttachBow(m_Bow);
                        m_Bow.GrabArrow(arrow);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Grabber grabObject = other.GetComponent<Grabber>();
            if (m_Bow.ClosestGrabber != null && grabObject != null && m_Bow.ClosestGrabber == grabObject)
            {
                m_Bow.CanGrabArrow = false;
                m_Bow.ClosestGrabber = null;
            }
        }
    }
}