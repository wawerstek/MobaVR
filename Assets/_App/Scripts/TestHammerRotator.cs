using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHammerRotator : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private Transform m_Hammer;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 targetDirection = m_Hammer.transform.position - m_Target.transform.position;
            Quaternion quaternion = Quaternion.LookRotation(targetDirection);
            m_Hammer.transform.rotation = quaternion;
            m_Hammer.transform.Rotate(Vector3.left, 90);
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector3 targetDirection = m_Hammer.transform.position - m_Target.transform.position;
            Quaternion quaternion = Quaternion.LookRotation(targetDirection);
            m_Hammer.transform.rotation = quaternion;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
        }
    }
}