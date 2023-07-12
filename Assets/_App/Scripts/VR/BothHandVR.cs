using UnityEngine;

namespace MobaVR
{
    public class BothHandVR : MonoBehaviour
    {
        [SerializeField] private InputVR m_InputVR;

        private void Update()
        {
            Vector3 position = (m_InputVR.LefHandInputVR.InsideHandPoint.transform.position +
                                m_InputVR.RightHandInputVR.InsideHandPoint.transform.position) / 2f;

            Quaternion rotation = Quaternion.Lerp(m_InputVR.LefHandInputVR.InsideHandPoint.transform.rotation,
                                                  m_InputVR.RightHandInputVR.InsideHandPoint.transform.rotation, 0.5f);

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