using System.Collections.Generic;
using Photon.Realtime;

namespace MobaVR
{
    public interface ITeam
    {
        public List<PlayerVR> Players { get; }
        
        public void AddPlayer(PlayerVR player);
        public void RemovePlayer(PlayerVR player);
    }
}