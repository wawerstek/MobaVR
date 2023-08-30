using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRes : MonoBehaviour
{
    public GameObject[] targets; // Массив мишен
    public bool allTargetsDisabled = false; // Переменная, отражающая состояние мишен

    private bool enableScheduled = false; // Планирование включения

    private void Update()
    {
        CheckTargets();
    }

    // Функция для проверки состояния мишен
    private void CheckTargets()
    {
        bool allDisabled = true;

        foreach (GameObject target in targets)
        {
            if (target.activeInHierarchy)
            {
                allDisabled = false;
                break;
            }
        }

        // Если все мишени отключены
        if (allDisabled)
        {
            //все мишени отключены
            allTargetsDisabled = true; // Устанавливаем флаг в true

                StartCoroutine(EnableTargetsAfterDelay(20f)); // Запускаем корутину для включения мишен после 20 секунд
                //enableScheduled = false; // Сбрасываем флаг планирования включения
  
        }
        else
        {
            allTargetsDisabled = false; // Устанавливаем флаг в false
        }
    }

    // Функция для включения мишен через задержку
    private IEnumerator EnableTargetsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //включаем мишени
        EnableTargetsImmediately();
    }

    // Функция для немедленного включения мишен
    public void EnableTargetsImmediately()
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(true);
        }
        enableScheduled = false; // Сбрасываем флаг планирования включения
        allTargetsDisabled = false; // Сбрасываем флаг в false
    }

    // Функция для планирования включения мишен
    public void ScheduleEnableTargets()
    {
        enableScheduled = true;
    }
}
