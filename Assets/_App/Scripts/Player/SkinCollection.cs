using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class SkinCollection : TeamItem
    {
        [Header("Wizard")]
        [SerializeField] private PlayerVR m_PlayerVR;
        [SerializeField] private PhotonView m_PhotonView;
        
        [Header("Face")]
        [SerializeField] private GameObject m_TopKnot;
        [SerializeField] private GameObject m_Beard;
        [SerializeField] private GameObject m_Sideburns;
        [SerializeField] private GameObject m_Neck;
        [SerializeField] private GameObject m_Hair;
        
        [Header("Skins")]
        [SerializeField] private List<Skin> m_Skins = new();

        private Skin m_ActiveSkin = null;
        private int m_SkinPosition = 0;

        public List<Skin> Skins => m_Skins;

        private void OnValidate()
        {
            if (m_PlayerVR == null)
            {
                m_PlayerVR = GetComponentInParent<PlayerVR>();
            }
            
            if (m_Skins.Count == 0)
            {
                m_Skins.AddRange(GetComponentsInChildren<Skin>(true));
            }

            if (m_PhotonView == null)
            {
                m_PhotonView = GetComponent<PhotonView>();
            }
        }

        private void Awake()
        {
            Clear();
            //SetSkin(0);
            RpcSetSkin(0);
        }

        private void Clear()
        {
            foreach (Skin skin in m_Skins)
            {
                skin.DeactivateSkin();
            }
        }
        
        public void SetFace()
        {
            m_TopKnot.SetActive(m_SkinPosition == 0);
            m_Sideburns.SetActive(m_SkinPosition == 1);
            m_Neck.SetActive(m_SkinPosition == 1 || m_SkinPosition == 2 || m_SkinPosition == 4 || m_SkinPosition == 5);
            m_Hair.SetActive(m_SkinPosition == 0 || m_SkinPosition == 3);
        }

        [ContextMenu("SetNextSkin")]
        public void SetNextSkin()
        {
            m_SkinPosition++;
            m_SkinPosition %= m_Skins.Count;
            SetSkin(m_SkinPosition);
        }

        [ContextMenu("SetPrevSkin")]
        public void SetPrevSkin()
        {
            m_SkinPosition--;
            m_SkinPosition %= m_Skins.Count;
            SetSkin(m_SkinPosition);
        }

        public void SetSkin(int position)
        {
            if (m_PhotonView != null)
            {
                m_PhotonView.RPC(nameof(RpcSetSkin), RpcTarget.AllBuffered, position);
            }
        }

        [PunRPC]
        private void RpcSetSkin(int position)
        {
            if (m_ActiveSkin != null)
            {
                m_ActiveSkin.DeactivateSkin();
            }
            
            m_SkinPosition = Math.Clamp(position, 0, m_Skins.Count - 1);
            m_ActiveSkin = m_Skins[m_SkinPosition];
            
            TeamType teamType = m_PlayerVR != null ? m_PlayerVR.TeamType : TeamType.RED;
            m_ActiveSkin.ActivateSkin(teamType);
            
            SetFace();
        }

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);
            if (m_ActiveSkin != null)
            {
                m_ActiveSkin.SetTeam(teamType);
            }
        }
    }
}