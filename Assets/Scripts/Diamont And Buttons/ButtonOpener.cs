using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpener : MonoBehaviour
{
    /*
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private List<GameObject> objectsToDestroy;
    [SerializeField] private float rotationDuration = 1.0f; // Duración de la animación de rotación

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
        RotateFirstObject();
        DestroyObjects();
        audioM.PlaySfx(5);
    }

    private void RotateFirstObject()
    {
        if (objectsToDestroy != null && objectsToDestroy.Count > 0)
        {
            GameObject firstObject = objectsToDestroy[0];
            if (firstObject != null)
            {
                StartCoroutine(RotateObject(firstObject, Vector3.up * 90, rotationDuration));
            }
        }
    }

    private IEnumerator RotateObject(GameObject obj, Vector3 byAngles, float duration)
    {
        Quaternion originalRotation = obj.transform.rotation;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(byAngles);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.rotation = targetRotation;
    }

    private void DestroyObjects()
    {
        if (objectsToDestroy != null && objectsToDestroy.Count > 1)
        {
            for (int i = 1; i < objectsToDestroy.Count; i++)
            {
                GameObject obj = objectsToDestroy[i];
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
    }*/
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private List<GameObject> objectsToDestroy;
    [SerializeField] private float rotationDuration = 1.0f; // Duración de la animación de rotación
    [SerializeField] private GameObject valve; // La válvula que debe rotar
    [SerializeField] private ParticleSystem particles; // Sistema de partículas
    [SerializeField] private float valveRotationDuration = 4.0f; // Duración de la rotación de la válvula
    [SerializeField] private float initialValveSpeed = 360.0f; // Velocidad de rotación inicial (grados por segundo)
    [SerializeField] private int valveTurns = 5; // Cantidad de vueltas que debe dar la válvula
    [SerializeField] private float particleDuration = 2.0f; // Duración de las partículas

    private AudioManager audioM;
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
        StartCoroutine(ValveSequence());
    }

    private IEnumerator ValveSequence()
    {
        // Rotar la válvula
        yield return StartCoroutine(RotateValve(valve, valveRotationDuration, initialValveSpeed, valveTurns));

        // Reproducir partículas
        particles.Play();
        yield return new WaitForSeconds(particleDuration);
        particles.Stop();

        // Rotar la puerta
        RotateFirstObject();
    }

    private IEnumerator RotateValve(GameObject obj, float duration, float speed, int turns)
    {
        float elapsedTime = 0f;
        float totalRotation = turns * 360f; // Total de rotación (5 vueltas completas)
        float currentRotation = 50f;

        while (elapsedTime < duration)
        {
            // Calcular el ángulo actual en función del tiempo y la velocidad
            float rotationStep = Mathf.Lerp(speed, 0.0f, elapsedTime / duration) * Time.deltaTime;
            currentRotation += rotationStep;
            obj.transform.Rotate(Vector3.left * rotationStep); // Rotar en el eje X negativo

            // Evitar que la rotación supere el número total de vueltas
            if (currentRotation >= totalRotation)
                break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void RotateFirstObject()
    {
        if (objectsToDestroy != null && objectsToDestroy.Count > 0)
        {
            GameObject firstObject = objectsToDestroy[0];
            if (firstObject != null)
            {
                StartCoroutine(RotateObject(firstObject, Vector3.up * 90, rotationDuration));
            }
        }
    }

    private IEnumerator RotateObject(GameObject obj, Vector3 byAngles, float duration)
    {
        Quaternion originalRotation = obj.transform.rotation;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(byAngles);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.rotation = targetRotation;
    }

    private void DestroyObjects()
    {
        if (objectsToDestroy != null && objectsToDestroy.Count > 1)
        {
            for (int i = 1; i < objectsToDestroy.Count; i++)
            {
                GameObject obj = objectsToDestroy[i];
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
