using UnityEngine;
using System.Collections.Generic;

public class ShowHideSprite : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> spriteObjects; // Lista de objetos con sprites
    public float maxDistance = 5f;
    public float minDistance = 1f;

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private bool isPlayerInRange = false;

    void Start()
    {
        // Obtener los SpriteRenderer de cada objeto en la lista
        foreach (GameObject spriteObject in spriteObjects)
        {
            SpriteRenderer renderer = spriteObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                spriteRenderers.Add(renderer);
                SetSpriteAlpha(renderer, 0);
            }
        }
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            foreach (GameObject spriteObject in spriteObjects)
            {
                float distance = Vector3.Distance(player.transform.position, spriteObject.transform.position);

                // Calcular la transparencia en función de la distancia
                float alpha = Mathf.Clamp01((maxDistance - distance) / (maxDistance - minDistance));

                // Ajustar la transparencia del sprite
                SetSpriteAlpha(spriteObject.GetComponent<SpriteRenderer>(), alpha);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                SetSpriteAlpha(renderer, 0);
            }
        }
    }

    void SetSpriteAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
