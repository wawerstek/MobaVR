using System;
using UnityEngine;

namespace MobaVR
{
    public class ThrowRotator : MonoBehaviour
    {
        [SerializeField] private Throwable m_Throwable;
        [SerializeField] private ThrowableSpell m_ThrowableSpell;
        [SerializeField] private Vector3 m_RotateAxis = new Vector3(1, 0, 0);
        [SerializeField] private float m_Speed = 45f;

        private bool m_IsThrown = false;

        private void OnValidate()
        {
            if (m_Throwable == null)
            {
                TryGetComponent(out m_Throwable);
            }
            
            if (m_ThrowableSpell == null)
            {
                TryGetComponent(out m_ThrowableSpell);
            }
        }

        private void OnEnable()
        {
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.AddListener(OnThrown);
            }

            if (m_ThrowableSpell != null)
            {
                m_ThrowableSpell.OnThrown += OnThrown;
            }
        }

        private void OnDisable()
        {
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.RemoveListener(OnThrown);
            }

            if (m_ThrowableSpell != null)
            {
                m_ThrowableSpell.OnThrown -= OnThrown;
            }
        }

        private void Update()
        {
            if (m_IsThrown)
            {
                transform.Rotate(m_RotateAxis * m_Speed);
            }
        }

        private void OnThrown()
        {
            m_IsThrown = true;
        }
    }
}