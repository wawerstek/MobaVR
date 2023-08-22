using System.Collections.Generic;
using UnityEngine;

namespace MobaVR.Content
{
    public class TowerModeContent : MonoBehaviour
    {
        [SerializeField] private List<MonsterPointSpawner> m_Spawners = new List<MonsterPointSpawner>();
        [SerializeField] private List<Trap> m_Traps = new List<Trap>();
        [SerializeField] private Lich m_Lich;
        [SerializeField] private Tower m_Tower;

        public List<MonsterPointSpawner> Spawners => m_Spawners;
        public List<Trap> Traps => m_Traps;
        public Lich Lich => m_Lich;
        public Tower Tower => m_Tower;
    }
}