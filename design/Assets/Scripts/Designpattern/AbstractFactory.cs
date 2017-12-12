using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class AbstractFactory
{
    public abstract YaBo  CreateYaBo();
    public abstract YaJia CreateYaJia();
}
public class NanChangFactory : AbstractFactory
{
    public override YaBo CreateYaBo()
    {
        return new NanChangYaBo();
    }

    public override YaJia CreateYaJia()
    {
        return new NanChangYaJia();
    }
}
public class WuHanFactory : AbstractFactory
{
    public override YaBo CreateYaBo()
    {
        return new WuHanYaBo();
    }

    public override YaJia CreateYaJia()
    {
        return new WuHanYaJia();
    }
}
public abstract class YaBo
{
    public abstract void Print();
}
public abstract class YaJia
{
    public abstract void Print();
}
public class NanChangYaBo : YaBo
{
    public override void Print()
    {
        Debug.Log("rextest NanChangeYabo");
    }
}
public class WuHanYaBo : YaBo
{
    public override void Print()
    {
        Debug.Log("rextest WuHanYaBo");
    }
}
public class NanChangYaJia : YaJia
{
    public override void Print()
    {
        Debug.Log("rextest NanChangYaJia");
    }
}
public class WuHanYaJia : YaJia
{
    public override void Print()
    {
        Debug.Log("rextest WuHanYaJia");
    }
}
