using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateIfAssistOn : MonoBehaviour
{

    [SerializeField] private JsonSaveGameManager saveGameManager;
    [SerializeField] private TakeDiamont diamondGrab;

    void Start()
    {
        if (saveGameManager.saveData.easyMode == false && diamondGrab.diamondTake==true)
        {
            gameObject.SetActive(false);
        }
    }
}
