using System;
using MobaVR.ClassicModeStateMachine;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    public class StateMachine : MonoBehaviourPun, IClassicModeState
    {
        [Header("States")]
        [SerializeField] private ModeState m_InitModeState;
        [SerializeField] private ModeState m_InactiveModeState;
        [SerializeField] private ModeState m_StartModeState;
        [SerializeField] private ModeState m_ReadyRoundState;
        [SerializeField] private ModeState m_PlayRoundState;
        [SerializeField] private ModeState m_CompleteRoundState;
        [SerializeField] private ModeState m_CompleteModeState;

        [Header("Current State")]
        [SerializeField] private bool m_IsInitOnAwake = true;
        [SerializeField] private ClassicModeState m_ModeState = ClassicModeState.MODE_INACTIVE;
        [SerializeField] private ModeState m_CurrentState;

        private ClassicMode m_Mode;

        public event Action<ModeState> OnStateChanged;

        public ModeState CurrentState => m_CurrentState;
        public ModeState InactiveModeState => m_InactiveModeState;
        public ModeState InitModeState => m_InitModeState;
        public ModeState StartModeState => m_StartModeState;
        public ModeState ReadyRoundState => m_ReadyRoundState;
        public ModeState PlayRoundState => m_PlayRoundState;
        public ModeState CompleteRoundState => m_CompleteRoundState;
        public ModeState CompleteModeState => m_CompleteModeState;
        public ClassicModeState ModeState => m_ModeState;
        public ClassicMode Mode => m_Mode;

        /*
        public StateMachine(ClassicMode mode)
        {
            Init(mode);
        }
        */

        private void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }

        public void Init(ClassicMode mode)
        {
            m_InitModeState.Init(mode, this);
            m_InactiveModeState.Init(mode, this);
            m_StartModeState.Init(mode, this);
            m_ReadyRoundState.Init(mode, this);
            m_PlayRoundState.Init(mode, this);
            m_CompleteRoundState.Init(mode, this);
            m_CompleteModeState.Init(mode, this);

            if (m_IsInitOnAwake)
            {
                //RpcDeactivateMode();
                DeactivateMode();
            }
        }

        public void SetState(ModeState nextState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            m_CurrentState = nextState;
            nextState.Enter();
            OnStateChanged?.Invoke(nextState);
        }

        public void InitMode()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcInitMode), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RpcInitMode()
        {
            m_ModeState = ClassicModeState.MODE_INIT;
            SetState(m_InitModeState);
        }

        public void DeactivateMode()
        {
            //if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcDeactivateMode), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RpcDeactivateMode()
        {
            m_ModeState = ClassicModeState.MODE_INACTIVE;
            SetState(m_InactiveModeState);
        }

        public void StartMode()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcStartMode), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RpcStartMode()
        {
            m_ModeState = ClassicModeState.MODE_START;
            SetState(m_StartModeState);
        }

        public void ReadyRound()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcReadyRound), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RpcReadyRound()
        {
            m_ModeState = ClassicModeState.ROUND_READY;
            SetState(m_ReadyRoundState);
        }

        public void PlayRound()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcPlayRound), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RpcPlayRound()
        {
            m_ModeState = ClassicModeState.ROUND_PLAY;
            SetState(m_PlayRoundState);
        }

        public void CompleteRound()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcCompleteRound), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RpcCompleteRound()
        {
            m_ModeState = ClassicModeState.ROUND_COMPLETE;
            SetState(m_CompleteRoundState);
        }

        public void CompleteMode()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcCompleteMode), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RpcCompleteMode()
        {
            m_ModeState = ClassicModeState.MODE_COMPLETE;
            SetState(m_CompleteModeState);
        }
    }
}