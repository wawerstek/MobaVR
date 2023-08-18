using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/Start Mode State")]
    public class StartModeState : ModeState<ClassicMode>
    {
        protected override void UpdatePlayer(PlayerVR player)
        {
            base.UpdatePlayer(player);
            //player.WizardPlayer.RestoreHp();
            player.WizardPlayer.Reborn();
        }

        public override void Enter()
        {
            m_Mode.ZoneManager.Hide();
            m_Mode.ModeView.RedTeamScoreView.Hide();
            m_Mode.ModeView.RedTeamKillScoreView.Hide();
            
            m_Mode.BlueTeam.Score = 0;
            m_Mode.ModeView.BlueTeamScoreView.SetScore(0); //
            m_Mode.ModeView.BlueTeamScoreView.Show();
            
            m_Mode.RedTeam.Score = 0;
            m_Mode.ModeView.RedTeamScoreView.SetScore(0); //
            m_Mode.ModeView.RedTeamScoreView.Show();

            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();
                m_Mode.ReadyRound();
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