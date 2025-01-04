using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Grab Settings")]
    public float grabDistance = 3f;          // Distancia máxima para agarrar objetos
    public float maxGrabRange = 2f;         // Rango máximo para mantener el objeto cerca del jugador
    public float grabSmoothness = 10f;      // Velocidad de seguimiento al holdPoint
    public Transform holdPoint;             // Punto donde se sostiene el objeto
    public float rotationSpeed = 100f;      // Velocidad de rotación al usar las teclas

    private GameObject grabbedObject;       // Referencia al objeto agarrado
    private Rigidbody grabbedRigidbody;     // Rigidbody del objeto agarrado

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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabDistance, LayerMask.GetMask("Arrastrable")))
        {
            grabbedObject = hit.collider.gameObject;
            grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();

            if (grabbedRigidbody != null)
            {
                grabbedRigidbody.useGravity = false;
                grabbedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }

            IgnoreCollisions(grabbedObject.GetComponent<Collider>(), true);
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
        float rotationInput = Input.GetKey(KeyCode.LeftArrow) ? -1f : Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;

        if (rotationInput != 0f)
        {
            grabbedObject.transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime, Space.World);
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
