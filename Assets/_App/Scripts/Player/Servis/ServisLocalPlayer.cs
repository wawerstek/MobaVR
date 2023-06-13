using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG
{
    public class ServisLocalPlayer : MonoBehaviour
    {
        //������ B 
        public ControllerBinding RunObuch = ControllerBinding.BButtonDown;

        public GameObject ChageSkinUI;
        private bool ChageSkinBool = false; // ����, �����������, �������� �� ���� ������ �����


        //���������� ������� ����������
        public GameObject _CALIBR;
        //���������� ������� ����������
        public GameObject CalibrovkaText;
        //���������� ������� ����������
        public GameObject SkinText;

        // Start is called before the first frame update
        void Start()
        {
            ChageSkinUI.SetActive(false);
        }

        void LateUpdate()
        {
            //���� ������ ������ B
            if (InputBridge.Instance.GetControllerBindingValue(RunObuch))
            {
                RunChageSkin();
            }
        }

        //������� ��������� ��� ������ �����
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
                    meshRenderer.enabled = false; // ��������� ��������� MeshRenderer
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
                    meshRenderer.enabled = true; // ��������� ��������� MeshRenderer
                }
            }
        }


        //������� ��������� ����������
        public void RunCalibr()
        {
            SkinText.SetActive(false);
            CalibrovkaText.SetActive(true);
            //������� ��������� ����������
            _CALIBR.GetComponent<Calibr>().calibr = false;
        }


    }

}