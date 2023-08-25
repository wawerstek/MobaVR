using System.Collections.Generic;
using UnityEngine;

namespace MobaVR.Content
{
    public class TowerModeContent : MonoBehaviour
    {
        [SerializeField] private TowerModeView m_ModeView;

        [Header("Tower")]
        [SerializeField] private List<Trap> m_Traps = new List<Trap>();
        [SerializeField] private Lich m_Lich;
        [SerializeField] private Tower m_Tower;

        [Header("Waves")]
        [SerializeField] private List<MonsterPointSpawner> m_Spawners = new List<MonsterPointSpawner>();
        [SerializeField] private List<MonsterWave> m_Waves = new();

        private bool m_IsVictory = true;
        private int m_CurrentWave = 0;

        public TowerModeView ModeView => m_ModeView;
        public List<MonsterPointSpawner> Spawners => m_Spawners;
        public List<Trap> Traps => m_Traps;
        public Lich Lich => m_Lich;
        public Tower Tower => m_Tower;
        public List<MonsterWave> Waves => m_Waves;
        public bool IsVictory
        {
            get => m_IsVictory;
            set => m_IsVictory = value;
        }
        public int CurrentWave
        {
            get => m_CurrentWave;
            set => m_CurrentWave = value;
        }
        public bool HasCurrentWave => m_CurrentWave < m_Waves.Count;
        public bool HasNextWave => m_CurrentWave < m_Waves.Count;
    }
}