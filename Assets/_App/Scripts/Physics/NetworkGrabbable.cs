using Photon.Pun;
using BNG;
using UnityEngine;

namespace MobaVR
{
    public class NetworkGrabbable : Grabbable, IPunObservable
    {
        //объявляем переменные 
        //говорим, что переменная view несёт в себе фотонавью, это тоже самое, что и PV
        PhotonView view;
        Rigidbody rb;

        //Используется для защиты нашей позиции, когда мы не являемся владельцами
        private Vector3 _syncStartPosition = Vector3.zero;
        private Vector3 _syncEndPosition = Vector3.zero;
        private Quaternion _syncStartRotation = Quaternion.identity;
        private Quaternion _syncEndRotation = Quaternion.identity;
        private bool _syncBeingHeld = false;

        // Значения интерполяции
        private float _lastSynchronizationTime = 0f;
        private float _syncDelay = 0f;
        private float _syncTime = 0f;

        //в старте говорим какие значения мы вешаем на наши переменные
        void Start()
        {
            view = GetComponent<PhotonView>();
            rb = GetComponent<Rigidbody>();
        }

        public override void Update()
        {
            base.Update();

            // Проверьте, ушел ли владелец или не назначен. запускаем функицю проверки, она ниже
            //CheckForNullOwner();

            // если объект не мой то мы зритель и мы видим, что...
            if (!view.IsMine && view.Owner != null && _syncEndPosition != null && _syncEndPosition != Vector3.zero)
            {
                //что на этом объекте включена кинематика, значит он у нас не имеет физических свойств и мы видим те траектори, куда он движется. И мы не видим, что он падает как физическое тело
                // rb.isKinematic = true;

                // Учитывает задержку, чтобы синхронизировать объект. Скорее всего это синхронизация положения предмета
                _syncTime += Time.deltaTime;

                float syncValue = _syncTime / _syncDelay;
                float dist = Vector3.Distance(_syncStartPosition, _syncEndPosition);

                // Если далеко, просто телепортируйся туда, чтобы не просчитывать каждый кадр как летит объект и уже если дистанция меньше 3-х то мы делаем синхронизацию планого движения.
                if (dist > 3f)
                {
                    transform.position = _syncEndPosition;
                    transform.rotation = _syncEndRotation;
                }
                else
                {
                    transform.position = Vector3.Lerp(_syncStartPosition, _syncEndPosition, syncValue);
                    transform.rotation = Quaternion.Lerp(_syncStartRotation, _syncEndRotation, syncValue);
                }

                BeingHeld = _syncBeingHeld;
            }
            // Если этот объект наш, то мы отключаем кинематику
            else if (view.IsMine)
            {
                if (rb)
                {
                    //rb.isKinematic = wasKinematic;

                    Grabber g = GetPrimaryGrabber();

                    /* эта штука не работатет 26.12.2022 

                    //Обновление локального преобразования в режиме реального времени
                    // Remote Player
                    if (photonView.IsMine)
                    {
                        if (ParentToHands)
                        {
                            gameObject.transform.parent = g.transform;
                            //предмет переместился в родительский элемент по иерархии
                            //transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero - GrabPositionOffset, fractionOfJourney);
                            //transform.localRotation = Quaternion.Lerp(transform.localRotation, grabTransform.localRotation, Time.deltaTime * 10);
                        }
                        // Расположите объект в мировом пространстве, используя физику
                    }
                   */
                }

                BeingHeld = heldByGrabbers != null && heldByGrabbers.Count > 0;
            }
        }

        ///// <summary>
        ///// Принудительное применение владельца к объектам сцены
        ///// </summary>
        //protected bool requestingOwnerShip = false;


        //функция проверки владельца
        //public virtual void CheckForNullOwner() {

        //     Только главный клиент должен проверять наличие пустого владельца
        //    if (!PhotonNetwork.IsMasterClient) {
        //        return;
        //    }

        //     Больше не запрашиваю права собственности, так как эта точка зрения принадлежит мне
        //    if (requestingOwnerShip && view.AmOwner) {
        //        requestingOwnerShip = false;
        //    }

        //     В настоящее время ожидает запроса на владение
        //    if (requestingOwnerShip) {
        //        return;
        //    }

        //     Главный клиент должен запросить право собственности, если оно еще не установлено. Это может быть объект сцены или если право собственности было потеряно
        //    if (view.AmOwner == false && view.Owner == null) {
        //        requestingOwnerShip = true;
        //        view.TransferOwnership(PhotonNetwork.MasterClient);
        //    }
        //}


        public override bool IsGrabbable()
        {
            //Если базу нельзя захватить, мы можем сбежать пораньше
            if (base.IsGrabbable() == false)
            {
                return false;
            }

            //Нет прикрепленного PhotonView
            if (view == null)
            {
                return true;
            }

            // Мы владеем этим объектом. Его можно схватить
            if (view.IsMine)
            {
                return true;
            }

            // Еще не подключен
            if (!PhotonNetwork.IsConnected)
            {
                return true;
            }

            return false;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // Это наша цель, отправляйте наши позиции другим игрокам
            if (stream.IsWriting && view.IsMine)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(BeingHeld);
            }
            // Получать Обновления
            else
            {
                // Position
                _syncStartPosition = transform.position;
                _syncEndPosition = (Vector3)stream.ReceiveNext();

                // Rotation
                _syncStartRotation = transform.rotation;
                _syncEndRotation = (Quaternion)stream.ReceiveNext();

                // Status
                _syncBeingHeld = (bool)stream.ReceiveNext();

                _syncTime = 0f;
                _syncDelay = Time.time - _lastSynchronizationTime;
                _lastSynchronizationTime = Time.time;
            }
        }
    }
}