using Unity.VisualScripting;
using UnityEngine;

namespace MobaVR
{
    public class FireballTrail : MonoBehaviour
    {
        [SerializeField] private Fireball m_Fireball;
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

            if (m_Fireball != null)
            {
                m_Fireball.OnThrown += Throw;
                m_Fireball.OnDestroySpell += DestroyTrail;
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
            m_Fireball.OnDestroySpell -= DestroyTrail;
        }
    }
}