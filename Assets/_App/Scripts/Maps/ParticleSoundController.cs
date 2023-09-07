using UnityEngine;

public class ParticleSoundController : MonoBehaviour
{
    public AudioClip[] sounds;  // Массив звуков, которые соответствуют разным значениям параметра particles
    private AudioSource audioSource;
    private ParticleSystem particleSystem;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Получаем текущее значение параметра particles из партикл-системы
        int particlesValue = Mathf.RoundToInt(particleSystem.main.startSize.constant);

        // Проверяем, находится ли значение в пределах допустимого диапазона индексов массива звуков
        if (particlesValue >= 0 && particlesValue < sounds.Length)
        {
            // Воспроизводим соответствующий звук
            audioSource.clip = sounds[particlesValue];
            audioSource.Play();
        }
    }
}