using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 teleportPos;

    [SerializeField] private bool toco = false;


    [SerializeField] private BlackScreenOnDeath loseCanvas;

    AudioManager audioM;

    private void Awake()
    {
        audioM = FindObjectOfType<AudioManager>();

        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            toco = true;
            player.transform.position = teleportPos;
            loseCanvas.fade();
            audioM.StopSFX(7);
        }
    }
}