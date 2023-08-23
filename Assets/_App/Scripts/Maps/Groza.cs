using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groza : MonoBehaviour
{
    public GameObject lightning1;
    public GameObject lightning2;
    public float minDelay = 3.0f; // минимальная задержка перед следующей вспышкой
    public float maxDelay = 15.0f; // максимальная задержка перед следующей вспышкой

    private void Start()
    {
        // Начать цикл вспышек
        StartCoroutine(FlashLightning());
    }

    System.Collections.IEnumerator FlashLightning()
    {
        while (true)
        {
            // Ожидание случайного времени перед следующей вспышкой
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            // Выбрать случайное молнию для вспышки
            GameObject selectedLightning = Random.Range(0, 2) == 0 ? lightning1 : lightning2;

            // Включить молнию
            selectedLightning.SetActive(true);

            // Ожидать 1 секунды
            yield return new WaitForSeconds(2f);

            // Выключить молнию
            selectedLightning.SetActive(false);
        }
    }
}
