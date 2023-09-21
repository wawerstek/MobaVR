using System;
using UnityEngine;

namespace MobaVR
{
    [Serializable]
    public class MonsterPointWave
    {
        public MonsterPointSpawner PointSpawner;
        public int MaxTotalCountMonster = -1;
        public int MaxCountMonster = -1;
        public float DelayBetweenMonster = 10f;
        public float DelayBetweenMaxMonster = 5f;
        public float StartDelay = 0f;
        public bool CanSpawn = true;

        public void Init()
        {
            PointSpawner.MaxTotalCountMonster = MaxTotalCountMonster;
            PointSpawner.MaxCountMonster = MaxCountMonster;
            PointSpawner.DelayBetweenMonster = DelayBetweenMonster;
            PointSpawner.DelayBetweenMaxMonster = DelayBetweenMaxMonster;
            PointSpawner.CanSpawn = CanSpawn;
            PointSpawner.StartDelay = StartDelay;
        }
    }
}