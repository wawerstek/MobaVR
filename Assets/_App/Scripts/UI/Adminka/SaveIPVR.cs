using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MobaVR;
public class SaveIPVR : MonoBehaviour
{
    
       
    [Header("IP-адрес")]
    public InputField ipAddressInput; // ссылка на поле ввода IP-адреса
    
    // Start is called before the first frame update
    void Awake()
    {
        LoadSavedIPAddress(); // Загрузить сохраненный IP-адрес при запуске приложения
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void SaveIPAddressVR()
    {
        SaveIPAddress(); // Сохранить IP-адрес перед загрузкой сцены

    }    
    
    // Сохранить IP-адрес в PlayerPrefs
    private void SaveIPAddress() 
    {
        string ipAddress = ipAddressInput.text;
        PlayerPrefs.SetString("LastIPAddress", ipAddress);
    }
    
    // Загрузить IP-адрес из PlayerPrefs
    private void LoadSavedIPAddress() 
    {
        string lastIPAddress = PlayerPrefs.GetString("LastIPAddress", "");
        if (!string.IsNullOrEmpty(lastIPAddress)) {
            ipAddressInput.text = lastIPAddress;
        }
    }
    
    
    
    
    
}
