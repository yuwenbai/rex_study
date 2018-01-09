//访问者模式
using System.Collections.Generic;
using UnityEngine;

public abstract class Vistor
{
    public abstract void VisitConcreteElementA(ElementA elementA);
    public abstract void VisitConcreteElementB(ElementB elementB);
}
public class ConcreteVistor1 : Vistor
{
    public override void VisitConcreteElementA(ElementA elementA)
    {
        Debug.Log(elementA.GetType().Name + " visited by " + this.GetType().Name);
    }

    public override void VisitConcreteElementB(ElementB elementB)
    {
        Debug.Log(elementB.GetType().Name + " visited by " + this.GetType().Name);
    }
}
public class ConcreteVistor2 : Vistor
{
    public override void VisitConcreteElementA(ElementA elementA)
    {
        Debug.Log(elementA.GetType().Name + " visited by " + this.GetType().Name);
    }

    public override void VisitConcreteElementB(ElementB elementB)
    {
        Debug.Log(elementB.GetType().Name + " visited by " + this.GetType().Name);
    }
}

public abstract class Element
{
    public abstract void Accept(Vistor vistor);
}
public class ElementA : Element
{
    public override void Accept(Vistor vistor)
    {
        vistor.VisitConcreteElementA(this);
    }
}
public class ElementB : Element
{
    public override void Accept(Vistor vistor)
    {
        vistor.VisitConcreteElementB(this);
    }
}
public class ObjectStructure
{
    private List<Element> _elements = new List<Element>();

    public void Attach(Element element)
    {
        _elements.Add(element);
    }
    public void Detach(Element element)
    {
        _elements.Remove(element);
    }

    public void Accept(Vistor vistor)
    {
        foreach (Element em in _elements)
        {
            em.Accept(vistor);
        }
    }
}