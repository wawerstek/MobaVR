using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public abstract class InputSpellBehaviour : SpellBehaviour
    {
        [SerializeField] protected SpellHandType m_SpellHandType = SpellHandType.RIGHT_HAND;
        [SerializeField] protected InputActionReference m_CastInput;

        protected HandInputVR m_MainHandInputVR;

        #region Unity

        protected virtual void OnEnable()
        {
            m_CastInput.action.started += OnStartCast;
            m_CastInput.action.performed += OnPerformedCast;
            m_CastInput.action.canceled += OnCanceledCast;
        }

        protected virtual void OnDisable()
        {
            m_CastInput.action.started -= OnStartCast;
            m_CastInput.action.performed -= OnPerformedCast;
            m_CastInput.action.canceled -= OnCanceledCast;
        }

        #endregion


        #region Input Callbacks

        protected virtual void OnStartCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnStartCast)}: started");
        }

        protected virtual void OnPerformedCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnPerformedCast)}: performed");
        }

        protected virtual void OnCanceledCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnCanceledCast)}: canceled");
        }

        #endregion

        #region Input

        public override bool IsInProgress()
        {
            return m_CastInput.action.inProgress;
        }

        public override bool IsPressed()
        {
            return m_CastInput.action.IsPressed();
        }

        public virtual void CheckInput()
        {
        }

        #endregion

        public override void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            base.Init(spellHandler, playerVR);
            switch (m_SpellHandType)
            {
                case SpellHandType.LEFT_HAND:
                    m_MainHandInputVR = playerVR.InputVR.LefHandInputVR;
                    break;
                case SpellHandType.RIGHT_HAND:
                    m_MainHandInputVR = playerVR.InputVR.RightHandInputVR;
                    break;
                case SpellHandType.BOTH:
                    break;
            }
        }
    }
}