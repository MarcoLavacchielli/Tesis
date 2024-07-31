using UnityEngine;
using UnityEngine.UI;

public class DiamondController : MonoBehaviour
{
    [SerializeField] private GameObject teleporter;
    public float pickUpRadius = 5f;
    private GameObject player;
    [SerializeField] GameObject AlwaysVisibleDiamond;
    [SerializeField] DistanceDisplay distanceDisplay;

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