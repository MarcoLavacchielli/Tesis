using UnityEngine;

public class DiamondController : MonoBehaviour
{
    [SerializeField] private GameObject teleporter;
    public float pickUpRadius = 5f;
    private GameObject player;
    [SerializeField] private GameObject AlwaysVisibleDiamond;
    [SerializeField] private DistanceDisplay distanceDisplay;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                    GlassController.Instance.DestroyButtonText();
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}