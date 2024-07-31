using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceDisplay : MonoBehaviour
{
    public Transform player; // Referencia al transform del jugador
    public Transform target; // Referencia al transform del objeto objetivo
    public Transform AlwaysVisibleDiamond;
    public TextMeshProUGUI distanceText; // Referencia al UI Text
                                         // Escala mínima y máxima
    public Vector3 minScale = new Vector3(10, 10, 10);
    public Vector3 maxScale = new Vector3(80, 80, 80);

    // Distancias para la interpolación
    public float minDistance = 50f;
    public float maxDistance = 100f;

    void Update()
    {
        // Calcula la distancia entre el jugador y el objetivo
        float distance = Vector3.Distance(player.position, target.position);

        // Actualiza el texto con la distancia en metros
        distanceText.text = distance.ToString("F2") + " meters";

        // Ajusta la escala del objetivo en función de la distancia
        float t = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
        AlwaysVisibleDiamond.localScale = Vector3.Lerp(minScale, maxScale, t);


    }
    public void MyShutdown()
    {
        distanceText.text = "";
        this.gameObject.SetActive(false);
    }
}
