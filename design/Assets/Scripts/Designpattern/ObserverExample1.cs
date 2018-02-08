//观察者模式
using System.Collections.Generic;
using UnityEngine;

public class IBM
{
    public string name;
    public void Attach(ObserverBase observerBase)
    {
        _ObserverList.Add(observerBase);
    }
    public void Notify()
    {
        foreach (var ob in _ObserverList)
        {
            ob.UpdateEvent(this);
        }
    }
    private List<ObserverBase> _ObserverList = new List<ObserverBase>();
}
public abstract class ObserverBase
{
    public abstract void UpdateEvent(IBM imb);
}
public class ObserverApple : ObserverBase
{
    public override void UpdateEvent(IBM imb)
    {
        Debug.Log("rextest ObserverApole receiver notify : " + imb.name);
    }
}
public class ObserverOrange : ObserverBase
{
    public override void UpdateEvent(IBM imb)
    {
        Debug.Log("rextest ObserverOrange receiver notify : " + imb.name);
    }
}