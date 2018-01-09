//模板模式
using UnityEngine;

public class TemplateStructure
{
}
public abstract class AbstractClass
{
    public abstract void PrimitiveOperation1();
    public abstract void PrimitiveOperation2();
    public void TemplateMethod()
    {
        PrimitiveOperation1();
        PrimitiveOperation2();
    }
}
public class ConcreteClassA : AbstractClass
{
    public override void PrimitiveOperation1()
    {
        Debug.Log("ConcreteClassA - > PrimitiveOperation1 ");
    }
    public override void PrimitiveOperation2()
    {
        Debug.Log("ConcreteClassA - > PrimitiveOperation2 ");
    }
}
public class ConcreteClassB : AbstractClass
{
    public override void PrimitiveOperation1()
    {
        Debug.Log("ConcreteClassB - > PrimitiveOperation1 ");
    }
    public override void PrimitiveOperation2()
    {
        Debug.Log("ConcreteClassB - > PrimitiveOperation2 ");
    }
}
