using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BodyHelper : MonoBehaviourPun
{
    private Transform m_Target;

    private void Start()
    {
        if (photonView.IsMine)
        {
            m_Target = GameObject.Find("CenterEyeAnchor").transform;
        }
    }

    private void Update()
    {
        if (photonView.IsMine && m_Target != null)
        {
            transform.position = m_Target.transform.position;
        }
    }
}