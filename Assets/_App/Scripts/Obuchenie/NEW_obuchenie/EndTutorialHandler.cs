using System.Collections;
using UnityEngine;

public class EndTutorialHandler : MonoBehaviour
{
    [Tooltip("Звук, который воспроизводится в конце обучения")]
    public AudioClip endSound;

    [Tooltip("FX эффект, который воспроизводится после звука")]
    public GameObject endEffectPrefab;

    [Tooltip("Массив объектов, которые будут выключены после воспроизведения звука и включены при старте")]
    public GameObject[] objectsToDisable;

    [Tooltip("Субтитры")]
    public GameObject SubEndUrok;
    
    private AudioSource audioSource;

    private void OnEnable()
    {
        // Включаем объекты из массива при старте
        foreach (GameObject obj in objectsToDisable)
        {
            SubEndUrok.SetActive(false);
            obj.SetActive(true);
        }

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning("AudioSource не найден на объекте. Добавление компонента...");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void EndTutorialSequence()
    {
        StartCoroutine(HandleEndSequence());
    }

    private IEnumerator HandleEndSequence()
    {
        if (endSound)
        {
            SubEndUrok.SetActive(true);
            audioSource.clip = endSound;
            audioSource.Play();
            yield return new WaitForSeconds(endSound.length);
        }

        if (endEffectPrefab)
        {
            GameObject effectInstance = Instantiate(endEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effectInstance, 5f); // уничтожаем объект FX через 5 секунд (или дольше, если эффект длится дольше)
        }

        // Добавляем задержку в 2 секунды перед выключением объектов
        yield return new WaitForSeconds(2f);

        foreach (GameObject obj in objectsToDisable)
        {
            SubEndUrok.SetActive(false);
            obj.SetActive(false);
        }

        gameObject.SetActive(false);
        //Destroy(gameObject); // уничтожаем объект, на котором лежит данный скрипт
    }
}