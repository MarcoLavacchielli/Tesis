using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Grab Settings")]
    public float grabDistance = 3f;
    public float maxGrabRange = 2f;
    public float grabSmoothness = 10f;
    public Transform holdPoint;
    public float rotationSpeed = 100f;
    public LayerMask grabbableLayer;

    private GameObject grabbedObject;
    private Rigidbody grabbedRigidbody;

    private void Update()
    {
        HandleGrabInput();
        if (grabbedObject != null) RotateGrabbedObject();
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

    private void TryGrabObject()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, grabDistance, grabbableLayer);

        foreach (var collider in nearbyObjects)
        {
            if (collider.CompareTag("Arrastrable"))
            {
                grabbedObject = collider.gameObject;
                grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();

                if (grabbedRigidbody != null)
                {
                    grabbedRigidbody.useGravity = false;
                    grabbedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }

                IgnoreCollisions(grabbedObject.GetComponent<Collider>(), true);
                return;
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
        if (Vector3.Distance(holdPoint.position, grabbedObject.transform.position) > maxGrabRange)
        {
            ReleaseObject();
            return;
        }

        Vector3 direction = holdPoint.position - grabbedObject.transform.position;
        grabbedRigidbody.velocity = direction * grabSmoothness;
    }

    private void RotateGrabbedObject()
    {
        float horizontalRotationInput = Input.GetKey(KeyCode.LeftArrow) ? -1f : Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
        if (horizontalRotationInput != 0f)
        {
            grabbedObject.transform.Rotate(Vector3.up, horizontalRotationInput * rotationSpeed * Time.deltaTime, Space.World);
        }

        float verticalRotationInput = Input.GetKey(KeyCode.UpArrow) ? -1f : Input.GetKey(KeyCode.DownArrow) ? 1f : 0f;
        if (verticalRotationInput != 0f)
        {
            grabbedObject.transform.Rotate(Vector3.right, verticalRotationInput * rotationSpeed * Time.deltaTime, Space.World);
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