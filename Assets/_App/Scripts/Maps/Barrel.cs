using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public AudioClip[] soundClips; // Звуковые клипы
    private AudioSource audioSource;
    private bool isPlayingSound = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSoundWithDelay());
    }

    private IEnumerator PlayRandomSoundWithDelay()
    {
        while (true)
        {
            if (!isPlayingSound)
            {
                int randomIndex = Random.Range(0, soundClips.Length);
                AudioClip selectedClip = soundClips[randomIndex];

                float randomDelay = Random.Range(5.0f, 15.0f); // Время в секундах (от 5 до 15 секунд)
                    //Debug.Log($"Следующий звук будет воспроизведен через {randomDelay} секунд");

                yield return new WaitForSeconds(randomDelay);

                isPlayingSound = true;
                audioSource.clip = selectedClip;
                audioSource.Play();
            }
            yield return null;
        }
    }

    private void Update()
    {
        if (isPlayingSound && !audioSource.isPlaying)
        {
            isPlayingSound = false;
        }
    }
}