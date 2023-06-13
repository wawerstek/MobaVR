using System;
using BNG;
using CloudFine.ThrowLab;
using UnityEngine;

namespace MobaVR
{
    //TODO: Need Refactor
    public class GrabbableThrowHandler : ThrowHandle
    {
        [SerializeField] private Grabbable m_Grabbable;
        [SerializeField] private bool m_UseGrabberToRecording;

        private Grabber m_Grabber;
        
        public override void OnAttach(GameObject hand, GameObject collisionRoot)
        {
            base.OnAttach(hand, collisionRoot);
            
            if (m_UseGrabberToRecording)
            {
                switch (Settings.sampleSourceType)
                {
                    case ThrowConfiguration.VelocitySource.DEVICE_CENTER_OF_MASS:
                    case ThrowConfiguration.VelocitySource.HAND_TRACKED_POSITION:
                        m_Grabber = hand.GetComponentInParent<Grabber>();
                        break;
                    case ThrowConfiguration.VelocitySource.OBJECT_CENTER:
                    case ThrowConfiguration.VelocitySource.OBJECT_CUSTOM_OFFSET:
                        m_Grabber = m_Grabbable.GetPrimaryGrabber();
                        break;
                }
            }
        }

        protected override void RecordVelocitySample(float deltaTime, float time)
        {
            if (!m_UseGrabberToRecording)
            {
                base.RecordVelocitySample(deltaTime, time);
                return;
            }
            
            //Transform anchor = GetSampleSource();

            if (m_Grabber == null)
            {
                return;
            }
            
            Vector3 currentPosition = m_Grabber.transform.position;
            Quaternion currentRotation = m_Grabber.transform.rotation;
            
            Vector3 positionDelta = currentPosition - _sampledPreviousPosition;
            Quaternion deltaRotation = currentRotation * Quaternion.Inverse(_sampledPreviousRotation);
            Vector3 angularDelta = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z);

            Vector3 velocity = m_Grabber.velocityTracker.GetVelocity();
            Vector3 angularVelocity = m_Grabber.velocityTracker.GetAngularVelocity();

            VelocitySample newSample = new VelocitySample(currentPosition, velocity, currentRotation, angularVelocity, time);
            _velocityHistory.Add(newSample);
            _sampledPreviousPosition = currentPosition;
            _sampledPreviousRotation = currentRotation;

            Debug.Log($"ThrowHandler Sample: {newSample}");
            Debug.Log($"ThrowHandler: positionDelta: {positionDelta}; deltaRotation: {deltaRotation}; angularDelta: {angularDelta}");
            Debug.Log($"ThrowHandler: currentPosition: {currentPosition}; currentRotation: {currentRotation}; euler: {currentRotation.eulerAngles}\n" +
                      $"velocity: {velocity}; angularVelocity: {angularVelocity}");
            
            ClearOldSamples();
            if (OnSampleRecorded != null)
            {
                OnSampleRecorded.Invoke(newSample);
            }
        }
    }
}