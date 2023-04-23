using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    [Serializable]
    [CreateAssetMenu(fileName = "InputAction", menuName = "MobaVR API/Create input action")]
    public class InputActionSO : ScriptableObject
    {
        [SerializeField] private InputActionReference m_HealthInput;
        [SerializeField] private InputActionReference m_SwitchModeLeftHandInput;
        [SerializeField] private InputActionReference m_SwitchModeRightHandInput;
        
        [SerializeField] private InputActionReference m_RightGrabInput;
        [SerializeField] private InputActionReference m_RightActivateInput;

        [SerializeField] private InputActionReference m_LeftGrabInput;
        [SerializeField] private InputActionReference m_LeftActivateInput;

        public InputActionReference HealthInput => m_HealthInput;
        public InputActionReference SwitchModeLeftHandInput => m_SwitchModeLeftHandInput;
        public InputActionReference SwitchModeRightHandInput => m_SwitchModeRightHandInput;
        public InputActionReference RightGrabInput => m_RightGrabInput;
        public InputActionReference RightActivateInput => m_RightActivateInput;
        public InputActionReference LeftGrabInput => m_LeftGrabInput;
        public InputActionReference LeftActivateInput => m_LeftActivateInput;
    }
}