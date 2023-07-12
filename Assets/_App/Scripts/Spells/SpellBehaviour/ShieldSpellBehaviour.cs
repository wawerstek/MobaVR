using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class ShieldSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private Shield m_Shield;

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
            
            OnPerformed?.Invoke();
            m_IsPerformed = true;
            m_Shield.Show(true);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            Interrupt();
        }

        protected override void Interrupt()
        {
            base.Interrupt();
            
            OnCompleted?.Invoke();
            m_IsPerformed = false;
            m_Shield.Show(false);
        }
    }
}