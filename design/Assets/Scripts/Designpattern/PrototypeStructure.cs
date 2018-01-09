//原型模式
public abstract class Prototype
{
    private string _id;
    public Prototype(string id)
    {
        this._id = id;
    }
    public string Id
    {
        get { return _id; }
    }
    public abstract Prototype Clone();
}
public class ConcretePrototype1 : Prototype
{
    public ConcretePrototype1(string id) : base(id)
    {

    }
    public override Prototype Clone()
    {
        return (ConcretePrototype1)this.MemberwiseClone();
    }
}
public class ConcretePrototype2 : Prototype
{
    public ConcretePrototype2(string id) : base(id)
    {

    }
    public override Prototype Clone()
    {
        return (ConcretePrototype2)this.MemberwiseClone();
    }
}
