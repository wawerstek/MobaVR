using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{

    public class Calibr : MonoBehaviour
    {
        public GameObject Left_Calibr;
        public GameObject Right_Calibr;
        public GameObject CalibrovkaText;

        public Transform pointA;
        public Transform pointB;
        public Transform pointC;//левая рука
        public Transform pointD;//правая рука

        public bool calibr;

        public ControllerBinding Button_Calibr = ControllerBinding.AButton; //нажатие

        void Start()
        {
            calibr = false;
            CalibrovkaText.SetActive(true);
        }



        void Update()
        {

            if (calibr == false)
            {

                if (InputBridge.Instance.GetControllerBindingValue(Button_Calibr))
                {
                    Left_Calibr = GameObject.Find("Left_Calibr");
                    Right_Calibr = GameObject.Find("Right_Calibr");

                    pointA = Left_Calibr.transform;
                    pointB = Right_Calibr.transform;

                    MoveToPointA();
                    RotateAroundPointC();
                    CalibrovkaText.SetActive(false);
                    calibr = true;
                }
            }

        }



        void MoveToPointA()
        {
            // вычисляем вектор разницы между текущей позицией точки С и точкой А
            Vector3 deltaPosition = pointA.position - pointC.position;

            // перемещаем родительский объект на вектор deltaPosition
            transform.position += deltaPosition;
        }

        void RotateAroundPointC()
        {
            // вычисляем вектор поворота для поворота родительского объекта вокруг точки С
            Vector3 rotationVector = pointB.position - pointA.position;
            Quaternion rotation = Quaternion.FromToRotation(pointD.position - pointC.position, rotationVector);

            // сохраняем текущее расстояние между родителем и точками С и Д
            float distance = Vector3.Distance(transform.position, pointC.position);

            // поворачиваем родительский объект вокруг точки С
            transform.rotation = rotation * transform.rotation;

            //возвращаем наклон к 0, чтобы вращался только по оси У
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

            // вычисляем вектор разницы между текущей позицией точки С и точкой А
            Vector3 deltaPosition2 = pointB.position - pointD.position;

            // перемещаем родительский объект на вектор deltaPosition
            transform.position += deltaPosition2;

        }


    }

}