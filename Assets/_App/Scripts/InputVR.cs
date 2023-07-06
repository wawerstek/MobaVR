using BNG;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    [ShowOdinSerializedPropertiesInInspector]
    public class InputVR : MonoBehaviour
    {
        [Header("Base")]
        public InputBridge InputBridge;
        public BNGPlayerController BngPlayerController;
        public CharacterController CharacterController;

        [Header("Screen")]
        public BaseDamageIndicator DamageIndicator;
        
        [Header("Tracked Devices")]
        public Transform HeadDevice;
        public Transform LeftHandDevice;
        public Transform RightHandDevice;
        
        [Space]
        [Header("IK v1")]
        public Transform IKHead;
        public Transform IKLeftHand;
        public Transform IKRightHand;
        
        [Space]
        [Header("IK v2")]
        public Transform HeadTarget;
        public Transform LeftHandTarget;
        public Transform RightHandTarget;

        [Space]
        [Header("Hand Controllers")]
        public HandController LeftController;
        public HandController RightController;
        public SkinnedMeshRenderer RightHandSkinnedMeshRenderer;
        public SkinnedMeshRenderer LeftHandSkinnedMeshRenderer;

        [Header("Left Hand Grabber")]
        public Grabber LeftGrabber;
        public Transform LeftBigFireballPoint;
        public Transform LeftSmallFireballPoint;

        [Header("Right Hand Grabber")]
        public Grabber RightGrabber;
        public Transform RightBigFireballPoint;
        public Transform RightSmallFireballPoint;

        [Header("Hand InputVR")]
        public HandInputVR LefHandInputVR;
        public HandInputVR RightHandInputVR;
    }
}