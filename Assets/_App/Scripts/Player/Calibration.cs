using BNG;
using RootMotion.FinalIK;
using UnityEngine;

namespace MobaVR
{
    public class Calibration : MonoBehaviour
    {
        public BaseGameSession _GameSession;
        [SerializeField] private GameObject _PlayerVR;


        [Header("Calibr traking")]
        //калировка местроположения
        public GameObject Left_Calibr;
        public GameObject Right_Calibr;
        public GameObject CalibrovkaText;

        public Transform pointA;
        public Transform pointB;
        public Transform pointC; //левая рука
        public Transform pointD; //правая рука

        public bool calibr;
        //----

        [Header("Calibr rost")]
        //Калибровка роста
        public VRIK[] local_Skins; //тело игрока, которое нужно увеличит
        //public GameObject _Left_ruka;
        //public GameObject _Right_ruka;
        public rost _rost; //скрипт смены роста у сетевой модельки
        public bool _kalibr_ruka;
        public float scaleMlp = 1f;

        public float x;
        public float y;
        public float z;

        public float _rostPlayer;
        public GameObject _CenterEyeAnchor;
        //переменная скрипта калибровки
        // public GameObject _CALIBR;
        //переменная надписи Калибровка
        // public GameObject CalibrovkaText;
        public bool runRost;


        public ControllerBinding Button_Calibr = ControllerBinding.AButton; //нажатие

        private void Start()
        {
            if (_GameSession == null)
            {
                _GameSession = FindObjectOfType<BaseGameSession>();
            }

            calibr = false;
            // CalibrovkaText.SetActive(true);


            runRost = false;
            _kalibr_ruka = false;
        }


        private void Update()
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
                    if (CalibrovkaText != null)
                    {
                        CalibrovkaText.SetActive(false);
                    }

                    calibr = true;

                    kalibr_rost();
                }
            }
        }


        private void MoveToPointA()
        {
            // вычисляем вектор разницы между текущей позицией точки С и точкой А
            Vector3 deltaPosition = pointA.position - pointC.position;

            // перемещаем родительский объект на вектор deltaPosition
            transform.position += deltaPosition;
        }

        private void RotateAroundPointC()
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


        public void kalibr_rost()
        {
            _PlayerVR = _GameSession.Player;

            _rostPlayer = _CenterEyeAnchor.transform.position.y;
            _rostPlayer += 0.1f;

            if (_PlayerVR.TryGetComponent(out rost rost))
            {
                rost.SetHeight(_rostPlayer);
            }
        }
    }
}