using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.Sound
{
    public class PvpModeSound : ModeSound
    {
        [SerializeField] private AudioClip m_ModeIntroClip;
        [SerializeField] private AudioClip m_RedVictoryClip;
        [SerializeField] private AudioClip m_BlueVictoryClip;
        [SerializeField] private AudioClip m_NoVictoryClip;
        [SerializeField] private AudioClip m_StartRoundClip;
        [SerializeField] private AudioClip m_CompleteRoundClip;
        [SerializeField] private AudioClip m_StartModeClip;
        [SerializeField] private AudioClip m_CompleteModeClip;

        public void PlayIntro()
        {
            //Play(m_ModeIntroClip);
            photonView.RPC(nameof(RpcPlayIntro), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayIntro()
        {
            Play(m_ModeIntroClip);
        }
        
        public void PlayRedVictory()
        {
            //Play(m_RedVictoryClip);
            photonView.RPC(nameof(RpcPlayRedVictory), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayRedVictory()
        {
            Play(m_RedVictoryClip);
        }

        public void PlayBlueVictory()
        {
            //Play(m_BlueVictoryClip);
            photonView.RPC(nameof(RpcPlayBlueVictory), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayBlueVictory()
        {
            Play(m_BlueVictoryClip);
        }

        public void PlayNoVictory()
        {
            //Play(m_NoVictoryClip);
            photonView.RPC(nameof(RpcPlayNoVictory), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayNoVictory()
        {
            Play(m_NoVictoryClip);
        }

        public void PlayStartRound()
        {
            //Play(m_StartRoundClip);
            photonView.RPC(nameof(RpcPlayStartRound), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayStartRound()
        {
            Play(m_StartRoundClip);
        }

        public void PlayCompleteRound()
        {
            //Play(m_CompleteRoundClip);
            photonView.RPC(nameof(RpcPlayCompleteRound), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayCompleteRound()
        {
            Play(m_CompleteRoundClip);
        }
        
        public void PlayStartMode()
        {
            //Play(m_StartModeClip);
            photonView.RPC(nameof(RpcPlayStartMode), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayStartMode()
        {
            Play(m_StartModeClip);
        }

        public void PlayCompleteMode()
        {
            //Play(m_CompleteModeClip);
            photonView.RPC(nameof(RpcPlayCompleteMode), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayCompleteMode()
        {
            Play(m_CompleteModeClip);
        }
    }
}