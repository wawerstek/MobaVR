using System;
using UnityEngine;

namespace MobaVR
{
    public class MirrorMap : MonoBehaviour
    {
        [SerializeField] private Transform m_PlayerTarget;
        [SerializeField] private Transform m_Mirror;

        public float x1 = 1;
        public float y1 = 1;
        public float z1 = -1;
        public float x2 = -1;
        public float y2 = 1;
        public float z2 = 1;

        private void Update()
        {
            if (m_Mirror == null || m_PlayerTarget == null)
            {
                return;
            }

            Vector3 localPlayer = m_Mirror.InverseTransformPoint(m_PlayerTarget.position);
            //transform.position = m_Mirror.TransformPoint(new Vector3(localPlayer.x, localPlayer.y, -localPlayer.z));
            transform.position =
                m_Mirror.TransformPoint(new Vector3(x1 * localPlayer.x, y1 * localPlayer.y, z1 * localPlayer.z));

            Vector3 lookAtMirror =
                m_Mirror.TransformPoint(new Vector3(x2 * localPlayer.x, y2 * localPlayer.y, z2 * localPlayer.z));
            transform.LookAt(lookAtMirror);
        }
    }
}