using UnityEngine;

namespace MobaVR
{

    public class CenterOfMass : MonoBehaviour
    {
        [SerializeField] private Transform m_CenterPoint;
        
        private Rigidbody m_Rigidbody;

        private void Awake()
        {
            if (TryGetComponent(out m_Rigidbody))
            {
                m_Rigidbody.centerOfMass = m_CenterPoint.localPosition;
            }
        }

        void Start()
        {
    
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}