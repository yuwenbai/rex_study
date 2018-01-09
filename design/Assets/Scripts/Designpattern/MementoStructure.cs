using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//备忘录模式
class MementoStructure
{
}
public class Originator
{
    public string _State;
    public string State
    {
        get
        {
            return _State;
        }
        set
        {
            Debug.Log("rextest Originator state set :" + value);
            _State = value;
        }
    }
    //resore original state
    public void SetMemento(Memento memento)
    {
        State = memento.State;
    }
    public Memento CreateMemento()
    {
        return new Memento(State);
    }
}
public class Memento
{
    public string _State;
    public Memento(string state)
    {
        _State = state;
    }

    public string State
    {
        get
        {
            return _State;
        }
    }
}

public class Caretaker
{
    private Memento _memento;

    // Gets or sets memento
    public Memento Memento
    {
        set { _memento = value; }
        get { return _memento; }
    }
}