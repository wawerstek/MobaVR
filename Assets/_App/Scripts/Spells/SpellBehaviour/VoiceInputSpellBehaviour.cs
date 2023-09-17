using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class VoiceInputSpellBehaviour : InputSpellBehaviour
    {
        [Header("Microphone")]
        [SerializeField] private bool m_UseActivateButton = true;
        [SerializeField] private float m_Duration = 10f;
        [SerializeField] private MicrophoneInput m_MicrophoneInput;
        [SerializeField] private float m_LoudnessSensibility = 100f;
        [SerializeField] private float m_Threshold = 0.1f;
        [SerializeField] private float m_MinVoiceValue = 10f;
        [SerializeField] [ReadOnly] private float m_CurrentLoudness;

        private bool m_CanUseMicrophone = false;

        public Action OnVoiced;
        
        protected override void OnStartCast(InputAction.CallbackContext context)
        {
            base.OnStartCast(context);
            OnStarted?.Invoke();
        }

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);
            
            if (!CanCast() 
                || HasBlockingSpells()
                || !m_IsAvailable)
            {
                return;
            }
            
            m_CanUseMicrophone = true;
            StartRecord();
            
            OnPerformed?.Invoke();
            m_IsPerformed = true;
            Invoke(nameof(Stop), m_Duration);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            /*
            if (m_IsPerformed)
            {
                m_IsPerformed = false;
                WaitCooldown();
                Interrupt();
            }
            */
        }

        protected override void Interrupt()
        {
            base.Interrupt();

            StopRecord();
            
            if (m_IsPerformed)
            {
                WaitCooldown();
            }
            
            CancelInvoke(nameof(Stop));
            OnCompleted?.Invoke();
            m_IsPerformed = false;
            m_CanUseMicrophone = false;
        }

        private void Stop()
        {
            WaitCooldown();
            Interrupt();
        }

        protected override void Update()
        {
            base.Update();

            if (!m_CanUseMicrophone || !m_IsPerformed)
            {
                return;
            }
            
            if (!m_MicrophoneInput.IsInitialized || !m_MicrophoneInput.IsRecorded)
            {
                return;
            }

            m_CurrentLoudness = m_MicrophoneInput.GetLoundessFromMicrophone() * m_LoudnessSensibility;
            if (m_CurrentLoudness < m_Threshold)
            {
                m_CurrentLoudness = 0;
            }

            if (m_CurrentLoudness >= m_MinVoiceValue)
            {
                ExecuteVoice();
            }
        }

        protected virtual void ExecuteVoice()
        {
            OnVoiced?.Invoke();
        }

        [ContextMenu("Start Record")]
        private void StartRecord()
        {
            m_MicrophoneInput.Start();
        }

        [ContextMenu("Stop Record")]
        private void StopRecord()
        {
            m_MicrophoneInput.Stop();
        }
    }
}