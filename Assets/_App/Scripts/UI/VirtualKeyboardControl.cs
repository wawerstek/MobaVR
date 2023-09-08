using UnityEngine;

public class VirtualKeyboardControl : MonoBehaviour
{
    
    //скрываем клавиатуру пико
    private TouchScreenKeyboard m_keyboard;

    void Start()
    {
        // Вызываем метод DisableKeyBoard через 0.5 секунды после старта сцены
        Invoke("DisableKeyBoard", 0.5f);
    }

    void LateUpdate()
    {
        // Скрываем виртуальную клавиатуру
        if (m_keyboard != null)
        {
            m_keyboard.active = false;
        }
    }

    private void DisableKeyBoard()
    {
        // Инициализируем виртуальную клавиатуру с пустой строкой и стандартным типом
        m_keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
    }
}