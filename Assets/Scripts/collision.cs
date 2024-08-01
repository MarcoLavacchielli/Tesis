using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("deberia estar muerta la cámara");
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Debug.Log("deberia estar muerta la cámara");
            this.gameObject.SetActive(false);
        }
    }
}
