using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class ShieldSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private Shield m_Shield;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Shield.gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_Shield.gameObject.SetActive(false);
        }

        protected override void OnStartCast(InputAction.CallbackContext context)
        {
            base.OnStartCast(context);
            if (!m_PhotonView.IsMine)
            {
                return;
            }

            OnStarted?.Invoke();
        }

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);
            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            OnPerformed?.Invoke();
            m_IsPerformed = true;
            m_Shield.Show(true);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            if (!m_PhotonView.IsMine)
            {
                return;
            }
            
            Interrupt();
        }

        protected override void Interrupt()
        {
            base.Interrupt();
            if (!m_PhotonView.IsMine)
            {
                return;
            }

            OnCompleted?.Invoke();
            m_IsPerformed = false;
            m_Shield.Show(false);
        }
    }
}