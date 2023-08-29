using System;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace MobaVR
{
    public class SkinRagdoll : MonoBehaviour
    {
        [SerializeField] private Transform m_Root;
        [SerializeField] private bool m_IsDisableOnEnable = true;
        [SerializeField] private float m_HideTimeout = 10f;

        private WizardPlayer m_Wizard;
        private Animator m_Animator;
        private EyeAnimationHandler m_EyeAnimationHandler;
        private VRIK m_Vrik;
        private List<HitCollider> m_HitColliders = new();
        private List<Rigidbody> m_ChildRigidbodies = new();
        private List<Collider> m_ChildColliders = new();


        private bool m_IsDie = false;

        private void OnEnable()
        {
            if (m_Wizard != null)
            {
                m_Wizard.OnDie += OnDie;
                m_Wizard.OnReborn += OnReborn;
            }

            //TODO: Костыль
            if (m_Wizard != null && m_IsDisableOnEnable && m_Wizard.IsLife)
            {
                OnReborn();
            }
        }

        private void OnDisable()
        {
            if (m_Wizard != null)
            {
                m_Wizard.OnDie -= OnDie;
                m_Wizard.OnReborn -= OnReborn;
            }
        }

        private void Awake()
        {
            m_Wizard = GetComponentInParent<WizardPlayer>();
            m_Vrik = GetComponent<VRIK>();
            m_Animator = GetComponent<Animator>();
            m_EyeAnimationHandler = GetComponent<EyeAnimationHandler>();

            m_HitColliders.AddRange(m_Root.GetComponentsInChildren<HitCollider>());
            foreach (HitCollider hitCollider in m_HitColliders)
            {
                if (hitCollider.TryGetComponent(out Rigidbody childRigidbody))
                {
                    m_ChildRigidbodies.Add(childRigidbody);
                }

                if (hitCollider.TryGetComponent(out Collider childCollider))
                {
                    m_ChildColliders.Add(childCollider);
                }
            }
        }

        private void Hide()
        {
            if (!m_Wizard.IsLife)
            {
                gameObject.SetActive(false);
            }
        }

        [ContextMenu("OnDie")]
        private void OnDie()
        {
            m_IsDie = true;
            gameObject.SetActive(true);

            Invoke(nameof(Hide), m_HideTimeout);
            m_Animator.enabled = false;
            //m_Vrik.enabled = false;
            //m_Animator.SetTrigger("Die");
            Invoke(nameof(ActivateRagDoll), 0.1f);
            //etRagDoll(true);
        }

        [ContextMenu("OnReborn")]
        private void OnReborn()
        {
            CancelInvoke(nameof(Hide));
            m_Animator.SetTrigger("Reborn");
            m_IsDie = false;
            gameObject.SetActive(true);
            SetRagDoll(false);
        }

        private void ActivateRagDoll()
        {
            SetRagDoll(true);
        }

        private void SetRagDoll(bool useRagDoll)
        {
            //m_BodyCollider.isTrigger = useRagDoll;
            gameObject.SetActive(true);

            m_EyeAnimationHandler.enabled = !useRagDoll;
            m_Vrik.enabled = !useRagDoll;
            m_Animator.enabled = !useRagDoll;

            foreach (Rigidbody childRigidbody in m_ChildRigidbodies)
            {
                childRigidbody.isKinematic = !useRagDoll;
                childRigidbody.useGravity = useRagDoll;
            }

            foreach (Collider childCollider in m_ChildColliders)
            {
                //childCollider.enabled = useRagDoll;
                childCollider.isTrigger = !useRagDoll;
            }
        }
    }
}