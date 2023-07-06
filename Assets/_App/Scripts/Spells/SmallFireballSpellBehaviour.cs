using System;
using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    //[CreateAssetMenu(fileName = "SmallFireball", menuName = "MobaVR API/Spells/Create small fireball")]
    public class SmallFireballSpellBehaviour : SpellBehaviour
    {
        private const string TAG = nameof(SmallFireballSpellBehaviour);
        [SerializeField] private SmallFireBall m_SmallFireballPrefab;

        [SerializeField] private SpellHandType m_SpellHandType = SpellHandType.RIGHT_HAND;
        [SerializeField] private InputActionReference m_CastInput;

        private SmallFireBall m_SmallFireBall;
        private HandInputVR m_MainHandInputVR;

        #region Unity

        private void OnEnable()
        {
            m_CastInput.action.started += OnStartCast;
            m_CastInput.action.performed += OnPerformedCast;
            m_CastInput.action.canceled += OnCanceledCast;
        }

        private void OnDisable()
        {
            m_CastInput.action.started -= OnStartCast;
            m_CastInput.action.performed -= OnPerformedCast;
            m_CastInput.action.canceled -= OnCanceledCast;
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

            if (!CanCast() && HasBlockingSpells())
            {
                return;
            }

            CreateSmallFireBall(m_MainHandInputVR.FingerPoint);
            ThrowSmallFireBall(m_SmallFireBall, m_MainHandInputVR.Grabber.transform.forward);
        }

        private void OnCanceledCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{TAG}: {nameof(OnCanceledCast)}: canceled");
        }

        #endregion

        private void CreateSmallFireBall(Transform point)
        {
            GameObject networkFireball = PhotonNetwork.Instantiate($"Spells/{m_SmallFireballPrefab.name}",
                                                                   point.transform.position,
                                                                   point.transform.rotation);
            
            if (networkFireball.TryGetComponent(out m_SmallFireBall))
            {
                m_SmallFireBall.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);

                Transform fireBallTransform = m_SmallFireBall.transform;
                fireBallTransform.parent = null;
                fireBallTransform.position = point.transform.position;
                fireBallTransform.rotation = Quaternion.identity;
            }
        }

        private void ThrowSmallFireBall(SmallFireBall fireBall, Vector3 direction)
        {
            if (fireBall != null)
            {
                fireBall.ThrowByDirection(direction);
                fireBall = null;
            }
        }

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
            return m_CastInput.action.inProgress;
        }
        
        public override bool IsPressed()
        {
            return m_CastInput.action.IsPressed();
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