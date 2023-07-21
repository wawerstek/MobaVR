using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private void Update()
    {
        // Получаем текущий угол поворота объекта
        Quaternion currentRotation = transform.rotation;

        // Вычисляем новый угол поворота с добавлением вращения
        Quaternion newRotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * currentRotation;

        // Применяем новый угол поворота к объекту
        transform.rotation = newRotation;
    }
}
