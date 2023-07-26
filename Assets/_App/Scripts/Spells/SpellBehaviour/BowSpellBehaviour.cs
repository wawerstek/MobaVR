using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class BowSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private BowSpell m_Bow;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Bow.gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_Bow.gameObject.SetActive(false);
        }

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

            if (!m_Bow.Grabbable.IsGrabbable())
            {
                if (m_SpellHandType == SpellHandType.LEFT_HAND)
                {
                    m_Bow.Grabbable.DropItem(m_RightHand.Grabber);
                }
                else
                {
                    m_Bow.Grabbable.DropItem(m_LeftHand.Grabber);
                }
            }
            
            m_Bow.Grabbable.GrabRemoteItem(m_MainHandInputVR.Grabber);
            m_Bow.Show(true);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            if (IsPerformed())
            {
                Interrupt();
            }
        }

        protected override void Interrupt()
        {
            base.Interrupt();

            OnCompleted?.Invoke();
            m_IsPerformed = false;
            m_Bow.Show(false);

            if (m_Bow != null)
            {
                m_Bow.Grabbable.DropItem(m_MainHandInputVR.Grabber);
            }
            
            WaitCooldown();
        }

        public override void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            base.Init(spellHandler, playerVR);
            //m_Bow.Grabbable.GrabRemoteItem(m_MainHandInputVR.Grabber);
        }
    }
}