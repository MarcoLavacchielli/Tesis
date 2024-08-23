using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDiamont : MonoBehaviour
{
    public float pickupRange = 2.0f;
    public Transform player;

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= pickupRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        Destroy(gameObject);
    }
}