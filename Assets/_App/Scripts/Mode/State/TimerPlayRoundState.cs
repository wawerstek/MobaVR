using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/Play Timer Round State")]
    public class TimerPlayRoundState : ModeState
    {
        [SerializeField] private float m_Time = 3f;
        [SerializeField] private bool m_IsSumKills = true;
        private float m_CurrentTime;
        private bool m_IsTimeout = false;

        private int m_RedScore = 0;
        private int m_RedRoundScore = 0;
        private int m_BlueScore = 0;
        private int m_BlueRoundScore = 0;

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

            m_IsTimeout = false;
            m_CurrentTime = m_Time;

            m_BlueScore = m_IsSumKills ? m_Mode.BlueTeam.Score : 0;
            m_RedScore = m_IsSumKills ? m_Mode.RedTeam.Score : 0;

            m_Mode.ModeView.BlueTeamKillScoreView.SetScore(m_BlueScore);
            m_Mode.ModeView.RedTeamKillScoreView.SetScore(m_RedScore);

            m_Mode.ModeView.RoundTimeView.Show();
            m_Mode.ModeView.RoundTimeView.UpdateTime(m_Time);

            m_Mode.ModeView.BlueTeamKillScoreView.Show();
            m_Mode.ModeView.RedTeamKillScoreView.Show();
            
            m_Mode.ZoneManager.Show();

            foreach (PlayerVR player in m_Mode.RedTeam.Players)
            {
                player.WizardPlayer.OnDie += OnDieRedPlayer;
            }

            foreach (PlayerVR player in m_Mode.BlueTeam.Players)
            {
                player.WizardPlayer.OnDie += OnDieBluePlayer;
            }
        }

        private void OnDieBluePlayer()
        {
            m_RedScore++;
            m_Mode.RedTeam.Kills = m_RedScore;
            m_Mode.ModeView.RedTeamKillScoreView.SetScore(m_RedScore);
        }

        private void OnDieRedPlayer()
        {
            m_BlueScore++;
            m_Mode.BlueTeam.Kills = m_BlueScore;
            m_Mode.ModeView.BlueTeamKillScoreView.SetScore(m_BlueScore);
        }

        public override void Update()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                //return;
            }

            if (m_IsTimeout)
            {
                return;
            }

            m_CurrentTime -= Time.deltaTime;
            m_Mode.ModeView.RoundTimeView.UpdateTime(m_CurrentTime);

            if (m_CurrentTime <= 0)
            {
                m_CurrentTime = 0f;
                m_IsTimeout = true;

                if (m_RedScore > m_BlueScore)
                {
                    m_Mode.RedTeam.Score++;
                    m_Mode.ModeView.RedTeamScoreView.SetScore(m_Mode.RedTeam.Score);
                    m_Mode.CompleteRound();
                }

                if (m_RedScore < m_BlueScore)
                {
                    m_Mode.BlueTeam.Score++;
                    m_Mode.ModeView.BlueTeamScoreView.SetScore(m_Mode.BlueTeam.Score);
                    m_Mode.CompleteRound();
                }

                if (m_RedScore == m_BlueScore)
                {
                    m_Mode.RedTeam.Score++;
                    m_Mode.ModeView.RedTeamScoreView.SetScore(m_Mode.RedTeam.Score);

                    m_Mode.BlueTeam.Score++;
                    m_Mode.ModeView.BlueTeamScoreView.SetScore(m_Mode.BlueTeam.Score);
                    
                    m_Mode.CompleteRound();
                }
            }
        }

        public override void Exit()
        {
            //m_Mode.ModeView.BlueTeamKillScoreView.Hide();
            //m_Mode.ModeView.RedTeamKillScoreView.Hide();

            m_Mode.ModeView.RoundTimeView.Hide();
            m_Mode.ZoneManager.Hide();

            foreach (PlayerVR player in m_Mode.RedTeam.Players)
            {
                player.WizardPlayer.OnDie -= OnDieRedPlayer;
            }

            foreach (PlayerVR player in m_Mode.BlueTeam.Players)
            {
                player.WizardPlayer.OnDie -= OnDieBluePlayer;
            }
        }
    }
}