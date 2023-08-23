using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInfoClass : MonoBehaviour
{
    //[Header("MenuClass")]
    //public AudioClip Defender; // Звук 1
    //public AudioClip Ranger; // Звук 2
    //public AudioClip Wizard; // Звук 3

    //[Header("MenuName")]
    //public AudioClip Name; // Звук 1

    //[Header("MenuHands")]
    //public AudioClip Hands; // Звук 1    
    
    //[Header("MenuGoTir")]
    //public AudioClip GoTir; // Звук 1

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) // Если на объекте нет AudioSource, добавляем его
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

   
    public void PlaySpecificSound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }


    public void StopAllSounds()
    {
                audioSource.Stop();

    }


}
