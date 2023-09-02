using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class ManualTraps : MonoBehaviour
    {
        [SerializeField] private List<Trap> m_Traps = new();
        [SerializeField] private float m_Delay = 2f;

        private bool m_IsActivated;
        
        public bool IsActivated => m_IsActivated;

        private void Reset()
        {
            CancelInvoke(nameof(Reset));
            CancelInvoke(nameof(Deactivate));
        }

        public void Activate()
        {
            if (m_IsActivated)
            {
                return;
            }
            
            m_IsActivated = true;
            
            foreach (Trap trap in m_Traps)
            {
                trap.Activate();
            }

            Invoke(nameof(Deactivate), m_Delay);
        }

        public void Deactivate()
        {
            if (!m_IsActivated)
            {
                return;
            }
            
            foreach (Trap trap in m_Traps)
            {
                trap.Deactivate();
            }
            
            m_IsActivated = false;
        }
    }
}