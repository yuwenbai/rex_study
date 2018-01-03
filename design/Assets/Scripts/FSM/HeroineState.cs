using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class HeroineState
{
    public HeroineState()
    {

    }
    public virtual void  Init()
    {
        Debug.Log("rextest BaseHeroineState Init");
    }
    public virtual void leave()
    {
        Debug.Log("rextest BaseHeroineState leave");
    }
}
public class DuckingState : HeroineState
{
    public override void Init()
    {
        Debug.Log("rextest DuckingState Init");
    }
    public override void leave()
    {
        Debug.Log("rextest DuckingState leave");
    }
}
public class AttackingState : HeroineState
{
    public override void Init()
    {
        Debug.Log("rextest AttackingState Init");
    }
    public override void leave()
    {
        Debug.Log("rextest AttackingState leave");
    }
}
