using UnityEngine;
using projectQ;

public class TestAction : BaseAction
{
    public TestAction()
    {
        Zero();
    }
    public override void Execute()
    {
        
    }

    public override void Init()
    {
        Debug.Log("TestAction  Init Delay Function --- 0 ");
        AnimTimeTick.Instance.SetAction(testdelay, 10, 1);
    }
    public void testdelay(string param)
    {
        Debug.Log("TestAction  Delay Function Exectute ---- 0");
        this.IsStopped = true;
    }
    public override void Step(float delta)
    {
        if (this.firstTick)
        {
            this.Init();
            this.firstTick = false;
        }
        //foreach (BaseAction act in listAct)
        //{
        //    if (act != null && false == act.IsStop())
        //    {
        //        act.Step(delta);
        //    }
        //}
    }
    public override bool IsStop()
    {
        return IsStopped;
    }
}
public class TestAction1 : BaseAction
{
    public TestAction1()
    {
        Zero();
    }
    public override void Execute()
    {

    }

    public override void Init()
    {
        Debug.Log("TestAction  Init Delay Function --- 1 ");
        this.IsStopped = false;
        AnimTimeTick.Instance.SetAction(testdelay, 20, 1);
    }
    public void testdelay(string param)
    {
        Debug.Log("TestAction  Delay Function Exectute ----  1");
        this.IsStopped = true;
    }
    public override void Step(float delta)
    {
        if (this.firstTick)
        {
            this.Init();
            this.firstTick = false;
        }
    }
    public override bool IsStop()
    {
        return IsStopped;
    }
}
public class TestAction2 : BaseAction
{
    public TestAction2()
    {
        Zero();
    }
    public override void Execute()
    {

    }

    public override void Init()
    {
        Debug.Log("TestAction  Init Delay Function --- 2 ");
        this.IsStopped = false;
        AnimTimeTick.Instance.SetAction(testdelay, 5, 1);
    }
    public void testdelay(string param)
    {
        Debug.Log("TestAction  Delay Function Exectute ----  2");
        this.IsStopped = true;
    }
    public override void Step(float delta)
    {
        if (this.firstTick)
        {
            this.Init();
            this.firstTick = false;
        }
    }
    public override bool IsStop()
    {
        return IsStopped;
    }
}
public class ParallelAction : BaseAction
{
    public ParallelAction()
    {
        Zero();
    }
    public override void Execute()
    {

    }
    public override void addAction(BaseAction act)
    {
        listAct.Enqueue(act);
    }

    public override void Init()
    {
        Debug.Log("TestAction  Init Delay Function --- ParallelAction ");
        this.IsStopped = false;
        //AnimTimeTick.Instance.SetAction(testdelay, 20, 1);
    }
    public void testdelay(string param)
    {
        //Debug.Log("TestAction  Delay Function Exectute ----  2");
        //this.IsStopped = true;
    }
    public override void Step(float delta)
    {
        if (this.firstTick)
        {
            this.Init();
            this.firstTick = false;
        }
        if (listAct.Count <= 0)
            return;
        IsStopped = true;
        foreach (BaseAction act in listAct)
        {
            act.Step(delta);
            if (!act.IsStop())
            {
                IsStopped = false;
            }
        }
    }

    public override bool IsStop()
    {
        return IsStopped;
    }
}
public class SerialAction : BaseAction
{
    public SerialAction()
    {
        Zero();
    }
    public override void Execute()
    {

    }
    public override void addAction(BaseAction act)
    {
        listAct.Enqueue(act);
    }
    public override void Init()
    {
        Debug.Log("TestAction  Init Delay Function --- 2 ");
        this.IsStopped = false;
        AnimTimeTick.Instance.SetAction(testdelay, 20, 1);
    }
    public void testdelay(string param)
    {
        Debug.Log("TestAction  Delay Function Exectute ----  2");
        this.IsStopped = true;
    }
    public override void Step(float delta)
    {
        if (this.firstTick)
        {
            this.Init();
            this.firstTick = false;
        }
        if (listAct.Count <= 0)
            return;
        BaseAction ba = listAct.Peek();
        if (ba.IsStop())
        {
            listAct.Dequeue();
            if (listAct.Count <= 0)
            {
                listAct.Clear();
                return;
            }
            ba = listAct.Peek();
        }
        else
        {
            ba.Step(delta);
        }
    }

    public override bool IsStop()
    {
        return listAct.Count == 0;
    }
}