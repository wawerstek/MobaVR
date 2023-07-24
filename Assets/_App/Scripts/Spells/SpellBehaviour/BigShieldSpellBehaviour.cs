using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class BigShieldSpellBehaviour : InputSpellBehaviour
    {
        [SerializeField] private BigShield m_ShieldPrefab;
        [SerializeField] private Transform m_SpawnPoint;
        [SerializeField] private bool m_IsPlaced = false;

        private BigShield m_CurrentShield;
        private int m_Number = 1;

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

            CreateShield();
        }

        protected override void OnCanceledCast(InputAction.CallbackContext context)
        {
            base.OnCanceledCast(context);

            if (m_IsPerformed && m_CurrentShield != null)
            {
                m_CurrentShield.Place();
                m_IsPlaced = true;
            }

            m_IsPerformed = false;
            OnCompleted?.Invoke();
        }

        protected override void Interrupt()
        {
            base.Interrupt();
            OnCompleted?.Invoke();
            m_IsPerformed = false;
            m_IsPlaced = false;
            if (m_CurrentShield != null)
            {
                m_CurrentShield.DestroySpell();
            }
        }

        private void CreateShield()
        {
            GameObject networkHammer = PhotonNetwork.Instantiate($"Spells/{m_ShieldPrefab.name}",
                                                                 m_SpawnPoint.position,
                                                                 m_SpawnPoint.rotation);

            if (networkHammer.TryGetComponent(out BigShield bigShield))
            {
                m_Number++;
                string handName = m_SpellHandType == SpellHandType.RIGHT_HAND ? "Right" : "Left";
                string fireballName = $"{m_ShieldPrefab.name}_{handName}_{m_Number}";
                networkHammer.name = fireballName;

                Transform fireBallTransform = bigShield.transform;
                fireBallTransform.parent = m_SpawnPoint.transform;
                fireBallTransform.localPosition = Vector3.zero;
                fireBallTransform.localRotation = Quaternion.identity;

                bigShield.OnInitSpell += () => OnInitSpell(bigShield);
                bigShield.OnDestroySpell += () => OnDestroySpell(bigShield);
                
                bigShield.Init(m_PlayerVR.WizardPlayer, m_PlayerVR.TeamType);
                m_CurrentShield = bigShield;
            }
        }

        private void OnInitSpell(BigShield shield)
        {
            shield.Prepare();
        }

        private void OnDestroySpell(BigShield shield)
        {
            if (shield != null)
            {
                shield.OnDestroySpell -= () => OnInitSpell(shield);
                shield.OnDestroySpell -= () => OnDestroySpell(shield);
            }
        }
    }
}