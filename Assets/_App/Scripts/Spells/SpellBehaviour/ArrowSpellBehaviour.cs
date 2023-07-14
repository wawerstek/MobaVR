using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class ArrowSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private GameObject m_ArrowPrefab;

        private GameObject m_CurrentArrow;
        private bool m_IsThrown = false;
        private int m_Number = 0;

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

            CreateArrow(m_MainHandInputVR.Grabber.transform);
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);

            if (!CanCast())
            {
                return;
            }

            m_IsPerformed = false;
            ThrowFireball();
        }

        protected override void Interrupt()
        {
            if (!m_IsThrown && m_CurrentArrow != null)
            {
                //m_CurrentArrow.Throw();

                m_CurrentArrow = null;
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

        private void CreateArrow(Transform point)
        {
            GameObject networkArrow = PhotonNetwork.Instantiate($"Spells/{m_ArrowPrefab.name}",
                                                                point.position,
                                                                point.rotation);

            m_IsThrown = false;

            m_Number++;
            string handName = m_SpellHandType == SpellHandType.RIGHT_HAND ? "Right" : "Left";
            string fireballName = $"{m_ArrowPrefab.name}_{handName}_{m_Number}";
            networkArrow.name = fireballName;

            Transform fireBallTransform = networkArrow.transform;
            fireBallTransform.parent = point.transform;
            fireBallTransform.localPosition = Vector3.zero;
            fireBallTransform.localRotation = Quaternion.identity;

            /*
            fireBall.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
            fireBall.OnInitSpell += () => OnInitSpell(fireBall);
            fireBall.OnDestroySpell += () => OnDestroySpell(fireBall);
            */

            m_CurrentArrow = networkArrow;
        }

        private void ThrowFireball()
        {
            if (m_CurrentArrow != null)
            {
                m_IsThrown = true;
                //m_CurrentArrow.Throw();
            }
        }

        private void OnInitSpell(BigFireBall fireBall)
        {
            m_IsThrown = false;
            m_IsPerformed = true;
        }

        #endregion
    }
}