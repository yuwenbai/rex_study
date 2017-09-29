/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public enum ModuleType
{
    None,//无
    Base,//基础模块
    Universal,//通用模块
    Special,//特殊模块
}

public abstract class BaseUIModule : ICanClearUp
{
    protected ModuleType _ModuleType = ModuleType.None;
    public ModuleType ModuleType
    {
        get { return _ModuleType; }
    }
    protected EventDispatcheHelper m_EventHelper = new EventDispatcheHelper();
    public BaseUIModule()
    {
        _ModuleType = ModuleType.Base;
        AddEvents();
        m_EventHelper.AddAllEvent();
    }
    /// <summary>
    /// 重连之后UIManager加载完毕
    /// </summary>
    public virtual void ReconnectedPreparedUIManager()
    {

    }
    public abstract void AddEvents();
    public void RemoveEvents()
    {
        m_EventHelper.RemoveAllEvent();
    }
    ///
    public virtual void ClearUp()
    { }
    public virtual void ClearUI()
    { }
}
public abstract class UniversalUIModule : BaseUIModule
{
    public UniversalUIModule()
    {
        _ModuleType = ModuleType.Universal;    
     }
}
public abstract class SpecialUIModule : BaseUIModule
{
    public SpecialUIModule()
    {
        _ModuleType = ModuleType.Special;    
     }
}