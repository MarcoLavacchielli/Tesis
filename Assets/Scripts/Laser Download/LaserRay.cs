using UnityEngine;
using UnityEngine.Events;

public class LaserRay : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserDistance = 8f;
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private UnityEvent OnHitTarget;
    [SerializeField] private GameObject startPrefab; // Prefab para el inicio del láser
    [SerializeField] private GameObject hitPrefab;   // Prefab para el punto de impacto

    private RaycastHit rayHit;
    private Ray ray;
    private GameObject instantiatedStartPrefab; // Referencia al startPrefab instanciado
    private GameObject instantiatedHitPrefab;   // Referencia al hitPrefab instanciado

    private void Awake()
    {
        lineRenderer.positionCount = 2;

        // Instanciar el prefab en el inicio del láser una vez
        if (startPrefab != null && instantiatedStartPrefab == null)
        {
            instantiatedStartPrefab = Instantiate(startPrefab, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        ray = new Ray(transform.position, transform.forward);

        // Actualizar la posición del startPrefab
        if (instantiatedStartPrefab != null)
        {
            instantiatedStartPrefab.transform.position = transform.position;
        }

        if (Physics.Raycast(ray, out rayHit, laserDistance, ~ignoreMask))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, rayHit.point);

            // Instanciar el prefab en el punto de impacto una vez y actualizar su posición
            if (hitPrefab != null)
            {
                if (instantiatedHitPrefab == null)
                {
                    // Instancia el prefab la primera vez
                    Quaternion rotation = Quaternion.Euler(0, 180, 0); // Rotación de 180 grados en el eje Y
                    instantiatedHitPrefab = Instantiate(hitPrefab, rayHit.point, rotation);
                }
                else
                {
                    // Actualiza la posición del prefab instanciado
                    instantiatedHitPrefab.transform.position = rayHit.point;
                }
            }

            // Debe existir un script MonoBehaviour llamado Target con un método público Hit
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
