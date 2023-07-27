using System;
using BNG;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Bow = MobaVR.Weapons.Bow.Bow;

namespace MobaVR
{
    public class ArrowSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private Quiver m_Quiver;
        [SerializeField] private bool m_UseQuiver = false;
        [SerializeField] private ArrowSpell m_ArrowPrefab;

        private ArrowSpell m_CurrentArrow;
        private bool m_IsThrown = false;
        private int m_Number = 0;
        private bool m_IsAttach = false;
        [SerializeField] [ReadOnly] private bool m_IsTriggerQuiver = false;

        #region Input Callbacks

        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_PhotonView.IsMine && m_Quiver != null)
            {
                m_Quiver.OnGrabberTriggerEnter += OnGrabberTriggerEnter;
                m_Quiver.OnGrabberTriggerExit += OnGrabberTriggerExit;
            }
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            if (m_PhotonView.IsMine && m_Quiver != null)
            {
                m_Quiver.OnGrabberTriggerEnter -= OnGrabberTriggerEnter;
                m_Quiver.OnGrabberTriggerExit -= OnGrabberTriggerExit;
            }
        }

        private void OnGrabberTriggerEnter(Grabber grabber)
        {
            if (grabber == m_MainHandInputVR.Grabber)
            {
                m_IsTriggerQuiver = true;
            }
        }
        
        private void OnGrabberTriggerExit(Grabber grabber)
        {
            if (grabber == m_MainHandInputVR.Grabber)
            {
                m_IsTriggerQuiver = false;
            }
        }

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

            if (!m_UseQuiver || (m_UseQuiver && m_IsTriggerQuiver))
            {
                OnPerformed?.Invoke();
                m_IsPerformed = true;
                m_IsThrown = false;
                m_IsAttach = false;

                CreateArrow(m_MainHandInputVR.Grabber.transform);
            }
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);

            if (!CanCast())
            {
                return;
            }
            
            m_IsPerformed = false;

            if (m_IsAttach)
            {
                Release();
            }
            else
            {
                Interrupt();
            }
        }

        protected override void Interrupt()
        {
            base.Interrupt();
            
            if (m_CurrentArrow != null)
            {
                m_CurrentArrow.DestroySpell();
                m_CurrentArrow = null;

                m_IsAttach = false;
                m_IsThrown = false;
            }

            m_IsPerformed = false;
            WaitCooldown();
            OnCompleted?.Invoke();
        }
        
        private void Release()
        {
            if (m_CurrentArrow != null)
            {
                //m_CurrentArrow.Release();
                m_CurrentArrow = null;

                m_IsPerformed = false;
                m_IsThrown = true;
                m_IsAttach = false;
            }
            
            WaitCooldown();
            OnCompleted?.Invoke();
        }

        #endregion

        #region Fireball

        private void CreateArrow(Transform point)
        {
            GameObject networkArrow = PhotonNetwork.Instantiate($"Spells/{m_ArrowPrefab.name}",
                                                                point.position,
                                                                point.rotation);

            m_IsThrown = false;

            if (networkArrow.TryGetComponent(out ArrowSpell arrowSpell))
            {
                m_Number++;
                string handName = m_SpellHandType == SpellHandType.RIGHT_HAND ? "Right" : "Left";
                string fireballName = $"{m_ArrowPrefab.name}_{handName}_{m_Number}";
                networkArrow.name = fireballName;

                Transform fireBallTransform = networkArrow.transform;
                fireBallTransform.parent = point.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;

                arrowSpell.Arrow.OnAttach.AddListener(OnAttach);
                
                arrowSpell.OnInitSpell += () => OnInitSpell(arrowSpell);
                arrowSpell.OnDestroySpell += () => OnDestroySpell(arrowSpell);

                arrowSpell.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
                m_CurrentArrow = arrowSpell;
            }
        }

        private void OnAttach(Bow bow)
        {
            m_IsAttach = true;
        }

        private void OnDestroySpell(ArrowSpell arrowSpell)
        {
            if (arrowSpell != null)
            {
                arrowSpell.OnInitSpell -= () => OnInitSpell(arrowSpell);
                arrowSpell.OnDestroySpell -= () => OnDestroySpell(arrowSpell);
                
                arrowSpell.Arrow.OnAttach.RemoveListener(OnAttach);
            }

            if (m_CurrentArrow == arrowSpell)
            {
                m_IsPerformed = false;
                OnCompleted?.Invoke();
            }
        }

        private void OnInitSpell(ArrowSpell arrowSpell)
        {
            m_IsThrown = false;
            m_IsPerformed = true;
        }

        #endregion
    }
}