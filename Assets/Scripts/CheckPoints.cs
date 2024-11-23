using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{

    public CheckTeleporter check;

    // Referencia al objeto feedback
    public GameObject targetObject;
    public Material targetMaterial;

    AudioManager audioM;

    private void Awake()
    {
        audioM = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            check.currentWaypointIndex += 1;

            // Cambiar el material del objeto al material especificado
            if (targetObject != null && targetMaterial != null)
            {
                Renderer renderer = targetObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = targetMaterial;
                    audioM.PlaySfx(16);
                }
            }

            Destroy(gameObject);
        }
    }

}
