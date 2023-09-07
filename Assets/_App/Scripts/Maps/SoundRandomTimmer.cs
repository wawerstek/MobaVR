using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomTimmer : MonoBehaviour
{
    public AudioClip[] sounds;  // Массив звуков, которые вы хотите воспроизводить
    public float initialDelay = 0f;  // Задержка перед первым воспроизведением звука
    public float timeInterval = 15f;  // Интервал времени между воспроизведением звука

    private AudioSource audioSource;
    private bool isPlayingSound = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Запустите метод PlayRandomSound с задержкой
        Invoke("PlayRandomSound", initialDelay);
    }

    private void PlayRandomSound()
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

    private void ResetIsPlayingSound()
    {
        isPlayingSound = false;
    }
}
