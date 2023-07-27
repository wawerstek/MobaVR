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
        protected int m_IdOwner = -1;
        protected bool m_IsDestroyed = false;
        protected bool m_IsInit = false;

        public Action OnDestroySpell;
        public Action OnInitSpell;
        //public Action OnThrown;

        public TeamItem Team => m_TeamItem;
        public TeamType TeamType => m_TeamItem != null ? m_TeamItem.TeamType : TeamType.RED;
        public WizardPlayer Owner
        {
            get => m_Owner;
            set => m_Owner = value;
        }
        public int IdOwner => m_IdOwner;

        protected virtual void OnValidate()
        {
            if (m_TeamItem == null)
            {
                TryGetComponent(out m_TeamItem);
            }
        }

        #region Init

        public virtual void Init(WizardPlayer wizardPlayer, TeamType teamType)
        {
            OnInitSpell?.Invoke();

            m_Owner = wizardPlayer;
            m_TeamType = teamType;
            photonView.RPC(nameof(RpcInit), RpcTarget.AllBuffered, teamType, m_Owner.photonView.Owner.ActorNumber);
        }

        [PunRPC]
        public virtual void RpcInit(TeamType teamType, int idOwner)
        {
            if (!photonView.IsMine && idOwner >= 0)
            {
                WizardPlayer[] players = FindObjectsOfType<WizardPlayer>();
                foreach (WizardPlayer wizardPlayer in players)
                {
                    if (wizardPlayer.photonView.Owner.ActorNumber == idOwner)
                    {
                        m_Owner = wizardPlayer;
                        break;
                    }
                }
            }

            m_TeamType = teamType;
            if (m_TeamItem)
            {
                m_TeamItem.SetTeam(teamType);
            }
        }

        #endregion


        #region Destroy Spell

        public virtual void DestroySpell()
        {
            photonView.RPC(nameof(RpcDestroyThrowable), RpcTarget.AllBuffered);
        }

        [PunRPC]
        protected virtual void RpcDestroyThrowable()
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