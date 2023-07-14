using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class BowSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private BowSpell m_Bow;

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
            m_Bow.Show(true);
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
            m_Bow.Show(false);
        }
    }
}