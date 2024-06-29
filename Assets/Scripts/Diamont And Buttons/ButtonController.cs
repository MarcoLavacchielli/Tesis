using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private Material activeMaterial; // Cambiado a material

    AudioManager audioM;

    private Material originalMaterial;
    private Renderer rend;
    private bool playerNearby;
    private bool isActive = false;

    public bool IsActive { get { return isActive; } }

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;

        audioM = FindObjectOfType<AudioManager>();

        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }
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
        rend.material = originalMaterial;
    }

    private void SetButtonActive()
    {
        isActive = true;
        rend.material = activeMaterial; // Cambiado a material
        audioM.PlaySfx(5);
        GlassController.Instance.CheckButtons();
    }
}