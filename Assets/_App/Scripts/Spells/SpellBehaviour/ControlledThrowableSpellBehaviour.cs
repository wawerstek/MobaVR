using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class ControlledThrowableSpellBehaviour : ThrowableSpellBehaviour
    {
        [SerializeField] private InputActionReference m_RedirectInput;

        #region Unity

        protected override void OnEnable()
        {
            base.OnEnable();

            if (m_PhotonView.IsMine)
            {
                m_RedirectInput.action.started += OnStartRedirect;
                m_RedirectInput.action.performed += OnPerformedRedirect;
                m_RedirectInput.action.canceled += OnCanceledRedirect;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (m_PhotonView.IsMine)
            {
                m_RedirectInput.action.started -= OnStartRedirect;
                m_RedirectInput.action.performed -= OnPerformedRedirect;
                m_RedirectInput.action.canceled -= OnCanceledRedirect;
            }
        }

        #endregion

        #region Input Callbacks

        protected void OnStartRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnStartRedirect)}: started");
        }

        protected void OnPerformedRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnPerformedRedirect)}: performed");

            if (!CanCast() 
                || HasBlockingSpells() 
                //|| HasBlockingInputs() 
                || !m_IsThrown)
            {
                return;
            }

            // TODO: set direction
            // Check transform from point
            int kInvert = m_SpellHandType == SpellHandType.RIGHT_HAND ? -1 : 1;
            Vector3 direction = m_MainHandInputVR.Grabber.transform.right * kInvert;

            if (m_CurrentSpell != null)
            {
                m_CurrentSpell.Throwable.ThrowByDirection(direction);
            }
        }

        protected void OnCanceledRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnCanceledRedirect)}: canceled");
        }

        #endregion

        #region Input

        public override bool IsInProgress()
        {
            return m_CastInput.action.inProgress || m_RedirectInput.action.inProgress;
        }

        public override bool IsPressed()
        {
            return m_CastInput.action.IsPressed() || m_RedirectInput.action.IsPressed();
        }

        #endregion
    }
}