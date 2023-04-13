using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdallTester : MonoBehaviour
{
    public Rigidbody m_Rigidbody;
    public float m_Force = 5000f;
    public float m_Radius = 10f;
    public float m_Modifier = 10f;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            m_Rigidbody.AddExplosionForce(m_Force, m_Rigidbody.transform.position, m_Radius, m_Force);
        }
    }
    
}
