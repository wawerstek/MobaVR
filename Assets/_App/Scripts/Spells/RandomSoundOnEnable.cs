using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundOnEnable : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        if (sounds.Length == 0) return; // проверка на наличие звуков в массиве

        int randomIndex = Random.Range(0, sounds.Length);
        
        audioSource.clip = sounds[randomIndex];
        audioSource.Play();
    }
}