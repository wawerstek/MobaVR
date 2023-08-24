using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine.PVE
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/PVE Mode State/Play Round State")]
    public class PlayRoundState : PveModeState
    {
        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();
                
                foreach (MonsterPointSpawner pointSpawner in m_Content.Spawners)
                {
                    pointSpawner.GenerateMonsters();
                }
            }
        }

        public override void Update()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (!m_Content.Lich.IsLife)
            {
                m_Mode.CompleteRound();
                return;
            }

            if (m_Mode.Players.Count(player => player.WizardPlayer.IsLife) <= 0)
            {
                m_Mode.CompleteRound();
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}