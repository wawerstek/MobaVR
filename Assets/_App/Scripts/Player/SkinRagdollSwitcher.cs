using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{
    public class SkinRagdollSwitcher : MonoBehaviour
    {
        private SkinCollection m_SkinCollection;

        private void Awake()
        {
            m_SkinCollection = GetComponent<SkinCollection>();
        }

        [ContextMenu("ActivateRagdoll")]
        public void ActivateRagdoll()
        {
            SetRagdoll(true);
        }

        [ContextMenu("DeactivateRagdoll")]
        public void DeactivateRagdoll()
        {
            SetRagdoll(false);
        }
        
        [ContextMenu("SetEmptyType")]
        public void SetEmptyType()
        {
            SetDieBehaviour(OnDieBehaviourType.NONE);
        }
        
        [ContextMenu("SetAnimType")]
        public void SetAnimType()
        {
            SetDieBehaviour(OnDieBehaviourType.ANIM);
        }
        
        [ContextMenu("SetRagDollType")]
        public void SetRagDollType()
        {
            SetDieBehaviour(OnDieBehaviourType.RAGDOLL);
        }

        public void SetDieBehaviour(OnDieBehaviourType dieBehaviourType)
        {
            if (m_SkinCollection == null)
            {
                return;
            }

            foreach (Skin skin in m_SkinCollection.AliveSkins)
            {
                if (skin.TryGetComponent(out OnDieHandler dieHandler))
                {
                    dieHandler.OnDieType = dieBehaviourType;
                }
            }
        }
        
        public void SetRagdoll(bool useRagdoll)
        {
            if (m_SkinCollection == null)
            {
                return;
            }

            foreach (Skin skin in m_SkinCollection.AliveSkins)
            {
                skin.SetInactiveOnDie = !useRagdoll;

                if (skin.TryGetComponent(out SkinRagdoll skinRagdoll))
                {
                    skinRagdoll.enabled = useRagdoll;
                }
                else
                {
                    skin.SetInactiveOnDie = false;
                }
            }
        }
    }
}