using System.Collections;
using UnityEngine;
using Photon.Pun;

public class perdeg_grusha : MonoBehaviourPunCallbacks
{
    public AudioClip[] perdeg;
    private int currentClipIndex = 0;

    private void OnEnable()
    {
        StartCoroutine(PlayPerdegSounds());
    }
    
    private void Start()
    {
        
    }

    IEnumerator PlayPerdegSounds()
    {
        while (currentClipIndex < perdeg.Length)
        {
            yield return new WaitForSeconds(7f);
            photonView.RPC("PlaySound", RpcTarget.All, currentClipIndex);
            currentClipIndex++;
        }
        photonView.RPC("DestroyObject", RpcTarget.All);
    }

    [PunRPC]
    void PlaySound(int index)
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source != null && index < perdeg.Length)
        {
            source.clip = perdeg[index];
            source.Play();
        }
    }

    [PunRPC]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}