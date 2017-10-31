﻿using UnityEngine;
using System.Text;
using System.Collections.Generic;

public enum LogColor
{
    NONE = 0,
    BLUE,
    YELLO,
    RED,
    GREEN,
    AQUA,
    WHITE,
}
public enum DLogType
{
    Log = 1,
    Warning,
    Error,
    Assert,
}
public enum DLogLevel
{
    None = 0,
    Log,
    Warning,
    Error,
    Assert,
}
public class DLog
{
   
    public static DLogLevel MinLogLevel = DLogLevel.None;
    private static bool IsShow(DLogType _type)
    {
        int ret = (int)_type - (int)MinLogLevel;
        if (ret < 0) return false;
        return true;
    }
    protected static string ColorString(LogColor _color)
    {
        string ret = null;
        switch(_color)
        {
            case LogColor.BLUE:
                ret = "<color=blue>";
                break;
            case LogColor.YELLO:
                ret = "<color=yellow>";
                break;
            case LogColor.RED:
                ret = "<color=red>";
                break;
            case LogColor.GREEN:
                ret = "<color=green>";
                break;
            case LogColor.AQUA:
                ret = "<color=aqua>";
                break;
            case LogColor.WHITE:
                ret = "<color=white>";
                break;
        }
        return ret;
    }
    public static string GetString(string _str, params object[] _params)
    {
        StringBuilder tmsgbuilder = new StringBuilder();
        tmsgbuilder.Append(_str);
        if (_params != null)
        {
            for (int i = 0; i < _params.Length; i++)
            {
                object tobj = _params[i];
                if (tobj == null) continue;
                tmsgbuilder.Append(tobj.ToString());
            }
        }

        return tmsgbuilder.ToString();
    }

    public static void LOGColor(DLogType _type, object _object, LogColor _color)
    {
        if (!IsShow(_type)) return;
        
        StringBuilder tbuilder = new StringBuilder();
        tbuilder.Append(_object == null ? "Null" : _object.ToString());

        string tcolorstr = ColorString(_color);
        if (tcolorstr != null)
        {
            tbuilder.Insert(0, tcolorstr);
            tbuilder.Append("</color>");
        }

        string tmsg = tbuilder.ToString();
        switch (_type)
        {
            case DLogType.Log:
                UnityEngine.Debug.Log(tmsg);
                break;
            case DLogType.Error:
                UnityEngine.Debug.LogError(tmsg);
                break;
            case DLogType.Warning:
                UnityEngine.Debug.LogWarning(tmsg);
                break;
            case DLogType.Assert:
                UnityEngine.Debug.LogAssertion(tmsg);
                break;
            default:
                break;
        }
    }
    public static void LOG(DLogType _type, object _object)
    {
        if (!IsShow(_type)) return;
        LOGColor(_type, _object, LogColor.NONE);
    }

    public static void LOGFormat(DLogType _type, string _msg,params object[] _params)
    {
        if (!IsShow(_type)) return;
        LOGColor(_type, GetString(_msg, _params), LogColor.NONE);
    }
}

