using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine.PVE
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/PVE Mode State/Init Mode State")]
    public class InitModeState : PveModeState
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
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
        }
    }
}