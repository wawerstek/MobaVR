using System;
using UnityEngine;

namespace MobaVR
{
    public class LocationSettingsView : MonoBehaviour
    {
        [SerializeField] private GameObject m_LocationView;

        private int m_MaxTryCount = 3;
        private int m_CurrentTry = 0;

        private void Start()
        {
            m_CurrentTry = 0;
            TryMoveToMainSettings();
        }

        private void TryMoveToMainSettings()
        {
            MenuHolder menuHolder = FindObjectOfType<MenuHolder>();
            if (menuHolder != null)
            {
                menuHolder.SetLocationPanel(m_LocationView);
            }
            else
            {
                m_CurrentTry++;
                if (m_CurrentTry < m_MaxTryCount)
                {
                    Invoke(nameof(TryMoveToMainSettings), 2f);
                }
            }
        }
    }
}