//原型模式 example
using System;
using UnityEngine;

public class CloneFactory
{
    public IAnimal GetClone(IAnimal animalSample)
    {
        return (IAnimal)animalSample.Clone();
    }
}
public interface IAnimal : ICloneable
{
    object Clone();
}

public class Sheep : IAnimal
{
    public Sheep()
    {
        Debug.Log("Made Sheep");
    }
    public object Clone()
    {
        Sheep sheep = null;

        try
        {
            sheep = (Sheep)base.MemberwiseClone();
        }
        catch (Exception e)
        {
            Debug.LogError("Error cloning Sheep");
        }

        return sheep;
    }

    public string ToStringEX()
    {
        return "Hello I'm a Sheep";
    }
}