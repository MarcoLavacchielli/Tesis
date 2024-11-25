using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float startDelay = 2f; // Segundos antes de iniciar el temporizador
    [SerializeField] private TextMeshProUGUI currentTimerText; // Texto TMP del temporizador actual
    [SerializeField] private TextMeshProUGUI previousTimerText; // Texto TMP del temporizador pausado

    [Header("Player Script Reference")]
    [SerializeField] private Grappling playerMovementScript; // Referencia al script `PlayerMovementGrappling`

    private float elapsedTime = 0f; // Tiempo transcurrido del temporizador actual
    private bool isTimerRunning = false; // Estado del temporizador
    private bool hasStarted = false; // Controla si ya inició alguna vez
    private bool wasScriptEnabled = true; // Estado anterior del script para detectar cambios

    void Start()
    {
        if (currentTimerText == null || previousTimerText == null || playerMovementScript == null)
        {
            Debug.LogError("¡Falta la referencia a uno o más componentes!");
            return;
        }

        // Comenzar el temporizador después de la demora inicial
        StartCoroutine(StartTimerAfterDelay());
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateCurrentTimerText();
        }

        // Detectar cambios en el estado del script PlayerMovementGrappling
        if (hasStarted)
        {
            bool isScriptEnabled = playerMovementScript.enabled;

            if (!isScriptEnabled && wasScriptEnabled) // Si el script acaba de desactivarse
            {
                PauseAndStoreTime();
            }
            else if (isScriptEnabled && !wasScriptEnabled) // Si el script acaba de activarse
            {
                ResetAndStartNewTimer();
            }

            // Actualizar el estado anterior del script
            wasScriptEnabled = isScriptEnabled;
        }
    }

    private void UpdateCurrentTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

        currentTimerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    private void PauseAndStoreTime()
    {
        // Pausar el temporizador
        isTimerRunning = false;

        // Guardar el tiempo actual en el texto de "previo"
        previousTimerText.text = currentTimerText.text;

        elapsedTime = 0f;
        UpdateCurrentTimerText();
    }

    private void ResetAndStartNewTimer()
    {
        // Reiniciar el temporizador actual
        elapsedTime = 0f;
        UpdateCurrentTimerText(); // Actualizar inmediatamente para mostrar 00:00:000
        isTimerRunning = true;
    }

    private System.Collections.IEnumerator StartTimerAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        isTimerRunning = true;
        hasStarted = true;
    }
}
