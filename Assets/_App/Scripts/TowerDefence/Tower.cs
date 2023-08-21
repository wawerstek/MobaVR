using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class Tower : MonoBehaviourPun
    {
        [SerializeField] private float m_MaxHp = 1000;
        [SerializeField] private float m_CurrentHp;

        public bool IsLife => m_CurrentHp > 0f;
    }
}