using System;
using Photon.Pun;
using UnityEngine;

namespace MobaVR.Sound
{
    public abstract class ModeSound : MonoBehaviourPun
    {
        protected AudioSource m_AudioSource;

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        public virtual void Play(AudioClip audioClip)
        {
            if (m_AudioSource == null || audioClip == null)
            {
                return;
            }
            
            m_AudioSource.PlayOneShot(audioClip);
        }
    }
}