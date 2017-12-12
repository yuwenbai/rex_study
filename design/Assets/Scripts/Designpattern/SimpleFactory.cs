using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Food
{
    public abstract void print();
}
public class TomatoEggs : Food
{
    public override void print()
    {
        Debug.Log("rextest TomatoEggs ");
    }
}
public class ShreddedPork : Food
{
    public override void print()
    {
        Debug.Log("rextest ShreddedPork ");
    }
}
class SimpleFactory
{
    public static Food CreateFood(int type)
    {
        Food food = null;
        if (type == 1)
        {
            food = new TomatoEggs();
        }
        else
        {
            food = new ShreddedPork();
        }
        return food;
    }
}
