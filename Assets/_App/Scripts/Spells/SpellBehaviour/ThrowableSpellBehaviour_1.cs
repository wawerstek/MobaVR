using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class ThrowableSpellBehaviour_1 : InputSpellBehaviour
    {
        [SerializeField] private InputActionReference m_RedirectInput;
        [SerializeField] private ThrowableSpell m_ThrowableSpell;
        //[SerializeField] private Throwable m_Throwable;

        private ThrowableSpell m_CurrentSpell;
        private Throwable m_Throwable;
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
            Debug.Log($"{SpellName}: {nameof(OnPerformedCast)}: PERFORMED = 1");
            OnPerformed?.Invoke();
            Debug.Log($"{SpellName}: {nameof(OnPerformedCast)}: PERFORMED = 2");
            m_IsPerformed = true;
            m_IsThrown = false;

            //CreateFireball(m_MainHandInputVR.Grabber.transform);
            CreateFireball(m_MainHandInputVR.InsideHandPoint);
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
            Debug.Log($"{SpellName}: {nameof(OnStartRedirect)}: started");
        }

        protected void OnPerformedRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnPerformedRedirect)}: performed");

            if (!CanCast() || HasBlockingSpells() || !m_IsThrown)
            {
                return;
            }

            // TODO: set direction
            // Check transform from point
            int kInvert = m_SpellHandType == SpellHandType.RIGHT_HAND ? -1 : 1;
            Vector3 direction = m_MainHandInputVR.Grabber.transform.right * kInvert;

            if (m_Throwable != null)
            {
                m_Throwable.ThrowByDirection(direction);
            }
        }

        protected void OnCanceledRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnCanceledRedirect)}: canceled");
        }

        protected override void Interrupt()
        {
            if (!m_IsThrown && m_Throwable != null)
            {
                m_Throwable.Throw();

                m_Throwable = null;
                m_CurrentSpell = null;
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
            Debug.Log($"{SpellName}: {nameof(CreateFireball)}: CreateFireball = 1");

            GameObject networkFireball = PhotonNetwork.Instantiate($"Spells/{m_ThrowableSpell.name}",
                                                                   point.position,
                                                                   point.rotation);

            Debug.Log($"{SpellName}: {nameof(CreateFireball)}: CreateFireball = 2");
            
            if (networkFireball.TryGetComponent(out ThrowableSpell fireBall))
            {
                m_Number++;
                string handName = m_SpellHandType == SpellHandType.RIGHT_HAND ? "Right" : "Left";
                string fireballName = $"{m_ThrowableSpell.name}_{handName}_{m_Number}";
                networkFireball.name = fireballName;

                Debug.Log($"{SpellName}: {nameof(CreateFireball)}: CreateFireball = 3");
                
                Transform fireBallTransform = fireBall.transform;
                fireBallTransform.parent = point.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;

                fireBall.OnInitSpell += () => OnInitSpell(fireBall);
                fireBall.OnDestroySpell += () => OnDestroySpell(fireBall);

                m_IsThrown = false;
                m_CurrentSpell = fireBall;
                
                fireBall.TryGetComponent(out m_Throwable);
                
                fireBall.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
                
                Debug.Log($"{SpellName}: {nameof(CreateFireball)}: CreateFireball = 4");
            }
        }

        private void ThrowFireball()
        {
            if (m_Throwable != null)
            {
                m_IsThrown = true;
                m_Throwable.Throw();
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
            }

            if (m_CurrentSpell == fireBall)
            {
                //m_CurrentFireBall = null;
                Debug.Log($"CurrentFireball == fireball OK; {m_CurrentSpell.name}; {fireBall.name}");
                m_IsPerformed = false;
                OnCompleted?.Invoke();
            }
            else
            {
                if (m_CurrentSpell != null)
                {
                    Debug.Log($"CurrentFireball == fireball FALSE; {m_CurrentSpell.name}; {fireBall.name}");
                }
                else
                {
                    Debug.Log($"CurrentFireball == fireball FALSE; NULL; {fireBall.name}");
                }
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