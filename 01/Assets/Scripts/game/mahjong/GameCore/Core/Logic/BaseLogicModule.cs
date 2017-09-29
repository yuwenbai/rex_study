/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using System;
public abstract class BaseLogicModule : ICanClearUp
{
    protected ModuleType _ModuleType = ModuleType.None;
    public ModuleType ModuleType
    {
        get { return _ModuleType; }
    }
    protected EventDispatcheHelper m_EventHelper = new EventDispatcheHelper();
    public BaseLogicModule()
    {
        _ModuleType = ModuleType.Base;
        AddEvents();
        m_EventHelper.AddAllEvent();
    }
    public abstract void AddEvents();
    public virtual void ClearUp()
    {

    }
    public void RevmoveEvents()
    {
        // m_EventHelper.RemoveAllEvent();
    }
    public virtual void ModuleReconnect()
    { }
}
public abstract class SpecialLogicModule : BaseLogicModule
{
    public SpecialLogicModule()
    {
        _ModuleType = ModuleType.Special;
    }
}
public abstract class UniversalLogicModule : BaseLogicModule
{
    public UniversalLogicModule()
    {
        _ModuleType = ModuleType.Universal;
    }
}
