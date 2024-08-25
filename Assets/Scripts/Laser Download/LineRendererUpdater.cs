using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class LineRendererPositionUpdater : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        UpdateLineRendererPosition();
    }

    private void Update()
    {
        UpdateLineRendererPosition();
    }

    private void UpdateLineRendererPosition()
    {
        if (lineRenderer == null)
            return;

        // Asegura que el Line Renderer tenga al menos un punto
        if (lineRenderer.positionCount == 0)
            lineRenderer.positionCount = 1;

        // Actualiza la posición del primer punto del Line Renderer
        lineRenderer.SetPosition(0, transform.position);
    }
}
