using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.Sound
{
    public class PveLichModeSound : ModeSound
    {
        [SerializeField] private AudioClip m_ModeIntroClip;
        [SerializeField] private AudioClip m_VictoryClip;
        [SerializeField] private AudioClip m_LoseClip;
        [SerializeField] private AudioClip m_StartModeClip;
        [SerializeField] private AudioClip m_CompleteModeClip;

        public void PlayIntro()
        {
            photonView.RPC(nameof(RpcPlayIntro), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayIntro()
        {
            Play(m_ModeIntroClip);
        }
        
        public void PlayVictory()
        {
            photonView.RPC(nameof(RpcPlayVictory), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayVictory()
        {
            Play(m_VictoryClip);
        }
        
        public void PlayLose()
        {
            photonView.RPC(nameof(RpcPlayLose), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayLose()
        {
            Play(m_LoseClip);
        }

        public void PlayStartMode()
        {
            photonView.RPC(nameof(RpcPlayStartMode), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayStartMode()
        {
            Play(m_StartModeClip);
        }

        public void PlayCompleteMode()
        {
            photonView.RPC(nameof(RpcPlayCompleteMode), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayCompleteMode()
        {
            Play(m_CompleteModeClip);
        }
    }
}