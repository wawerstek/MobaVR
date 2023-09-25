using UnityEngine;
using System.Collections;

public class AdaptiveHUD : MonoBehaviour
{
    public Transform playerHead;
    public float upperLimit = 1f;
    public float lowerLimit = -0.5f;
    public float followSpeedVertical = 5.0f;

    private Vector3 initialHudOffset;

    
    

    

    void Start()
    {
        initialHudOffset = transform.localPosition;
    
 
    }

    void Update()
    {
        VerticalMovement();
       
    }

    private void VerticalMovement()
    {
        float verticalAngle = Vector3.Angle(playerHead.forward, Vector3.up) - 90.0f;
        float yOffset = Mathf.Clamp(verticalAngle / 90.0f, lowerLimit, upperLimit);
        Vector3 targetPosition = new Vector3(transform.localPosition.x, initialHudOffset.y + yOffset, transform.localPosition.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, followSpeedVertical * Time.deltaTime);
    }





}