using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    enum LichGameState
    {
        NONE,
        ACTIVE,
        STOP,
        GAME_OVER
    }
    
    public class LichGame : MonoBehaviourPunCallbacks
    {
        [Header("Classic Mode")]
        [SerializeField] private ClassicMode m_GameSession;
        [SerializeField] private PlayerStateSO m_PlayState;
        [SerializeField] private PlayerStateSO m_ReadyState;
        
        [Header("Lich")]
        [SerializeField] private List<MonsterPointSpawner> m_Spawners = new List<MonsterPointSpawner>();
        [SerializeField] private Lich m_Lich;
        [SerializeField] private LichGameState m_State = LichGameState.NONE;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartGame();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                StopGame();
            }
        }

        private IEnumerator CheckPlayers()
        {
            yield return new WaitForSeconds(2f);
            
            WizardPlayer[] players = FindObjectsOfType<WizardPlayer>();
            WizardPlayer[] lifePlayers = players.Where(player => player.IsLife).ToArray();
            if (lifePlayers.Length == 0)
            {
                SetGameOver();
            }
            
            StartCoroutine(CheckPlayers());
        }

        [ContextMenu("RestartGame")]
        public void Restart()
        {
            
        }

        [ContextMenu("StartGame")]
        public void StartGame()
        {
            StartCoroutine(CheckPlayers());
            
            m_State = LichGameState.ACTIVE;
            
            //if (PhotonNetwork.IsMasterClient)
            {
                foreach (MonsterPointSpawner pointSpawner in m_Spawners)
                {
                    pointSpawner.GenerateMonsters();
                }
            }
            
            if (PhotonNetwork.IsMasterClient && m_GameSession != null)
            {
                foreach (PlayerVR player in m_GameSession.Players)
                {
                    player.SetState(m_PlayState);
                    player.WizardPlayer.Reborn();
                }
            }

            if (!m_Lich.IsLife)
            {
                m_Lich.Init();
            }
            else
            {
                m_Lich.Activate();
            }
        }
        
        [ContextMenu("StopGame")]
        public void StopGame()
        {
            m_State = LichGameState.STOP;

            if (m_Lich.IsLife)
            {
                m_Lich.Deactivate();
            }

            foreach (MonsterPointSpawner pointSpawner in m_Spawners)
            {
                pointSpawner.ClearMonsters();
            }
            
            if (PhotonNetwork.IsMasterClient && m_GameSession != null)
            {
                foreach (PlayerVR player in m_GameSession.Players)
                {
                    player.SetState(m_ReadyState);
                    player.WizardPlayer.Reborn();
                }
            }
        }

        [ContextMenu("SetGameOver")]
        public void SetGameOver()
        {
            m_State = LichGameState.GAME_OVER;
            
            if (m_Lich.IsLife)
            {
                m_Lich.Deactivate();
            }
            
            foreach (MonsterPointSpawner pointSpawner in m_Spawners)
            {
                pointSpawner.ClearMonsters();
            }
            
            if (PhotonNetwork.IsMasterClient && m_GameSession != null)
            {
                foreach (PlayerVR player in m_GameSession.Players)
                {
                    player.SetState(m_ReadyState);
                    player.WizardPlayer.Reborn();
                }
            }
        }
    }
}