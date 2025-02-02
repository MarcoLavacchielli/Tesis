using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlatformPuzzle : MonoBehaviour
{
    [SerializeField] private List<Transform> objects; // Lista de objetos
    [SerializeField] private List<Transform> platforms; // Lista de plataformas correspondientes
    [SerializeField] private float requiredDistance = 1f; // Distancia para considerar que un objeto está en su plataforma
    [SerializeField] private GameObject feedbackObject; // Objeto que se activará al fallar
    [SerializeField] private AudioSource errorSound; // Sonido al fallar
  //  [SerializeField] private GameObject successAction; // Objeto o evento que se activará al completar el puzzle

    private bool playerInRange = false; // Indica si el jugador está cerca

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CheckPuzzleCompletion();
        }
    }

    private void CheckPuzzleCompletion()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (Vector3.Distance(objects[i].position, platforms[i].position) > requiredDistance)
            {
                StartCoroutine(TriggerFailureFeedback());
                return;
            }
        }

        Debug.Log("¡Puzzle completado!");
        //successAction.SetActive(true); // Ejecuta la acción al resolver el puzzle
    }

    private IEnumerator TriggerFailureFeedback()
    {
        feedbackObject.SetActive(true);
        errorSound.Play();

        yield return new WaitForSeconds(1f);

        feedbackObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = false;
        }
    }
}
