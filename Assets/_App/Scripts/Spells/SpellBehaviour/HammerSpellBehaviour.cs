#define UNITY_EDITOR

using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class HammerSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private InputActionReference m_RedirectInput;
        [SerializeField] private HammerSpell m_HammerPrefab;

        private HammerSpell m_CurrentHammer;
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

            OnPerformed?.Invoke();
            m_IsPerformed = true;
            m_IsThrown = false;

            //CreateHammer(m_MainHandInputVR.Grabber.transform); Не работает??
            CreateHammer(m_MainHandInputVR.InsideHandPoint);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);
            Release();
        }

        protected void OnStartRedirect(InputAction.CallbackContext context)
        {
            AppDebug.Log($"{TAG}: {nameof(OnStartRedirect)}: started");
        }

        protected void OnPerformedRedirect(InputAction.CallbackContext context)
        {
            AppDebug.Log($"{TAG}: {nameof(OnPerformedRedirect)}: performed");

            if (!CanCast() || HasBlockingSpells() || !m_IsThrown)
            {
                return;
            }
            
            return;

            // TODO: set direction
            // Check transform from point
            int kInvert = m_SpellHandType == SpellHandType.RIGHT_HAND ? -1 : 1;
            Vector3 direction = m_MainHandInputVR.Grabber.transform.right * kInvert;

            if (m_CurrentHammer != null)
            {
                //m_CurrentHammer.ThrowByDirection(direction);
            }
        }

        protected void OnCanceledRedirect(InputAction.CallbackContext context)
        {
            AppDebug.Log($"{TAG}: {nameof(OnCanceledRedirect)}: canceled");
        }

        protected override void Interrupt()
        {
            if (!m_IsThrown && m_CurrentHammer != null)
            {
                //m_CurrentHammer.Throw();
                //m_CurrentHammer.Show(false);

                m_CurrentHammer = null;
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

        private void Release()
        {
            if (!CanCast())
            {
                return;
            }

            Throw();
        }

        private void CreateHammer(Transform point)
        {
            GameObject networkHammer = PhotonNetwork.Instantiate($"Spells/{m_HammerPrefab.name}",
                                                                   point.position,
                                                                   point.rotation);

            if (networkHammer.TryGetComponent(out HammerSpell hammerSpell))
            {
                m_Number++;
                string handName = m_SpellHandType == SpellHandType.RIGHT_HAND ? "Right" : "Left";
                string fireballName = $"{m_HammerPrefab.name}_{handName}_{m_Number}";
                networkHammer.name = fireballName;

                Transform hammerSpellTransform = hammerSpell.transform;
                hammerSpellTransform.parent = point.transform;
                hammerSpellTransform.localPosition = Vector3.zero;
                hammerSpellTransform.localRotation = Quaternion.identity;

                hammerSpell.OnInitSpell += () => OnInitSpell(hammerSpell);
                hammerSpell.OnDestroySpell += () => OnDestroySpell(hammerSpell);

                m_IsThrown = false;
                m_CurrentHammer = hammerSpell;
                
                hammerSpell.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);

                /*if (hammerSpell.TryGetComponent(out m_Throwable))
                {
                    m_Throwable.OnReleased.AddListener(Release);
                }*/
            }
        }

        private void Throw()
        {
            if (m_CurrentHammer != null)
            {
                m_IsThrown = true;
                m_CurrentHammer.Throwable.Throw();
            }
        }

        private void OnInitSpell(ThrowableSpell fireBall)
        {
            m_IsThrown = false;
            m_IsPerformed = true;
        }

        private void OnDestroySpell(ThrowableSpell fireBall)
        {
            if (fireBall != null)
            {
                fireBall.OnInitSpell -= () => OnInitSpell(fireBall);
                fireBall.OnDestroySpell -= () => OnDestroySpell(fireBall);
                
                /*if (fireBall.TryGetComponent(out Throwable throwable))
                {
                    throwable.OnReleased.RemoveListener(Release);
                }*/
            }

            if (m_CurrentHammer == fireBall)
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