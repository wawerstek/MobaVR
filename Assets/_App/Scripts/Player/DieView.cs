using MobaVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieView : MonoBehaviour
{
    [SerializeField] private WizardPlayer wizardPlayer;

    public GameObject TextDie;


    public AudioClip[] sounds;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        wizardPlayer.OnDie += OnDie;
        wizardPlayer.OnReborn += OnReborn;
    }

    private void OnDisable()
    {
        wizardPlayer.OnDie -= OnDie;
        wizardPlayer.OnReborn -= OnReborn;
    }

    private void OnDie()
    {
        PlayRandomSound();
        TextDie.SetActive(true);
    }

    private void OnReborn()
    {
        TextDie.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void PlayRandomSound()
    {
        if (sounds.Length > 0)
        {
            // Генерируем случайный индекс для выбора случайного звука из массива
            int randomIndex = Random.Range(0, sounds.Length);

            // Воспроизводим звук, соответствующий случайному индексу
            AudioClip soundToPlay = sounds[randomIndex];
            audioSource.PlayOneShot(soundToPlay);
        }

    }

}
