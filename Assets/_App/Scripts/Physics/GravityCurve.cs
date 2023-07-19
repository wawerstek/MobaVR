using System;
using UnityEngine;

namespace MobaVR
{
    public class GravityCurve : BaseCustomGravity
    {
        [SerializeField] private GravityCurveSO m_GravityCurveSO;

        private float m_VerticalVelocity = 0f;
        private float m_CurrentTime = 0f;
        
        public GravityCurveSO GravityCurveSo
        {
            get => m_GravityCurveSO;
            set => m_GravityCurveSO = value;
        }

        protected override void Awake()
        {
            base.Awake();
            if (GravityCurveSo == null)
            {
                GravityCurveSo = Resources.Load<GravityCurveSO>("Api/Physics/GravityCurve");
            }
        }

        public override void UpdatePhysics()
        {
            if (GravityCurveSo == null)
            {
                return;
            }
            
            m_VerticalVelocity = GravityCurveSo.AnimationCurve.Evaluate(m_CurrentTime);

            Vector3 gravity = new Vector3(0.0f, m_VerticalVelocity, 0.0f) * Time.deltaTime;
            m_Rigidbody.velocity += gravity;
            m_CurrentTime += Time.deltaTime;
        }

        protected override void OnThrown()
        {
            base.OnThrown();
            m_Rigidbody.useGravity = false;
        }
    }
}