using System;
using Photon.Realtime;

namespace MobaVR
{
    [Serializable]
    public class PlayerData
    {
        public int ActorNumber;
        public string IdUser;
        public string NickName;

        public Player Player;

        public PlayerData()
        {
        }
    }
}