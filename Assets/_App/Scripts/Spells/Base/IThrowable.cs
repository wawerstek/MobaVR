using UnityEngine;

namespace MobaVR.Base
{
    public interface IThrowable
    {
        public void Throw();
        public void ThrowByDirection(Vector3 direction);
    }
}