#if SERVER && DEBUG || !SERVER
#define DEBUG_LOG
#endif

using System;
using System.Collections.Generic;
#if SERVER && DEBUG
using System.Diagnostics;
#endif
using System.Linq;
using System.Text;

namespace Game
{
    public class FileLog : ILog
    {
        System.IO.StreamWriter writer;
        ILog mSafeLog;
        int time = 0;
        object[] temp1Objs = new object[1];
        object[] temp2Objs = new object[2];
        object[] temp3Objs = new object[3];
        object[] temp4Objs = new object[4];
        public bool console { get; set; }
        public bool errorOutAssert { get; set; }
        public eLogFlushLevel flushLevel { get; set; }
        public bool out_time { get; set; }

        public FileLog(string path, bool app)
        {
            errorOutAssert = true;
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
                writer = new System.IO.StreamWriter(path + System.DateTime.Now.ToString("yyyyMMdd HH_mm_ss") + ".log", app, System.Text.UTF8Encoding.UTF8);
            }
            Log("start log");
        }

        public void Log(string format)
        {
            if (writer == null) return;
            begin();
            var msg = format;
            if (console)
                Console.WriteLine(format);
            if (out_time)
            {
                writer.Write(System.DateTime.Now);
                writer.Write(":log:");
            }
            else
            {
                writer.Write("log:");
            }
            writer.WriteLine(msg);
            if (flushLevel >= eLogFlushLevel.eLog)
            {
                writer.Flush();
            }
            end();
        }

        public void LogWarning(string format)
        {
            if (writer == null) return;
            begin();
            var msg = format;
            if (console)
                Console.WriteLine(msg);
            if (out_time)
            {
                writer.Write(System.DateTime.Now);
                writer.Write(":warning:");
            }
            else
            {
                writer.Write("warning:");
            }
            writer.WriteLine(msg);
            if (flushLevel >= eLogFlushLevel.eWarning)
            {
                writer.Flush();
            }
            end();
        }

        public void LogError(string format)
        {
            if (writer == null) return;
            begin();
            var msg = format;
            if (console)
                Console.WriteLine(msg);
            if (out_time)
            {
                writer.Write(System.DateTime.Now);
                writer.Write(":error:");
            }
            else
            {
                writer.Write("error:");
            }
            writer.WriteLine(msg);
            if (errorOutAssert)
            {
                //writer.Write("\nstack:");
                //writer.WriteLine(System.Environment.StackTrace);
#if SERVER && DEBUG
                Debug.Assert(false, msg);
#endif
            }
            if (flushLevel >= eLogFlushLevel.eError)
            {
                writer.Flush();
            }
            end();
        }
        public void Log(string format, params object[] objs)
        {
            if (writer == null) return;
            begin();
            var msg = string.Format(format, objs);
            if (console)
                Console.WriteLine(format, objs);
            if (out_time)
            {
                writer.Write(System.DateTime.Now);
                writer.Write(":log:");
            }
            else
            {
                writer.Write("log");
            }
            writer.WriteLine(msg);
            if (flushLevel >= eLogFlushLevel.eLog)
            {
                writer.Flush();
            }
            end();
        }
        public void LogDebug(string format, params object[] objs)
        {
#if DEBUG_LOG
            if (writer == null) return;
            begin();
            var msg = string.Format(format, objs);
            if (console)
                Console.WriteLine(msg);
            if (out_time)
            {
                writer.Write(System.DateTime.Now);
                writer.Write(":LogDebug:");
            }
            else
            {
                writer.Write("LogDebug");
            }
            writer.WriteLine(msg);
            if (flushLevel >= eLogFlushLevel.eWarning)
            {
                writer.Flush();
            }
            end();
#endif
        }
        public void LogWarning(string format, params object[] objs)
        {
            if (writer == null) return;
            begin();
            var msg = string.Format(format, objs);
            if (console)
                Console.WriteLine(msg);
            if (out_time)
            {
                writer.Write(System.DateTime.Now);
                writer.Write(":warning:");
            }
            else
            {
                writer.Write("warning");
            }
            writer.WriteLine(msg);
            if (flushLevel >= eLogFlushLevel.eWarning)
            {
                writer.Flush();
            }
            end();
        }
        public void LogError(string format, params object[] objs)
        {
            if (writer == null) return;
            begin();
            var msg = string.Format(format, objs);
            if (console)
                Console.WriteLine(msg);
            if (out_time)
            {
                writer.Write(System.DateTime.Now);
                writer.Write(":error:");
            }
            else
            {
                writer.Write("error:");
            }
            writer.WriteLine(msg);
            if (errorOutAssert)
            {
                //writer.Write("\nstack:");
                //writer.WriteLine(System.Environment.StackTrace);
#if SERVER && DEBUG
                //Debug.Assert(false, msg);
#endif
            }
            if (flushLevel >= eLogFlushLevel.eError)
            {
                writer.Flush();
            }
            end();
        }
        public void Flush()
        {
            if (writer == null) return;
            writer.Flush();
        }
        public void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }
        public ILog ToThreadSafe()
        {
            lock (this)
            {
                if (mSafeLog == null)
                    return mSafeLog = new SafeLogWap(this);
                return mSafeLog;
            }
        }
