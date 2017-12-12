using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class AbsFood
{
    public abstract void Print();
}
public class TomatoScrambledEggs : AbsFood
{
    public override void Print()
    {
        Debug.Log("西红柿炒蛋好了！");
    }
}
/// <summary>
/// 土豆肉丝这道菜
/// </summary>
public class ShreddedPorkWithPotatoes : AbsFood
{
    public override void Print()
    {
        Debug.Log("土豆肉丝好了");
    }
}
public abstract class Factory
{
    //工厂方法
    public abstract AbsFood CreateFoodFactory();
}
public class TomatoScrambledEggsFactory : Factory
{
    public override AbsFood CreateFoodFactory()
    {
        return new TomatoScrambledEggs();
    }
}
public class ShreddedPorkWithPotatoesFactory : Factory
{
    public override AbsFood CreateFoodFactory()
    {
        return new ShreddedPorkWithPotatoes();
    }
}
