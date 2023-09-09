using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class VRTextInput : MonoBehaviour {

        // Ссылка на компонент InputField, который будет управляться этим скриптом
        UnityEngine.UI.InputField thisInputField;

        // Определяет, будет ли скрипт прикрепляться к виртуальной клавиатуре VR
        public bool AttachToVRKeyboard = true;

        // Определяет, активировать ли клавиатуру при выборе вводного поля
        public bool ActivateKeyboardOnSelect = true;

        // Определяет, деактивировать ли клавиатуру при снятии фокуса с вводного поля
        public bool DeactivateKeyboardOnDeselect = false;

        // Ссылка на экземпляр виртуальной клавиатуры, которая будет управлять этим вводным полем
        public VRKeyboard AttachedKeyboard;

        // Флаги, указывающие, находится ли вводное поле в фокусе в данный момент и было ли оно в фокусе на предыдущем кадре
        bool isFocused, wasFocused = false;

        void Awake() {
            // Получаем компонент InputField, который прикреплен к этому объекту
            thisInputField = GetComponent<UnityEngine.UI.InputField>();
            
            // Если InputField найден и виртуальная клавиатура прикреплена, привязываем ее к InputField
            if(thisInputField && AttachedKeyboard != null) {
                AttachedKeyboard.AttachToInputField(thisInputField);
            }
        }

        void Update() {
            // Проверяем, в фокусе ли вводное поле в данный момент
            isFocused = thisInputField != null && thisInputField.isFocused;

            // Проверяем, изменилось ли состояние фокуса вводного поля, и вызываем соответствующие методы
            if(isFocused == true && wasFocused == false) {
                OnInputSelect(); // Вызывается при получении фокуса
            }
            else if (isFocused == false && wasFocused == true) {
                //OnInputDeselect(); // Может быть вызван при потере фокуса, но в данном коде закомментирован
            }

            // Обновляем состояние фокуса для следующего кадра
            wasFocused = isFocused;
        }

        public void OnInputSelect() {
            // Активируем клавиатуру, если она была выключена, и привязываем ее к вводному полю
            if (ActivateKeyboardOnSelect && AttachedKeyboard != null && !AttachedKeyboard.gameObject.activeInHierarchy) {
                AttachedKeyboard.gameObject.SetActive(true);
                AttachedKeyboard.AttachToInputField(thisInputField);
            }
        }

        public void OnInputDeselect() {
            // Деактивируем клавиатуру, если это задано в настройках, и она активна
            if (DeactivateKeyboardOnDeselect && AttachedKeyboard != null && AttachedKeyboard.gameObject.activeInHierarchy) {
                AttachedKeyboard.gameObject.SetActive(false);
            }
        }

        // Метод, вызываемый для отключения клавиатуры (Enter_Of - странный и нестандартный название, возможно, ошибка)
        public void Enter_Of()
        {
            AttachedKeyboard.gameObject.SetActive(false);
        }

        // Метод вызывается при добавлении этого компонента к объекту для первоначальной настройки AttachedKeyboard
        void Reset() {
            // Поиск экземпляра VRKeyboard в сцене и его привязка к AttachedKeyboard
            var keyboard = GameObject.FindObjectOfType<VRKeyboard>();
            if(keyboard) {
                AttachedKeyboard = keyboard;
                Debug.Log("Found and attached Keyboard to " + AttachedKeyboard.transform.name);
            }
        }
    }
}
