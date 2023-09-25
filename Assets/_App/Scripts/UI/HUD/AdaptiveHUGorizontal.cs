using UnityEngine;
using System.Collections;

public class AdaptiveHUGorizontal : MonoBehaviour
{
    public Transform playerHead; // Голова игрока (VR-камера).
    public float upperLimit = 1f;
    public float lowerLimit = -0.5f;
    public float followSpeedHorizontal = 3.0f;

    private Vector3 initialHudOffset;

    void Start()
    {
        initialHudOffset = transform.localPosition;
    }

    void Update()
    {
    
        HorizontalMovement();
    }



    private void HorizontalMovement()
    {
        Vector3 desiredPosition = playerHead.position;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeedHorizontal * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, playerHead.rotation, followSpeedHorizontal * Time.deltaTime);
    }




}