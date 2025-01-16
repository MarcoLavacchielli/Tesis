using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public LockColumn[] columnas; // Array de columnas
    private int columnaActual = 0; // Índice de la columna activa
    public float interactionRadius = 5f;   // Radio de interacción
    public LayerMask playerLayerMask;      // LayerMask para el jugador

    //[SerializeField] private Material activeMaterial;
    [SerializeField] private GameObject objectWithDisolveShader; // Referencia al objeto con el shader
    [SerializeField] private GameObject DestroyObject;
    [SerializeField] private float disolveDuration = 5.0f; // Duración de la animación del disolve
    [SerializeField] private float initialDisolveValue = -1.5f; // Valor inicial del disolve
    [SerializeField] private float targetDisolveValue = 1.0f; // Valor final del disolve

    public AudioManager audioM;
    private Material originalMaterial;
    private Renderer rend;
    private Collider objectCollider;

    void Start()
    {
        ActivarColumnaActual();

        // Configurar el material original del objeto
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalMaterial = rend.material;
        }

        // Configurar el collider del objeto con el shader
        if (objectWithDisolveShader != null)
        {
            objectCollider = objectWithDisolveShader.GetComponent<Collider>();
            if (objectCollider == null)
            {
                Debug.LogError("El objeto con el shader no tiene un collider asignado!");
            }
        }

        if (DestroyObject == null)
        {
            Debug.Log("No se ha asignado un objeto para destruir!");
        }

        audioM = FindObjectOfType<AudioManager>();
        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }
    }

    void Update()
    {
        // Comprobar si el jugador está en el rango y presiona Enter
        if (IsPlayerInRange() && Input.GetKeyDown(KeyCode.Return))
        {
            ProcesarColumnaActual();
        }
    }

    private void ProcesarColumnaActual()
    {
        if (columnaActual >= 0 && columnaActual < columnas.Length)
        {
            if (columnas[columnaActual].TryStopColumn())
            {
                // Columna resuelta correctamente
                Debug.Log("Código correcto. Avanzando a la siguiente columna.");
                columnaActual++;
                if (columnaActual >= columnas.Length)
                {
                    // Puzzle completado
                    Debug.Log("¡Puzzle resuelto! Desbloqueando...");
                    ActivarDesbloqueo(); // Llamar al método de desbloqueo
                }
                else
                {
                    ActivarColumnaActual();
                }
            }
            else
            {
                // Código incorrecto, retroceder una columna
                Debug.Log("Código incorrecto. Retrocediendo a la columna anterior.");
                if (columnaActual > 0)
                {
                    columnaActual--;
                }
                ActivarColumnaActual();
            }
        }
    }

    private void ActivarColumnaActual()
    {
        for (int i = 0; i < columnas.Length; i++)
        {
            if (i == columnaActual)
            {
                columnas[i].ResetColumn();
            }
        }
    }

    // Función que verifica si el jugador está dentro del radio de interacción
    private bool IsPlayerInRange()
    {
        // Buscar al jugador en la escena por su capa
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return false;

        // Verificar si el jugador está en la capa correcta usando LayerMask
        if ((playerLayerMask.value & (1 << playerObject.layer)) != 0)
        {
            float distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);
            return distanceToPlayer <= interactionRadius;
        }
        return false;
    }

    // Dibuja el Gizmo en la escena para visualizar el radio de interacción
    private void OnDrawGizmos()
    {
        // Configura el color del Gizmo
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Verde semitransparente
        // Dibuja la esfera de interacción
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void ActivarDesbloqueo()
    {
        // Cambiar material al activo
        /*if (rend != null && activeMaterial != null)
        {
            rend.material = activeMaterial;
        }*/

        // Reproducir sonido
        if (audioM != null)
        {
            audioM.PlaySfx(5); // Puedes ajustar el índice del sonido
        }

        // Iniciar la animación de disolver
        if (objectWithDisolveShader != null)
        {
            StartCoroutine(DisolveObject());
        }

        // Destruir el objeto asignado
        if (DestroyObject != null)
        {
            Destroy(DestroyObject);
        }
    }

    private IEnumerator DisolveObject()
    {
        // Obtener el material del objeto con el shader
        Material disolveMaterial = objectWithDisolveShader.GetComponent<Renderer>().material;

        float elapsedTime = 0f;
        float currentDisolveValue = initialDisolveValue;

        // Reproducir un sonido adicional (opcional)
        if (audioM != null)
        {
            audioM.PlaySfx(12);
        }

        // Animar el valor de _DisolveAmount de -1.5 a 1
        while (elapsedTime < disolveDuration)
        {
            currentDisolveValue = Mathf.Lerp(initialDisolveValue, targetDisolveValue, elapsedTime / disolveDuration);
            disolveMaterial.SetFloat("_DisolveAmount", currentDisolveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que el valor final es exactamente 1
        disolveMaterial.SetFloat("_DisolveAmount", targetDisolveValue);

        // Desactivar el collider del objeto
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }
}
