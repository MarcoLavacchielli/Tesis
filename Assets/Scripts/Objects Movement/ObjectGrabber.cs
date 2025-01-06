using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Grab Settings")]
    public float grabDistance = 3f;          // Maximum distance to grab objects
    public float maxGrabRange = 2f;         // Maximum range to keep the object close to the player
    public float grabSmoothness = 10f;      // Follow speed to the holdPoint
    public Transform holdPoint;             // Point where the object is held
    public float rotationSpeed = 100f;      // Rotation speed using keys
    public LayerMask grabbableLayer;        // Layer for grabbable objects

    private GameObject grabbedObject;       // Reference to the grabbed object
    private Rigidbody grabbedRigidbody;     // Rigidbody of the grabbed object

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
            if (collider.CompareTag("Arrastrable"))  // Check tag
            {
                grabbedObject = collider.gameObject;
                grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();

                if (grabbedRigidbody != null)
                {
                    grabbedRigidbody.useGravity = false;
                    grabbedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }

                IgnoreCollisions(grabbedObject.GetComponent<Collider>(), true);
                return; // Exit after grabbing the first valid object
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
        Transform playerView = Camera.main.transform; // Assume the camera represents the player's view

        // Rotación izquierda-derecha (eje global 'up')
        float horizontalRotationInput = Input.GetKey(KeyCode.LeftArrow) ? -1f : Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
        if (horizontalRotationInput != 0f)
        {
            grabbedObject.transform.Rotate(Vector3.up, horizontalRotationInput * rotationSpeed * Time.deltaTime, Space.World);
        }

        // Rotación arriba-abajo (eje relativo al jugador)
        float verticalRotationInput = Input.GetKey(KeyCode.UpArrow) ? -1f : Input.GetKey(KeyCode.DownArrow) ? 1f : 0f;
        if (verticalRotationInput != 0f)
        {
            Vector3 rotationAxis = playerView.right; // Eje relativo al jugador
            grabbedObject.transform.Rotate(rotationAxis, verticalRotationInput * rotationSpeed * Time.deltaTime, Space.World);
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
