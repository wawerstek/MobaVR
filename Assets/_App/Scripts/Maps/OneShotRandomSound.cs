using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MobaVR.Utils;
using UnityEngine;

public class OneShotRandomSound : MonoBehaviour
{
    public AudioClip[] sounds;
    public bool canInterrupt = false;
    public float maxFadeInVolume = 0.5f;
    public float minFadeOutVolume = 0f;
    public float fadeDuration = 1f;

    private AudioSource audioSource;
    private bool isPlayingSound = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (audioSource == null)
        {
            return;
        }

        if (!isPlayingSound || audioSource.loop || canInterrupt)
        {
            if (sounds.Length > 0)
            {
                audioSource.clip = sounds[Random.Range(0, sounds.Length)];
                audioSource.Play();

                if (!audioSource.loop)
                {
                    isPlayingSound = true;
                    CancelInvoke(nameof(Reset));
                    Invoke(nameof(Reset), audioSource.clip.length);
                }
            }
        }
    }

    public void Stop()
    {
        if (audioSource == null)
        {
            return;
        }

        CancelInvoke(nameof(Reset));
        isPlayingSound = true;
        audioSource.Stop();
    }

    public void FadeIn()
    {
        if (audioSource == null)
        {
            return;
        }

        if (sounds.Length > 0)
        {
            audioSource.clip = sounds[Random.Range(0, sounds.Length)];
            audioSource.volume = 0f;
            audioSource.DOFade(maxFadeInVolume, fadeDuration)
                       .OnStart(audioSource.Play);
        }
    }

    public void FadeOut()
    {
        if (audioSource == null)
        {
            return;
        }

        audioSource.DOFade(minFadeOutVolume, fadeDuration)
                   .OnComplete(audioSource.Stop);
    }

    private void Reset()
    {
        isPlayingSound = false;
    }
}