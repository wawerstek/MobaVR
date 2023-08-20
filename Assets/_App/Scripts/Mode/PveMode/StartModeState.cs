using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine.PVE
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/PVE Mode State/Start Mode State")]
    public class StartModeState : PveModeState
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
                
                if (!m_Content.Lich.IsLife)
                {
                    m_Content.Lich.Init();
                }
                else
                {
                    m_Content.Lich.Activate();
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