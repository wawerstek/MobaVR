using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public abstract class InputSpellBehaviour : SpellBehaviour, ISpellInput
    {
        [SerializeField] protected SpellHandType m_SpellHandType = SpellHandType.RIGHT_HAND;
        [SerializeField] protected InputActionReference m_CastInput;

        protected HandInputVR m_LeftHand;
        protected HandInputVR m_RightHand;
        protected HandInputVR m_MainHandInputVR;

        #region MonoBehaviour

        protected virtual void OnEnable()
        {
            if (m_PhotonView.IsMine)
            //if (m_PlayerVR != null && m_PlayerVR.IsMine)
            {
                m_CastInput.action.started += OnStartCast;
                m_CastInput.action.performed += OnPerformedCast;
                m_CastInput.action.canceled += OnCanceledCast;
            }
        }

        protected virtual void OnDisable()
        {
            if (m_PhotonView.IsMine)
            //if (m_PlayerVR != null && m_PlayerVR.IsMine)
            {
                m_CastInput.action.started -= OnStartCast;
                m_CastInput.action.performed -= OnPerformedCast;
                m_CastInput.action.canceled -= OnCanceledCast;
            }
        }

        #endregion

        #region Spell

        public override void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            base.Init(spellHandler, playerVR);
            m_LeftHand = playerVR.InputVR.LefHandInputVR;
            m_RightHand = playerVR.InputVR.RightHandInputVR;

            switch (m_SpellHandType)
            {
                case SpellHandType.LEFT_HAND:
                    m_MainHandInputVR = m_PlayerVR.InputVR.LefHandInputVR;
                    break;
                case SpellHandType.RIGHT_HAND:
                    m_MainHandInputVR = m_PlayerVR.InputVR.RightHandInputVR;
                    break;
                case SpellHandType.BOTH:
                    m_MainHandInputVR = m_PlayerVR.InputVR.RightHandInputVR;
                    break;
            }
        }

        #endregion

        #region Input Callbacks

        protected virtual void OnStartCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnStartCast)}: started");
        }

        protected virtual void OnPerformedCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnPerformedCast)}: performed");
        }

        protected virtual void OnCanceledCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnCanceledCast)}: canceled");
        }

        #endregion

        #region Input

        public virtual bool IsInProgress()
        {
            return m_CastInput.action.inProgress;
        }

        public virtual bool IsPressed()
        {
            return m_CastInput.action.IsPressed();
        }

        #endregion
    }
}