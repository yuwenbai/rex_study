using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
class EventDispatcher : SingletonTamplate<EventDispatcher>
{
    private IDictionary<GEnum.NamedEvent, System.Action<object[]>> m_event;

    public EventDispatcher()
    {
        m_event = new Dictionary<GEnum.NamedEvent, Action<object[]>>();
    }
    public static void AddEvent(GEnum.NamedEvent k, System.Action<object[]> v)
    {
        if (Instance.m_event.ContainsKey(k))
        {
            Instance.m_event[k] -= v;
            Instance.m_event[k] += v;
        } else
        {
            Instance.m_event[k] += v;
        }
    }
    public static void FireEvent(GEnum.NamedEvent k , params object[] var)
    {
        if (Instance.m_event.ContainsKey(k) && Instance.m_event[k] != null)
        {
            Instance.m_event[k](var);
        }
        else
        {
            UnityEngine.Debug.Log("rextest FireEvent Get The Invalid NamedEvent k " + k);
        }
    }
}