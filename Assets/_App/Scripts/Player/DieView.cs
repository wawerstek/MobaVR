using MobaVR;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DieView : MonoBehaviour
{
    [SerializeField] private WizardPlayer wizardPlayer;

    public GameObject DiePanel;
    public GameObject DieInfoPanel;
    public TextMeshProUGUI KillerName;


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
        DiePanel.SetActive(true);
    }

    private void OnReborn()
    {
        DiePanel.SetActive(false);
        DieInfoPanel.SetActive(false);
    }

    public void SetDieInfo(string nickname)
    {
        DieInfoPanel.SetActive(true);
        KillerName.text = nickname;
    }

    public void PlayRandomSound()
    {
        if (sounds.Length > 0)
        {
            // ���������� ��������� ������ ��� ������ ���������� ����� �� �������
            int randomIndex = Random.Range(0, sounds.Length);

            // ������������� ����, ��������������� ���������� �������
            AudioClip soundToPlay = sounds[randomIndex];
            audioSource.PlayOneShot(soundToPlay);
        }

    }

}
