using System;
using UnityEngine;

namespace MobaVR
{
    public abstract class BaseCustomGravity : BasePhysics
    {
        protected Rigidbody m_Rigidbody;
        protected Throwable m_Throwable;

        protected bool m_IsThrown = false;

        protected void OnEnable()
        {
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.AddListener(OnThrown);
            }
        }

        protected void OnDisable()
        {
            if (m_Throwable != null)
            {
                m_Throwable.OnThrown.RemoveListener(OnThrown);
            }
        }

        protected virtual void Awake()
        {
            TryGetComponent(out m_Rigidbody);
            TryGetComponent(out m_Throwable);
        }

        protected void Update()
        {
            if (!m_IsThrown)
            {
                return;
            }

            UpdatePhysics();
        }

        protected virtual void OnThrown()
        {
            m_IsThrown = true;
        }
    }
}