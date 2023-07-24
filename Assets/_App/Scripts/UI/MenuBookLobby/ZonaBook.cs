using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobaVR;
using BNG;

public class ZonaBook : MonoBehaviour
{
    public GameObject objectToActivate; //тут меню книги
    public GameObject Book_Menu; //тут внешний вид страницы меню. Именно задний план
    public GameObject Book; //тут внешний вид страницы стандартной, который видят другие игроки
    public Transform destinationParent; //точка, куда должны перенестись скины
    private int playerCount = 0; //количество игроков, находящихся в зоне
    //public MeshRenderer meshRenderer; //визуальный вид зоны
    public GameObject meshRenderer; //визуальный вид зоны
    public string targetID; //ID скина. Сюда можно его присылать и от сюда забирать
    public GameObject[] childObjects;

    //звук
    public AudioClip soundOnEnter;
    public AudioClip soundOffSoundMaps;
    public AudioClip soundOnExit;
    public AudioClip soundOnSoundMaps;

    private AudioSource audioSource;

    //туман
    public float fogDensityTarget = 0.5f; // Целевое значение fog density
    public float transitionDuration = 1f; // Длительность перехода в секундах

    private bool isInTrigger = false; // Флаг, указывающий на нахождение внутри триггера
    private float initialFogDensity; // Начальное значение fog density
    private float transitionTimer = 0f; // Таймер для перехода




    private void Start()
    {
        // Получаем компонент AudioSource из текущего объекта или его дочерних объектов
        audioSource = GetComponent<AudioSource>();
        // Сохраняем начальное значение fog density
        initialFogDensity = RenderSettings.fogDensity;

    }



    private void Update()
    {
        if (isInTrigger)
        {
            // Если в триггере, начинаем плавно менять fog density к целевому значению
            if (transitionTimer < transitionDuration)
            {
                transitionTimer += Time.deltaTime;
                float t = transitionTimer / transitionDuration;
                float newDensity = Mathf.Lerp(initialFogDensity, fogDensityTarget, t);
                RenderSettings.fogDensity = newDensity;
            }
            else
            {
                // Завершили переход
                RenderSettings.fogDensity = fogDensityTarget;
            }
        }
        else
        {
            // Если не в триггере, плавно меняем fog density к начальному значению
            if (transitionTimer > 0f)
            {
                transitionTimer -= Time.deltaTime;
                float t = transitionTimer / transitionDuration;
                float newDensity = Mathf.Lerp(initialFogDensity, fogDensityTarget, t);
                RenderSettings.fogDensity = newDensity;
            }
            else
            {
                // Завершили переход
                RenderSettings.fogDensity = initialFogDensity;
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerLocalPlayer"))
        {
            playerCount++;
            if (playerCount == 1)
            {

                // Воспроизводим звук при входе в коллайдер
                if (soundOnEnter != null)
                {
                    audioSource.PlayOneShot(soundOnEnter);
                    audioSource.PlayOneShot(soundOffSoundMaps);
                }

                //запускаем туман
                isInTrigger = true;

                meshRenderer.SetActive(false);
                objectToActivate.SetActive(true);
                Book_Menu.SetActive(true);
                Book.SetActive(false);


               
                int childCount = other.transform.childCount;
                childObjects = new GameObject[childCount];

                // Проход для подсчета количества дочерних объектов
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = other.transform.GetChild(i);
                    childObjects[i] = child.gameObject;
                }

                // Проход для перемещения их в destinationParent
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = childObjects[i].transform;


                    child.SetParent(destinationParent);
                    child.localPosition = Vector3.zero;
                    child.localRotation = Quaternion.identity;
                    child.localScale = Vector3.one;
                }

                UpdateTargetID(targetID);


            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerLocalPlayer"))
        {
            playerCount--;
            if (playerCount == 0)
            {
                // Воспроизводим звук при выходе из коллайдера
                if (soundOnExit != null)
                {
                    audioSource.PlayOneShot(soundOnExit);
                    audioSource.PlayOneShot(soundOnSoundMaps);
                }

                //отключаем туман
                isInTrigger = false;


                meshRenderer.SetActive(true);
                objectToActivate.SetActive(false);
                Book_Menu.SetActive(false);
                Book.SetActive(true);

                int childCount = destinationParent.transform.childCount;

                // Создаем временный массив для хранения ссылок на дочерние объекты
                GameObject[] childObjects = new GameObject[childCount];

                // Проходимся по дочерним объектам и сохраняем ссылки
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = destinationParent.transform.GetChild(i);
                    childObjects[i] = child.gameObject;
                }

                // Перемещаем дочерние объекты обратно в other
                foreach (GameObject childObject in childObjects)
                {
                    Transform childTransform = childObject.transform;
                    childTransform.SetParent(other.transform);
                    childTransform.localPosition = Vector3.zero;
                    childTransform.localRotation = Quaternion.identity;
                    childTransform.localScale = Vector3.one;
                    childObject.SetActive(false);
                }



                //// Переносим дочерние объекты из destinationParent обратно в other
                //foreach (Transform child in destinationParent)
                //{
                //    child.SetParent(other.transform);
                //    child.localPosition = Vector3.zero;
                //    child.localRotation = Quaternion.identity;
                //    child.localScale = Vector3.one;
                //    child.gameObject.SetActive(false);
                //}

            }
        }  
    }

    public void UpdateTargetID(string newTargetID)
    {
        targetID = newTargetID;

        // Проход по дочерним объектам и включение объектов с нужным ID
        foreach (Transform child in destinationParent)
        {
            Skin skin = child.GetComponent<Skin>();
            skin.gameObject.SetActive(false);

            if (skin != null && skin.ID == targetID)
            {
                skin.gameObject.SetActive(true);
            }
        }
    }


}
