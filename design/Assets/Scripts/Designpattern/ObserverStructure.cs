using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//观察者模式
public class ObserverStructure
{
}

public abstract class Subject
{
    private List<Observer> _observers = new List<Observer>();
    public void Attach(Observer observer)
    {
        _observers.Add(observer);
    }
    public void Detach(Observer observer)
    {
        _observers.Remove(observer);
    }
    public void Notify()
    {
        foreach (Observer o in _observers)
        {
            o.Update();
        }
    }
}

/// <summary>
/// The 'Observer' abstract class
/// </summary>
public abstract class Observer
{
    public abstract void Update();
}

public class ConcreteObserver : Observer
{
    private string _name;
    private string _observerState;
    private ConcreteSubject _subject;

    // Constructor
    public ConcreteObserver(
      ConcreteSubject subject, string name)
    {
        this._subject = subject;
        this._name = name;
    }
    public override void Update()
    {
        _observerState = _subject.SubjectState;
        Debug.Log("Observer " + _name + "'s new state is " + _observerState);
    }
}
/// <summary>
/// The 'ConcreteSubject' class
/// </summary>
public class ConcreteSubject : Subject
{
    private string _subjectState;

    // Gets or sets subject state
    public string SubjectState
    {
        get { return _subjectState; }
        set { _subjectState = value; }
    }
}
