using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAssistMode : MonoBehaviour
{
    [SerializeField] private Toggle difficultyToggle; // El Toggle del UI
    [SerializeField] private JsonSaveGameManager saveGameManager; // Referencia al manager de guardado

    void Start()
    {
        // Busca el JsonSaveGameManager en la escena si no está asignado
        if (saveGameManager == null)
        {
            saveGameManager = FindObjectOfType<JsonSaveGameManager>();
        }

        if (saveGameManager == null)
        {
            Debug.LogError("JsonSaveGameManager no encontrado en la escena.");
            return;
        }

        // Configura el estado inicial del Toggle basado en el valor de la key
        difficultyToggle.isOn = saveGameManager.saveData.GetDifficulty();

        // Suscríbete al evento onValueChanged del Toggle
        difficultyToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        // Actualiza el valor de easyMode en el save data
        saveGameManager.saveData.SetDifficulty(isOn);

        // Guarda el cambio en el archivo JSON
        saveGameManager.SaveGame();
    }
}
