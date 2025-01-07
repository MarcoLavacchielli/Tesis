using System.Collections;
using UnityEngine;

public class PowerSwitcher : MonoBehaviour
{
    [Header("General Settings")]
    public float grabDistance = 3f;           // Maximum distance to interact with objects
    public float throwForce = 10f;            // Base force to throw objects
    public Transform holdPoint;               // Point where the object is held
    public LayerMask grabbableLayer;          // Layer for grabbable objects
    public float switchDistanceLimit = 5f;    // Maximum distance to switch places with an object

    private GameObject grabbedObject;         // Currently grabbed object
    private Rigidbody grabbedRigidbody;       // Rigidbody of the grabbed object

    private void Update()
    {
        HandleGrabInput();
        HandleThrowInput();
        HandleSwitchPositionInput();
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
        grabbedRigidbody.velocity = direction * 10f; // Adjust smoothness factor if needed
    }

    private void ThrowObject()
    {
        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.useGravity = true;
            grabbedRigidbody.constraints = RigidbodyConstraints.None;

            Vector3 throwDirection = Camera.main.transform.forward;
            grabbedRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);

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
        else
        {
            Debug.Log("No nearby objects to switch positions with or out of range.");
        }
    }

    private void SwitchPositionWithObject(GameObject targetObject)
    {
        // Disable collisions between the player and the object temporarily
        Rigidbody playerRigidbody = GetComponent<Rigidbody>();
        Rigidbody targetRigidbody = targetObject.GetComponent<Rigidbody>();

        if (playerRigidbody != null) playerRigidbody.isKinematic = true;
        if (targetRigidbody != null) targetRigidbody.isKinematic = true;

        // Save initial positions
        Vector3 playerPosition = transform.position;
        Vector3 objectPosition = targetObject.transform.position;

        // Temporarily disable gravity to avoid strange behavior during the swap
        if (targetRigidbody != null)
        {
            targetRigidbody.useGravity = false;
        }

        // Move the player to the object's position
        transform.position = objectPosition;

        // Use a coroutine to move the object after a small delay
        StartCoroutine(MoveObjectAfterDelay(targetObject, playerPosition, playerRigidbody, targetRigidbody));
    }

    private IEnumerator MoveObjectAfterDelay(GameObject targetObject, Vector3 targetPosition, Rigidbody playerRigidbody, Rigidbody targetRigidbody)
    {
        yield return new WaitForFixedUpdate(); // Wait for one physics frame to avoid collision issues

        // Move the object to the player's previous position
        targetObject.transform.position = targetPosition;

        // Reactivate gravity for the object after the swap
        if (targetRigidbody != null)
        {
            targetRigidbody.useGravity = true;
            targetRigidbody.isKinematic = false;
        }

        // Reactivate the player's Rigidbody if needed
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }

        // Re-enable collisions between the player and the object
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
