using UnityEngine;

public class ShowHideSprite : MonoBehaviour
{
    public GameObject player;
    public GameObject spriteObject;
    public float maxDistance = 5f;
    public float minDistance = 1f;

    private SpriteRenderer spriteRenderer;
    private bool isPlayerInRange = false;

    void Start()
    {
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        SetSpriteAlpha(0);
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            float distance = Vector3.Distance(player.transform.position, spriteObject.transform.position);

            float alpha = Mathf.Clamp01((maxDistance - distance) / (maxDistance - minDistance));

            SetSpriteAlpha(alpha);
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
            SetSpriteAlpha(0);
        }
    }

    void SetSpriteAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}