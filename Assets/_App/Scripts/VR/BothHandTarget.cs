using UnityEngine;

namespace MobaVR
{
    public class BothHandTarget : MonoBehaviour
    {
        [SerializeField] private InputVR m_InputVR;
        [SerializeField] private Transform m_LeftTarget;
        [SerializeField] private Transform m_RightTarget;

        private void Update()
        {
            Vector3 position = (m_LeftTarget.position + m_RightTarget.position) / 2f;
            Quaternion rotation = Quaternion.Lerp(m_LeftTarget.transform.rotation, m_RightTarget.transform.rotation, 0.5f);

            transform.position = position;
            transform.rotation = rotation;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(m_InputVR.LefHandInputVR.InsideHandPoint.transform.position, m_InputVR.LefHandInputVR.InsideHandPoint.transform.forward * 10f);
            Gizmos.DrawLine(m_InputVR.RightHandInputVR.InsideHandPoint.transform.position, m_InputVR.RightHandInputVR.InsideHandPoint.transform.forward * 10f);
            Gizmos.DrawLine(transform.position, transform.forward * 10f);
        }
    }
}