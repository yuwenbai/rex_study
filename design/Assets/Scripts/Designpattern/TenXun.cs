using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TenXun {
    public string info { get; set; }
    private List<IObserver> observers = new List<IObserver>();
    public TenXun(string info)
    {
        this.info = info;
    }
    public void addObserver(IObserver ob)
    {
        observers.Add(ob);
    }
    public void update()
    {
        foreach(IObserver ob in observers)
        {
            if (ob != null)
            {
                ob.ReceiveAndPrint(this);
            }
        }
    }
}
