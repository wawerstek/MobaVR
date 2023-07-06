using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    [CreateAssetMenu(fileName = "BigFireball", menuName = "MobaVR API/Spells/Create big fireball")]
    public class BigFireballSpell : SpellStateSO
    {
        private const string TAG = nameof(BigFireballSpell);

        [SerializeField] private BigFireBall m_BigFireballPrefab;

        [SerializeField] private SpellHandType m_SpellHandType = SpellHandType.RIGHT_HAND;
        [SerializeField] private InputActionReference m_CastInput;
        [SerializeField] private InputActionReference m_RedirectInput;

        private BigFireBall m_FireBall;
        private HandInputVR m_MainHandInputVR;

        #region Unity

        private void OnEnable()
        {
            m_CastInput.action.started += OnStartCast;
            m_CastInput.action.performed += OnPerformedCast;
            m_CastInput.action.canceled += OnCanceledCast;

            m_RedirectInput.action.started += OnStartRedirect;
            m_RedirectInput.action.performed += OnPerformedRedirect;
            m_RedirectInput.action.canceled += OnCanceledRedirect;
        }

        private void OnDisable()
        {
            m_CastInput.action.started -= OnStartCast;
            m_CastInput.action.performed -= OnPerformedCast;
            m_CastInput.action.canceled -= OnCanceledCast;

            m_RedirectInput.action.started -= OnStartRedirect;
            m_RedirectInput.action.performed -= OnPerformedRedirect;
            m_RedirectInput.action.canceled -= OnCanceledRedirect;
        }

        #endregion

        #region Input Callbacks

        private void OnStartCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnStartCast)}: started");
        }

        private void OnPerformedCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnPerformedCast)}: performed");

            if (!CanCast)
            {
                return;
            }

            m_IsPerformed = true;

            CreateFireball(m_MainHandInputVR.Grabber.transform);
        }

        private void OnCanceledCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnCanceledCast)}: canceled");

            if (!CanCast)
            {
                return;
            }

            ThrowFireball();
        }

        private void OnStartRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnStartRedirect)}: started");
        }

        private void OnPerformedRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnPerformedRedirect)}: performed");

            if (!CanCast)
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

        private void OnCanceledRedirect(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnCanceledRedirect)}: canceled");
        }

        #endregion

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

                m_FireBall.OnInitSpell += OnInitSpell;
                m_FireBall.OnDestroySpell += OnDestroySpell;
            }
        }

        private void ThrowFireball()
        {
            if (m_FireBall != null)
            {
                m_FireBall.Throw();
            }
        }

        private void OnDestroySpell()
        {
            m_FireBall.OnInitSpell -= OnInitSpell;
            m_FireBall.OnDestroySpell -= OnDestroySpell;

            m_FireBall = null;
            m_IsPerformed = false;
        }

        private void OnInitSpell()
        {
        }

        #endregion

        public override void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            base.Init(spellHandler, playerVR);
            switch (m_SpellHandType)
            {
                case SpellHandType.LEFT_HAND:
                    m_MainHandInputVR = playerVR.InputVR.LefHandInputVR;
                    break;
                case SpellHandType.RIGHT_HAND:
                    m_MainHandInputVR = playerVR.InputVR.RightHandInputVR;
                    break;
                case SpellHandType.BOTH:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void CheckInput()
        {
        }

        public override bool IsInProgress()
        {
            return m_CastInput.action.inProgress || m_RedirectInput.action.inProgress;
        }
        
        public override bool IsPressed()
        {
            return m_CastInput.action.IsPressed() || m_RedirectInput.action.IsPressed();
        }

        public override void Enter()
        {
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
        }
    }
}