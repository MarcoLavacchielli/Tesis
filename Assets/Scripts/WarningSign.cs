using UnityEngine;

public class WarningSign : MonoBehaviour
{
    public Material warningMaterial;   // Material HDRP/Lit que queremos modificar
    public float redTimer = 2f;        // Tiempo que el material se pone rojo
    public float whiteTimer = 2f;      // Tiempo que el material se pone blanco
    public float blinkTimer = 2f;      // Tiempo que el material parpadea
    public float redEmissiveIntensity = 500f;  // Intensidad de emisión para el color rojo
    public float whiteEmissiveIntensity = 200f; // Intensidad de emisión para el color blanco

    private Color whiteColor = Color.white;
    private Color redColor = Color.red;
    private float timer = 0f;          // Temporizador general para el ciclo
    private int cycleMode = 0;         // Control del ciclo (0: rojo, 1: blanco, 2: parpadeo)

    private void Start()
    {
        if (warningMaterial != null)
        {
            // Asegurarse de que el material HDRP tenga la emisión activada
            warningMaterial.EnableKeyword("_EMISSION");
        }
        // Inicializa en rojo al comenzar
        SetEmissionColor(redColor, redEmissiveIntensity);
    }

    private void Update()
    {
        // Controlar el ciclo mediante el temporizador
        timer += Time.deltaTime;

        switch (cycleMode)
        {
            case 0: // Modo Rojo
                if (timer >= redTimer)
                {
                    timer = 0f;  // Reiniciar temporizador
                    cycleMode = 1; // Cambiar a blanco
                    SetEmissionColor(whiteColor, whiteEmissiveIntensity); // Establecer color blanco
                }
                break;

            case 1: // Modo Blanco
                if (timer >= whiteTimer)
                {
                    timer = 0f;  // Reiniciar temporizador
                    cycleMode = 2; // Cambiar a parpadeo
                }
                break;

            case 2: // Modo Parpadeo entre Blanco y Rojo
                if (timer >= blinkTimer)
                {
                    timer = 0f;  // Reiniciar temporizador
                    cycleMode = 0; // Volver a rojo
                    SetEmissionColor(redColor, redEmissiveIntensity); // Establecer color rojo
                }
                else
                {
                    // Parpadeo entre blanco y rojo
                    float lerp = Mathf.PingPong(Time.time * 2f, 1);
                    Color blendedColor = Color.Lerp(whiteColor * whiteEmissiveIntensity, redColor * redEmissiveIntensity, lerp);
                    warningMaterial.SetColor("_EmissiveColor", blendedColor);
                }
                break;
        }
    }

    private void SetEmissionColor(Color color, float intensity)
    {
        if (warningMaterial != null)
        {
            // Escalar el color de emisión con la intensidad deseada
            Color emissiveColor = color * intensity;
            warningMaterial.SetColor("_EmissiveColor", emissiveColor);
        }
    }
}
