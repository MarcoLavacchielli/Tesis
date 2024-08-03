using UnityEngine;
using System.Collections.Generic;

public class DiamondController : MonoBehaviour
{
    [SerializeField] private GameObject teleporter;
    public float pickUpRadius = 5f;
    private GameObject player;
    [SerializeField] private GameObject AlwaysVisibleDiamond;
    [SerializeField] private DistanceDisplay distanceDisplay;
    [SerializeField] private List<Light> lights; // Lista de luces

    AudioManager audioM;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        audioM = FindObjectOfType<AudioManager>();

        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }
    }

    void Update()
    {
        if (GlassController.Instance.IsGlassDestroyed)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= pickUpRadius)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(AlwaysVisibleDiamond);
                    distanceDisplay.MyShutdown();
                    Destroy(gameObject);
                    teleporter.SetActive(true);
                    GlassController.Instance.UpdateObjectiveText("Objective: Escape");
                    changeMusic();
                    GlassController.Instance.DestroyButtonText();

                    if (GlassController.Instance.diamontText != null)
                    {
                        GlassController.Instance.diamontText.SetActive(false);
                    }

                    // Cambiar el color de las luces a rojo
                    foreach (var light in lights)
                    {
                        light.color = Color.red;
                    }
                }
            }
        }
    }

    void changeMusic()
    {
        audioM.StopMusic(0);
        audioM.PlayMusic(1);
        audioM.PlaySfx(7);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}
