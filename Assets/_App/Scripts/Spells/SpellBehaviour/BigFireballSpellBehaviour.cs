using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class BigFireballSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private InputActionReference m_RedirectInput;
        [SerializeField] private BigFireBall m_BigFireballPrefab;

        private BigFireBall m_FireBall;
        private bool m_IsThrown = false;

        #region Unity

        protected override void OnEnable()
        {
            base.OnEnable();

            m_RedirectInput.action.started += OnStartRedirect;
            m_RedirectInput.action.performed += OnPerformedRedirect;
            m_RedirectInput.action.canceled += OnCanceledRedirect;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_RedirectInput.action.started -= OnStartRedirect;
            m_RedirectInput.action.performed -= OnPerformedRedirect;
            m_RedirectInput.action.canceled -= OnCanceledRedirect;
        }

        #endregion

        #region Input Callbacks

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
            m_IsThrown = false;
            CreateFireball(m_MainHandInputVR.Grabber.transform);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);

            if (!CanCast())
            {
                return;
            }

            ThrowFireball();
        }

        protected void OnStartRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnStartRedirect)}: started");
        }

        protected void OnPerformedRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnPerformedRedirect)}: performed");

            if (!CanCast() || HasBlockingSpells() || !m_IsThrown)
            {
                return;
            }

            // TODO: set direction
            // Check transform from point
            int kInvert = m_SpellHandType == SpellHandType.RIGHT_HAND ? -1 : 1;
            Vector3 direction = m_MainHandInputVR.Grabber.transform.right * kInvert;

            if (m_FireBall != null)
            {
                m_FireBall.ThrowByDirection(direction);
            }
        }

        protected void OnCanceledRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnCanceledRedirect)}: canceled");
        }

        protected override void Interrupt()
        {
            if (!m_IsThrown && m_FireBall != null)
            {
                m_FireBall.Throw();
            }

            m_FireBall = null;
            m_IsPerformed = false;
            OnCompleted?.Invoke();
        }

        #endregion

        /*
                public override bool CanCast()
                {
                    bool canCast = base.CanCast();
                    //return canCast && m_MainHandInputVR.GrabbableTrigger.
                }
        */

        #region Fireball

        private void CreateFireball(Transform point)
        {
            GameObject networkFireball = PhotonNetwork.Instantiate($"Spells/{m_BigFireballPrefab.name}",
                                                                   point.position,
                                                                   point.rotation);

            if (networkFireball.TryGetComponent(out m_FireBall))
            {
                Transform fireBallTransform = m_FireBall.transform;
                fireBallTransform.parent = point.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;

                m_IsThrown = false;

                m_FireBall.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
                m_FireBall.OnInitSpell += OnInitSpell;
                m_FireBall.OnDestroySpell += OnDestroySpell;
            }
        }

        private void ThrowFireball()
        {
            if (m_FireBall != null)
            {
                m_IsThrown = true;
                m_FireBall.Throw();
            }
        }

        private void OnInitSpell()
        {
            m_IsThrown = false;
            m_IsPerformed = true;
        }

        private void OnDestroySpell()
        {
            if (m_FireBall != null)
            {
                m_FireBall.OnInitSpell -= OnInitSpell;
                m_FireBall.OnDestroySpell -= OnDestroySpell;
            }

            m_FireBall = null;
            m_IsPerformed = false;
            OnCompleted?.Invoke();
        }

        #endregion

        #region Input

        public override bool IsInProgress()
        {
            return m_CastInput.action.inProgress || m_RedirectInput.action.inProgress;
        }

        public override bool IsPressed()
        {
            return m_CastInput.action.IsPressed() || m_RedirectInput.action.IsPressed();
        }

        #endregion
    }
}