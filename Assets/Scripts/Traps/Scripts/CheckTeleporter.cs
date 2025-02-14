using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTeleporter : MonoBehaviour
{
    [SerializeField] private List<Vector3> waypoints;
    [SerializeField] private PlayerMovementGrappling player;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Grappling grapplingScript;
    [SerializeField] private Material teleporterMaterial; // Referencia al material del shader

    public int currentWaypointIndex = 0;
    private AudioManager audioM;

    private void Awake()
    {
        currentWaypointIndex = 0;
        audioM = FindObjectOfType<AudioManager>();

        // Asegurar que el shader comienza con _IsActive en 0
        if (teleporterMaterial != null)
        {
            teleporterMaterial.SetFloat("_IsActive", 0);
        }
    }

    public void ReachNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count - 1)
        {
            currentWaypointIndex++;
            Debug.Log("Nuevo waypoint alcanzado: " + waypoints[currentWaypointIndex]);
        }
    }

    public void Death()
    {
        Debug.Log("Spike trap activated!");
        grapplingScript.InterruptGrapple();

        if (playerRb != null && waypoints.Count > 0)
        {
            playerRb.isKinematic = true;
            player.transform.position = waypoints[currentWaypointIndex];
            Debug.Log("Jugador teletransportado al punto de origen: " + waypoints[currentWaypointIndex]);
            audioM.PlaySfx(15);

            // Activar el shader inmediatamente
            if (teleporterMaterial != null)
            {
                teleporterMaterial.SetFloat("_IsActive", 1);
                StartCoroutine(ResetShader());
            }

            StartCoroutine(ResetRigidbody());
        }
    }

    private IEnumerator ResetShader()
    {
        yield return new WaitForSeconds(0.25f);

        float duration = 0.5f;
        float elapsed = 0f;
        float startValue = 1;
        float endValue = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            teleporterMaterial.SetFloat("_IsActive", Mathf.Lerp(startValue, endValue, t));
            yield return null;
        }

        teleporterMaterial.SetFloat("_IsActive", 0);
    }

    private IEnumerator ResetRigidbody()
    {
        yield return null;
        playerRb.isKinematic = false;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        Debug.Log("Movimiento del jugador habilitado después de la teletransportación.");
    }
}
