using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestruction : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("bala destruida");
        Destroy(gameObject);
    }
}
