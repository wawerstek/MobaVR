using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{

    public class LocalVR : MonoBehaviour
    {
        [Header("DieSkin")]
        public SkinDieRespawn[] skinDieRespawnObjects;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        public void DieLocal()
        {

            // Выполняем функцию Die() на каждом объекте в массиве
            foreach (SkinDieRespawn obj in skinDieRespawnObjects)
            {
                //заменяем материало на прозрачный
                obj.Die();
            }

            //нужно отключить возможность стрельбы
            //нужно занести инфу о смерти игрока, а тому от кого прилетел последний шар внести инфу о убийстве

        }

        public void RespawnLocal()
        {
            //Возвращаться видимость скина
            // Выполняем функцию Die() на каждом объекте в массиве
            foreach (SkinDieRespawn obj in skinDieRespawnObjects)
            {
                //заменяем материало на прозрачный
                obj.Respawn();
            }
            

        }

    }
}