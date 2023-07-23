using System;
using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class ThrowableSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private InputActionReference m_RedirectInput;
        [SerializeField] private ThrowableSpell m_ThrowableSpell;

        private ThrowableSpell m_CurrentSpell;
        private bool m_IsGrabbed = false;
        private bool m_IsThrown = false;
        private int m_Number = 0;

        #region Unity

        protected override void OnEnable()
        {
            base.OnEnable();

            m_RedirectInput.action.started += OnStartRedirect;
            m_RedirectInput.action.performed += OnPerformedRedirect;
            m_RedirectInput.action.canceled += OnCanceledRedirect;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_RedirectInput.action.started -= OnStartRedirect;
            m_RedirectInput.action.performed -= OnPerformedRedirect;
            m_RedirectInput.action.canceled -= OnCanceledRedirect;
        }

        #endregion

        #region Input Callbacks

        protected override void OnStartCast(InputAction.CallbackContext context)
        {
            base.OnStartCast(context);
            OnStarted?.Invoke();
        }

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);
            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            if (m_IsGrabbed)
            {
                return;
            }

            OnPerformed?.Invoke();
            m_IsPerformed = true;
            m_IsThrown = false;
            m_IsGrabbed = false;

            CreateSpell(m_MainHandInputVR.InsideHandPoint);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);

            if (!CanCast())
            {
                return;
            }

            if (m_CurrentSpell != null && !m_IsGrabbed)
            {
                Interrupt();
            }
        }

        protected void OnStartRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnStartRedirect)}: started");
        }

        protected void OnPerformedRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnPerformedRedirect)}: performed");

            if (!CanCast() || HasBlockingSpells() || !m_IsThrown)
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

        protected override void Interrupt()
        {
            if (!m_IsThrown && m_CurrentSpell != null)
            {
                m_CurrentSpell.DestroySpell();

                m_IsGrabbed = false;
                m_IsThrown = false;
                m_CurrentSpell = null;
                m_IsPerformed = false;
            }

            OnCompleted?.Invoke();
        }

        #endregion

        #region Fireball

        private void CreateSpell(Transform point)
        {
            GameObject networkSpell = PhotonNetwork.Instantiate($"Spells/{m_ThrowableSpell.name}",
                                                                   point.position,
                                                                   point.rotation);

            if (networkSpell.TryGetComponent(out ThrowableSpell throwableSpell))
            {
                m_Number++;
                string handName = m_SpellHandType == SpellHandType.RIGHT_HAND ? "Right" : "Left";
                string spellName = $"{m_ThrowableSpell.name}_{handName}_{m_Number}";
                networkSpell.name = spellName;

                throwableSpell.Throwable.OnGrabbed.AddListener(grabber => Grab(grabber, throwableSpell));
                throwableSpell.Throwable.OnReleased.AddListener(() => Throw(throwableSpell));

                Transform throwableSpellTransform = throwableSpell.transform;
                throwableSpellTransform.parent = point.transform;
                throwableSpellTransform.localPosition = Vector3.zero;
                throwableSpellTransform.localRotation = Quaternion.identity;

                throwableSpell.OnInitSpell += () => OnInitSpell(throwableSpell);
                throwableSpell.OnDestroySpell += () => OnDestroySpell(throwableSpell);

                m_IsGrabbed = false;
                m_IsThrown = false;
                m_CurrentSpell = throwableSpell;

                throwableSpell.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
            }
        }

        private void Grab(Grabber grabber, ThrowableSpell throwableSpell)
        {
            if (throwableSpell == m_CurrentSpell)
            {
                m_IsGrabbed = true;
            }
        }

        private void Throw(ThrowableSpell throwableSpell)
        {
            if (m_CurrentSpell != null && throwableSpell == m_CurrentSpell)
            {
                m_IsGrabbed = false;
                m_IsThrown = true;
            }
        }

        private void OnInitSpell(ThrowableSpell fireBall)
        {
            m_IsThrown = false;
            m_IsPerformed = true;
        }

        private void OnDestroySpell(ThrowableSpell throwableSpell)
        {
            if (throwableSpell != null)
            {
                throwableSpell.OnInitSpell -= () => OnInitSpell(throwableSpell);
                throwableSpell.OnDestroySpell -= () => OnDestroySpell(throwableSpell);

                throwableSpell.Throwable.OnGrabbed.RemoveListener(grabber => Grab(grabber, throwableSpell));
                throwableSpell.Throwable.OnReleased.RemoveListener(() => Throw(throwableSpell));
            }

            if (m_CurrentSpell == throwableSpell)
            {
                m_IsGrabbed = false;
                m_IsThrown = false;
                m_IsPerformed = false;
                OnCompleted?.Invoke();
            }
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