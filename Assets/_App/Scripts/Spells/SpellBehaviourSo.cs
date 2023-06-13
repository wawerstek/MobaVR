using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public abstract class SpellBehaviourSO : ScriptableObject
    {
        [SerializeField] protected InputActionReference m_Input;
        [SerializeField] protected SpellHandType m_HandType;

        protected WizardPlayer m_WizardPlayer;

        public void Init(WizardPlayer wizardPlayer)
        {
            m_WizardPlayer = wizardPlayer;
        }

        private void OnEnable()
        {
            m_Input.action.started += OnStartAction;
            m_Input.action.performed += OnPerformedAction;
            m_Input.action.canceled += OnCanceledAction;
        }

        private void OnDisable()
        {
            m_Input.action.started -= OnStartAction;
            m_Input.action.performed -= OnPerformedAction;
            m_Input.action.canceled -= OnCanceledAction;
        }

        protected abstract void OnStartAction(InputAction.CallbackContext context);
        protected abstract void OnPerformedAction(InputAction.CallbackContext context);
        protected abstract void OnCanceledAction(InputAction.CallbackContext context);
    }
}