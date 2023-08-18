using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Classic Mode State/Complete Round State")]
    public class CompleteRoundState : PvpClassicModeState
    {
        [SerializeField] private float m_WinScore = 5;

        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();
                
                if (m_Mode.BlueTeam.Score >= m_WinScore)
                {
                    m_Mode.CompleteMode();
                    return;
                }

                if (m_Mode.RedTeam.Score >= m_WinScore)
                {
                    m_Mode.CompleteMode();
                    return;
                }

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