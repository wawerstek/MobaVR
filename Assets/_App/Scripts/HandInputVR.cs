using BNG;
using UnityEngine;

namespace MobaVR
{
    public class HandInputVR : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private HandController m_HandController;
        [SerializeField] private Grabber m_Grabber;
        [SerializeField] private RemoteGrabber m_RemoteGrabber;

        [Header("Points")]
        [SerializeField] private Transform m_InsideHandPoint;
        [SerializeField] private Transform m_OutsideHandPoint;
        [SerializeField] private Transform m_FingerPoint;

        public HandController HandController => m_HandController;
        public Grabber Grabber => m_Grabber;
        public RemoteGrabber RemoteGrabber => m_RemoteGrabber;
        public Transform InsideHandPoint => m_InsideHandPoint;
        public Transform OutsideHandPoint => m_OutsideHandPoint;
        public Transform FingerPoint => m_FingerPoint;
    }
}