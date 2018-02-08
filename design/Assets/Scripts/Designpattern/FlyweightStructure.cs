//享元模式
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The 'Flyweight' abstract class
/// </summary>
public abstract class Flyweight
{
    public abstract void Operation(int externalState);
}
public class FlyweightFactory
{
    private Hashtable flyweights = new Hashtable();
    public FlyweightFactory()
    {
        flyweights.Add("X", new ConcreteFlyweight());
        flyweights.Add("Y", new ConcreteFlyweight());
        flyweights.Add("Z", new ConcreteFlyweight());
    }

    public Flyweight GetFlyweight(string key)
    {
        return ((Flyweight)flyweights[key]);
    }
}
public class ConcreteFlyweight : Flyweight
{
    public override void Operation(int externalState)
    {
        Debug.Log("rextest ConcreteFlyweight " + externalState);
    }
}
public class UnsharedConcreteFlyweight : Flyweight
{
    public override void Operation(int externalState)
    {
        Debug.Log("rextest UnsharedConcreteFlyweight " + externalState);
    }
}


/// <summary>
/// The 'FlyweightFactory' class
/// </summary>
public  class CharacterFactory
{
    private Dictionary<char, CharacterBase> _characters =
      new Dictionary<char, CharacterBase>();

    public CharacterBase GetCharacter(char key)
    {
        // Uses "lazy initialization"
        CharacterBase character = null;
        if (_characters.ContainsKey(key))
        {
            character = _characters[key];
        }
        else
        {
            switch (key)
            {
                case 'A': character = new CharacterA(); break;
                case 'B': character = new CharacterB(); break;
                //...
                case 'Z': character = new CharacterZ(); break;
            }
            _characters.Add(key, character);
        }
        return character;
    }
}

/// <summary>
/// The 'Flyweight' abstract class
/// </summary>
public abstract class CharacterBase
{
    protected char symbol;
    protected int width;
    protected int height;
    protected int ascent;
    protected int descent;
    protected int pointSize;

    public abstract void Display(int pointSize);
}

/// <summary>
/// A 'ConcreteFlyweight' class
/// </summary>
public class CharacterA : CharacterBase
{
    // Constructor
    public CharacterA()
    {
        this.symbol = 'A';
        this.height = 100;
        this.width = 120;
        this.ascent = 70;
        this.descent = 0;
    }

    public override void Display(int pointSize)
    {
        this.pointSize = pointSize;
        Debug.Log(this.symbol +
          " (pointsize " + this.pointSize + ")");
    }
}

/// <summary>
/// A 'ConcreteFlyweight' class
/// </summary>
public class CharacterB : CharacterBase
{
    // Constructor
    public CharacterB()
    {
        this.symbol = 'B';
        this.height = 100;
        this.width = 140;
        this.ascent = 72;
        this.descent = 0;
    }

    public override void Display(int pointSize)
    {
        this.pointSize = pointSize;
        Debug.Log(this.symbol +
          " (pointsize " + this.pointSize + ")");
    }

}

// ... C, D, E, etc.

/// <summary>
/// A 'ConcreteFlyweight' class
/// </summary>
public class CharacterZ : CharacterBase
{
    // Constructor
    public CharacterZ()
    {
        this.symbol = 'Z';
        this.height = 100;
        this.width = 100;
        this.ascent = 68;
        this.descent = 0;
    }

    public override void Display(int pointSize)
    {
        this.pointSize = pointSize;
        Debug.Log(this.symbol + " (pointsize " + this.pointSize + ")");
    }
}