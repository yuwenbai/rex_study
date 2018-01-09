using System.Collections.Generic;
using UnityEngine;

//构造模式
public abstract class Build
{
    public abstract void BuildPartA();
    public abstract void BuildPartB();
    public abstract Product GetResult();
};
public class ConcreteBuilder1 : Build
{
    private Product _product = new Product();
    public override void BuildPartA()
    {
        _product.Add("PartA");
    }

    public override void BuildPartB()
    {
        _product.Add("PartB");
    }

    public override Product GetResult()
    {
        return _product;
    }
}
public class ConcreteBuilder2 : Build
{
    private Product _product = new Product();
    public override void BuildPartA()
    {
        _product.Add("PartX");
    }

    public override void BuildPartB()
    {
        _product.Add("PartY");
    }

    public override Product GetResult()
    {
        return _product;
    }
}
public class Product
{
    private List<string> _part = new List<string>();
    public void Add(string part)
    {
        _part.Add(part);
    }
    public void Show()
    {
        foreach (string part in _part)
        {
            Debug.Log("rextest part name is " + part);
        }
    }
}
public class ProductDirector
{
    public void Constuct(Build build)
    {
        build.BuildPartA();
        build.BuildPartB();
    }
}