using BNG;
using UnityEngine;

namespace MobaVR
{
    public class InputActionVR : MonoBehaviour
    {
        private InputBridge m_InputBridge;

        private void Awake()
        {
            m_InputBridge = FindObjectOfType<InputBridge>();
        }
    }
}