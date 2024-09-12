using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDiamont : MonoBehaviour
{
    public float pickupRange = 2.0f;
    public Transform player;

    public bool diamondTake = false;

    [SerializeField] private List<Light> lights;

    public AudioManager audioM;

    public CameraTransition camTransition;

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
        //Destroy(gameObject);
        gameObject.SetActive(false);
        foreach (var light in lights)
        {
            light.color = Color.red;
        }
        changeMusic();
        diamondTake = true;
        camTransition.callCinematic();
    }
    void changeMusic()
    {
        audioM.StopMusic(0);
        audioM.PlayMusic(1);
        //audioM.PlaySfx(7);
    }
}