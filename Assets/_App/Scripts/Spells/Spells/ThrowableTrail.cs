﻿using Unity.VisualScripting;
using UnityEngine;

namespace MobaVR
{
    public class ThrowableTrail : MonoBehaviour
    {
        [SerializeField] private ThrowableSpell m_Spell;
        [SerializeField] private AnimationCurve m_StartWidth;
        [SerializeField] private AnimationCurve m_DestroyWidth;
        [SerializeField] private float m_StartTime;
        [SerializeField] private float m_DestroyTime;
        [SerializeField] private TrailRenderer m_TrailRenderer;

        private void OnEnable()
        {
            if (m_TrailRenderer != null)
            {
                m_TrailRenderer.time = m_StartTime;
                m_TrailRenderer.widthCurve = m_StartWidth;
            }

            if (m_Spell != null)
            {
                m_Spell.OnThrown += Throw;
                m_Spell.OnDestroySpell += DestroyTrail;
            }
        }

        private void Throw()
        {
            m_TrailRenderer.time = m_DestroyTime;
            m_TrailRenderer.widthCurve = m_DestroyWidth;
        }

        private void DestroyTrail()
        {
            m_TrailRenderer.transform.parent = null;
            Destroy(m_TrailRenderer.gameObject, 2f);
        }

        private void OnDisable()
        {
            m_Spell.OnDestroySpell -= DestroyTrail;
        }
    }
}