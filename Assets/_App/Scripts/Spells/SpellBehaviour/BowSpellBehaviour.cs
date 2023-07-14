using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class BowSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private GameObject m_Bow;

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
            m_Bow.SetActive(true);
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
            m_Bow.SetActive(false);
        }
    }
}