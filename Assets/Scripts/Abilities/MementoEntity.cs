using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MementoEntity : MonoBehaviour
{
    protected MementoState _myMemento;
    void Awake()
    {
        _myMemento = new MementoState();

        //StartCoroutine(StartToSaveStates()); COLOCAR EN EL TRIGGERENTER (CREO)
    }

    protected abstract void SaveStates();

    protected abstract void LoadStates(object[] parameters);

    protected IEnumerator StartToSaveStates()
    {
        while (true)
        {
            SaveStates();

            //yield return new WaitForEndOfFrame();

            //Esto:
            yield return new WaitUntil(() => !TriggerOfMemento.ItsRewindTime);

            //Es lo mismo que esto:
            // while (Debugger.ItsRewindTime)
            // {
            //     yield return null;
            // }
        }
    }

    public void TryLoadStates()
    {
        if (_myMemento.MemoriesAmount == 0) return;

        //Tomamos uno de esos recuerdos
        var lastMemory = _myMemento.LoadMemory();

        //Lo pasamos en LoadStates
        LoadStates(lastMemory);
    }
}
