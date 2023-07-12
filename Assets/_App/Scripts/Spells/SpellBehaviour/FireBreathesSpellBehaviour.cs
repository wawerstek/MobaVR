using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class FireBreathesSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private FireBreath m_FireBreath;
        [SerializeField] private bool m_CheckDistanceAngle = false;
        [SerializeField] private float m_MaxDistance = 2f;
        [SerializeField] private float m_MaxAngle = 120f;

        private HandInputVR m_LeftHand;
        private Transform m_LeftTarget;
        private HandInputVR m_RightHand;
        private Transform m_RightTarget;

        private float Distance => Vector3.Distance(m_LeftTarget.transform.position, m_RightTarget.transform.position);
        private float Angle => Vector3.Angle(m_LeftTarget.transform.forward, m_RightTarget.transform.forward);

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            if (m_CheckDistanceAngle)
            {
                if (Distance > m_MaxDistance)
                {
                    return;
                }

                if (Angle > m_MaxAngle)
                {
                    return;
                }
            }

            m_IsPerformed = true;
            m_SpellsHandler.SetCurrentSpell(this);
            m_FireBreath.Show(true);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            Hide();
        }

        protected void Hide()
        {
            m_IsPerformed = false;
            m_FireBreath.Show(false);
        }

        public override void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            base.Init(spellHandler, playerVR);
            if (m_SpellHandType == SpellHandType.BOTH)
            {
                m_LeftHand = playerVR.InputVR.LefHandInputVR;
                m_RightHand = playerVR.InputVR.RightHandInputVR;

                m_LeftTarget = playerVR.LeftHandTarget;
                m_RightTarget = playerVR.RightHandTarget;
            }
        }

        public override void SpellEnter()
        {
        }

        public override void SpellUpdate()
        {
            if (!m_IsPerformed)
            {
                return;
            }
            
            if (Distance > m_MaxDistance || Angle > m_MaxAngle)
            {
                Hide();
            }
        }

        public override void SpellExit()
        {
        }
    }
}