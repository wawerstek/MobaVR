using System;
using UnityEngine;

namespace MobaVR
{
    public class SimpleIK : MonoBehaviour
    {
        [Header("Body")]
        [SerializeField] private Transform m_HeadTarget;
        [SerializeField] private Transform m_HeadIK;

        [Header("Body")]
        [SerializeField] private Transform m_BodyIK;
        [SerializeField] private Transform m_BodyTarget;

        private void Update()
        {
            if (m_BodyTarget != null && m_BodyIK != null)
            {
                m_BodyIK.position = m_BodyTarget.transform.position;
                m_BodyIK.rotation = m_BodyTarget.transform.rotation;
            }

            if (m_HeadTarget != null && m_HeadIK != null)
            {
                m_HeadIK.position = m_HeadTarget.transform.position;
                m_HeadIK.rotation = m_HeadTarget.transform.rotation;
            }
        }
    }
}