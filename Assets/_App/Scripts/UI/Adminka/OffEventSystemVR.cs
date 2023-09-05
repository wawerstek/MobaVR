using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffEventSystemVR : MonoBehaviour
{
    //скрипт, который отключает EventSystem, если игра запущена на компьютере
    public ManagerDevice managerDevice; // Ссылка на объект ManagerDevice

    private void Awake()
    {
        managerDevice = GameObject.Find("DeviceManager").GetComponent<ManagerDevice>();
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!managerDevice.PlayerCrate && gameObject.activeSelf) // Проверяем, нужно ли создавать игрока
        {
            // Выключаем (деактивируем) объект
            gameObject.SetActive(false);
            
        }
    }
}
