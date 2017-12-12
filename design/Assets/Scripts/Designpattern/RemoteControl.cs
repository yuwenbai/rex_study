using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//桥接模式

public class RemoteControl
{
    TV Implementor;
    public TV TVtor
    {
        get
        {
            return Implementor;
        }
        set
        {
            Implementor = value;
        }
    }

}
public abstract class TV
{
    public abstract void On();
    public abstract void Off();
}

public class ChangeHong : TV
{
    public override void On()
    {
        Debug.Log("rextest Changhong On!!!");
    }
    public override void Off()
    {

    }
}
public class Sony : TV
{
    public override void On()
    {
        Debug.Log("rextest Sony On!!!");
    }
    public override void Off()
    {

    }
}
