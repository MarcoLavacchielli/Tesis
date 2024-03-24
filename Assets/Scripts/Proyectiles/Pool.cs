using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pool<T> : MonoBehaviour
{
    //Delegado que devuelve tipo T, por lo que aca voy a guardar metodo de COMO se crea el objeto
    private Func<T> _factoryMethod;

    //Delegado que toma por parametro tipo T, aca voy a guardar COMO se prende la bala una vez la llame el cliente
    private Action<T> _turnOnCallback;

    //Delegado que toma por parametro tipo T, aca voy a guardar COMO se apaga la bala una vez se regrese al pool
    private Action<T> _turnOffCallback;

    //Mi "cajon" donde voy a guardar los objetos disponibles para su uso
    private List<T> _currentStock;

    public Pool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialAmount)
    {
        _currentStock = new List<T>();

        _factoryMethod = factoryMethod;

        _turnOnCallback = turnOnCallback;

        _turnOffCallback = turnOffCallback;

        for (int i = 0; i < initialAmount; i++)
        {
            T newObj = _factoryMethod();

            _turnOffCallback(newObj);

            _currentStock.Add(newObj);
        }
    }

    public T GetObject()
    {
        T result;

        if (_currentStock.Count == 0)
        {
            result = _factoryMethod();
        }
        else
        {
            result = _currentStock[0];
            _currentStock.RemoveAt(0);
        }

        _turnOnCallback(result);

        return result;
    }

    public void ReturnObject(T obj)
    {
        _turnOffCallback(obj);

        _currentStock.Add(obj);
    }
}
