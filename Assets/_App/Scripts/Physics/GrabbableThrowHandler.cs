using System;
using BNG;
using CloudFine.ThrowLab;
using UnityEngine;

namespace MobaVR
{
    //TODO: Need Refactor
    public class GrabbableThrowHandler : ThrowHandle, IPhysics
    {
        [SerializeField] private ThrowConfigurationSetSO m_ThrowConfigurationSetSO;
        [SerializeField] private bool m_UseGrabberToRecording = true;
        
        private Rigidbody m_Rigidbody;
        private Throwable m_Throwable;
        private Grabbable m_Grabbable;
        private Grabber m_Grabber;
        
        public bool UseGrabberToRecording
        {
            get => m_UseGrabberToRecording;
            set => m_UseGrabberToRecording = value;
        }

        private void OnEnable()
        {
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.AddListener(OnThrown);
                m_Throwable.OnGrabbed.AddListener(OnGrabItem);
                m_Throwable.OnReleased.AddListener(OnReleaseItem);
            }
        }

        private void OnDisable()
        {
            if (m_Throwable != null)
            {
                m_Throwable.OnGrabbed.RemoveListener(OnGrabItem);
                m_Throwable.OnReleased.RemoveListener(OnReleaseItem);
                m_Throwable.OnThrown.RemoveListener(OnReleaseItem);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out m_Rigidbody);
            TryGetComponent(out m_Throwable);
            TryGetComponent(out m_Grabbable);
            
            if (m_ThrowConfigurationSetSO == null)
            {
                m_ThrowConfigurationSetSO = Resources.Load<ThrowConfigurationSetSO>("Api/Physics/ThrowConfigurationSet");
            }

            if (m_ThrowConfigurationSetSO != null)
            {
                _throwConfigurationSet = m_ThrowConfigurationSetSO.ThrowConfigurationSet;
            }
        }
        
        private void OnGrabItem(Grabber grabber)
        {
            OnAttach(grabber.gameObject, grabber.gameObject);
        }
        
        private void OnReleaseItem()
        {
            OnDetach();
        }
        
        private void OnThrown()
        {
            m_Rigidbody.useGravity = true;
        }

        public override void OnAttach(GameObject hand, GameObject collisionRoot)
        {
            base.OnAttach(hand, collisionRoot);
            
            if (UseGrabberToRecording)
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
            if (!UseGrabberToRecording)
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

        public void UpdatePhysics()
        {
    
        }
    }
}