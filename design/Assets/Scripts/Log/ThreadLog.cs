#if SERVER && DEBUG || !SERVER
#define DEBUG_LOG
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class ThreadLog : ILog
    {
        System.IO.StreamWriter writer;
        //object mAddLock = new object();
        object mWriteLock = new object();
        object[] temp1Objs = new object[1];
        object[] temp2Objs = new object[2];
        object[] temp3Objs = new object[3];
        object[] temp4Objs = new object[4];

        
        public bool console { get; set; }

        public bool errorOutAssert { get; set; }

        public eLogFlushLevel flushLevel { get; set; }
        public bool out_time { get; set ; }

        struct LogInfo
        {
            public DateTime time;
            public string logHead;
            public string slog;
            public string stack;
        }

        ThreadSafeBlockQuere<LogInfo> mLogs = new ThreadSafeBlockQuere<LogInfo>(100);

        //List<LogInfo> mLogs = new List<LogInfo>();
        //List<LogInfo> mTempLogs = new List<LogInfo>();
        public ThreadLog(string path, bool app)
        {
            //errorOutAssert = true;
            out_time = true;
            flushLevel = eLogFlushLevel.eError;
            var dir = System.IO.Path.GetDirectoryName(path);
            try
            {
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                writer = new System.IO.StreamWriter(path, app, System.Text.UTF8Encoding.UTF8);
            }
            catch (Exception e)
            {
                if (console)
                    Console.WriteLine(string.Format("start log error!msg:{0}", e));
                writer = new System.IO.StreamWriter(path + System.DateTime.Now.ToString("yyyyMMdd HHmmss") + ".log", app, System.Text.UTF8Encoding.UTF8);
            }
            Log("start log");
        }

        public void Dispose()
        {
            lock (mWriteLock)
            {
                if (writer != null)
                {
                    Flush();
                    writer.Dispose();
                    writer = null;
                }
            }
        }

        public void Flush()
        {
            //lock(mAddLock)
            //{
            //    var temp = mTempLogs;
            //    mTempLogs = mLogs;
            //    mLogs = temp;
            //}
            lock (mWriteLock)
            {
                if (writer == null)
                    return;
                //if (mTempLogs.Count == 0)
                //    return;
                //foreach (var log in mTempLogs)
                mLogs.ProcessDatas((log) =>
                {
                    {
                        if (out_time)
                        {
                            writer.Write(log.time);
                        }
                        writer.Write(log.logHead);
                        writer.WriteLine(log.slog);
                        if (!string.IsNullOrEmpty(log.stack))
                        {
                            writer.Write("\nstack:");
                            writer.WriteLine(log.stack);
                        }
                    }
                });
                writer.Flush();
            }
        }

        void AddLog(string logHead, string slog, string stack)
        {
            LogInfo logInfo;
            logInfo.logHead = logHead;
            logInfo.slog = slog;
            logInfo.stack = stack;
            logInfo.time = System.DateTime.Now;
            if (console)
                Console.WriteLine(slog);
            //lock (mAddLock)
            {
                mLogs.Add(logInfo);
            }
        }

        public void LogDebug(string format, params object[] objs)
        {
#if DEBUG_LOG
            AddLog(":LogDebug:", string.Format(format, objs), null);
#endif
        }

        public void LogDebug(string format)
        {
#if DEBUG_LOG
            AddLog(":LogDebug:", format, null);
#endif
        }

        public void LogDebug(string format, object obj1)
        {
#if DEBUG_LOG
            this.temp1Objs[0] = obj1;
            AddLog(":LogDebug:", string.Format(format, temp1Objs), null);
#endif
        }

        public void LogDebug(string format, object obj1, object obj2)
        {
#if DEBUG_LOG
            this.temp2Objs[0] = obj1;
            this.temp2Objs[1] = obj2;
            AddLog(":LogDebug:", string.Format(format, temp2Objs), null);
#endif
        }

        public void LogDebug(string format, object obj1, object obj2, object obj3)
        {
#if DEBUG_LOG
            this.temp3Objs[0] = obj1;
            this.temp3Objs[1] = obj2;
            this.temp3Objs[2] = obj3;
            AddLog(":LogDebug:", string.Format(format, temp3Objs), null);
#endif
        }

        public void LogDebug(string format, object obj1, object obj2, object obj3, object obj4)
        {
#if DEBUG_LOG
            this.temp4Objs[0] = obj1;
            this.temp4Objs[1] = obj2;
            this.temp4Objs[2] = obj3;
            this.temp4Objs[3] = obj4;
            AddLog(":LogDebug:", string.Format(format, temp4Objs), null);
#endif
        }

        public void Log(string format, params object[] objs)
        {
            AddLog(":log:", string.Format(format , objs ), null);
        }

        public void Log(string format)
        {
            AddLog(":log:", format, null);
        }

        public void Log(string format, object obj1)
        {
            this.temp1Objs[0] = obj1;
            AddLog(":log:", string.Format(format, temp1Objs), null);
        }

        public void Log(string format, object obj1, object obj2)
        {
            this.temp2Objs[0] = obj1;
            this.temp2Objs[1] = obj2;
            AddLog(":log:", string.Format(format, temp2Objs), null);
        }

        public void Log(string format, object obj1, object obj2, object obj3)
        {
            this.temp3Objs[0] = obj1;
            this.temp3Objs[1] = obj2;
            this.temp3Objs[2] = obj3;
            AddLog(":log:", string.Format(format, temp3Objs), null);
        }

        public void Log(string format, object obj1, object obj2, object obj3, object obj4)
        {
            this.temp4Objs[0] = obj1;
            this.temp4Objs[1] = obj2;
            this.temp4Objs[2] = obj3;
            this.temp4Objs[3] = obj4;
            AddLog(":log:", string.Format(format, temp4Objs), null);
        }

        public void LogError(string format, params object[] objs)
        {
            AddLog(":error:", string.Format(format, objs), null);
        }
        public void LogError(string format)
        {
            AddLog(":error:", format, null);
        }
        public void LogError(string format, object obj1)
        {
            this.temp1Objs[0] = obj1;
            AddLog(":error:", string.Format(format, temp1Objs), null);
        }


        public void LogError(string format, object obj1, object obj2)
        {
            this.temp2Objs[0] = obj1;
            this.temp2Objs[1] = obj2;
            AddLog(":error:", string.Format(format, temp2Objs), null);
        }

        public void LogError(string format, object obj1, object obj2, object obj3)
        {
            this.temp3Objs[0] = obj1;
            this.temp3Objs[1] = obj2;
            this.temp3Objs[2] = obj3;
            AddLog(":error:", string.Format(format, temp3Objs), null);
        }

        public void LogError(string format, object obj1, object obj2, object obj3, object obj4)
        {
            this.temp4Objs[0] = obj1;
            this.temp4Objs[1] = obj2;
            this.temp4Objs[2] = obj3;
            this.temp4Objs[3] = obj4;
            AddLog(":error:", string.Format(format, temp4Objs), null);
        }

        public void LogWarning(string format, params object[] objs)
        {
            AddLog(":warning:", string.Format(format, objs), null);
        }

        public void LogWarning(string format)
        {
            AddLog(":warning:", format, null);
        }
        public void LogWarning(string format, object obj1)
        {
            this.temp1Objs[0] = obj1;
            AddLog(":warning:", string.Format(format, temp1Objs), null);
        }

        public void LogWarning(string format, object obj1, object obj2)
        {
            this.temp2Objs[0] = obj1;
            this.temp2Objs[1] = obj2;
            AddLog(":warning:", string.Format(format, temp2Objs), null);
        }

        public void LogWarning(string format, object obj1, object obj2, object obj3)
        {
            this.temp3Objs[0] = obj1;
            this.temp3Objs[1] = obj2;
            this.temp3Objs[2] = obj3;
            AddLog(":warning:", string.Format(format, temp3Objs), null);
        }

        public void LogWarning(string format, object obj1, object obj2, object obj3, object obj4)
        {
            this.temp4Objs[0] = obj1;
            this.temp4Objs[1] = obj2;
            this.temp4Objs[2] = obj3;
            this.temp4Objs[3] = obj4;
            AddLog(":warning:", string.Format(format, temp4Objs), null);
        }

        public ILog ToThreadSafe()
        {
            return this;
        }
    }
}
