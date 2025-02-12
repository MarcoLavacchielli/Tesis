using UnityEngine;
using UnityEngine.UI;

public class FloatingImage : MonoBehaviour
{
    public RectTransform imageTransform; // La imagen a mover
    public RectTransform canvasTransform; // El Canvas donde se mueve
    public float speed = 100f; // Velocidad de movimiento
    public float minChangeDirectionTime = 1f; // Tiempo m�nimo antes de cambiar direcci�n
    public float maxChangeDirectionTime = 6f; // Tiempo m�ximo antes de cambiar direcci�n
    public float rotationSpeed = 30f; // Velocidad de rotaci�n

    private Vector2 direction;
    private float timer;

    void Start()
    {
        if (imageTransform == null)
            imageTransform = GetComponent<RectTransform>();

        if (canvasTransform == null)
            canvasTransform = imageTransform.root.GetComponent<RectTransform>();

        SetRandomDirection();
        timer = Random.Range(minChangeDirectionTime, maxChangeDirectionTime);
    }

    void Update()
    {
        // Mover la imagen con suavizado
        imageTransform.anchoredPosition = Vector2.Lerp(imageTransform.anchoredPosition, imageTransform.anchoredPosition + direction * speed, Time.deltaTime);
        timer -= Time.deltaTime;

        // Rotar la imagen
        imageTransform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Verificar colisi�n con los bordes
        CheckBounds();

        // Cambiar direcci�n aleatoriamente despu�s de un tiempo aleatorio
        if (timer <= 0)
        {
            SetRandomDirection();
            timer = Random.Range(minChangeDirectionTime, maxChangeDirectionTime);
        }
    }

    void CheckBounds()
    {
        Vector2 pos = imageTransform.anchoredPosition;
        Vector2 size = imageTransform.sizeDelta / 2;
        Vector2 canvasSize = canvasTransform.sizeDelta / 2;

        if (pos.x - size.x < -canvasSize.x || pos.x + size.x > canvasSize.x)
        {
            direction.x *= -1; // Invertir direcci�n horizontal
        }
        if (pos.y - size.y < -canvasSize.y || pos.y + size.y > canvasSize.y)
        {
            direction.y *= -1; // Invertir direcci�n vertical
        }
    }

    void SetRandomDirection()
    {
        direction = Random.insideUnitCircle.normalized; // Direcci�n aleatoria
    }
}
