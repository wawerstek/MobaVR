using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine.PVP
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/Init Mode State")]
    public class InitModeState : PvpClassicModeState
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