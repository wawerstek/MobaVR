using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/States")]
    public class StatesSO : ScriptableObject
    {
        [Header("States")]
        [SerializeField] private ModeState m_InitModeState;
        [SerializeField] private ModeState m_InactiveModeState;
        [SerializeField] private ModeState m_StartModeState;
        [SerializeField] private ModeState m_ReadyRoundState;
        [SerializeField] private ModeState m_PlayRoundState;
        [SerializeField] private ModeState m_CompleteRoundState;
        [SerializeField] private ModeState m_CompleteModeState;

        public ModeState InitModeState => m_InitModeState;
        public ModeState InactiveModeState => m_InactiveModeState;
        public ModeState StartModeState => m_StartModeState;
        public ModeState ReadyRoundState => m_ReadyRoundState;
        public ModeState PlayRoundState => m_PlayRoundState;
        public ModeState CompleteRoundState => m_CompleteRoundState;
        public ModeState CompleteModeState => m_CompleteModeState;
    }
}