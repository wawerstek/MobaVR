using System;
using UnityEngine;

namespace MobaVR
{
    public class NoGravity : BaseCustomGravity
    {
        private const string TAG = nameof(NoGravity);
        
        private float m_GravityDelay = 1.2f;
        
        public float GravityDelay
        {
            get => m_GravityDelay;
            set => m_GravityDelay = value;
        }

        public override void UpdatePhysics()
        {
            //TODO
        }

        protected override void OnThrown()
        {
            base.OnThrown();
            Invoke(nameof(ActivateGravity), m_GravityDelay);
        }
        
        private void ActivateGravity()
        {
            m_Rigidbody.useGravity = true;
        }
    }
}