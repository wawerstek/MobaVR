using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.Sound
{
    public class TowerModeSound : ModeSound
    {
        [SerializeField] private AudioClip m_ModeIntroClip;
        [SerializeField] private AudioClip m_WaveVictoryClip;
        [SerializeField] private AudioClip m_WaveLoseClip;
        [SerializeField] private AudioClip m_WaveReadyClip;
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
        
        public void PlayWaveVictory()
        {
            //Play(m_RedVictoryClip);
            photonView.RPC(nameof(RpcPlayWaveVictory), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayWaveVictory()
        {
            Play(m_WaveVictoryClip);
        }
        
        public void PlayWaveLose()
        {
            //Play(m_RedVictoryClip);
            photonView.RPC(nameof(RpcPlayWaveLose), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayWaveLose()
        {
            Play(m_WaveLoseClip);
        }
        
        public void PlayWaveReady()
        {
            //Play(m_RedVictoryClip);
            photonView.RPC(nameof(RpcPlayWaveReady), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayWaveReady()
        {
            Play(m_WaveReadyClip);
        }

        public void PlayVictory()
        {
            //Play(m_BlueVictoryClip);
            photonView.RPC(nameof(RpcPlayVictory), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayVictory()
        {
            Play(m_VictoryClip);
        }
        
        public void PlayLose()
        {
            //Play(m_BlueVictoryClip);
            photonView.RPC(nameof(RpcPlayLose), RpcTarget.All);
        }
        
        [PunRPC]
        public void RpcPlayLose()
        {
            Play(m_LoseClip);
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