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

    [Header("Material Settings")]
    public Material grabbedMat;             // Material when the object is grabbed
    public Material standByMat;             // Material when the object is not grabbed

    private GameObject grabbedObject;       // Reference to the grabbed object
    private Rigidbody grabbedRigidbody;     // Rigidbody of the grabbed object
    private Renderer grabbedRenderer;       // Renderer of the grabbed object

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray from the center of the screen
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance, grabbableLayer))
        {
            if (hit.collider.CompareTag("Arrastrable")) // Check for the correct tag
            {
                grabbedObject = hit.collider.gameObject;
                grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();
                grabbedRenderer = grabbedObject.GetComponent<Renderer>();

                if (grabbedRigidbody != null)
                {
                    grabbedRigidbody.useGravity = false;
                    grabbedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }

                if (grabbedRenderer != null && grabbedMat != null)
                {
                    grabbedRenderer.material = grabbedMat; // Change to grabbed material
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

        if (grabbedRenderer != null && standByMat != null)
        {
            grabbedRenderer.material = standByMat; // Change back to standby material
        }

        IgnoreCollisions(grabbedObject.GetComponent<Collider>(), false);

        grabbedObject = null;
        grabbedRigidbody = null;
        grabbedRenderer = null;
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
        Transform playerView = Camera.main.transform;

        float horizontalRotationInput = Input.GetKey(KeyCode.LeftArrow) ? -1f : Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
        if (horizontalRotationInput != 0f)
        {
            grabbedObject.transform.Rotate(Vector3.up, horizontalRotationInput * rotationSpeed * Time.deltaTime, Space.World);
        }

        float verticalRotationInput = Input.GetKey(KeyCode.UpArrow) ? -1f : Input.GetKey(KeyCode.DownArrow) ? 1f : 0f;
        if (verticalRotationInput != 0f)
        {
            Vector3 rotationAxis = playerView.right;
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
