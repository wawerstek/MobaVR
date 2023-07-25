using System;
using BNG;
using Sirenix.OdinInspector;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class PickupSpellBehaviour : SpellBehaviour
    {
        [SerializeField] private HandType m_HandType;
        [SerializeField] [ReadOnly] private bool m_IsTriggered = false;
        [SerializeField] [ReadOnly] private bool m_IsPickuped = false;

        private HandInputVR m_HandInputVR;
        private Grabbable m_Grabbable;
        private Grabbable m_TriggeredGrabbable;
        private Grabber m_Grabber;
        private RemoteGrabber m_RemoteGrabber;

        #region Spells

        public override void Init(SpellHandler spellHandler, PlayerVR playerVR)
        {
            base.Init(spellHandler, playerVR);
            switch (m_HandType)
            {
                case HandType.HandLeft:
                    m_HandInputVR = playerVR.InputVR.LefHandInputVR;
                    break;
                case HandType.HandRight:
                    m_HandInputVR = playerVR.InputVR.RightHandInputVR;
                    break;
            }

            Subscribe();
        }

        private void Subscribe()
        {
            if (m_HandInputVR != null && m_PhotonView.IsMine)
            //if (m_HandInputVR != null && m_PlayerVR != null && m_PlayerVR.IsMine)
            {
                m_HandInputVR.RemoteGrabber.OnEnterGrabbableEvent.AddListener(OnRemoteEnter);
                m_HandInputVR.RemoteGrabber.OnExitGrabbableEvent.AddListener(OnRemoteExit);

                m_HandInputVR.Grabber.onGrabEvent.AddListener(OnGrab);
                m_HandInputVR.Grabber.onReleaseEvent.AddListener(OnRelease);

                m_HandInputVR.Grabber.GrabAction.action.started += OnStartCast;
                m_HandInputVR.Grabber.GrabAction.action.performed += OnPerformedCast;
                m_HandInputVR.Grabber.GrabAction.action.canceled += OnCanceledCast;
            }
        }

        private void Unsubscribe()
        {
            //if (m_HandInputVR != null && m_PhotonView.IsMine)
            if (m_HandInputVR != null && m_PlayerVR != null && m_PlayerVR.IsMine)
            {
                m_HandInputVR.RemoteGrabber.OnEnterGrabbableEvent.RemoveListener(OnRemoteEnter);
                m_HandInputVR.RemoteGrabber.OnExitGrabbableEvent.RemoveListener(OnRemoteExit);

                m_HandInputVR.Grabber.onGrabEvent.RemoveListener(OnGrab);
                m_HandInputVR.Grabber.onReleaseEvent.RemoveListener(OnRelease);

                m_HandInputVR.Grabber.GrabAction.action.started -= OnStartCast;
                m_HandInputVR.Grabber.GrabAction.action.performed -= OnPerformedCast;
                m_HandInputVR.Grabber.GrabAction.action.canceled -= OnCanceledCast;
            }
        }

        #endregion

        #region MonoBehaviour

        private void OnDisable()
        {
            Unsubscribe();
        }

        #endregion

        #region Events

        private void OnStartCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnStartCast)}: started");

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            if (m_IsTriggered)
            {
                m_IsPerformed = true;
            }
        }

        private void OnPerformedCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnPerformedCast)}: performed");

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            if (m_IsTriggered)
            {
                m_IsPerformed = true;
            }
        }

        private void OnCanceledCast(InputAction.CallbackContext context)
        {
            Debug.Log($"{SpellName}: {nameof(OnCanceledCast)}: canceled");
            if (!m_PhotonView.IsMine)
            {
                return;
            }
            m_IsPerformed = false;
        }

        private void OnRemoteEnter(Grabbable grabbable)
        {
            Debug.Log($"{SpellName}: {nameof(OnRemoteEnter)}: {grabbable}");
            /*
            if (grabbable.TryGetComponent(out Fireball fireball))
            {
                return;
            }
            */

            ///
            /// TODO: Костыль
            /// У игрока не всегда сбрасывается исТриггер
            if (grabbable.TryGetComponent(out BaseSpell baseSpell))
            {
                return;
            }
            
            if (grabbable.TryGetComponent(out Spell spell))
            {
                return;
            }

            m_TriggeredGrabbable = grabbable;
            m_IsTriggered = true;
        }

        private void OnRemoteExit(Grabbable grabbable)
        {
            Debug.Log($"{SpellName}: {nameof(OnRemoteExit)}: {grabbable}");

            if (grabbable.TryGetComponent(out BaseSpell baseSpell))
            {
                return;
            }

            if (grabbable.TryGetComponent(out Spell spell))
            {
                return;
            }

            if (m_TriggeredGrabbable != null && m_TriggeredGrabbable == grabbable)
            {
                m_IsTriggered = false;
                m_TriggeredGrabbable = null;
            }
        }

        private void OnGrab(Grabbable grabbable)
        {
            Debug.Log($"{SpellName}: {nameof(OnGrab)}: {grabbable}");

            ///
            /// TODO: Костыль
            /// У игрока не всегда сбрасывается исТриггер
            if (grabbable.TryGetComponent(out BaseSpell baseSpell))
            {
                return;
            }

            m_Grabbable = grabbable;
            m_IsPerformed = true;
            m_IsPickuped = true;
        }

        private void OnRelease(Grabbable grabbable)
        {
            Debug.Log($"{SpellName}: {nameof(OnRelease)}: {grabbable}");

            m_Grabbable = null;
            m_IsPerformed = false;
            m_IsPickuped = false;
        }

        #endregion
    }
}