using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invoker {

    public Commond _commond;
    public Invoker(Commond commond)
    {
        this._commond = commond;
    }
    public void ExecuteCommond()
    {
        this._commond.Action();
    }
}
