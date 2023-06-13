using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/Inactive Mode State")]
    public class InactiveModeState : ModeState
    {
        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();
            }

            m_Mode.ModeView.BlueTeamScoreView.Hide();
            m_Mode.ModeView.RedTeamScoreView.Hide();
            m_Mode.ModeView.RoundTimeView.Hide();
            m_Mode.ModeView.VictoryView.Hide();
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}