using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine.Tower
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Tower Mode State/Play Round State")]
    public class PlayRoundState : TowerModeState
    {
        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();
            }
            
            foreach (MonsterPointSpawner pointSpawner in m_Content.Spawners)
            {
                pointSpawner.GenerateMonsters();
            }
            
            foreach (Trap trap in m_Content.Traps)
            {
                trap.enabled = true;
            }
        }

        public override void Update()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            /*
            if (!m_Content.Lich.IsLife)
            {
                m_Mode.CompleteRound();
                return;
            }
            */
            
            if (!m_Content.Tower.IsLife)
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