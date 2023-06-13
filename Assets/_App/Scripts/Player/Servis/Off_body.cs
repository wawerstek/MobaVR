using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//скрипт получает команду по потключению отображения тела локального игрока. Получает команду из главного меню админа
public class Off_body : MonoBehaviour
{

    public ChangeSkinPlayer _ChangeSkinPlayer; // Ссылка на скрипт, содержащий функцию SkinON

    public GameObject[] objectsToDisable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void DisableAllObjects(bool isActive)
    //{
    //    foreach (GameObject obj in objectsToDisable)
    //    {
    //        obj.SetActive(isActive); // Выключаем или включаем объект 
    //    }
    //}

    public void DisableAllObjects(bool isActive)
    {
        for (int i = 0; i < objectsToDisable.Length; i++)
        {
            GameObject obj = objectsToDisable[i];

            if (isActive)
            {
                if (i<2)
                {
                    obj.SetActive(true); // Включаем первые 2 объекта
                }
                _ChangeSkinPlayer.SkinON();
            }
            else
            {
                obj.SetActive(false); // Выключаем все объекты
            }
        }
    }



}
