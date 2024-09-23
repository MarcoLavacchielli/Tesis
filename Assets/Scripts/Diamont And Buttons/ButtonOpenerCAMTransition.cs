using System.Collections;
using UnityEngine;

public class ButtonOpenerCAMTransition : MonoBehaviour
{
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private GameObject objectWithDisolveShader;
    [SerializeField] private float disolveDuration = 5.0f;
    [SerializeField] private float initialDisolveValue = -1.5f;
    [SerializeField] private float targetDisolveValue = 1.0f;

    [SerializeField] private GameObject player; // Referencia al jugador
    [SerializeField] private Transform cameraWaypoint;  // Waypoint donde se posicionar� la c�mara
    [SerializeField] private Transform cameraTransform; // Transform de la c�mara
    [SerializeField] private float cameraDuration = 2.5f; // Duraci�n total de la transici�n de la c�mara
    [SerializeField] private float cameraLerpSpeed = 1.0f; // Velocidad del Lerp de la c�mara

    public AudioManager audioM;
    private Material originalMaterial;
    private Renderer rend;
    private bool playerNearby;
    private Collider objectCollider;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;

        objectCollider = objectWithDisolveShader.GetComponent<Collider>();
        if (objectCollider == null)
        {
            Debug.LogError("El objeto con el shader no tiene un collider asignado!");
        }

        audioM = FindObjectOfType<AudioManager>();
        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }

        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned!");
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned!");
        }

        if (cameraWaypoint == null)
        {
            Debug.LogError("Camera Waypoint is not assigned!");
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
        audioM.PlaySfx(5);
        StartCoroutine(DisolveObject());
        StartCoroutine(HandleCameraAndPlayer());
    }

    private IEnumerator DisolveObject()
    {
        Material disolveMaterial = objectWithDisolveShader.GetComponent<Renderer>().material;
        float elapsedTime = 0f;
        float currentDisolveValue = initialDisolveValue;

        audioM.PlaySfx(12);

        while (elapsedTime < disolveDuration)
        {
            currentDisolveValue = Mathf.Lerp(initialDisolveValue, targetDisolveValue, elapsedTime / disolveDuration);
            disolveMaterial.SetFloat("_DisolveAmount", currentDisolveValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        disolveMaterial.SetFloat("_DisolveAmount", targetDisolveValue);
        objectCollider.enabled = false;
    }

    private IEnumerator HandleCameraAndPlayer()
    {
        // Desactivar el jugador
        player.SetActive(false);

        // Guardar la posici�n y rotaci�n original de la c�mara
        originalCameraPosition = cameraTransform.position;
        originalCameraRotation = cameraTransform.rotation;

        float elapsedTime = 0f;

        // Mover la c�mara hacia la posici�n y rotaci�n del waypoint de manera suave
        while (elapsedTime < cameraDuration)
        {
            cameraTransform.position = Vector3.Lerp(originalCameraPosition, cameraWaypoint.position, elapsedTime * cameraLerpSpeed / cameraDuration);
            cameraTransform.rotation = Quaternion.Lerp(originalCameraRotation, cameraWaypoint.rotation, elapsedTime * cameraLerpSpeed / cameraDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la c�mara est� exactamente en el waypoint al final de la transici�n
        cameraTransform.position = cameraWaypoint.position;
        cameraTransform.rotation = cameraWaypoint.rotation;

        // Esperar el tiempo de la transici�n
        yield return new WaitForSeconds(cameraDuration);

        elapsedTime = 0f;

        // Mover la c�mara de vuelta a su posici�n y rotaci�n original de manera suave
        while (elapsedTime < cameraDuration)
        {
            cameraTransform.position = Vector3.Lerp(cameraWaypoint.position, originalCameraPosition, elapsedTime * cameraLerpSpeed / cameraDuration);
            cameraTransform.rotation = Quaternion.Lerp(cameraWaypoint.rotation, originalCameraRotation, elapsedTime * cameraLerpSpeed / cameraDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la c�mara est� exactamente en la posici�n original al final de la transici�n
        cameraTransform.position = originalCameraPosition;
        cameraTransform.rotation = originalCameraRotation;

        // Reactivar el jugador
        player.SetActive(true);
    }
}
