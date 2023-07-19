using System;
using UnityEngine;

namespace MobaVR
{
    public abstract class BasePhysics : MonoBehaviour, IPhysics
    {
        public abstract void UpdatePhysics();
    }
}