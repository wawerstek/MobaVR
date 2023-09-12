using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotRandomSound : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audioSource;
    private bool isPlayingSound = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (!isPlayingSound)
        {
            if (sounds.Length > 0)
            {
                audioSource.clip = sounds[Random.Range(0, sounds.Length)];
                audioSource.Play();
                isPlayingSound = true;

                Invoke(nameof(Reset), audioSource.clip.length);
            }
        }
    }

    public void Stop()
    {
        CancelInvoke(nameof(Reset));
        isPlayingSound = true;
        audioSource.Stop();
    }

    private void Reset()
    {
        isPlayingSound = false;
    }
}