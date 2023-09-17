using Sirenix.OdinInspector;
using UnityEngine;

namespace MobaVR
{
    public class VoiceSpellBehaviour : SpellBehaviour
    {
        [SerializeField] private MicrophoneInput m_MicrophoneInput;
        [SerializeField] private float m_LoudnessSensibility = 100f;
        [SerializeField] private float m_Threshold = 0.1f;
        [SerializeField] [ReadOnly] private float m_CurrentLoudness;

        protected override void Update()
        {
            base.Update();
            if (!m_MicrophoneInput.IsInitialized || !m_MicrophoneInput.IsRecorded)
            {
                return;
            }

            m_CurrentLoudness = m_MicrophoneInput.GetLoundessFromMicrophone() * m_LoudnessSensibility;
            if (m_CurrentLoudness < m_Threshold)
            {
                m_CurrentLoudness = 0;
            }
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