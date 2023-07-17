using System;
using UnityEngine;

namespace MobaVR
{
    public class GravityFireball : MonoBehaviour
    {
        private const string TAG = nameof(GravityFireball);

        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private ThrowableSpell m_Fireball;

        [SerializeField] private bool m_UseCustomCurve = true;
        [SerializeField] private AnimationCurve m_AnimationCurve;

        [SerializeField] private float m_Gravity;
        [SerializeField] private float m_TerminalVelocity = 40.0f;
        private float m_VerticalVelocity = 0f;
        private bool m_IsThrown = false;
        private float m_CurrentTime = 0f;

        private void OnEnable()
        {
            if (m_Fireball != null)
            {
                m_Fireball.OnThrown += OnThrown;
                m_Fireball.OnDestroySpell += OnDestroySpell;
            }
        }

        private void OnDestroySpell()
        {
            m_IsThrown = false;
        }

        private void OnDisable()
        {
            m_Fireball.OnThrown -= OnThrown;
        }

        private void OnThrown()
        {
            m_IsThrown = true;
        }

        private void Update()
        {
            if (m_IsThrown)
            {
                Vector3 gravity;

                if (m_UseCustomCurve)
                {
                    m_VerticalVelocity = m_AnimationCurve.Evaluate(m_CurrentTime);
                    gravity = new Vector3(0.0f, m_VerticalVelocity, 0.0f) * Time.deltaTime;
                    
                    m_CurrentTime += Time.deltaTime;
                }
                else
                {
                    if (m_VerticalVelocity > m_TerminalVelocity)
                    {
                        m_VerticalVelocity += m_Gravity * Time.deltaTime;
                    }

                    gravity = new Vector3(0.0f, m_VerticalVelocity, 0.0f) * Time.deltaTime;
                }

                m_Rigidbody.velocity += gravity;

                Debug.Log($"{TAG}: velocity: {m_Rigidbody.velocity}, y = {m_VerticalVelocity}");
            }
        }
    }
}