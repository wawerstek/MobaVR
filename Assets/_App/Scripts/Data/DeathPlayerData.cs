using System;
using System.Collections.Generic;

namespace MobaVR
{
    [Serializable]
    public class DeathPlayerData
    {
        public PlayerVR DeadPlayer;
        public PlayerVR KillPlayer;
        public List<PlayerVR> AssistPlayers = new();
    }
}