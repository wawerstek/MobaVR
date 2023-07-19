using System;
using UnityEngine;

namespace MobaVR
{
    public class RealGravity : BaseCustomGravity
    {
        private const string TAG = nameof(RealGravity);
        
        public override void UpdatePhysics()
        {
            //TODO
        }

        protected override void OnThrown()
        {
            base.OnThrown();
            m_Rigidbody.useGravity = true;
        }
    }
}