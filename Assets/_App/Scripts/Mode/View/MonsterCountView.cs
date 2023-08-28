using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MobaVR
{
    public class MonsterCountView : BaseTextView
    {
        [SerializeField] private TextMeshPro m_TextView;
        [SerializeField] private PhotonView m_PhotonView;

        public override void Show()
        {
            gameObject.SetActive(true);
            m_TextView.enabled = true;
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            m_TextView.enabled = false;
        }

        public override void UpdateText(string message)
        {
            m_PhotonView.RPC(nameof(RpcUpdateText), RpcTarget.All, message);
        }

        [PunRPC]
        private void RpcUpdateText(string message)
        {
            m_TextView.text = message;
        }
    }
}