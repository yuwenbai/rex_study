using System;
using System.Collections.Generic;

namespace Game
{
	public delegate void LogDelegate(string format, params object[] objs);
    public struct MyLog
    {
        public static ILog _log
        {
            get { if (mLog == null) mLog = Logger.Get("Mylog"); return mLog; }
            set { mLog = value; }
        }
#if SERVER

        public static void LogDebug(string format, params object[] objs)
        {
            _log.Log(format, objs);
        }
        public static void LogDebug(string format)
        {
            _log.LogDebug(format);
        }
        public static void LogDebug(string format, object obj1)
        {
            _log.LogDebug(format, obj1);
        }
        public static void LogDebug(string format, object obj1, object obj2)
        {
            _log.LogDebug(format, obj1, obj2);
        }
        public static void LogDebug(string format, object obj1, object obj2, object obj3)
        {
            _log.LogDebug(format, obj1, obj2, obj3);
        }
        public static void LogDebug(string format, object obj1, object obj2, object obj3, object obj4)
        {
            _log.LogDebug(format, obj1, obj2, obj3, obj4);
        }

        public static void Log(string format, params object[] objs)
        {
            _log.Log(format, objs);
        }
        public static void Log(string format)
        {
            _log.Log(format);
        }
        public static void Log(string format, object obj1)
        {
            _log.Log(format, obj1);
        }
        public static void Log(string format, object obj1, object obj2)
        {
            _log.Log(format, obj1, obj2);
        }
        public static void Log(string format, object obj1, object obj2, object obj3)
        {
            _log.Log(format, obj1,obj2,obj3);
        }
        public static void Log(string format, object obj1, object obj2, object obj3, object obj4)
        {
            _log.Log(format, obj1, obj2, obj3 , obj4);
        }

        public static void LogWarning(string format, params object[] objs)
        {
            _log.LogWarning(format, objs);
        }
        public static void LogWarning(string format)
        {
            _log.LogWarning(format);
        }
        public static void LogWarning(string format, object obj1)
        {
            _log.LogWarning(format,obj1);
        }
        public static void LogWarning(string format, object obj1, object obj2)
        {
            _log.LogWarning(format, obj1,obj2);
        }
        public static void LogWarning(string format, object obj1, object obj2, object obj3)
        {
            _log.LogWarning(format, obj1, obj2 , obj3);
        }
        public static void LogWarning(string format, object obj1, object obj2, object obj3, object obj4)
        {
            _log.LogWarning(format, obj1, obj2, obj3 , obj4);
        }
        public static void LogError(string format, params object[] objs)
        {
            _log.LogError(format, objs);
        }
        public static void LogError(string format)
        {
            _log.LogError(format);
        }
        public static void LogError(string format, object obj1)
        {
            _log.LogError(format,obj1);
        }
        public static void LogError(string format, object obj1, object obj2)
        {
            _log.LogError(format, obj1,obj2);
        }
        public static void LogError(string format, object obj1, object obj2, object obj3)
        {
            _log.LogError(format, obj1, obj2 , obj3);
        }
        public static void LogError(string format, object obj1, object obj2, object obj3, object obj4)
        {
            _log.LogError(format, obj1, obj2, obj3 , obj4);
        }
#else	
        public static LogDelegate LogDebug
		{
			get { if (_LogDebug == null) _LogDebug = _log.LogDebug ; return _LogDebug; }
			set { _LogDebug = value; }
		}
        public static LogDelegate Log
		{
			get { if (_Log == null) _Log = _log.Log ; return _Log; }
			set { _Log = value; }
		}
        public static LogDelegate LogWarning
		{
			get { if (_LogWarning == null) _LogWarning = _log.LogWarning; return _LogWarning; }
			set { _LogWarning = value; }
		}
		public static LogDelegate LogError
		{
			get { if (_LogError == null) _LogError = _log.LogError; return _LogError; }
			set { _LogError = value; }
		}
        static LogDelegate _LogDebug;
        static LogDelegate _Log;
        static LogDelegate _LogWarning;
        static LogDelegate _LogError;
#endif
        static ILog mLog;
    }

	public struct SafeLog
	{
		public static ILog _log
		{
			get { if (mLog == null) mLog = Logger.Get("SafeLog").ToThreadSafe(); return mLog; }
			set { mLog = value; }
		}
#if SERVER
        public static void Log(string format, params object[] objs)
        {
            _log.Log(format, objs);
        }
        public static void Log(string format)
        {
            _log.Log(format);
        }
        public static void Log(string format, object obj1)
        {
            _log.Log(format, obj1);
        }
        public static void Log(string format, object obj1, object obj2)
        {
            _log.Log(format, obj1, obj2);
        }
        public static void Log(string format, object obj1, object obj2, object obj3)
        {
            _log.Log(format, obj1,obj2,obj3);
        }
        public static void Log(string format, object obj1, object obj2, object obj3, object obj4)
        {
            _log.Log(format, obj1, obj2, obj3 , obj4);
        }

