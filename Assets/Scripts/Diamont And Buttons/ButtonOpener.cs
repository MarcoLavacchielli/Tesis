using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpener : MonoBehaviour
{
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private List<GameObject> objectsToDestroy;

    AudioManager audioM;

    private Material originalMaterial;
    private Renderer rend;
    private bool playerNearby;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;

        audioM = FindObjectOfType<AudioManager>();

        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(activationKey))
        {
            ActivateButton();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void OnDestroy()
    {
        rend.material = originalMaterial;
    }

    private void ActivateButton()
    {
        rend.material = activeMaterial;
        DestroyObjects();
        audioM.PlaySfx(5);
    }

    private void DestroyObjects()
    {
        if (objectsToDestroy != null && objectsToDestroy.Count > 0)
        {
            foreach (GameObject obj in objectsToDestroy)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }
        else
        {
            Debug.LogError("No objects assigned to destroy or list is empty!");
        }
    }
}
