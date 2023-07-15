using UnityEngine;
using System.Collections;

public class BulletsDamage : MonoBehaviour
{
    [Range(1, 100)]
    public int Damage = 34;

    void OnCollisionEnter(Collision other)
    {
        //Destroy (gameObject);
    }
}