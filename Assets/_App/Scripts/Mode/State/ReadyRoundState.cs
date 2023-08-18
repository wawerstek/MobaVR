using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/Ready Round State")]
    public class ReadyRoundState : ModeState<ClassicMode>
    {
        [SerializeField] private float m_Time = 3f;
        private float m_CurrentTime;
        private bool m_IsWaiting = false;

        protected override void UpdatePlayer(PlayerVR player)
        {
            base.UpdatePlayer(player);
            //player.WizardPlayer.RestoreHp();
            
            player.WizardPlayer.Reborn();
        }

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();
                
                if (m_Mode.Environment != null)
                {
                    m_Mode.Environment.ResetEnvironment();
                }
            }

            m_Mode.ModeView.PreRoundTimeView.Show();
            m_Mode.ModeView.PreRoundTimeView.UpdateTime(m_Time);
            
            m_IsWaiting = true;
            m_CurrentTime = m_Time;
        }

        public override void Update()
        {
            if (m_IsWaiting)
            {
                m_CurrentTime -= Time.deltaTime;
                m_Mode.ModeView.PreRoundTimeView.UpdateTime(m_CurrentTime);
                
                if (m_CurrentTime <= 0)
                {
                    m_CurrentTime = 0f;
                    m_IsWaiting = false;
                    m_Mode.PlayRound();
                    m_StateMachine.PlayRound();
                }
            }
        }

        public override void Exit()
        {
            m_Mode.ModeView.PreRoundTimeView.Hide();
        }
    }
}