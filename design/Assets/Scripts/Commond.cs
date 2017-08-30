using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Commond {
    public Receiver _receiver;
    public Commond(Receiver receiver)
    {
        this._receiver = receiver;
    }
    public abstract void Action();
}
