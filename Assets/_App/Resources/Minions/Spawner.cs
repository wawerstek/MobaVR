using System.Collections;
using UnityEngine;

namespace MobaVR
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Transform m_DestinationPoint;
        [SerializeField] private Transform m_Parent;
        [SerializeField] private BlueMinion m_Minion;

        [SerializeField] private float m_DeltaPosition = 2f;
        [SerializeField] private float m_Delay = 5f;
        [SerializeField] private int m_Counter = 4;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(m_Delay);

            for (int i = 0; i < m_Counter; i++)
            {
                //BlueMinion minion = Instantiate(m_Minion, Vector3.zero, Quaternion.identity, transform);
                BlueMinion minion = Instantiate(m_Minion, Vector3.zero, Quaternion.identity, m_Parent);
                minion.transform.localPosition = Vector3.zero;

                //BlueMinion minion = Instantiate(m_Minion, m_Parent);
                //Vector3 position = transform.localPosition;
                //position.x += (m_DeltaPosition * i) - m_DeltaPosition * m_Counter ;
                //minion.transform.localPosition = position;
                minion.transform.LookAt(m_DestinationPoint);
                minion.SetDestination(m_DestinationPoint);
            }

            StartCoroutine(Spawn());
        }
    }
}
