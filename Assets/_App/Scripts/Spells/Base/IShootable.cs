using UnityEngine;

namespace MobaVR.Base
{
    public interface IShootable
    {
        public void Shoot();
        public void ShootByDirection(Vector3 direction);
    }
}