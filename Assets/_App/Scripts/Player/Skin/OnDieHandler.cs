using System;
using System.Web.UI.WebControls;
using UnityEngine;

namespace MobaVR
{
    public class OnDieHandler : MonoBehaviour
    {
        [SerializeField] private bool m_IsDisableOnEnable = true;
        [SerializeField] private OnDieBehaviourType m_OnDieType = OnDieBehaviourType.NONE;
        [SerializeField] private float m_HideDelay = 10f;

        private Skin m_Skin;
        private SkinCollection m_SkinCollection;
        private WizardPlayer m_Wizard;
        private OnDieBehaviour m_OnDieBehaviour;

        /*
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
        */

        //private void OnDisable()

        private void OnDisable()
        {
            //StopAllCoroutines();
        }

        private void OnDestroy()
        {
            if (m_Wizard != null)
            {
                m_Wizard.OnDie -= OnDie;
                m_Wizard.OnReborn -= OnReborn;
            }
        }

        private void OnEnable()
        {
            if (m_OnDieBehaviour is { IsInit: false })
            {
                m_OnDieBehaviour.Init(m_Skin, m_HideDelay);
            }
        }

        public OnDieBehaviourType OnDieType
        {
            get => m_OnDieType;
            set
            {
                m_OnDieType = value;
                switch (m_OnDieType)
                {
                    case OnDieBehaviourType.NONE:
                        m_OnDieBehaviour = new EmptySkinOnDie();
                        break;
                    case OnDieBehaviourType.RAGDOLL:
                        m_OnDieBehaviour = new RagdollSkinOnDie();
                        break;
                    case OnDieBehaviourType.ANIM:
                        m_OnDieBehaviour = new AnimationSkinOnDie();
                        break;
                }

                if (IsActiveSkin())
                {
                    m_OnDieBehaviour.Init(m_Skin, m_HideDelay);
                }
            }
        }

        private void Awake()
        {
            m_Skin = GetComponent<Skin>();
            m_SkinCollection = GetComponentInParent<SkinCollection>();
            m_Wizard = GetComponentInParent<WizardPlayer>();

            /// Подписываемся в Awake, так как сейчас игровой объект может отключаться родителем
            /// Тяжело следить за подписками
            if (m_Wizard != null)
            {
                m_Wizard.OnDie += OnDie;
                m_Wizard.OnReborn += OnReborn;
            }

            OnDieType = m_OnDieType;
        }

        [ContextMenu("OnDie")]
        private void OnDie()
        {
            if (!IsActiveSkin())
            {
                return;
            }

            if (m_OnDieBehaviour != null)
            {
                m_OnDieBehaviour.Die();
            }
        }

        [ContextMenu("OnReborn")]
        private void OnReborn()
        {
            if (!IsActiveSkin())
            {
                return;
            }

            if (m_OnDieBehaviour != null)
            {
                m_OnDieBehaviour.Reborn();
            }
        }

        [ContextMenu("SetEmptyType")]
        public void SetEmptyType()
        {
            OnDieType = OnDieBehaviourType.NONE;
        }

        [ContextMenu("SetAnimType")]
        public void SetAnimType()
        {
            OnDieType = OnDieBehaviourType.ANIM;
        }

        [ContextMenu("SetRagDollType")]
        public void SetRagDollType()
        {
            OnDieType = OnDieBehaviourType.RAGDOLL;
        }

        private bool IsActiveSkin()
        {
            if (m_SkinCollection == null || m_Skin == null)
            {
                return false;
            }

            return m_SkinCollection.AliveActiveSkin == m_Skin;
        }
    }
}