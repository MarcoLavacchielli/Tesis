using System.Collections;
using UnityEngine;

public class PowerSwitcher : MonoBehaviour
{
    [Header("General Settings")]
    public float grabDistance = 3f;
    public float throwForce = 10f;
    public Transform holdPoint;
    public LayerMask grabbableLayer;
    public float switchDistanceLimit = 5f;

    private GameObject grabbedObject;
    private Rigidbody grabbedRigidbody;
    private Vector3 lastPlayerPosition;

    [Header("Shader Settings")]
    public Material teleportMaterial;

    [SerializeField] CapsuleCollider charContr;

    private void Start()
    {
        lastPlayerPosition = transform.position;

        if (teleportMaterial != null)
        {
            teleportMaterial.SetFloat("_IsActive", 0f);
        }
    }

    private void Update()
    {
        HandleGrabInput();
        HandleThrowInput();
        HandleSwitchPositionInput();
        CheckIfPlayerTeleported();
    }

    private void FixedUpdate()
    {
        if (grabbedObject != null) MoveObjectSmoothly();
    }

    private void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedObject == null) TryGrabObject();
            else ReleaseObject();
        }
    }

    private void HandleThrowInput()
    {
        if (Input.GetMouseButtonDown(0) && grabbedObject != null)
        {
            ThrowObject();
        }
    }

    private void HandleSwitchPositionInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && grabbedObject == null)
        {
            TrySwitchPositionWithNearbyObject();
        }
    }

    private void CheckIfPlayerTeleported()
    {
        float teleportThreshold = 2f;
        if (Vector3.Distance(transform.position, lastPlayerPosition) > teleportThreshold)
        {
            if (grabbedObject != null)
            {
                ReleaseObject();
            }
        }
        lastPlayerPosition = transform.position;
    }

    private void TryGrabObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance, grabbableLayer))
        {
            if (hit.collider.CompareTag("Poder de la mano celestial"))
            {
                grabbedObject = hit.collider.gameObject;
                grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();

                if (grabbedRigidbody != null)
                {
                    grabbedRigidbody.useGravity = false;
                    grabbedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }

                IgnoreCollisions(hit.collider, true);
            }
        }
    }

    private void ReleaseObject()
    {
        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.useGravity = true;
            grabbedRigidbody.constraints = RigidbodyConstraints.None;
        }

        IgnoreCollisions(grabbedObject.GetComponent<Collider>(), false);

        grabbedObject = null;
        grabbedRigidbody = null;
    }

    private void MoveObjectSmoothly()
    {
        Vector3 direction = holdPoint.position - grabbedObject.transform.position;
        grabbedRigidbody.velocity = direction * 10f;
    }

    private void ThrowObject()
    {
        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.useGravity = true;
            grabbedRigidbody.constraints = RigidbodyConstraints.None;
            grabbedRigidbody.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
            grabbedObject = null;
            grabbedRigidbody = null;
        }
    }

    private void TrySwitchPositionWithNearbyObject()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, switchDistanceLimit, grabbableLayer);
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in nearbyObjects)
        {
            if (collider.CompareTag("Poder de la mano celestial"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestObject = collider.gameObject;
                    closestDistance = distance;
                }
            }
        }

        if (closestObject != null)
        {
            SwitchPositionWithObject(closestObject);
        }
    }

    private void SwitchPositionWithObject(GameObject targetObject)
    {
        Rigidbody playerRigidbody = GetComponent<Rigidbody>();
        Rigidbody targetRigidbody = targetObject.GetComponent<Rigidbody>();

        if (playerRigidbody != null) playerRigidbody.isKinematic = true;
        if (targetRigidbody != null) targetRigidbody.isKinematic = true;

        Vector3 playerPosition = transform.position;
        Vector3 objectPosition = targetObject.transform.position;

        if (targetRigidbody != null)
        {
            targetRigidbody.useGravity = false;
        }

        // Activar el shader
        if (teleportMaterial != null)
        {
            teleportMaterial.SetFloat("_IsActive", 1f);
        }

        //-----------------------------------------------------
        RaycastHit hit;
        
        Vector3 p1 = objectPosition + charContr.center + Vector3.up * - charContr.height * 0.5F;
        Vector3 p2 = p1 + Vector3.up * charContr.height;

        Vector3 teleportPosition = objectPosition;

        if (Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 1))
        {

            var distanceToObstacle = hit.distance;

            if (distanceToObstacle < charContr.radius)
            {
                teleportPosition = teleportPosition - hit.normal * (charContr.radius - distanceToObstacle);
            }

        }

        transform.position = teleportPosition;
        

        StartCoroutine(MoveObjectAfterDelay(targetObject, playerPosition, playerRigidbody, targetRigidbody));
        StartCoroutine(ResetShaderEffect());
    }

    private IEnumerator ResetShaderEffect()
    {
        yield return new WaitForSeconds(0.3f);

        float duration = 0.5f; // Duración de la transición
        float elapsedTime = 0f;
        float startValue = 1f;
        float endValue = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            teleportMaterial.SetFloat("_IsActive", newValue);
            yield return null;
        }

        teleportMaterial.SetFloat("_IsActive", 0f);
    }

    private IEnumerator MoveObjectAfterDelay(GameObject targetObject, Vector3 targetPosition, Rigidbody playerRigidbody, Rigidbody targetRigidbody)
    {
        yield return new WaitForFixedUpdate();
        targetObject.transform.position = targetPosition;
        if (targetRigidbody != null)
        {
            targetRigidbody.useGravity = true;
            targetRigidbody.isKinematic = false;
        }
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }
        Collider playerCollider = GetComponent<Collider>();
        Collider objectCollider = targetObject.GetComponent<Collider>();
        if (playerCollider != null && objectCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, objectCollider, false);
        }
    }

    private void IgnoreCollisions(Collider objectCollider, bool ignore)
    {
        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null && objectCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, objectCollider, ignore);
        }
    }
}
