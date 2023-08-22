using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessetMenu : MonoBehaviour
{
    public GameObject objectToActivate; // Объект, который вы хотите активировать через инспектор

    void Start()
    {
        // При старте скрипта начнем корутину
        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(5); // Ждем 5 секунд
        objectToActivate.SetActive(true);   // Активируем объект
    }
}
