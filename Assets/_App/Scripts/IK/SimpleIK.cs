using System;
using UnityEngine;

namespace MobaVR
{
    public class AnimalIK : MonoBehaviour
    {
        [SerializeField] private Transform m_Root;
        
        [Header("Head")]
        [SerializeField] private Transform m_IKHead;
        [SerializeField] private Transform m_TargetHead;
        
        [Header("Head")]
        [SerializeField] private Transform m_IKBody;
        [SerializeField] private Transform m_TargetBody;

        private void Update()
        {
            if (m_IKHead != null && m_TargetHead != null){}
        }
    }
}