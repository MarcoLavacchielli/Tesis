using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserRay : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserDistance = 8f;
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private UnityEvent OnHitTarget;
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private Vector2 detectionRange = new Vector2(5f, 5f);
    [SerializeField] private Vector3 detectionOffset = Vector3.zero;
    [SerializeField] private Vector3 detectionInclination = Vector3.zero;
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] public bool alwaysUpdateLineRenderer = false;
    [SerializeField] private float animationSpeed = 5f;

    private RaycastHit rayHit;
    private Ray ray;
    private GameObject instantiatedStartPrefab;
    private GameObject instantiatedHitPrefab;
    private Queue<GameObject> hitPrefabPool = new Queue<GameObject>();
    private float lastUpdateTime;
    private GameObject lastHitObject = null;
    private float currentLineLength = 0f;
    private bool isAnimating = false;
    private bool hasHit = false;
    private bool hitPrefabInstantiated = false; // New flag to check if hitPrefab has been instantiated

    private void Awake()
    {
        lineRenderer.positionCount = 2;

        if (startPrefab != null && instantiatedStartPrefab == null)
        {
            instantiatedStartPrefab = Instantiate(startPrefab, transform.position, Quaternion.identity);
        }

        // Perform an initial raycast to draw the LineRenderer immediately
        PerformRaycast(forceUpdate: true);
    }

    private void Update()
    {
        if (IsPlayerWithinRange())
        {
            if (Time.time >= lastUpdateTime + updateInterval)
            {
                lastUpdateTime = Time.time;
                PerformRaycast();
            }
        }
        else if (Time.time >= lastUpdateTime + updateInterval)
        {
            PerformRaycast();
        }

        if (isAnimating)
        {
            AnimateLaser();
        }
    }

    private void PerformRaycast(bool forceUpdate = false)
    {
        ray = new Ray(transform.position, transform.forward);

        if (instantiatedStartPrefab != null)
        {
            instantiatedStartPrefab.transform.position = transform.position;
        }

        if (Physics.Raycast(ray, out rayHit, laserDistance, ~ignoreMask))
        {
            if (!hasHit)
            {
                // Start animation only if it hasn't hit before
                currentLineLength = 0f;
                isAnimating = true;
                hasHit = true;
                hitPrefabInstantiated = false; // Reset hitPrefabInstantiated flag
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * currentLineLength);

            if (!hitPrefabInstantiated && !isAnimating)
            {
                // Instantiate hitPrefab when the animation is done
                if (hitPrefab != null)
                {
                    Quaternion rotation = Quaternion.Euler(0, 180, 0);
                    instantiatedHitPrefab = GetPooledHitPrefab(rayHit.point, rotation);
                    hitPrefabInstantiated = true;
                }
            }

            if (hitPrefab != null && instantiatedHitPrefab != null)
            {
                var trailRenderer = instantiatedHitPrefab.GetComponent<TrailRenderer>();
                if (trailRenderer != null)
                {
                    trailRenderer.enabled = false;
                }

                instantiatedHitPrefab.transform.position = rayHit.point;

                if (trailRenderer != null)
                {
                    trailRenderer.enabled = true;
                }

                if (!instantiatedHitPrefab.activeSelf)
                {
                    instantiatedHitPrefab.SetActive(true);
                }
            }

            lastHitObject = rayHit.collider.gameObject;

            if (!forceUpdate && rayHit.collider.TryGetComponent(out Target target))
            {
                target.Hit();
                OnHitTarget?.Invoke();
            }
        }
        else
        {
            if (isAnimating)
            {
                currentLineLength = laserDistance;
                isAnimating = false;
                hitPrefabInstantiated = false; // Reset flag to allow hitPrefab instantiation
            }
            else
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + transform.forward * laserDistance);
            }

            if (instantiatedHitPrefab != null)
            {
                instantiatedHitPrefab.SetActive(false);
            }

            lastHitObject = null;
        }
    }

    private void AnimateLaser()
    {
        if (isAnimating)
        {
            currentLineLength += animationSpeed * Time.deltaTime;

            if (currentLineLength >= Vector3.Distance(transform.position, rayHit.point))
            {
                currentLineLength = Vector3.Distance(transform.position, rayHit.point);
                isAnimating = false; // Stop animation when the full length is reached
                hitPrefabInstantiated = false; // Allow hitPrefab to be instantiated after animation
            }

            lineRenderer.SetPosition(1, transform.position + transform.forward * currentLineLength);
        }
    }

    private GameObject GetPooledHitPrefab(Vector3 position, Quaternion rotation)
    {
        if (hitPrefabPool.Count > 0)
        {
            GameObject pooledObject = hitPrefabPool.Dequeue();
            pooledObject.SetActive(true);
            pooledObject.transform.position = position;
            pooledObject.transform.rotation = rotation;
            return pooledObject;
        }
        else
        {
            return Instantiate(hitPrefab, position, rotation);
        }
    }

    private void ReturnHitPrefabToPool(GameObject obj)
    {
        obj.SetActive(false);
        hitPrefabPool.Enqueue(obj);
    }

    private bool IsPlayerWithinRange()
    {
        Vector3 detectionCenter = transform.position + detectionOffset;
        Quaternion inclinationRotation = Quaternion.Euler(detectionInclination);
        Collider[] hits = Physics.OverlapBox(detectionCenter, new Vector3(detectionRange.x / 2, 1, detectionRange.y / 2), inclinationRotation, LayerMask.GetMask("Player"));
        return hits.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 detectionCenter = transform.position + detectionOffset;
        Quaternion inclinationRotation = Quaternion.Euler(detectionInclination);
        Gizmos.matrix = Matrix4x4.TRS(detectionCenter, inclinationRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(detectionRange.x, 1, detectionRange.y));

        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * laserDistance);

        if (rayHit.collider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rayHit.point, 0.23f);
        }
    }
}
