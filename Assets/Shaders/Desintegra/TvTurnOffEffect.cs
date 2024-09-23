using UnityEngine;
using UnityEngine.UI;

public class TvTurnOffEffect : MonoBehaviour
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
    private bool effectActive = false;

    void Start()
    {
        blackScreenRect = blackScreen.GetComponent<RectTransform>();
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);  // Comienza transparente
        blackScreen.enabled = false;  // Desactiva la pantalla negra inicialmente
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            TriggerTVEffect();
        }*/
    }

    // Llama a esta función desde otro script para activar el efecto
    public void TriggerTVEffect()
    {
        if (!effectActive)
        {
            StartEffect();  // Comienza el efecto si no está activo
            StartCoroutine(RunTVEffect());
        }
    }

    private void StartEffect()
    {
        blackScreen.enabled = true;  // Activa la pantalla negra
        elapsedTime = 0f;
        effectFinished = false;
        effectActive = true;
        alphaFadingStarted = false;
    }

    private System.Collections.IEnumerator RunTVEffect()
    {
        while (!effectFinished)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime <= effectDuration)
            {
                // Comienza a aumentar el alpha del color cuando inicie el efecto
                if (!alphaFadingStarted)
                {
                    alphaFadingStarted = true;
                }

                // Si el alpha fading ha empezado, aumenta el alpha progresivamente
                if (alphaFadingStarted)
                {
                    float alphaValue = Mathf.Lerp(0, 1, elapsedTime / 1f);  // Aumenta de 0 a 1 en un segundo
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
                // Una vez que el efecto ha terminado, se considera completo
                effectFinished = true;
                effectActive = false;
                //blackScreen.enabled = false;  // Desactiva la pantalla negra al final OPCIONAL
            }

            yield return null;  // Espera al siguiente frame
        }
    }

    private void CreateGlitchEffect()
    {
        for (int i = 0; i < numberOfGlitches; i++)
        {
            float randomX = Random.Range(-blackScreenRect.rect.width / 2, blackScreenRect.rect.width / 2);
            float randomWidth = Random.Range(3f, 15f);  // Ancho aleatorio para las barras
            float randomHeight = Random.Range(10f, 100f);  // Altura aleatoria para las barras

            GameObject glitch = new GameObject("GlitchLine");
            glitch.transform.SetParent(blackScreen.transform);

            Image glitchImage = glitch.AddComponent<Image>();
            glitchImage.color = new Color(0f, 1f, 0f, 0.7f);  // Verde transparente (inspirado en Matrix)

            RectTransform glitchRect = glitch.GetComponent<RectTransform>();
            glitchRect.sizeDelta = new Vector2(randomWidth, randomHeight);  // Líneas de diferentes tamaños
            glitchRect.anchoredPosition = new Vector2(randomX, Random.Range(-blackScreenRect.rect.height / 2, blackScreenRect.rect.height / 2));

            Destroy(glitch, Random.Range(0.1f, glitchEffectDuration));  // Destruye el glitch después de un breve momento
        }
    }

    private void ShakeScreen()
    {
        // Simula un efecto de ruido Perlin para la sacudida
        float shakeX = Mathf.PerlinNoise(Time.time * 10, 0) * shakeAmount - shakeAmount / 2;
        float shakeY = Mathf.PerlinNoise(0, Time.time * 10) * shakeAmount - shakeAmount / 2;

        blackScreen.transform.localPosition = new Vector3(shakeX, shakeY, 0);
    }
}
