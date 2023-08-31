using System.Collections;
using UnityEngine;

namespace MobaVR
{
    public class RagdollSkinOnDie : OnDieBehaviour
    {
        private float m_AnimDelay = 0.2f;
        
        public override void Die()
        {
            m_Skin.gameObject.SetActive(true);
            
            m_Animator.enabled = true;
            m_EyeAnimationHandler.enabled = false;
            m_Vrik.enabled = false;

            if (m_AnimDelay > 0)
            {
                m_Skin.StartCoroutine(WaitAndActivateRagDolls());
            }
            else
            {
                ActivateRagDoll();
            }
        }

        public override void Reborn()
        {
            DeactivateRagDoll();
        }

        private IEnumerator WaitAndActivateRagDolls()
        {
            yield return new WaitForSeconds(m_AnimDelay);
            ActivateRagDoll();
        }
        
        private void ActivateRagDoll()
        {
            SetRagDoll(true);
        }

        private void DeactivateRagDoll()
        {
            SetRagDoll(false);
        }
        
        private void SetRagDoll(bool useRagDoll)
        {
            if (useRagDoll)
            {
                if (m_HideTimeout > 0)
                {
                    m_Skin.StartCoroutine(WaitAndHideSkin());
                }
                else
                {
                    HideSkin();
                }
            }
            else
            {
                m_Skin.StopAllCoroutines();
                /*
                if (m_HideTimeout > 0)
                {
                    m_Skin.StopCoroutine(WaitAndHideSkin());
                }
                */
            }

            /*
            if (m_AnimDelay > 0)
            {
                m_Skin.StopCoroutine(WaitAndActivateRagDolls());
            }
            */
            
            m_Skin.gameObject.SetActive(true);

            m_EyeAnimationHandler.enabled = !useRagDoll;
            m_Vrik.enabled = !useRagDoll;
            m_Animator.enabled = !useRagDoll;
            
            foreach (Collider childCollider in m_ChildColliders)
            {
                childCollider.isTrigger = !useRagDoll;
            }

            foreach (Rigidbody childRigidbody in m_ChildRigidbodies)
            {
                childRigidbody.ResetInertiaTensor();
                childRigidbody.velocity = Vector3.zero;
                childRigidbody.angularVelocity = Vector3.zero;
                
                childRigidbody.isKinematic = !useRagDoll;
                childRigidbody.useGravity = useRagDoll;
                
                childRigidbody.ResetInertiaTensor();
                childRigidbody.velocity = Vector3.zero;
                childRigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}