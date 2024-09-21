using UnityEngine;
using UnityEngine.UI;

public class TVTurnOnSmEffect : MonoBehaviour
{
    public Image blackScreen;  // Asigna tu imagen negra desde el inspector
    public float glitchFrequency = 0.1f;  // Frecuencia de los glitches
    public float effectDuration = 3f;  // Duración total del efecto
    public int numberOfGlitches = 20;  // Cantidad de glitches/lineas
    public float glitchEffectDuration = 0.2f;  // Duración del glitch
    public float shakeAmount = 5f;  // Cantidad de sacudida

    private float elapsedTime = 0f;
    private RectTransform blackScreenRect;
    private bool effectFinished = false;
    private bool alphaFadingStarted = false;

    void Start()
    {
        blackScreenRect = blackScreen.GetComponent<RectTransform>();
        blackScreen.color = Color.black;  // Comienza completamente negro
    }

    void Update()
    {
        if (!effectFinished)
        {
            elapsedTime += Time.deltaTime;

            // Comienza a crear glitches hasta que termine la duración del efecto
            if (elapsedTime <= effectDuration)
            {
                // Empieza a disminuir el alpha del color cuando quede 1 segundo de animación
                if (elapsedTime >= effectDuration - 1f && !alphaFadingStarted)
                {
                    alphaFadingStarted = true;
                }

                // Si el alpha fading ha empezado, reduce el alpha progresivamente
                if (alphaFadingStarted)
                {
                    float alphaValue = Mathf.Lerp(1, 0, (elapsedTime - (effectDuration - 1f)) / 1f);
                    blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alphaValue);
                }

                // Genera glitches de manera aleatoria
                if (Random.value < glitchFrequency)
                {
                    CreateGlitchEffect();
                }

                // Sacudir la pantalla con ruido Perlin
                ShakeScreen();
            }
            else
            {
                // Una vez que el efecto ha terminado, apaga el script
                effectFinished = true;
                blackScreen.enabled = false;  // Desactiva la imagen negra
                this.enabled = false;  // Desactiva el script
            }
        }
    }

    void CreateGlitchEffect()
    {
        // Simula un efecto de glitch o líneas verticales apareciendo en la pantalla
        for (int i = 0; i < numberOfGlitches; i++)
        {
            // Genera una serie de "glitches" en posiciones aleatorias
            float randomX = Random.Range(-blackScreenRect.rect.width / 2, blackScreenRect.rect.width / 2);
            float randomWidth = Random.Range(3f, 15f);  // Ancho aleatorio para las barras
            float randomHeight = Random.Range(10f, 100f);  // Altura aleatoria para las barras

            // Crea un glitch visual (puede ser un rectángulo o línea)
            GameObject glitch = new GameObject("GlitchLine");
            glitch.transform.SetParent(blackScreen.transform);

            Image glitchImage = glitch.AddComponent<Image>();
            glitchImage.color = new Color(0f, 1f, 0f, 0.7f);  // Verde transparente (inspirado en Matrix)

            RectTransform glitchRect = glitch.GetComponent<RectTransform>();
            glitchRect.sizeDelta = new Vector2(randomWidth, randomHeight);  // Líneas de diferentes tamaños
            glitchRect.anchoredPosition = new Vector2(randomX, Random.Range(-blackScreenRect.rect.height / 2, blackScreenRect.rect.height / 2));

            // Destruye el glitch después de un breve momento
            Destroy(glitch, Random.Range(0.1f, glitchEffectDuration));
        }
    }

    void ShakeScreen()
    {
        // Simula un efecto de ruido Perlin para la sacudida
        float shakeX = Mathf.PerlinNoise(Time.time * 10, 0) * shakeAmount - shakeAmount / 2;
        float shakeY = Mathf.PerlinNoise(0, Time.time * 10) * shakeAmount - shakeAmount / 2;

        blackScreen.transform.localPosition = new Vector3(shakeX, shakeY, 0);
    }
}
