using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("Bob Settings")]
    public float bobFrequency = 2.0f;      // Frecuencia del movimiento de bobbing
    public float bobHeight = 0.05f;        // Altura máxima del bobbing
    public float bobSideMovement = 0.02f;  // Movimiento lateral
    public float movementThreshold = 0.1f; // Velocidad mínima para activar el bobbing

    private float defaultYPos;             // Posición inicial en Y de la cámara
    private float bobTimer = 0.0f;         // Temporizador para el ciclo de bobbing
    private Vector3 defaultLocalPos;       // Posición inicial local de la cámara
    [SerializeField] private Rigidbody playerRigidbody;

    void Start()
    {
        // Guardamos la posición inicial de la cámara
        defaultLocalPos = transform.localPosition;
        defaultYPos = transform.localPosition.y;

        // Buscamos el Rigidbody en el objeto padre
        //playerRigidbody = GetComponentInParent<Rigidbody>();

        // Aseguramos que tenemos un Rigidbody en el personaje
        /*if (playerRigidbody == null)
        {
            Debug.LogWarning("No se encontró un Rigidbody en el objeto padre.");
        }*/
    }

    void Update()
    {
        if (playerRigidbody != null && playerRigidbody.velocity.magnitude > movementThreshold)
        {
            // Aumentamos el temporizador basado en el tiempo y la frecuencia
            bobTimer += Time.deltaTime * bobFrequency;

            // Calculamos el nuevo desplazamiento en Y y X usando funciones sinusoidales
            float bobOffsetY = Mathf.Sin(bobTimer) * bobHeight;
            float bobOffsetX = Mathf.Cos(bobTimer * 2) * bobSideMovement;

            // Aplicamos el desplazamiento a la posición inicial
            transform.localPosition = new Vector3(defaultLocalPos.x + bobOffsetX, defaultYPos + bobOffsetY, defaultLocalPos.z);
        }
        else
        {
            // Reiniciamos la posición de la cámara cuando no hay movimiento
            bobTimer = 0.0f;
            transform.localPosition = defaultLocalPos;
        }
    }
}
