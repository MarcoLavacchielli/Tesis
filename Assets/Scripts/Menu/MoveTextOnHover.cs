using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public class MoveTextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textoMover;
    public float posicionFinalX;
    private float posicionInicialX;
    private bool mouseEncima = false;
    private float velocidad = 5f;

    private Color colorInicial;
    private Color colorFinal = Color.cyan;

    private void Start()
    {
        posicionInicialX = textoMover.rectTransform.localPosition.x;
        colorInicial = textoMover.color;
    }

    private void Update()
    {
        float objetivoX = mouseEncima ? posicionFinalX : posicionInicialX;
        Vector3 nuevaPosicion = new Vector3(objetivoX, textoMover.rectTransform.localPosition.y, textoMover.rectTransform.localPosition.z);
        textoMover.rectTransform.localPosition = Vector3.Lerp(textoMover.rectTransform.localPosition, nuevaPosicion, Time.unscaledDeltaTime * velocidad);

        textoMover.color = Color.Lerp(textoMover.color, mouseEncima ? colorFinal : colorInicial, Time.unscaledDeltaTime * velocidad);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEncima = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseEncima = false;
    }

    private void OnDisable()
    {
        textoMover.rectTransform.localPosition = new Vector3(posicionInicialX, textoMover.rectTransform.localPosition.y, textoMover.rectTransform.localPosition.z);
        textoMover.color = colorInicial;
        mouseEncima = false;
    }
}
