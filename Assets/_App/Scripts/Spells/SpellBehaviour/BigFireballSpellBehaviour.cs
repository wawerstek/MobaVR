#define UNITY_EDITOR

using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class BigFireballSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private InputActionReference m_RedirectInput;
        [SerializeField] private BigFireBall m_BigFireballPrefab;

        private BigFireBall m_CurrentFireBall;
        private bool m_IsThrown = false;
        private int m_Number = 0;

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


            //TODO
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
            AppDebug.Log($"{SpellName}: {nameof(OnStartRedirect)}: started");
        }

        protected void OnPerformedRedirect(InputAction.CallbackContext context)
        {
            AppDebug.Log($"{SpellName}: {nameof(OnPerformedRedirect)}: performed");

            if (!CanCast() || HasBlockingSpells() || !m_IsThrown)
            {
                return;
            }

            // TODO: set direction
            // Check transform from point
            int kInvert = m_SpellHandType == SpellHandType.RIGHT_HAND ? -1 : 1;
            Vector3 direction = m_MainHandInputVR.Grabber.transform.right * kInvert;

            if (m_CurrentFireBall != null)
            {
                m_CurrentFireBall.ThrowByDirection(direction);
            }
        }

        protected void OnCanceledRedirect(InputAction.CallbackContext context)
        {
            AppDebug.Log($"{SpellName}: {nameof(OnCanceledRedirect)}: canceled");
        }

        protected override void Interrupt()
        {
            if (!m_IsThrown && m_CurrentFireBall != null)
            {
                m_CurrentFireBall.Throw();

                m_CurrentFireBall = null;
                m_IsPerformed = false;
            }

            OnCompleted?.Invoke();

            //m_CurrentFireBall = null;
            //m_IsPerformed = false;
            //OnCompleted?.Invoke();
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
            
            if (networkFireball.TryGetComponent(out BigFireBall fireBall))
            {
                m_Number++;
                string handName = m_SpellHandType == SpellHandType.RIGHT_HAND ? "Right" : "Left";
                string fireballName = $"{m_BigFireballPrefab.name}_{handName}_{m_Number}";
                networkFireball.name = fireballName;

                Transform fireBallTransform = fireBall.transform;
                fireBallTransform.parent = point.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;

                fireBall.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
                fireBall.OnInitSpell += () => OnInitSpell(fireBall);
                fireBall.OnDestroySpell += () => OnDestroySpell(fireBall);

                m_IsThrown = false;
                m_CurrentFireBall = fireBall;
            }
        }

        private void ThrowFireball()
        {
            if (m_CurrentFireBall != null)
            {
                m_IsThrown = true;
                m_CurrentFireBall.Throw();
            }
        }

        private void OnInitSpell(BigFireBall fireBall)
        {
            m_IsThrown = false;
            m_IsPerformed = true;
        }

        private void OnDestroySpell(BigFireBall fireBall)
        {
            if (fireBall != null)
            {
                fireBall.OnInitSpell -= () => OnInitSpell(fireBall);
                fireBall.OnDestroySpell -= () => OnDestroySpell(fireBall);
            }

            if (m_CurrentFireBall == fireBall)
            {
                //m_CurrentFireBall = null;
                m_IsPerformed = false;
                OnCompleted?.Invoke();
            }
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