        public static void LogWarning(string format, params object[] objs)
        {
            _log.LogWarning(format, objs);
        }
        public static void LogWarning(string format)
        {
            _log.LogWarning(format);
        }
        public static void LogWarning(string format, object obj1)
        {
            _log.LogWarning(format,obj1);
        }
        public static void LogWarning(string format, object obj1, object obj2)
        {
            _log.LogWarning(format, obj1,obj2);
        }
        public static void LogWarning(string format, object obj1, object obj2, object obj3)
        {
            _log.LogWarning(format, obj1, obj2 , obj3);
        }
        public static void LogWarning(string format, object obj1, object obj2, object obj3, object obj4)
        {
            _log.LogWarning(format, obj1, obj2, obj3 , obj4);
        }
        public static void LogError(string format, params object[] objs)
        {
            _log.LogError(format, objs);
        }
        public static void LogError(string format)
        {
            _log.LogError(format);
        }
        public static void LogError(string format, object obj1)
        {
            _log.LogError(format,obj1);
        }
        public static void LogError(string format, object obj1, object obj2)
        {
            _log.LogError(format, obj1,obj2);
        }
        public static void LogError(string format, object obj1, object obj2, object obj3)
        {
            _log.LogError(format, obj1, obj2 , obj3);
        }
        public static void LogError(string format, object obj1, object obj2, object obj3, object obj4)
        {
            _log.LogError(format, obj1, obj2, obj3 , obj4);
        }
#else
        public static LogDelegate LogDebug
        {
            get { if (_LogDebug == null) _LogDebug = _log.LogDebug; return _LogDebug; }
            set { _LogDebug = value; }
        }
        public static LogDelegate Log
        {
            get { if (_Log == null) _Log = _log.Log; return _Log; }
            set { _Log = value; }
        }
        public static LogDelegate LogWarning
        {
            get { if (_LogWarning == null) _LogWarning = _log.LogWarning; return _LogWarning; }
            set { _LogWarning = value; }
        }
        public static LogDelegate LogError
        {
            get { if (_LogError == null) _LogError = _log.LogError; return _LogError; }
            set { _LogError = value; }
        }
        static LogDelegate _LogDebug;
        static LogDelegate _Log;
        static LogDelegate _LogWarning;
        static LogDelegate _LogError;
#endif
        static ILog mLog;
	}


	static public class Logger
    {
		//public static ILog Get<T>() { return GetOrCreateLog(typeof(T).Name); }
		public static ILog Get(string name)
        {
            return GetOrCreateLog(name);
        }
        //public static ILog Game { get { return GetOrCreateLog("Game"); } }
        //public static ILog Net { get { return GetOrCreateLog("Net"); } }

        static Dictionary<string, ILog> mLogs = new Dictionary<string,ILog>();
        static Dictionary<string, ILog> mSafeLogs = new Dictionary<string, ILog>();
        static ILog GetOrCreateLog( string name )
        {
            ILog log;
            lock (mLogs)
            {
                if (!mLogs.TryGetValue(name, out log))
                {
                    log = LogCreater(name);
                    mLogs.Add(name, log);
                    mSafeLogs.Add(name, log.ToThreadSafe());
                }
            }
            return log;
        }
        
        public static ILog AddLog( string name , ILog log )
        {
            lock(mLogs)
            {
                mLogs[name] = log;
                mSafeLogs[name] = log;
            }
            return log;
        }

        public delegate ILog CreateLog( string name );

        public static CreateLog LogCreater { get; set; }

        //public static void SetCreateLog(CreateLog createFun)
        //{
        //    CreateLogFun = createFun;
        //}

		public static void FlushAll()
		{
			lock( mLogs )
			{
				foreach( var log in mSafeLogs.Values )
				{
					log.Flush();
				}
			}
		}
        public static void Process(Action<ILog> _callback)
        {
            lock (mLogs)
            {
                foreach (var log in mSafeLogs.Values)
                {
                    _callback(log);
                }
            }
        }
        public static void Stop()
		{
			lock (mLogs)
			{
				foreach (var log in mSafeLogs.Values)
				{
					log.Dispose();
				}
                mSafeLogs.Clear();
                mLogs.Clear();
			}
		}
    }


}
