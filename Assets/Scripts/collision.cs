using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{

    AudioManager audioM;

    private void Awake()
    {
        audioM = FindObjectOfType<AudioManager>();

        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("deberia estar muerta la cámara");
            this.gameObject.SetActive(false);
            audioM.PlaySfx(8);
            //Destroy(this.gameObject);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Debug.Log("deberia estar muerta la cámara");
            this.gameObject.SetActive(false);
        }
    }*/
}
