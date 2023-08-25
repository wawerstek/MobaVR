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
        private int m_MonsterCount = 0;
        private int m_CurrentMonsterCount = 0;

        private int m_PlayerCount = 0;
        private int m_CurrentPlayerCount = 0;

        private bool m_IsPlay = false;

        private void OnMonsterDie(Monster monster)
        {
            if (!m_IsPlay)
            {
                return;
            }

            m_CurrentMonsterCount++;
            if (m_CurrentMonsterCount >= m_MonsterCount)
            {
                m_IsPlay = false;
                m_Content.IsVictory = true;
                m_Mode.CompleteRound();
            }
        }

        private void OnTowerDie()
        {
            if (!m_IsPlay)
            {
                return;
            }

            m_IsPlay = false;
            m_Content.IsVictory = false;
            m_Mode.CompleteRound();
        }

        private void OnPlayerDie()
        {
            if (m_IsPlay)
            {
                return;
            }

            m_CurrentPlayerCount++;
            if (m_CurrentPlayerCount >= m_Mode.Players.Count)
            {
                m_Content.IsVictory = false;
                m_Mode.CompleteRound();
                m_IsPlay = false;
            }
        }

        public override void Enter()
        {
            m_MonsterCount = 0;
            m_CurrentMonsterCount = 0;

            if (PhotonNetwork.IsMasterClient)
            {
                UpdatePlayers();

                foreach (PlayerVR player in m_Mode.Players)
                {
                    player.WizardPlayer.OnDie += OnPlayerDie;
                }

                foreach (MonsterPointSpawner pointSpawner in m_Content.Spawners)
                {
                    pointSpawner.OnMonsterDie += OnMonsterDie;
                }

                m_Content.Tower.OnDie.AddListener(OnTowerDie);

                m_MonsterCount = 0;
                if (m_Content.CurrentWave < m_Content.Waves.Count)
                {
                    MonsterWave monsterWave = m_Content.Waves[m_Content.CurrentWave];
                    foreach (MonsterPointWave pointSpawner in monsterWave.Points)
                    {
                        if (pointSpawner.CanSpawn)
                        {
                            m_MonsterCount += pointSpawner.MaxTotalCountMonster;
                            pointSpawner.Init();
                            pointSpawner.PointSpawner.GenerateMonsters();
                        }
                    }
                }
                else
                {
                    m_Mode.CompleteRound();
                }
            }

            foreach (Trap trap in m_Content.Traps)
            {
                trap.enabled = true;
            }

            m_IsPlay = true;
        }


        public override void Update()
        {
        }

        public override void Exit()
        {
            m_IsPlay = false;
            
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            foreach (MonsterPointSpawner pointSpawner in m_Content.Spawners)
            {
                pointSpawner.OnMonsterDie -= OnMonsterDie;
            }

            foreach (PlayerVR player in m_Mode.Players)
            {
                player.WizardPlayer.OnDie -= OnPlayerDie;
            }

            m_Content.Tower.OnDie.RemoveListener(OnTowerDie);
        }
    }
}