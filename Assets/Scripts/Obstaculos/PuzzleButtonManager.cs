using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material correctMaterial;
    [SerializeField] private GameObject feedbackObject;
    [SerializeField] private AudioSource errorSound;

    private int currentIndex = 0;
    private bool isResetting = false;

    public void CheckButton(GameObject pressedButton)
    {
        if (isResetting) return;

        if (pressedButton == buttons[currentIndex])
        {
            pressedButton.GetComponent<Renderer>().material = correctMaterial;
            currentIndex++;

            if (currentIndex >= buttons.Count)
            {
                Debug.Log("¡Puzzle completado!");
                // Aquí puedes agregar la lógica para cuando se completa el puzzle
            }
        }
        else
        {
            StartCoroutine(ResetPuzzle());
        }
    }

    private IEnumerator ResetPuzzle()
    {
        isResetting = true;
        feedbackObject.SetActive(true);
        errorSound.Play();

        yield return new WaitForSeconds(1f);

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Renderer>().material = defaultMaterial;
        }

        feedbackObject.SetActive(false);
        currentIndex = 0;
        isResetting = false;
    }
}
