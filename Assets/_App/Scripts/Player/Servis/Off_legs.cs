using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Off_legs : MonoBehaviour
{
    //этот скрипт отключает или включает ноги на персонажах. Принимает данные из скрипта ServisUI
    [SerializeField] private GameObject[] Legs;

    public void ToggleObjects(bool isActive)
    {
        foreach (GameObject obj in Legs)
        {
            obj.SetActive(isActive);
        }
    }
}
