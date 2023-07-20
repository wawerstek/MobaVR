using System;
using UnityEngine;

namespace MobaVR
{
    public class ThrowFaceRotator : MonoBehaviour
    {
        private const string TAG = nameof(ThrowFaceRotator);
        
        [SerializeField] private Throwable m_Throwable;
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Vector3 m_RotateAxis = new Vector3(1, 0, 0);
        [SerializeField] private float m_MaxSpeed = 45f;
        [SerializeField] private float m_StepSpeed = 1f;
        [SerializeField] private float m_MagnitudeK = 2f;


        private float m_CurrentSpeed = 0f;
        private float m_CurrentStep = 0f;
        private bool m_IsThrown = false;

        private void OnValidate()
        {
            if (m_Throwable == null)
            {
                TryGetComponent(out m_Throwable);
            }
            
            if (m_Rigidbody == null)
            {
                TryGetComponent(out m_Rigidbody);
            }
        }

        private void OnEnable()
        {
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.AddListener(OnThrown);
            }

        }

        private void OnDisable()
        {
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.RemoveListener(OnThrown);
            }
        }

        private void Update()
        {
            if (m_IsThrown)
            {
                /*
                m_CurrentSpeed += m_StepSpeed * Time.deltaTime;
                m_CurrentSpeed = Mathf.Clamp(m_CurrentSpeed, 0, m_MaxSpeed);
                //transform.Rotate(m_RotateAxis * m_MaxSpeed);
                transform.Rotate(m_RotateAxis * m_CurrentSpeed);
                */
            }
        }

        private void OnThrown()
        {
            m_IsThrown = true;

            Vector3 velocity = m_Rigidbody.velocity;
            float magnitude = velocity.magnitude;
            
            Debug.Log($"{TAG}: velocity = {velocity}; magnitude = {magnitude}");

            Debug.DrawRay(transform.position, transform.position + m_Rigidbody.velocity * 100f, Color.blue, 100f);
            
            Vector3 targetDirection = transform.position - m_Rigidbody.velocity;
            Quaternion quaternion = Quaternion.LookRotation(targetDirection);
            Debug.DrawRay(transform.position, targetDirection, Color.red, 100f);

            transform.rotation = quaternion;
        }
    }
}