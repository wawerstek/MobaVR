using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace MobaVR
{
    [Serializable]
    public class HitDataFull
    {
        #region Hit

        public HitActionType Action;
        public float Amount;
        public bool IsCritical;
        public bool IsFatal;
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Direction;

        #endregion

        #region Team

        public TeamType TeamType;
        //public Team Team;

        #endregion

        #region Owner Source

        public Player Player;
        public bool IsMine;
        public int ActorNumber;
        public string PlayerId;
        public PhotonView PhotonView;
        public Transform Source;
        public Transform Owner;
        //public PlayerRef InstigatorRef;

        #endregion


    }
}