using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRunSound : MonoBehaviour
{

    public AudioClip[] songs;
    private AudioSource audioSource;
    private int currentSongIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (songs.Length > 0)
        {
            PlayNextSong();
        }
    }

    private void PlayNextSong()
    {
        // ¬оспроизводим звук из массива по текущему индексу
        AudioClip soundToPlay = songs[currentSongIndex];
        audioSource.clip = soundToPlay;
        audioSource.Play();

        // ќбновл€ем текущий индекс песни дл€ следующего воспроизведени€
        currentSongIndex++;

        // ≈сли текущий индекс превышает размер массива, сбрасываем его к начальному значению
        if (currentSongIndex >= songs.Length)
        {
            currentSongIndex = 0;
        }
    }

    private void Update()
    {
        // ѕровер€ем, закончилась ли текуща€ песн€, и запускаем следующую
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }
}
