using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomTimmer : MonoBehaviour
{
    public AudioClip[] sounds; // Массив звуков, которые вы хотите воспроизводить
    public float initialDelay = 0f; // Задержка перед первым воспроизведением звука
    public float timeInterval = 15f; // Интервал времени между воспроизведением звука
    public bool isPlayOnStart = true;
    public bool useRandom = true;

    private AudioSource audioSource;
    private bool isPlayingSound = false;
    private int currentPosition = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Запустите метод PlayRandomSound с задержкой
        if (isPlayOnStart)
        {
            Invoke("PlayRandomSound", initialDelay);
        }
    }

    public void PlayRandomSound()
    {
        if (!isPlayingSound)
        {
            if (sounds.Length > 0)
            {
                int randomIndex = Random.Range(0, sounds.Length);
                audioSource.clip = sounds[randomIndex];
                audioSource.Play();
                isPlayingSound = true;

                // Запустите таймер для проверки, завершилось ли воспроизведение звука
                Invoke("ResetIsPlayingSound", audioSource.clip.length);
            }
        }

        // Запустите таймер для воспроизведения следующего звука с интервалом
        Invoke("PlayRandomSound", timeInterval);
    }

    public void PlayOneSound()
    {
        if (!isPlayingSound)
        {
            if (sounds.Length > 0)
            {
                int index;
                if (useRandom)
                {
                    index = Random.Range(0, sounds.Length);
                }
                else
                {
                    index = currentPosition % sounds.Length;
                    currentPosition++;
                }

                audioSource.clip = sounds[index];
                audioSource.Play();
                isPlayingSound = true;

                Invoke("ResetIsPlayingSound", audioSource.clip.length);
            }
        }
    }

    private void ResetIsPlayingSound()
    {
        isPlayingSound = false;
    }
}