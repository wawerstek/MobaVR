using System.Collections.Generic;
using UnityEngine;

namespace MobaVR.Content
{
    public class PveModeContent : MonoBehaviour
    {
        [SerializeField] private List<MonsterPointSpawner> m_Spawners = new List<MonsterPointSpawner>();
        [SerializeField] private Lich m_Lich;

        public List<MonsterPointSpawner> Spawners => m_Spawners;
        public Lich Lich => m_Lich;
    }
}