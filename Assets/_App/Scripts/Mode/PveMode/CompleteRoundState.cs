using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.ClassicModeStateMachine.PVE
{
    [Serializable]
    [CreateAssetMenu(menuName = "API/PVE Mode State/Complete Round State")]
    public class CompleteRoundState : PveModeState
    {
        public override void Enter()
        {
            m_Content.Lich.RpcPause_Monster();
            
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