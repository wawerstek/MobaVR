using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine.Tower
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Tower Mode State/Init Mode State")]
    public class InitModeState : TowerModeState
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
                
                foreach (MonsterPointSpawner pointSpawner in m_Content.Spawners)
                {
                    pointSpawner.ClearMonsters();
                }
            }

            foreach (Trap trap in m_Content.Traps)
            {
                trap.enabled = false;
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