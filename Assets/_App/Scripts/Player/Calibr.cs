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
        public Transform pointC;//����� ����
        public Transform pointD;//������ ����

        public bool calibr;

        public ControllerBinding Button_Calibr = ControllerBinding.AButton; //�������

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
            // ��������� ������ ������� ����� ������� �������� ����� � � ������ �
            Vector3 deltaPosition = pointA.position - pointC.position;

            // ���������� ������������ ������ �� ������ deltaPosition
            transform.position += deltaPosition;
        }

        void RotateAroundPointC()
        {
            // ��������� ������ �������� ��� �������� ������������� ������� ������ ����� �
            Vector3 rotationVector = pointB.position - pointA.position;
            Quaternion rotation = Quaternion.FromToRotation(pointD.position - pointC.position, rotationVector);

            // ��������� ������� ���������� ����� ��������� � ������� � � �
            float distance = Vector3.Distance(transform.position, pointC.position);

            // ������������ ������������ ������ ������ ����� �
            transform.rotation = rotation * transform.rotation;

            //���������� ������ � 0, ����� �������� ������ �� ��� �
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

            // ��������� ������ ������� ����� ������� �������� ����� � � ������ �
            Vector3 deltaPosition2 = pointB.position - pointD.position;

            // ���������� ������������ ������ �� ������ deltaPosition
            transform.position += deltaPosition2;

        }


    }

}