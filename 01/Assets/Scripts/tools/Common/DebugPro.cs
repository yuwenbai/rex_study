using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugPro {
    public enum EnumLog
    {
        System,
        UI,
        NetWork,
        MemoryData,
        HeadUrl,
    }

    public static int LogMaxSaveCount = 1000;
    private static List<string> logList = new List<string>();
    private static StringBuilder logTemp = new StringBuilder();
    private const string LogProfix = "Log: ";
    private const string WarningLogProfix = "WarningLog: ";
    private const string ErrorLogProfix = "ErrorLog: ";

    public static void Log(EnumLog enu,params object[] strs)
    {
#if __DEBUG_LOG
        StringBuilder sb = new StringBuilder();
        sb.Append("=====================");
        sb.AppendLine();
        sb.Append("||");
        sb.Append(enu.ToString());
        sb.Append("==");
        for(int i=0; i< strs.Length;++i)
        {
            if (strs[i] == null)
                sb.Append("null");
            else
                sb.Append(strs[i].ToString());
            sb.AppendLine();
        }
        if(enu == EnumLog.HeadUrl)
        {
            sb.Append("时间：").Append(Time.realtimeSinceStartup);
        }
        sb.Append("=====================");
        sb.AppendLine();
        LogWarning(sb.ToString());
#endif
    }
    public static void Log(string str)
    {
        projectQ.QLoger.LOG(str);
        AddLog(LogProfix, str);
    }
    public static void LogWarning(string str)
    {
        Debug.LogWarning(str);
        AddLog(WarningLogProfix, str);
    }
    public static void LogError(string str)
    {
        projectQ.QLoger.ERROR(str);
        AddLog(ErrorLogProfix, str);
    }


    private static void AddLog(string profix,string log)
    {
        logTemp.Length = 0;
        logTemp.Append(profix).Append(log);
        logList.Add(logTemp.ToString());

        if (logList.Count >= LogMaxSaveCount)
        {
            SaveLog();
        }
    }

    private static void SaveLog()
    {
        //本地保存
    }

    #region　颜色日志信息 
    public enum ColorType
    {
        None,
        Green,
        Yellow,
        Red,
        Aqua,
    }
    private static StringBuilder _debugStringBuilder = new StringBuilder();
    private static bool AppendInfo(ColorType type,params object[] strs)
    {
        if (strs == null || strs.Length == 0)
        {
            return false;
        }
        _debugStringBuilder.Length = 0;
        bool isColored = true;
        switch (type)
        {
            case ColorType.Green:
                _debugStringBuilder.Append("<color=green>");
                break;
            case ColorType.Yellow:
                _debugStringBuilder.Append("<color=yellow>");
                break;
            case ColorType.Red:
                _debugStringBuilder.Append("<color=red>");
                break;
            case ColorType.Aqua:
                _debugStringBuilder.Append("<color=aqua>");
                break;
            default:
                isColored = false;
                break;
        }
       
        for (int i = 0; i < strs.Length; i++)
        {
            _debugStringBuilder.Append(strs[i]);
        }
        if (isColored)
        {
            _debugStringBuilder.Append("</color>");
        }
        return true;
    }
    public static void DebugNormalInfo(params object[] strs)
    {
        if (AppendInfo(ColorType.None, strs))
        {
           projectQ.QLoger.LOG(_debugStringBuilder.ToString());
        }
    }
    public static void DebugInfo(params object[] strs)
    {
        if (AppendInfo(ColorType.Green, strs))
        {
            projectQ.QLoger.LOG(_debugStringBuilder.ToString());
        }
    }
    public static void DebugWarning(params object[] strs)
    {
        if (AppendInfo(ColorType.Yellow, strs))
        {
            projectQ.QLoger.LOG(_debugStringBuilder.ToString());
        }
    }
    public static void DebugError(params object[] strs)
    {
        if (AppendInfo(ColorType.Red, strs))
        {
            projectQ.QLoger.LOG(_debugStringBuilder.ToString());
        }
    }

    public static void LogBundle(params object[] strs)
    {
        if (AppendInfo(ColorType.Aqua, strs))
        {
            projectQ.QLoger.LOG(projectQ.LogType.ELog, _debugStringBuilder.ToString());
        }
    }
    #endregion
}
