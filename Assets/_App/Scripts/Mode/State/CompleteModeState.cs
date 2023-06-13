using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/Complete Mode State")]
    public class CompleteModeState : ModeState
    {
        protected override void UpdatePlayer(PlayerVR player)
        {
            base.UpdatePlayer(player);
            player.WizardPlayer.Reborn();
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();
            }
            
            m_Mode.ModeView.VictoryView.Show();
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
            m_Mode.ModeView.VictoryView.Hide();
        }
    }
}