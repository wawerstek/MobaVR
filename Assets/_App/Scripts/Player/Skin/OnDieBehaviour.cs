using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace MobaVR
{
    public abstract class OnDieBehaviour : IOnDieBehaviour
    {
        protected Skin m_Skin;
        
        protected Transform m_Root;
        protected WizardPlayer m_Wizard;
        protected Animator m_Animator;
        protected EyeAnimationHandler m_EyeAnimationHandler;
        protected VRIK m_Vrik;
        
        protected float m_HideTimeout = 10f;
        
        protected List<HitCollider> m_HitColliders = new();
        protected List<Rigidbody> m_ChildRigidbodies = new();
        protected List<Collider> m_ChildColliders = new();

        protected bool m_IsInit = false;

        public bool IsInit => m_IsInit;
        
        protected IEnumerator WaitAndHideSkin()
        {
            yield return new WaitForSeconds(m_HideTimeout);
            HideSkin();
        }

        protected void HideSkin()
        {
            if (!m_Wizard.IsLife)
            {
                m_Skin.gameObject.SetActive(false);
            }
        }
        
        public virtual void Init(Skin skin, float hideTimout)
        {
            m_Skin = skin;
            m_Root = m_Skin.Armature;
            m_HideTimeout = hideTimout;

            m_IsInit = true;
            
            //m_Skin.gameObject.SetActive(true);
            
            m_Wizard = m_Skin.GetComponentInParent<WizardPlayer>();
            m_Vrik = m_Skin.GetComponent<VRIK>();
            m_Animator = m_Skin.GetComponent<Animator>();
            m_EyeAnimationHandler = m_Skin.GetComponent<EyeAnimationHandler>();

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
            
            foreach (Rigidbody childRigidbody in m_ChildRigidbodies)
            {
                childRigidbody.isKinematic = true;
                childRigidbody.useGravity = false;
            }

            foreach (Collider childCollider in m_ChildColliders)
            {
                childCollider.isTrigger = true;
            }
        }

        public abstract void Die();
        public abstract void Reborn();
    }
}