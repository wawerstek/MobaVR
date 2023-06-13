using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/Init Mode State")]
    public class InitModeState : ModeState
    {
        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();
                m_StateMachine.DeactivateMode();
            }
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
        }
    }
}