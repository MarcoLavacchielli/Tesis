using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VigilantCam : MonoBehaviour
{
    [SerializeField]
    float viewRadius = 10f;

    [SerializeField]
    float senseRadius = 1.5f;

    [SerializeField]
    float viewAngle = 90f;

    [SerializeField]
    LayerMask wallMask;

    public Transform playerTransform;

    public EnergyBar energyBar;

    private CameraWatcher cameraWatcher;
    private Renderer renderer;

    private bool playerDetected = false;
    private Coroutine energyRecoveryCoroutine;

    private void Start()
    {
        cameraWatcher = GetComponent<CameraWatcher>();
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        bool currentlyDetected = InFieldOfView(playerTransform.transform.position) && InLineOfSight(playerTransform.transform.position);

        if (currentlyDetected)
        {
            Debug.Log("visto");

            energyBar.EnergyConsumptionFunction();

            cameraWatcher.SetPlayerDetected(true);
            renderer.material.color = Color.red;

            if (energyRecoveryCoroutine != null)
            {
                StopCoroutine(energyRecoveryCoroutine);
                energyRecoveryCoroutine = null;
            }
        }
        else
        {
            cameraWatcher.SetPlayerDetected(false);
            renderer.material.color = Color.white;

            if (playerDetected && energyRecoveryCoroutine == null)
            {
                energyRecoveryCoroutine = StartCoroutine(StartEnergyRecovery());
            }
        }

        playerDetected = currentlyDetected;
    }

    private IEnumerator StartEnergyRecovery()
    {
        yield return new WaitForSeconds(3f);

        while (!playerDetected)
        {
            energyBar.EnergyRecoveryFunction();
            yield return null;
        }

        energyRecoveryCoroutine = null;
    }

    public bool InFieldOfView(Vector3 point)
    {
        var dir = point - transform.position;
        if (dir.magnitude > viewRadius)
            return false;

        var angle = Vector3.Angle(transform.forward, dir);
        return angle <= viewAngle / 2;
    }

    public bool InLineOfSight(Vector3 point)
    {
        var dir = point - transform.position;
        return !Physics.Raycast(transform.position, dir, dir.magnitude, wallMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, senseRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        var vector = transform.forward * viewRadius;

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, viewAngle / 2, 0) * vector);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -viewAngle / 2, 0) * vector);
    }
}