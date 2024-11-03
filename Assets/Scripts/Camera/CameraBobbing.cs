using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("Bob Settings")]
    public float bobFrequency = 2.0f;      // Frecuencia del movimiento de bobbing
    public float bobHeight = 0.05f;        // Altura m�xima del bobbing
    public float bobSideMovement = 0.02f;  // Movimiento lateral
    public float movementThreshold = 0.1f; // Velocidad m�nima para activar el bobbing

    private float defaultYPos;             // Posici�n inicial en Y de la c�mara
    private float bobTimer = 0.0f;         // Temporizador para el ciclo de bobbing
    private Vector3 defaultLocalPos;       // Posici�n inicial local de la c�mara
    [SerializeField] private Rigidbody playerRigidbody;

    void Start()
    {
        // Guardamos la posici�n inicial de la c�mara
        defaultLocalPos = transform.localPosition;
        defaultYPos = transform.localPosition.y;

        // Buscamos el Rigidbody en el objeto padre
        //playerRigidbody = GetComponentInParent<Rigidbody>();

        // Aseguramos que tenemos un Rigidbody en el personaje
        /*if (playerRigidbody == null)
        {
            Debug.LogWarning("No se encontr� un Rigidbody en el objeto padre.");
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

            // Aplicamos el desplazamiento a la posici�n inicial
            transform.localPosition = new Vector3(defaultLocalPos.x + bobOffsetX, defaultYPos + bobOffsetY, defaultLocalPos.z);
        }
        else
        {
            // Reiniciamos la posici�n de la c�mara cuando no hay movimiento
            bobTimer = 0.0f;
            transform.localPosition = defaultLocalPos;
        }
    }
}
