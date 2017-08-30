using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subscriber : IObserver
{
    public string name { get; set; }
    public Subscriber(string name)
    {
        this.name = name;
    }
    public void ReceiveAndPrint(TenXun tenxun)
    {
        Debug.Log("name is " + name + " tenxun is " + tenxun.info);
    }
}
