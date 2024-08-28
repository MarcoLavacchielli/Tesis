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

    private RaycastHit rayHit;
    private Ray ray;
    private GameObject instantiatedStartPrefab;
    private GameObject instantiatedHitPrefab;
    private Queue<GameObject> hitPrefabPool = new Queue<GameObject>();
    [SerializeField] private float updateInterval = 0.1f;
    private float lastUpdateTime;

    private void Awake()
    {
        lineRenderer.positionCount = 2;

        if (startPrefab != null && instantiatedStartPrefab == null)
        {
            instantiatedStartPrefab = Instantiate(startPrefab, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Time.time >= lastUpdateTime + updateInterval)
        {
            lastUpdateTime = Time.time;
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        ray = new Ray(transform.position, transform.forward);

        if (instantiatedStartPrefab != null)
        {
            instantiatedStartPrefab.transform.position = transform.position;
        }

        if (Physics.Raycast(ray, out rayHit, laserDistance, ~ignoreMask))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, rayHit.point);

            if (hitPrefab != null)
            {
                if (instantiatedHitPrefab == null)
                {
                    Quaternion rotation = Quaternion.Euler(0, 180, 0);
                    instantiatedHitPrefab = GetPooledHitPrefab(rayHit.point, rotation);
                }
                else
                {
                    instantiatedHitPrefab.transform.position = rayHit.point;
                }
            }

            if (rayHit.collider.TryGetComponent(out Target target))
            {
                target.Hit();
                OnHitTarget?.Invoke();
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * laserDistance);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * laserDistance);

        if (rayHit.collider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rayHit.point, 0.23f);
        }
    }
}
