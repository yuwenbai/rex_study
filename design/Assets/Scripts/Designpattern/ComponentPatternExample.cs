//组件模式
using System.Collections.Generic;
using UnityEngine;

public class ComponentPatternExample
{

}
public class RPGGame
{
    public void OutPutInput()
    {
        Debug.Log("OutPutInput");
    }

    public void OutPutPhysics()
    {
        Debug.Log("OutPutPhysics");
    }

    public void OutPutGraphics()
    {
        Debug.Log("OutPutGraphics");
    }
    //组件
    private InputComponent inputComponent;
    private PhysicsComponent physicsComponent;
    private GraphicsComponent graphicsComponent;
    public List<BaseComponent> ComponentList = new List<BaseComponent>();
    public void Start()
    {
        ComponentList.Add(new InputComponent());
        ComponentList.Add(new PhysicsComponent());
        ComponentList.Add(new GraphicsComponent());
    }
    public void Update()
    {
        foreach (var Component in ComponentList)
        {
            Component.Update(this);
        }
    }
}
public interface BaseComponent
{
    void Update(RPGGame game);
}
public class GraphicsComponent : BaseComponent
{
    public void Update(RPGGame game)
    {
        game.OutPutGraphics();
    }
}

public class PhysicsComponent : BaseComponent
{
    public void Update(RPGGame game)
    {
        game.OutPutPhysics();
    }
}

public class InputComponent : BaseComponent
{
    public void Update(RPGGame game)
    {
        game.OutPutInput();
    }
}
