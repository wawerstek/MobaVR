using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerCamersMaps : MonoBehaviour
{
    public Camera[] cameras; // Массив камер
    [SerializeField] private Transform[] cameraTargets; // Массив для хранения новых точек размещения камеры

    void Awake()
    {
        // Подписываемся на событие перехода между сценами
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCameraTargets();

        // Проверяем, что были найдены точки для хотя бы одной камеры
        if (cameraTargets != null && cameraTargets.Length > 0)
        {
            MoveCamerasToTargets();
        }
    }

    void FindCameraTargets()
    {
        // Изначально устанавливаем массив cameraTargets как null
        cameraTargets = null;

        // Создаем список для хранения найденных объектов
        List<Transform> targetList = new List<Transform>();

        // Ищем объекты, имена которых начинаются с "targetCamera"
        for (int i = 1; ; i++)
        {
            GameObject targetObject = GameObject.Find("targetCamera" + i);

            // Если нашли объект с таким именем, добавляем его Transform в список
            if (targetObject != null)
            {
                targetList.Add(targetObject.transform);
            }
            else
            {
                // Если не нашли объект, прерываем цикл
                break;
            }
        }

        // Преобразуем список в массив
        cameraTargets = targetList.ToArray();
    }

    void MoveCamerasToTargets()
    {
        // Перемещаем каждую камеру к соответствующей точке
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameraTargets != null && cameraTargets.Length > i && cameras[i] != null)
            {
                cameras[i].transform.position = cameraTargets[i].position;
                cameras[i].transform.rotation = cameraTargets[i].rotation;
            }
        }
    }


	//если нажимаем на кнопки камер    
    public void RssetCamers()
	{
		//обнуляем массив
		cameraTargets = null;
		FindCameraTargets();
 		// Проверяем, что были найдены точки для хотя бы одной камеры
        if (cameraTargets != null && cameraTargets.Length > 0)
        {
            MoveCamerasToTargets();
        }
	}
}
