using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine.Tower
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/Tower Mode State/Complete Round State")]
    public class CompleteRoundState : TowerModeState
    {
        public override void Enter()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (m_Content.Lich.IsLife)
                {
                    m_Content.Lich.Deactivate();
                }
                
                foreach (MonsterPointSpawner pointSpawner in m_Content.Spawners)
                {
                    pointSpawner.ClearMonsters();
                }
                
                m_Mode.CompleteMode();
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