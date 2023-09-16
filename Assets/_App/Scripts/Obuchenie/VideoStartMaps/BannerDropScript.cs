using System.Collections;
using UnityEngine;
using Photon.Pun;
using MobaVR;
public class BannerDropScript : MonoBehaviourPunCallbacks
{
    public Transform topPoint;
    public Transform bottomPoint;
    public GameObject shield;
    public AudioClip chainSound;
    public GameObject[] bannerObjects;
    public bool runZvuk;

    private AudioSource audioSource;
    [SerializeField] private ScenesEnvironment m_SceneEnvironment;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (m_SceneEnvironment == null)
        {
            m_SceneEnvironment = FindObjectOfType<ScenesEnvironment>();
        }

        runZvuk = false;
    }

    [PunRPC]
    public void LowerShield()
    {
        if(runZvuk)
        {
            StartCoroutine(RaiseBanner());
        }
        else
        {
            StartCoroutine(LowerAndActivateBanner());
        }
    }

    private IEnumerator LowerAndActivateBanner()
    {
        float journeyLength = Vector3.Distance(topPoint.position, bottomPoint.position);
        float startTime = Time.time;
        float journeyTime = 6.0f;

        audioSource.PlayOneShot(chainSound);

        while (true)
        {
            float distCovered = (Time.time - startTime) * (journeyLength / journeyTime);
            float fracJourney = distCovered / journeyLength;
            shield.transform.position = Vector3.Lerp(topPoint.position, bottomPoint.position, fracJourney);

            if (fracJourney >= 1) break;

            yield return null;
        }

        ActivateBanner();
    }

    private IEnumerator RaiseBanner()
    {
        float journeyLength = Vector3.Distance(bottomPoint.position, topPoint.position);
        float startTime = Time.time;
        float journeyTime = 6.0f;
        audioSource.PlayOneShot(chainSound);
        while (true)
        {
            float distCovered = (Time.time - startTime) * (journeyLength / journeyTime);
            float fracJourney = distCovered / journeyLength;
            shield.transform.position = Vector3.Lerp(bottomPoint.position, topPoint.position, fracJourney);

            if (fracJourney >= 1) break;

            yield return null;
        }

        LoadNewScene();
    }

    private void ActivateBanner()
    {
        bannerObjects[0].SetActive(true);
        AudioSource bannerAudio = bannerObjects[0].GetComponent<AudioSource>();
        if (bannerAudio != null)
        {
            bannerAudio.Play();
        }
        StartCoroutine(LoadNewSceneAfterSound(bannerAudio.clip.length));
    }

    private IEnumerator LoadNewSceneAfterSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        runZvuk = true;
        TriggerLowerShield();
    }

    private void LoadNewScene()
    {
        m_SceneEnvironment.ShowSkyLandWithPropMap();
    }

    public void TriggerLowerShield()
    {
        photonView.RPC("LowerShield", RpcTarget.All);
    }
}
