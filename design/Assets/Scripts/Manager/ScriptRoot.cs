using UnityEngine;
using System.Collections;
using Game;
using System.IO;
using System;

public class ScriptRoot : MonoBehaviour
{
    int d;
    private static ScriptRoot instance = null;


    FileLog fileLog = null;

    public static ScriptRoot Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
    }
    public T ScriptsDataSet<T>() where T : MonoBehaviour
    {
        T data = gameObject.GetComponent<T>();
        if (data == null)
        {
            data = gameObject.AddComponent<T>();
        }

        return data;
    }
    private void Init()
    {
        
        DontDestroyOnLoad(gameObject);
        ScriptsDataSet<TestException>();
        ScriptsDataSet<TestBundle>();
        ScriptsDataSet<TestLoadNewAB>();
        ScriptsDataSet<Car>();
    }
    // Use this for initialization
    void Start()
    {
        InitLog();
        //Application.logMessageReceivedThreaded += logCallback;
    }
    //public static void logCallback(string log, string stackTrace, UnityEngine.LogType _type)
    //{
    //    //string path = Application.dataPath + "/LogFile/" + System.DateTime.Now.ToString("yyyyMMdd") + ".log";
    //    string path = Application.dataPath + "/LogFile/" + System.DateTime.Now.ToString("yyyyMMdd") + "/";

    //    //if (!System.IO.File.Exists(path))
    //    //{

    //    //}
    //    //else
    //    //{
    //    //    Debug.LogError("rextest 222");
    //    //}
    //    var dir = System.IO.Path.GetDirectoryName(path);
    //    if (!System.IO.Directory.Exists(dir))
    //    {

    //    }
    //    if (fileLog == null)
    //    {
    //        fileLog = new FileLog(path, true);
    //    }
    //    fileLog.Log(log);
    //    fileLog.Log(stackTrace);
    //    fileLog.Flush();
    //}
    public static void InitLog()
    {
        if (Game.Logger.LogCreater != null)
            return;
        //var path1 = Application.persistentDataPath;
        var path = Application.dataPath + "/LogFile";
        Game.Logger.LogCreater = (name) =>
        {
            string logRootPath = path + "/log/";
            string filePath = logRootPath + name + ".log";
            try
            {
                FileInfo fileinfo = new FileInfo(filePath);
                if (fileinfo.Exists)
                {
                    string lastPath = string.Format("{0}lastlog.log", logRootPath);
                    File.Delete(lastPath);
                    fileinfo.MoveTo(lastPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogFormat("create log:{0},msg:{1}", name, e);
            }
            var log = new FileLog(filePath, false);
            log.console = false;
            log.errorOutAssert = true;
            return log;
        };
        MyLog._log = Game.Logger.Get("MyLog").ToThreadSafe();
        MyLog.LogDebug = Debug.LogFormat;
        MyLog.Log = Debug.LogFormat;
        MyLog.LogError = Debug.LogErrorFormat;
        MyLog.LogWarning = Debug.LogWarningFormat;
        MyLog._log.flushLevel = eLogFlushLevel.eAll;
        SafeLog._log = MyLog._log;
        SafeLog.LogDebug = Debug.LogFormat;
        SafeLog.Log = Debug.LogFormat;
        SafeLog.LogError = Debug.LogErrorFormat;
        SafeLog.LogWarning = Debug.LogWarningFormat;
        SafeLog._log.flushLevel = eLogFlushLevel.eAll;
        Application.logMessageReceivedThreaded += _Log;
    }
    static void _Log(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            MyLog._log.Log(condition);
            //var _GameConfig = GameConfig.singleton;
            //if (_GameConfig != null && _GameConfig.mCurPlatform != null && _GameConfig.mCurPlatform.debug > 0)
            //    MyLog._log.Flush();`  
        }
        else if (type == LogType.Error)
        {
            MyLog._log.LogError(condition);
            //if (ClientNetMgr.singleton != null && maxSendErrorCount > 0)
            //{
            //    var slog = "Account:" + account + ", log:" + condition + ";\nstack:" + stackTrace;
            //    if (ClientNetMgr.singleton.SendClientLog(LogLevel.Error, slog))
            //    {
            //        maxSendErrorCount--;
            //    }
            //}
            ////MyLog._log.Flush();
        }
        else if (type == LogType.Exception)
        {
            var msg = "msg:" + condition + ".stackTrace:" + stackTrace;
            //if (ClientNetMgr.singleton != null && maxSendErrorCount > 0)
            //{
            //    var device = string.Format("deviceModel:{0},deviceName:{1},deviceType:{2},os:{3}", UnityEngine.SystemInfo.deviceModel, UnityEngine.SystemInfo.deviceName, UnityEngine.SystemInfo.deviceType, UnityEngine.SystemInfo.operatingSystem);
            //    var slog = "Account:" + account + ", log:" + condition + ";\nstack:" + stackTrace;

            //    if (ClientNetMgr.singleton.SendClientLog(LogLevel.Error, device + slog))
            //        maxSendErrorCount--;
            //}
            MyLog._log.LogError(msg);
        }
        else
        {
            MyLog._log.LogWarning(condition + ";\nstack:" + stackTrace);
            //MyLog._log.Flush();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        
        if (GUILayout.Button("main script"))
        {
            ScriptsDataSet<main>();
        }
    }
}
