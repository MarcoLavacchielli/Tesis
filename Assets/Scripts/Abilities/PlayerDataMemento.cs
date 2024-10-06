using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDataMemento : MementoEntity
{
    private void Start()
    {
        startRecording();
    }
    protected override void LoadStates(object[] parameters)
    {
        // _myMemento.SaveMemory(transform.position, transform.rotation); ESTO ERA LO QUE TRAIA PROBLEMAS

        transform.position = (Vector3)parameters[0];
        transform.rotation = (Quaternion)parameters[1];
    }

    protected override void SaveStates()
    {
        _myMemento.SaveMemory(transform.position, transform.rotation);
    }

    public void startRecording()
    {
        StartCoroutine(StartToSaveStates());
        Debug.Log("comienza grabación");
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            startRecording();

        }
    }*/
   
}
