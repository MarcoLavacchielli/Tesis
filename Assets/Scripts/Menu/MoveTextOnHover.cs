using UnityEngine;
using TMPro;  // Necesario si estás usando TextMeshPro
using UnityEngine.EventSystems;
using Color = UnityEngine.Color; // Importante para definir colores

public class MoveTextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textoMover;  // Referencia al texto (si es TextMeshPro)
    // Si usas el Text normal de Unity, usa: public Text textoMover;

    public float posicionFinalX; // Define a qué posición en X quieres mover el texto
    private float posicionInicialX; // Guarda la posición original en X

    private bool mouseEncima = false; // Estado del mouse sobre el botón
    private float velocidad = 5f; // Velocidad de interpolación

    private Color colorInicial; // Color original del texto
    private Color colorFinal = Color.green; // Color final cuando el mouse está sobre el texto

    private void Start()
    {
        // Guardamos la posición inicial en X del texto
        posicionInicialX = textoMover.rectTransform.localPosition.x;

        // Guardamos el color original del texto
        colorInicial = textoMover.color;
    }

    private void Update()
    {
        // Interpolación lerpeada en el eje X
        float objetivoX = mouseEncima ? posicionFinalX : posicionInicialX;
        Vector3 nuevaPosicion = new Vector3(objetivoX, textoMover.rectTransform.localPosition.y, textoMover.rectTransform.localPosition.z);
        textoMover.rectTransform.localPosition = Vector3.Lerp(textoMover.rectTransform.localPosition, nuevaPosicion, Time.deltaTime * velocidad);

        // Cambiar el color del texto
        textoMover.color = Color.Lerp(textoMover.color, mouseEncima ? colorFinal : colorInicial, Time.deltaTime * velocidad);
    }

    // Evento cuando el mouse entra en el botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEncima = true; // Cambiamos el estado a verdadero
    }

    // Evento cuando el mouse sale del botón
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseEncima = false; // Cambiamos el estado a falso
    }
}
