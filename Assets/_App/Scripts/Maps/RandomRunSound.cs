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
        if (songs.Length <= 0)
        {
            return;
        }
        
        // ������������� ���� �� ������� �� �������� �������
        AudioClip soundToPlay = songs[currentSongIndex];
        audioSource.clip = soundToPlay;
        audioSource.Play();

        // ��������� ������� ������ ����� ��� ���������� ���������������
        currentSongIndex++;

        // ���� ������� ������ ��������� ������ �������, ���������� ��� � ���������� ��������
        if (currentSongIndex >= songs.Length)
        {
            currentSongIndex = 0;
        }
    }

    private void Update()
    {
        // ���������, ����������� �� ������� �����, � ��������� ���������
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }
}
