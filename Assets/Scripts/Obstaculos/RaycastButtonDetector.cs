using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastButtonDetector : MonoBehaviour
{
    public Camera playerCamera; // Asignar la cámara en el inspector
    public float rayDistance = 5f;
    public LayerMask buttonLayer;
    public PuzzleButtonManager puzzleManager; // Referencia al PuzzleButtonManager

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DetectButton();
        }
    }

    private void DetectButton()
    {
        /* Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

         if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, buttonLayer))
         {
             puzzleManager.CheckButton(hit.collider.gameObject);
         }*/
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, buttonLayer))
        {
            GameObject button = hit.collider.gameObject;
            PuzzleButtonManager puzzleManager = button.transform.parent?.GetComponent<PuzzleButtonManager>();

            if (puzzleManager != null)
            {
                puzzleManager.CheckButton(button);
            }
        }
    }
}
