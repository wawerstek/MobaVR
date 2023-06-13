using UnityEngine;

namespace MobaVR
{
    public abstract class BasePlayerSpawner<T> : MonoBehaviour
    {
        public abstract T SpawnPlayer(Team team);
    }
}