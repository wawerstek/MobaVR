using BNG;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class PickupSpellInputBehaviour : InputSpellBehaviour
    {
        private Grabbable m_Grabbable;

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            m_IsPerformed = true;
            Pickup();
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            m_IsPerformed = false;
        }

        private void Pickup()
        {
        }
    }
}