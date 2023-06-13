using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG
{
    public class ServisLocalPlayer : MonoBehaviour
    {
        //кнопка B 
        public ControllerBinding RunObuch = ControllerBinding.BButtonDown;

        public GameObject ChageSkinUI;
        private bool ChageSkinBool = false; // Флаг, указывающий, включено ли меню выбора скина


        //переменная скрипта калибровки
        public GameObject _CALIBR;
        //переменная надписи Калибровка
        public GameObject CalibrovkaText;
        //переменная надписи Калибровка
        public GameObject SkinText;

        // Start is called before the first frame update
        void Start()
        {
            ChageSkinUI.SetActive(false);
        }

        void LateUpdate()
        {
            //если нажата кнопка B
            if (InputBridge.Instance.GetControllerBindingValue(RunObuch))
            {
                RunChageSkin();
            }
        }

        //функция включения мен выбора скина
        public void RunChageSkin()
        {
            
            if(!ChageSkinBool)
            {
                ChageSkinUI.SetActive(true);
                ChageSkinBool = true;
            }
            else
            {
                ChageSkinUI.SetActive(false);
                ChageSkinBool = false;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ChageSkin"))
            {
                RunChageSkin();

                MeshRenderer meshRenderer = other.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false; // Выключаем компонент MeshRenderer
                }


            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ChageSkin"))
            {
                RunChageSkin();

                MeshRenderer meshRenderer = other.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = true; // Выключаем компонент MeshRenderer
                }
            }
        }


        //функция включения калибровки
        public void RunCalibr()
        {
            SkinText.SetActive(false);
            CalibrovkaText.SetActive(true);
            //функция включения калибровки
            _CALIBR.GetComponent<Calibr>().calibr = false;
        }


    }

}