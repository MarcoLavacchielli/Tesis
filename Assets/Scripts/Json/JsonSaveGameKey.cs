using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonSaveGameKey
{

    public bool easyMode = false;

    internal bool GetDifficulty()
    {
        return easyMode;
    }

    internal void SetDifficulty(bool value)
    {
        easyMode = value;
    }
}
