using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Experimental.GraphView;
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



    private void Update()
    {
        if (InFieldOfView(playerTransform.transform.position) && InLineOfSight(playerTransform.transform.position))
        {
            Debug.Log("visto");
        }
    }

    public bool InFieldOfView(Vector3 point)
    {
        var dir = point - transform.position;
        if (dir.magnitude > viewRadius)
            return false;

        if (dir.magnitude < senseRadius)
            return true;

        var angle = Vector3.Angle(dir, transform.forward);
        return angle <= viewAngle / 2;
    }

    public bool InLineOfSight(Vector3 point)
    {
        if (!InFieldOfView(point))
            return false;

        var dir = point - transform.position;
        return !Physics.Raycast(transform.position, dir, dir.magnitude, wallMask);
    }
    /*
    public Node GetClosestNodeInView()
    {
        foreach (var node in nodeGrid.AllNodes)
        {
            if (InFieldOfView(node.transform.position) && InLineOfSight(node.transform.position))
            {
                return node;
            }
        }

        return null;
    }
    */

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


    /*
    public float rotationSpeed = 30f; // Velocidad de rotación en grados por segundo
    [SerializeField] float _radius;
    public Transform PlayerTransform;
    private bool movingForward = true;

    void Update()
    {
        // Si estamos moviéndonos hacia adelante
        if (movingForward)
        {
            // Rotar hacia la derecha (45 grados en el eje Y)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Si hemos alcanzado los 45 grados, cambiar dirección
            if (transform.rotation.eulerAngles.y >= 90)
            {
                movingForward = false;
            }
        }
        else
        {
            // Rotar hacia la izquierda (-45 grados en el eje Y)
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

            // Si hemos alcanzado los -45 grados, cambiar dirección
            if (transform.rotation.eulerAngles.y <= 0)
            {
                movingForward = true;
            }
        }
    }
    public void OnDrawGizmos()
    {
        

        Gizmos.color = Color.green;
        Gizmos.DrawLine(PlayerTransform.position + PlayerTransform.up * 0.5f, PlayerTransform.position + PlayerTransform.up * 0.5f + PlayerTransform.right * _radius);
        Gizmos.DrawLine(PlayerTransform.position - PlayerTransform.up * 0.5f, PlayerTransform.position - PlayerTransform.up * 0.5f + PlayerTransform.right * _radius);

    }
    */


    
}
