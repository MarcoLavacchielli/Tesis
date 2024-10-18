using UnityEngine;
using UnityEngine.UI;

public class TVTurnOnSmEffect : MonoBehaviour
{
    public Image blackScreen;
    public float glitchFrequency = 0.1f;  // Frecuencia de los glitches
    public float effectDuration = 3f;  // Duración total del efecto
    public int numberOfGlitches = 20;  // Cantidad de glitches/lineas
    public float glitchEffectDuration = 0.2f;  // Duración del glitch
    public float shakeAmount = 5f;  // Cantidad de sacudida
    public Color glitchLineColor = new Color(0f, 1f, 0f, 0.7f);  // Color de las líneas de glitch

    private float elapsedTime = 0f;
    private RectTransform blackScreenRect;
    private bool effectFinished = false;
    private bool alphaFadingStarted = false;

    void Start()
    {
        blackScreenRect = blackScreen.GetComponent<RectTransform>();
        blackScreen.color = Color.black;
    }

    void Update()
    {
        if (!effectFinished)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime <= effectDuration)
            {
                if (elapsedTime >= effectDuration - 1f && !alphaFadingStarted)
                {
                    alphaFadingStarted = true;
                }

                if (alphaFadingStarted)
                {
                    float alphaValue = Mathf.Lerp(1, 0, (elapsedTime - (effectDuration - 1f)) / 1f);
                    blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alphaValue);
                }

                if (Random.value < glitchFrequency)
                {
                    CreateGlitchEffect();
                }

                ShakeScreen();
            }
            else
            {
                effectFinished = true;
                blackScreen.enabled = false;
                this.enabled = false;
            }
        }
    }

    void CreateGlitchEffect()
    {
        for (int i = 0; i < numberOfGlitches; i++)
        {
            float randomX = Random.Range(-blackScreenRect.rect.width / 2, blackScreenRect.rect.width / 2);
            float randomWidth = Random.Range(3f, 15f);
            float randomHeight = Random.Range(10f, 100f);

            GameObject glitch = new GameObject("GlitchLine");
            glitch.transform.SetParent(blackScreen.transform);

            Image glitchImage = glitch.AddComponent<Image>();
            glitchImage.color = glitchLineColor;  // Utilizamos la variable para definir el color

            RectTransform glitchRect = glitch.GetComponent<RectTransform>();
            glitchRect.sizeDelta = new Vector2(randomWidth, randomHeight);
            glitchRect.anchoredPosition = new Vector2(randomX, Random.Range(-blackScreenRect.rect.height / 2, blackScreenRect.rect.height / 2));

            Destroy(glitch, Random.Range(0.1f, glitchEffectDuration));
        }
    }

    void ShakeScreen()
    {
        float shakeX = Mathf.PerlinNoise(Time.time * 10, 0) * shakeAmount - shakeAmount / 2;
        float shakeY = Mathf.PerlinNoise(0, Time.time * 10) * shakeAmount - shakeAmount / 2;

        blackScreen.transform.localPosition = new Vector3(shakeX, shakeY, 0);
    }
}
