using UnityEngine;
using TMPro;  // Necesario si est�s usando TextMeshPro
using UnityEngine.EventSystems;

public class MoveTextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textoMover;  // Referencia al texto (si es TextMeshPro)
    // Si usas el Text normal de Unity, usa: public Text textoMover;

    public float posicionFinalX; // Define a qu� posici�n en X quieres mover el texto
    private float posicionInicialX; // Guarda la posici�n original en X

    private bool mouseEncima = false; // Estado del mouse sobre el bot�n
    private float velocidad = 5f; // Velocidad de interpolaci�n

    private void Start()
    {
        // Guardamos la posici�n inicial en X del texto
        posicionInicialX = textoMover.rectTransform.localPosition.x;
    }

    private void Update()
    {
        // Interpolaci�n lerpeada en el eje X
        float objetivoX = mouseEncima ? posicionFinalX : posicionInicialX;
        Vector3 nuevaPosicion = new Vector3(objetivoX, textoMover.rectTransform.localPosition.y, textoMover.rectTransform.localPosition.z);
        textoMover.rectTransform.localPosition = Vector3.Lerp(textoMover.rectTransform.localPosition, nuevaPosicion, Time.deltaTime * velocidad);
    }

    // Evento cuando el mouse entra en el bot�n
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEncima = true; // Cambiamos el estado a verdadero
    }

    // Evento cuando el mouse sale del bot�n
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseEncima = false; // Cambiamos el estado a falso
    }
}
