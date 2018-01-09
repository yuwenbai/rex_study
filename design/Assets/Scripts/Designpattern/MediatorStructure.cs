using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//中介者模式
class MediatorStructure
{
}

abstract class Mediator
{
    public abstract void Send(string message, Colleague colleague);
}
class ConcreteMediator : Mediator
{
    public override void Send(string message, Colleague colleague)
    {
        colleague.NotifySend(message);
    }
}
abstract class Colleague
{
    protected Mediator mediator;
    public Colleague(Mediator mediator)
    {
        this.mediator = mediator;
    }
    public abstract void ColleagueSend(string message);
    public abstract void NotifySend(string message);
}

class ConcreteColleague : Colleague
{
    public ConcreteColleague(Mediator mediator) : base(mediator)
    {

    }

    public override void ColleagueSend(string message)
    {
        mediator.Send(message, this);
    }

    public override void NotifySend(string message)
    {
        Debug.Log("rextest ConcreteColleague " + message);
    }
}
class DisConcreteColleague : Colleague
{
    public DisConcreteColleague(Mediator mediator) : base(mediator)
    {

    }
    public override void ColleagueSend(string message)
    {
        mediator.Send(message, this);
    }

    public override void NotifySend(string message)
    {
        Debug.Log("rextest DisConcreteColleague " + message);
    }
}



