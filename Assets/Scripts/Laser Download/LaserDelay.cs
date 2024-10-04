using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDelay : MonoBehaviour
{
    [SerializeField] private List<GameObject> listOne;
    [SerializeField] private List<GameObject> listTwo;
    [SerializeField] private List<GameObject> listThree;
    [SerializeField] private List<GameObject> listFour;
    [SerializeField] private List<GameObject> listFive;
    [SerializeField] private List<GameObject> listSix;

    [SerializeField] private float activationDelayOne = 1f;
    [SerializeField] private float activationDelayTwo = 2f;
    [SerializeField] private float activationDelayThree = 3f;
    [SerializeField] private float activationDelayFour = 4f;
    [SerializeField] private float activationDelayFive = 5f;
    [SerializeField] private float activationDelaySix = 6f;

    void Start()
    {
        // Desactivamos inicialmente todos los objetos de todas las listas
        SetActiveForList(listOne, false);
        SetActiveForList(listTwo, false);
        SetActiveForList(listThree, false);
        SetActiveForList(listFour, false);
        SetActiveForList(listFive, false);
        SetActiveForList(listSix, false);

        // Activamos cada lista después de los retrasos correspondientes
        StartCoroutine(ActivateListAfterDelay(listOne, activationDelayOne));
        StartCoroutine(ActivateListAfterDelay(listTwo, activationDelayTwo));
        StartCoroutine(ActivateListAfterDelay(listThree, activationDelayThree));
        StartCoroutine(ActivateListAfterDelay(listFour, activationDelayFour));
        StartCoroutine(ActivateListAfterDelay(listFive, activationDelayFive));
        StartCoroutine(ActivateListAfterDelay(listSix, activationDelaySix));
    }

    // Función para activar/desactivar todos los objetos en una lista
    private void SetActiveForList(List<GameObject> list, bool isActive)
    {
        foreach (GameObject obj in list)
        {
            if (obj != null)
                obj.SetActive(isActive);
        }
    }

    // Coroutine que activa una lista después de un retraso
    private IEnumerator ActivateListAfterDelay(List<GameObject> list, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetActiveForList(list, true); // Activamos la lista después del retraso
    }
}
