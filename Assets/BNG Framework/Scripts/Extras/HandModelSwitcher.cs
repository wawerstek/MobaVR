using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    // Класс для смены модели руки
    public class HandModelSwitcher : MonoBehaviour {

        // ID модели руки, который будет использоваться для переключения
        public int HandModelId = 1;

        // Ссылка на селектор модели руки
        public HandModelSelector hms;

        void Start() {
            // Если селектор модели руки не указан, пытаемся найти его в сцене
            if(hms == null) {
                hms = FindObjectOfType<HandModelSelector>();
            }

            // Если селектор модели руки все еще не найден, выводим сообщение об ошибке
            if(hms == null) {
                Debug.Log("Селектор модели руки не найден в сцене. Невозможно переключить модели рук");
            }
        }

        // Функция вызывается, когда другой объект входит в зону триггера этого объекта
        public void OnTriggerEnter(Collider other) {
            // Проверяем, является ли объект, вошедший в триггер, "граббером" (Grabber)
            if(other.gameObject.GetComponent<Grabber>() != null) {
                // Переключаем на заданную модель руки
                hms.ChangeHandsModel(HandModelId, false);
            }
        }
    }
}