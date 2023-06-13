using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using MobaVR;


namespace MobaVR
{
    public class ChangeBall : MonoBehaviour
    {
        //скрипт смены шаров
        [SerializeField] private BigFireBall _BigFireballPrefab;

       
        public GameSession _GameSession;
        [SerializeField] private GameObject _PlayerVR;

        private void Start()
        {
            Debug.Log("Вывод в консоль1");

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Grabber"))
            {
                Debug.Log("Вывод в консоль");

                //теперь переменная _PlayerVR содержит в себе сетевую версию игрока
                _PlayerVR = _GameSession.localPlayer;

                //Можно обратиться к любому скрипту на remoteplayer
                _PlayerVR.GetComponent<WizardPlayer>().m_BigFireballPrefab = _BigFireballPrefab;
               // _PlayerVR.GetComponent<WizardPlayer>().ChangeBall(_BigFireballPrefab);
                // Передаем объект из _BigFireballPrefab в m_BigFireballPrefab скрипта WizardPlayer

            }
        }


    }
}