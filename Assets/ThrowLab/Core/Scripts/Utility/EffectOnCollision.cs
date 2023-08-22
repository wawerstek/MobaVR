using System.Collections;
using UnityEngine;
using Photon.Pun;
public class EffectOnCollision : MonoBehaviour {

    public ParticleSystem _particleEffect;
    public AudioSource _audio;
    public MeshRenderer _flashRenderer;
    private bool MyObject = false;

    private void OnCollisionEnter(Collision collision)
    {
        
        // Получаем PhotonView компонент объекта, вошедшего в коллайдер
        PhotonView otherPhotonView = collision.gameObject.GetComponent<PhotonView>();

        // Проверяем, является ли объект локальным
        if (otherPhotonView != null && otherPhotonView.IsMine)
        {
            MyObject = true;
        }
        
        StartCoroutine(Effect());    
    }

    private void OnTriggerEnter(Collider other)
    {
        // Получаем PhotonView компонент объекта, вошедшего в коллайдер
        PhotonView otherPhotonView = other.gameObject.GetComponent<PhotonView>();

        // Проверяем, является ли объект локальным
        if (otherPhotonView != null && otherPhotonView.IsMine)
        {
            MyObject = true;
        }
        
        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        if (_particleEffect)
        {
            _particleEffect.Play();
        }
        if (_audio)
        {
            _audio.Play();
        }
        if (_flashRenderer)
        {
            StartCoroutine(FlashRenderer(_flashRenderer));
        }

        // Подождите, пока все эффекты будут воспроизведены (предположительно 3 секунды, настройте это значение под вашу задачу)
        yield return new WaitForSeconds(1);
        
        
        // Отключите объект, только если это объект локального игрока
        if (MyObject)
        {
            // Отключите объект
            gameObject.SetActive(false);
           
        }
        
    }
    
    private IEnumerator FlashRenderer(MeshRenderer renderer)
    {
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(.1f);
        renderer.material.SetColor("_EmissionColor", Color.clear);
    }
}