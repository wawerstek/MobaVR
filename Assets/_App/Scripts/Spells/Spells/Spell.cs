using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class Spell : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected TeamItem m_TeamItem;
        [SerializeField] protected float m_DestroyLifeTime = 20.0f;

        protected TeamType m_TeamType;
        protected WizardPlayer m_Owner;
        protected bool m_IsDestroyed = false;

        public Action OnDestroySpell;
        public Action OnInitSpell;
        //public Action OnThrown;

        public TeamItem Team => m_TeamItem;
        public WizardPlayer Owner
        {
            get => m_Owner;
            set => m_Owner = value;
        }

        protected virtual void OnValidate()
        {
            if (m_TeamItem == null)
            {
                TryGetComponent(out m_TeamItem);
            }
        }

        #region Destroy Spell

        public virtual void DestroySpell()
        {
            photonView.RPC(nameof(RpcDestroy), RpcTarget.AllBuffered);
        }

        [PunRPC]
        protected virtual void RpcDestroy()
        {
            if (m_IsDestroyed)
            {
                return;
            }
            
            m_IsDestroyed = true;
            
            if (photonView.IsMine)
            {
                gameObject.SetActive(false);
                Invoke(nameof(DelayDestroy), 4f);
            }
            else
            {
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }

        protected void DelayDestroy()
        {
            gameObject.SetActive(false);
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        #endregion
    }
}