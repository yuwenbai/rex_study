using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteCommand : Commond {

    public ConcreteCommand(Receiver receiver) : base(receiver)
    {
    }
    public override void Action()
    {
        _receiver.Run1000Meters();
    }
}
