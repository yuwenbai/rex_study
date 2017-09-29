using UnityEngine;
using System.Collections.Generic;

public abstract class BaseAction
{
    public bool IsStopped;
    public bool firstTick;
    public Queue<BaseAction> listAct;
    public object[] obj_param;
    public virtual void addAction(BaseAction act)
    {
        listAct.Enqueue(act);
    }
    public void Zero()
    {
        firstTick = true;
        IsStopped = false;
        listAct = new Queue<BaseAction>();
    }
    public abstract void Init();
    public abstract void Step(float delta);
    public abstract void Execute();
    public abstract bool IsStop();
}