using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private Color activeColor = Color.green;

    private Color originalColor;
    private Renderer rend;
    private bool playerNearby;
    private bool isActive = false;

    public bool IsActive { get { return isActive; } }

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(activationKey))
        {
            SetButtonActive();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void OnDestroy()
    {
        rend.material.color = originalColor;
    }

    private void SetButtonActive()
    {
        isActive = true;
        rend.material.color = activeColor;
        GlassController.Instance.CheckButtons();
    }
}