//        public void LogDebug(string format)
//        {
//#if DEBUG_LOG
//            LogDebug(format,ArrayPool<object>.zero);
//#endif
//        }
        public void LogDebug(string format, object obj1)
        {
#if DEBUG_LOG
            var objs = temp1Objs;
            objs[0] = obj1;
            LogDebug(format, objs);
#endif
        }

        public void LogDebug(string format, object obj1, object obj2)
        {
#if DEBUG_LOG
            var objs = temp2Objs;
            objs[0] = obj1;
            objs[1] = obj2;
            LogDebug(format, objs);
#endif
        }

        public void LogDebug(string format, object obj1, object obj2, object obj3)
        {
#if DEBUG_LOG
            var objs = temp3Objs;
            objs[0] = obj1;
            objs[1] = obj2;
            objs[2] = obj3;
            LogDebug(format, objs);
#endif
        }

        public void LogDebug(string format, object obj1, object obj2, object obj3, object obj4)
        {
#if DEBUG_LOG
            var objs = temp4Objs;
            int i = 0;
            objs[i++] = obj1;
            objs[i++] = obj2;
            objs[i++] = obj3;
            objs[i++] = obj4;
            LogDebug(format, objs);
#endif
        }
        public void Log(string format, object obj1)
        {
            var objs = temp1Objs;
            objs[0] = obj1;
            Log(format, objs);
        }

        public void Log(string format, object obj1, object obj2)
        {
            var objs = temp2Objs;
            objs[0] = obj1;
            objs[1] = obj2;
            Log(format, objs);
        }

        public void Log(string format, object obj1, object obj2, object obj3)
        {
            var objs = temp3Objs;
            objs[0] = obj1;
            objs[1] = obj2;
            objs[2] = obj3;
            Log(format, objs);
        }

        public void Log(string format, object obj1, object obj2, object obj3, object obj4)
        {
            var objs = temp4Objs;
            int i = 0;
            objs[i++] = obj1;
            objs[i++] = obj2;
            objs[i++] = obj3;
            objs[i++] = obj4;
            Log(format, objs);
        }

        public void LogWarning(string format, object obj1)
        {
            var objs = temp1Objs;
            int i = 0;
            objs[i++] = obj1;
            LogWarning(format, objs);
        }

        public void LogWarning(string format, object obj1, object obj2)
        {
            var objs = temp2Objs;
            int i = 0;
            objs[i++] = obj1;
            objs[i++] = obj2;
            LogWarning(format, objs);
        }

        public void LogWarning(string format, object obj1, object obj2, object obj3)
        {
            var objs = temp3Objs;
            int i = 0;
            objs[i++] = obj1;
            objs[i++] = obj2;
            objs[i++] = obj3;
            LogWarning(format, objs);
        }

        public void LogWarning(string format, object obj1, object obj2, object obj3, object obj4)
        {
            var objs = temp4Objs;
            int i = 0;
            objs[i++] = obj1;
            objs[i++] = obj2;
            objs[i++] = obj3;
            objs[i++] = obj4;
            LogWarning(format, objs);
        }

        public void LogError(string format, object obj1)
        {
            var objs = temp1Objs;
            int i = 0;
            objs[i++] = obj1;
            LogError(format, objs);
        }

        public void LogError(string format, object obj1, object obj2)
        {
            var objs = temp2Objs;
            int i = 0;
            objs[i++] = obj1;
            objs[i++] = obj2;
            LogError(format, objs);
        }

        public void LogError(string format, object obj1, object obj2, object obj3)
        {
            var objs = temp3Objs;
            int i = 0;
            objs[i++] = obj1;
            objs[i++] = obj2;
            objs[i++] = obj3;
            LogError(format, objs);
        }

        public void LogError(string format, object obj1, object obj2, object obj3, object obj4)
        {
            var objs = temp4Objs;
            int i = 0;
            objs[i++] = obj1;
            objs[i++] = obj2;
            objs[i++] = obj3;
            objs[i++] = obj4;
            LogError(format, objs);
        }
        void begin()
        {
            time = System.Environment.TickCount;
        }
        void end()
        {
            if (writer == null) return;
            var end_time = System.Environment.TickCount;
            if (end_time - time > 500)
            {
                writer.WriteLine(":flush logs use time:{0}", end_time - time);
            }
        }

        public void LogDebug(string format)
        {
            throw new NotImplementedException();
        }

        class SafeLogWap : ILog
        {
            ILog log;
            public bool console { get { return log.console; } set { log.console = value; } }
            public bool errorOutAssert { get { return log.errorOutAssert; } set { log.errorOutAssert = value; } }
            public eLogFlushLevel flushLevel { get { return log.flushLevel; } set { log.flushLevel = value; } }

            public bool out_time { get { return log.out_time; } set { log.out_time = value ; } }

            public SafeLogWap(ILog _log)
            {
                log = _log;
            }
            public void Log(string format, params object[] objs)
            {
                lock (log)
                {
                    log.Log(format, objs);
                }
            }
            public void LogWarning(string format, params object[] objs)
            {
                lock (log)
                {
                    log.LogWarning(format, objs);
                }
            }
            public void LogError(string format, params object[] objs)
            {
                lock (log)
                {
                    log.LogError(format, objs);
                }
            }

            public void Flush()
            {
                lock (log)
                {
                    log.Flush();
                }
            }
            public ILog ToThreadSafe()
            {
                return this;
            }
            public void Dispose()
            {
                lock (log)
                {
                    log.Dispose();
                }
            }
            public void LogDebug(string format, params object[] objs)
            {
                lock (log)
                {
                    log.LogDebug(format, objs);
                }
            }

            public void LogDebug(string format)
            {
                lock (log)
                {
                    log.LogDebug(format);
                }
            }

            public void LogDebug(string format, object obj1)
            {
                lock (log)
                {
                    log.LogDebug(format, obj1);
                }
            }

            public void LogDebug(string format, object obj1, object obj2)
            {
                lock (log)
                {
                    log.LogDebug(format, obj1, obj2);
                }
            }

            public void LogDebug(string format, object obj1, object obj2, object obj3)
            {
                lock (log)
                {
                    log.LogDebug(format, obj1, obj2, obj3);
                }
            }

            public void LogDebug(string format, object obj1, object obj2, object obj3, object obj4)
            {
                lock (log)
                {
                    log.LogDebug(format, obj1, obj2, obj3, obj4);
                }
            }

            public void Log(string format, object obj1)
            {
                lock (log)
                {
                    log.Log(format, obj1);
                }
            }

            public void Log(string format, object obj1, object obj2)
            {
                lock (log)
                {
                    log.Log(format, obj1, obj2);
                }
            }

            public void Log(string format, object obj1, object obj2, object obj3)
            {
                lock (log)
                {
                    log.Log(format, obj1, obj2, obj3);
                }
            }

            public void Log(string format, object obj1, object obj2, object obj3, object obj4)
            {
                lock (log)
                {
                    log.Log(format, obj1, obj2, obj3, obj4);
                }
            }

            public void LogWarning(string format, object obj1)
            {
                lock (log)
                {
                    log.LogWarning(format, obj1);
                }
            }

            public void LogWarning(string format, object obj1, object obj2)
            {
                lock (log)
                {
                    log.LogWarning(format, obj1, obj2);
                }
            }

            public void LogWarning(string format, object obj1, object obj2, object obj3)
            {
                lock (log)
                {
                    log.LogWarning(format, obj1, obj2, obj3);
                }
            }

            public void LogWarning(string format, object obj1, object obj2, object obj3, object obj4)
            {
                lock (log)
                {
                    log.LogWarning(format, obj1, obj2, obj3, obj4);
                }
            }

            public void LogError(string format, object obj1)
            {
                lock (log)
                {
                    log.LogError(format, obj1);
                }
            }

            public void LogError(string format, object obj1, object obj2)
            {
                lock (log)
                {
                    log.LogError(format, obj1, obj2);
                }
            }

            public void LogError(string format, object obj1, object obj2, object obj3)
            {
                lock (log)
                {
                    log.LogError(format, obj1, obj2, obj3);
                }
            }

            public void LogError(string format, object obj1, object obj2, object obj3, object obj4)
            {
                lock (log)
                {
                    log.LogError(format, obj1, obj2, obj3, obj4);
                }
            }

            public void Log(string format)
            {
                lock (log)
                {
                    log.Log(format);
                }
            }

            public void LogWarning(string format)
            {
                lock (log)
                {
                    log.LogWarning(format);
                }
            }

            public void LogError(string format)
            {
                lock (log)
                {
                    log.LogError(format);
                }
            }
        }
    }
}
