using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using MobaVR;


namespace MobaVR
{
    public class ChangeBall : MonoBehaviour
    {
        //������ ����� �����
        [SerializeField] private BigFireBall _BigFireballPrefab;

       
        public GameSession _GameSession;
        [SerializeField] private GameObject _PlayerVR;

        private void Start()
        {
            Debug.Log("����� � �������1");

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Grabber"))
            {
                Debug.Log("����� � �������");

                //������ ���������� _PlayerVR �������� � ���� ������� ������ ������
                _PlayerVR = _GameSession.localPlayer;

                //����� ���������� � ������ ������� �� remoteplayer
                _PlayerVR.GetComponent<WizardPlayer>().m_BigFireballPrefab = _BigFireballPrefab;
               // _PlayerVR.GetComponent<WizardPlayer>().ChangeBall(_BigFireballPrefab);
                // �������� ������ �� _BigFireballPrefab � m_BigFireballPrefab ������� WizardPlayer

            }
        }


    }
}