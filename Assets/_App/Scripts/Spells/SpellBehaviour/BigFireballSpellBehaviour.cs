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

        protected override void OnPerformedCast(InputAction.CallbackContext context)
        {
            base.OnPerformedCast(context);
            Debug.Log($"Spell: BigFireballSpellBehaviour: CanCast: {CanCast()}, Block: {HasBlockingSpells()}");


            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            Debug.Log($"Spell: BigFireballSpellBehaviour: OnPerformedCast start");


            m_IsPerformed = true;
            m_SpellsHandler.SetCurrentSpell(this);

            m_IsThrown = false;
            CreateFireball(m_MainHandInputVR.Grabber.transform);
            
            Debug.Log($"Spell: BigFireballSpellBehaviour: OnPerformedCast end");
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
                                                                   point.transform.position,
                                                                   point.transform.rotation);

            if (networkFireball.TryGetComponent(out m_FireBall))
            {
                m_FireBall.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);

                Transform fireBallTransform = m_FireBall.transform;
                fireBallTransform.parent = point.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;

                m_IsThrown = false;
                
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

        private void OnDestroySpell()
        {
            if (m_FireBall != null)
            {
                m_FireBall.OnInitSpell -= OnInitSpell;
                m_FireBall.OnDestroySpell -= OnDestroySpell;

                m_FireBall = null;
            }

            m_IsPerformed = false;

            //m_SpellsHandler.DeactivateCurrentSpell(this);
        }

        private void OnInitSpell()
        {
        }

        #endregion

        public void Update()
        {
            Debug.Log($"BigFireball: phase = {m_CastInput.action.phase}");
        }

        public override bool IsInProgress()
        {
            return m_CastInput.action.inProgress || m_RedirectInput.action.inProgress;
        }

        public override bool IsPressed()
        {
            return m_CastInput.action.IsPressed() || m_RedirectInput.action.IsPressed();
        }

        public override void SpellEnter()
        {
        }

        public override void SpellUpdate()
        {
        }

        public override void SpellExit()
        {
            Debug.Log($"Spell: BigFireballSpellBehaviour: SpellExit: {m_FireBall}: isTrhown: {m_IsThrown}");
            
            if (!m_IsThrown && m_FireBall != null)
            {
                //TODO
                Debug.Log($"Spell: BigFireballSpellBehaviour: SpellExit != null: {m_FireBall}");
                
                m_FireBall.Throw();
            }
        }
    }
}