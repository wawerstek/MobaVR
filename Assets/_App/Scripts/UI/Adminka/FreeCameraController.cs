using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 2.0f;

    public Transform rotationPivot; // Ссылка на объект-поворот

    private bool isCameraControlEnabled = true; // Флаг, разрешающий управление камерой

    void OnEnable()
    {
        // Скрываем курсор при запуске
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
            //включаем управление камерой
        isCameraControlEnabled = true;
    }

    void Update()
    {
        if (rotationPivot == null)
        {
            return;
        }

        // Проверяем, разрешено ли управление камерой
        if (isCameraControlEnabled)
        {
            // Управление перемещением с клавиатуры
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Рассчитываем вектор перемещения в горизонтальной плоскости относительно ориентации rotationPivot
            Vector3 movement = rotationPivot.TransformDirection(new Vector3(horizontalInput, 0, verticalInput)) * movementSpeed * Time.deltaTime;

            // Применяем перемещение к позиции родительского объекта (элемента rotationPivot)
            transform.parent.position += movement;

            // Управление вращением с мышью (вокруг RotationPivot)
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed; // Инвертируем ось Y для соблюдения стандартного поведения Unity

            rotationPivot.Rotate(Vector3.right, mouseY);
            transform.Rotate(Vector3.up, mouseX);
        }

        // Проверяем нажатие клавиши Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // выключаем управление камерой
            isCameraControlEnabled = false;
            Cursor.lockState = isCameraControlEnabled ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isCameraControlEnabled;
        }
    }
}
