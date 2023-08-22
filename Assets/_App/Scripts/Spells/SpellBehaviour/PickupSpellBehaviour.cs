#define UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
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
        private RemoteGrabber m_RemoteGrabber;

        private bool m_IsSubscribed = false;

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

        protected override void OnEnable()
        {
            base.OnEnable();
            Subscribe();
        }

        private void Subscribe()
        {
            if (m_HandInputVR != null && m_PhotonView.IsMine && !m_IsSubscribed)
            {
                m_HandInputVR.RemoteGrabber.OnEnterGrabbableEvent.AddListener(OnRemoteEnter);
                m_HandInputVR.RemoteGrabber.OnExitGrabbableEvent.AddListener(OnRemoteExit);

                m_HandInputVR.Grabber.onGrabEvent.AddListener(OnGrab);
                m_HandInputVR.Grabber.onReleaseEvent.AddListener(OnRelease);

                m_HandInputVR.Grabber.GrabAction.action.started += OnStartCast;
                m_HandInputVR.Grabber.GrabAction.action.performed += OnPerformedCast;
                m_HandInputVR.Grabber.GrabAction.action.canceled += OnCanceledCast;

                m_IsSubscribed = true;
            }
        }

        private void Unsubscribe()
        {
            if (m_HandInputVR != null && m_PhotonView.IsMine)
            {
                m_HandInputVR.RemoteGrabber.OnEnterGrabbableEvent.RemoveListener(OnRemoteEnter);
                m_HandInputVR.RemoteGrabber.OnExitGrabbableEvent.RemoveListener(OnRemoteExit);

                m_HandInputVR.Grabber.onGrabEvent.RemoveListener(OnGrab);
                m_HandInputVR.Grabber.onReleaseEvent.RemoveListener(OnRelease);

                m_HandInputVR.Grabber.GrabAction.action.started -= OnStartCast;
                m_HandInputVR.Grabber.GrabAction.action.performed -= OnPerformedCast;
                m_HandInputVR.Grabber.GrabAction.action.canceled -= OnCanceledCast;

                m_IsSubscribed = false;
            }
        }

        #endregion

        #region MonoBehaviour

        protected override void OnDisable()
        {
            base.OnDisable();
            Unsubscribe();
        }

        #endregion

        #region Events

        private bool IsValidRemoteGrabbables()
        {
            if (m_HandInputVR == null || m_HandInputVR.Grabber == null)
            {
                return false;
            }

            m_HandInputVR.Grabber.GrabsInTrigger.RemoveNullKeys();
            List<Grabbable> grabbables = new();
            grabbables.AddRange(m_HandInputVR.Grabber.GrabsInTrigger.ValidGrabbables.Values.ToList());
            grabbables.AddRange(m_HandInputVR.Grabber.GrabsInTrigger.ValidRemoteGrabbables.Values.ToList());

            if (grabbables.Count > 0)
            {
                foreach (Grabbable grabbable in grabbables)
                {
                    if (grabbable.gameObject.activeSelf && grabbable.enabled)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnStartCast(InputAction.CallbackContext context)
        {
            AppDebug.Log($"{SpellName}: {nameof(OnStartCast)}: started");

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            //if (m_IsTriggered)
            if (IsValidRemoteGrabbables())
            {
                m_IsPerformed = true;
            }
        }

        private void OnPerformedCast(InputAction.CallbackContext context)
        {
            AppDebug.Log($"{SpellName}: {nameof(OnPerformedCast)}: performed");

            if (!CanCast() || HasBlockingSpells())
            {
                return;
            }

            if (IsValidRemoteGrabbables())
            {
                m_IsPerformed = true;
            }

            //if (m_IsTriggered)
            {
                //m_IsPerformed = true;
            }
        }

        private void OnCanceledCast(InputAction.CallbackContext context)
        {
            AppDebug.Log($"{SpellName}: {nameof(OnCanceledCast)}: canceled");
            //if (!m_IsPickuped)
            {
                m_IsPerformed = false;
            }
        }

        ///
        /// TODO: Костыль
        /// У игрока не всегда сбрасывается исТриггер
        private void OnRemoteEnter(Grabbable grabbable)
        {
            AppDebug.Log($"{SpellName}: {nameof(OnRemoteEnter)}: {grabbable}");

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
            AppDebug.Log($"{SpellName}: {nameof(OnRemoteExit)}: {grabbable}");

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

        ///
        /// TODO: Костыль
        /// У игрока не всегда сбрасывается исТриггер
        private void OnGrab(Grabbable grabbable)
        {
            AppDebug.Log($"{SpellName}: {nameof(OnGrab)}: {grabbable}");

            if (grabbable.TryGetComponent(out BaseSpell baseSpell))
            {
                return;
            }

            if (grabbable.TryGetComponent(out Spell spell))
            {
                return;
            }

            m_Grabbable = grabbable;
            m_IsPerformed = true;
            m_IsPickuped = true;
        }

        private void OnRelease(Grabbable grabbable)
        {
            AppDebug.Log($"{SpellName}: {nameof(OnRelease)}: {grabbable}");

            m_Grabbable = null;
            m_IsPerformed = false;
            m_IsPickuped = false;
        }

        #endregion
    }
}