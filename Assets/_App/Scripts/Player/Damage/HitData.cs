using Photon.Pun;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace MobaVR
{
    public struct HitData
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

        #endregion
        
        #region Owner Source

        public bool IsMine;
        public int PhotonId;
        public string PlayerId;
        public Transform Source;
        public Transform Owner;
        public PhotonView PhotonView;
        //public PlayerRef InstigatorRef;

        #endregion
    }
}