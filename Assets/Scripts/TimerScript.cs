using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float startDelay = 2f; // Segundos antes de iniciar el timer
    [SerializeField] private TextMeshProUGUI currentTimerText; // Referencia al texto TMP actual
    [SerializeField] private TextMeshProUGUI previousTimerText; // Referencia al texto TMP pausado

    [Header("Player Reference")]
    [SerializeField] private GameObject player; // Referencia al jugador

    private float elapsedTime = 0f; // Tiempo transcurrido del temporizador actual
    private bool isTimerRunning = false; // Estado del temporizador
    private bool hasStarted = false; // Controla si ya inició alguna vez

    void Start()
    {
        if (currentTimerText == null || previousTimerText == null)
        {
            Debug.LogError("¡Falta la referencia a uno o más textos TMP!");
            return;
        }

        // Comenzar el timer después de la demora inicial
        StartCoroutine(StartTimerAfterDelay());
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateCurrentTimerText();
        }

        // Detectar cambios en el estado del jugador
        if (player != null && hasStarted)
        {
            if (!player.activeInHierarchy && isTimerRunning)
            {
                PauseAndMoveToPreviousText();
            }
            else if (player.activeInHierarchy && !isTimerRunning)
            {
                ResetAndStartNewTimer();
            }
        }
    }

    private void UpdateCurrentTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

        currentTimerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    private void PauseAndMoveToPreviousText()
    {
        isTimerRunning = false;
        previousTimerText.text = currentTimerText.text; // Pasar el tiempo actual al texto de "previo"
    }

    private void ResetAndStartNewTimer()
    {
        elapsedTime = 0f; // Reiniciar el tiempo transcurrido
        isTimerRunning = true;
    }

    private System.Collections.IEnumerator StartTimerAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        isTimerRunning = true;
        hasStarted = true;
    }
}
