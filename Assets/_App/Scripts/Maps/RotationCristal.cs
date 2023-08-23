using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCristal : MonoBehaviour
{
    public float rotationSpeed = 60.0f; // Скорость вращения в градусах в секунду
    public float pistonSpeed = 1.0f; // Скорость движения вверх и вниз
    public float pistonRange = 0.3f; // Расстояние движения вверх и вниз от начальной позиции

    private Vector3 initialPosition;
    private bool movingUp = true;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Вращение объекта вокруг своей оси Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Движение объекта вверх и вниз как поршень
        if (movingUp)
        {
            transform.position += Vector3.up * pistonSpeed * Time.deltaTime;
            if (transform.position.y >= initialPosition.y + pistonRange)
                movingUp = false;
        }
        else
        {
            transform.position -= Vector3.up * pistonSpeed * Time.deltaTime;
            if (transform.position.y <= initialPosition.y)
                movingUp = true;
        }
    }
}
