using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawn : MonoBehaviour
{
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private List<GameObject> objectsToActivate;

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
        ActivateObjects();
        audioM.PlaySfx(5);
    }

    private void ActivateObjects()
    {
        if (objectsToActivate != null && objectsToActivate.Count > 0)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null && !obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogError("No objects assigned to activate or list is empty!");
        }
    }
}
