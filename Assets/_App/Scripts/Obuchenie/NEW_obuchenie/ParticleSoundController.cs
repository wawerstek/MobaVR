using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class ParticleSoundController : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private AudioSource audioSource;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (particleSystem.isPlaying && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!particleSystem.isPlaying && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}