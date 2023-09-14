using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]



//скрипт падения предметов
public class FallingObject : MonoBehaviour
{
    public AudioClip[] impactSounds;
    private AudioSource audioSource;
    private bool isFalling = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
            PlayRandomImpactSound();
            isFalling = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isFalling = true; // Предмет снова начал падать или двигаться.
    }

    private void PlayRandomImpactSound()
    {
        int randomIndex = Random.Range(0, impactSounds.Length);
        audioSource.PlayOneShot(impactSounds[randomIndex]);
    }
}