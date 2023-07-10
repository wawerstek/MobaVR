using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class ShieldSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private Shield m_Shield;

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            m_IsPerformed = true;
            m_SpellsHandler.SetCurrentSpell(this);
            m_Shield.Show(true);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            
            m_IsPerformed = false;
            m_Shield.Show(false);
            
            //m_SpellsHandler.DeactivateCurrentSpell(this);
        }

        private void Update()
        {
            Debug.Log($"Shield: phase = {m_CastInput.action.phase}");
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