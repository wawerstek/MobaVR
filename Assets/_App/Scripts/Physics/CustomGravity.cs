using System;
using UnityEngine;

namespace MobaVR
{
    public class CustomGravity : BaseCustomGravity
    {
        private const string TAG = nameof(CustomGravity);

        [SerializeField] private CustomGravitySO m_GravitySO;

        private float m_VerticalVelocity = 0f;
        private float m_CurrentTime = 0f;

        public CustomGravitySO GravitySo
        {
            get => m_GravitySO;
            set => m_GravitySO = value;
        }

        protected override void Awake()
        {
            base.Awake();
            if (GravitySo == null)
            {
                GravitySo = Resources.Load<CustomGravitySO>("Api/Physics/CustomGravity");
            }
        }

        public override void UpdatePhysics()
        {
            if (GravitySo == null)
            {
                return;
            }

            if (m_VerticalVelocity > GravitySo.TerminalVelocity)
            {
                m_VerticalVelocity += GravitySo.Gravity * Time.deltaTime;
            }

            Vector3 gravity = new Vector3(0.0f, m_VerticalVelocity, 0.0f) * Time.deltaTime;
            m_Rigidbody.velocity += gravity;
        }

        protected override void OnThrown()
        {
            base.OnThrown();
            m_Rigidbody.useGravity = false;
        }
    }
}