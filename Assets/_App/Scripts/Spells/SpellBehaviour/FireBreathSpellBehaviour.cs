using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class FireBreathSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private FireBreath m_FireBreath;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            m_FireBreath.gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_FireBreath.gameObject.SetActive(false);
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
            m_FireBreath.Show(true);
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
            m_FireBreath.Show(false);
        }
    }
}