using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoState : MonoBehaviour
{
    private Stack<object[]> _rememberStack;

    //Esto:
    public int MemoriesAmount => _rememberStack.Count;

    //Es igual a esto:
    // public int GetMemoriesAmount()
    // {
    //     return _rememberStack.Count;
    // }
    public MementoState()
    {
        _rememberStack = new Stack<object[]>();
    }
    public void SaveMemory(params object[] parameters)
    {
        _rememberStack.Push(parameters);
    }

    public object[] LoadMemory()
    {
        var memory = _rememberStack.Pop();

        return memory;
    }
}
