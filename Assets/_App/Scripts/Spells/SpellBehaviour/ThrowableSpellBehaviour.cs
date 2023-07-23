using System;
using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class ThrowableSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] protected ThrowableSpell m_ThrowableSpell;

        protected ThrowableSpell m_CurrentSpell;
        protected bool m_IsGrabbed = false;
        protected bool m_IsThrown = false;
        protected int m_Number = 0;

        #region Unity

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
            if (!CanCast() || HasBlockingSpells() || HasBlockingInputs())
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
    }
}