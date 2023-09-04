using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class KillZoneManager : MonoBehaviourPun
    {
        [SerializeField] private List<GameObject> m_Zones;

        public void Show()
        {
            foreach (GameObject zone in m_Zones)
            {
                zone.SetActive(true);
            }
        }

        public void Hide()
        {
            foreach (GameObject zone in m_Zones)
            {
                zone.SetActive(false);
            }
        }
    }
}