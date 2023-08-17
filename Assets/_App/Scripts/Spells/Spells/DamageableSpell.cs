using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace MobaVR
{
    public class DamageableSpell : MonoBehaviourPun
    {
        [SerializeField] private float m_Damage = 10f;
        private Spell m_Spell;

        public UnityEvent<Collider> OnSpellTriggerEnter;
        public UnityEvent<Collision> OnSpellCollisionEnter;

        public UnityEvent<WizardPlayer> OnPlayerTriggerEnter;
        public UnityEvent<Shield> OnShieldTriggerEnter;
        public UnityEvent<BigShield> OnBigShieldTriggerEnter;
        public UnityEvent<IExploding> OnHittableTriggerEnter;

        private void Awake()
        {
            TryGetComponent(out m_Spell);
        }

        #region Trigger

        private void TriggerPlayer(WizardPlayer wizardPlayer)
        {
            if (wizardPlayer == m_Spell.Owner)
            {
                return;
            }

            if (wizardPlayer.photonView.Owner.ActorNumber == photonView.Owner.ActorNumber)
            {
                return;
            }

            if (wizardPlayer.TeamType == m_Spell.TeamType)
            {
                return;
            }

            if (photonView.IsMine)
            {
                HitData hitData = new HitData()
                {
                    Amount = m_Damage
                };
                //wizardPlayer.Hit(m_Damage);
                //TODO: DAMAGE
                //wizardPlayer.Hit(hitData);
            }

            OnPlayerTriggerEnter?.Invoke(wizardPlayer);
        }

        private void TriggerShield(Shield shield)
        {
            //TODO:
            //Умения не должны проходить через щит
            if (shield.TeamType == m_Spell.TeamType)
            {
                return;
            }

            if (photonView.IsMine)
            {
                //TODO: DAMAGE
                //shield.Hit(m_Damage);
            }

            OnShieldTriggerEnter?.Invoke(shield);
        }

        private void TriggerBigShield(BigShield shield)
        {
            if (shield.TeamType == m_Spell.TeamType)
            {
                return;
            }

            OnBigShieldTriggerEnter?.Invoke(shield);
        }

        private void TriggerEnemy(IExploding exploding)
        {
            if (photonView.IsMine)
            {
                //hit.RpcHit(m_Damage);
            }

            OnHittableTriggerEnter?.Invoke(exploding);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out PhotonView colliderPhoton)
                && colliderPhoton == photonView)
            {
                return;
            }

            if (other.CompareTag("RemotePlayer")
                && other.transform.TryGetComponent(out WizardPlayer wizardPlayer))
            {
                TriggerPlayer(wizardPlayer);
                OnSpellTriggerEnter?.Invoke(other);
                return;
            }

            if (other.CompareTag("LifeCollider")
                && other.transform.TryGetComponent(out HitCollider damagePlayer))
            {
                //TriggerPlayer(damagePlayer.WizardPlayer);
                OnSpellTriggerEnter?.Invoke(other);
                return;
            }

            if (other.CompareTag("Item"))
            {
                Shield shield = other.GetComponentInParent<Shield>();
                if (shield != null)
                {
                    TriggerShield(shield);
                    OnSpellTriggerEnter?.Invoke(other);
                    return;
                }
            }

            if (other.CompareTag("Item"))
            {
                BigShield shield = other.GetComponentInParent<BigShield>();
                if (shield != null)
                {
                    TriggerBigShield(shield);
                    OnSpellTriggerEnter?.Invoke(other);
                    return;
                }
            }

            if (other.CompareTag("Enemy")
                && other.TryGetComponent(out IExploding hitEnemy))
            {
                TriggerEnemy(hitEnemy);
                OnSpellTriggerEnter?.Invoke(other);
                return;
            }
        }

        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine)
            {
                //return;
            }

            if (!collision.transform.CompareTag("RemotePlayer")
                && !collision.transform.CompareTag("Player"))
            {
                OnSpellCollisionEnter?.Invoke(collision);
            }
        }
    }
}