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
        [SerializeField] private ModeState<ClassicMode> m_InitModeState;
        [SerializeField] private ModeState<ClassicMode> m_InactiveModeState;
        [SerializeField] private ModeState<ClassicMode> m_StartModeState;
        [SerializeField] private ModeState<ClassicMode> m_ReadyRoundState;
        [SerializeField] private ModeState<ClassicMode> m_PlayRoundState;
        [SerializeField] private ModeState<ClassicMode> m_CompleteRoundState;
        [SerializeField] private ModeState<ClassicMode> m_CompleteModeState;

        public ModeState<ClassicMode> InitModeState => m_InitModeState;
        public ModeState<ClassicMode> InactiveModeState => m_InactiveModeState;
        public ModeState<ClassicMode> StartModeState => m_StartModeState;
        public ModeState<ClassicMode> ReadyRoundState => m_ReadyRoundState;
        public ModeState<ClassicMode> PlayRoundState => m_PlayRoundState;
        public ModeState<ClassicMode> CompleteRoundState => m_CompleteRoundState;
        public ModeState<ClassicMode> CompleteModeState => m_CompleteModeState;
    }
}