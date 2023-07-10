using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class FireBreathSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private FireBreath m_FireBreath;

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            m_IsPerformed = true;
            m_SpellsHandler.SetCurrentSpell(this);
            m_FireBreath.Show(true);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            
            m_IsPerformed = false;
            m_FireBreath.Show(false);
        }

        public override void SpellEnter()
        {
        }

        public override void SpellUpdate()
        {
        }

        public override void SpellExit()
        {
        }
    }
}