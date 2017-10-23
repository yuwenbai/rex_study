using System;
#if SERVER
using System.Diagnostics;
#endif
namespace Game
{
    public enum eLogFlushLevel
    {
        eNone,
        eError,
        eWarning,
        eLog,
        eDebug,
        eAll,
    }
    public interface ILog : IDisposable
    {
        bool console { get; set; }
        bool errorOutAssert { get; set; }
        bool out_time { get; set; }

        eLogFlushLevel flushLevel { get; set; }


        void LogDebug(string format, params object[] objs);
        void LogDebug(string format);
        void LogDebug(string format, object obj1);
        void LogDebug(string format, object obj1, object obj2);
        void LogDebug(string format, object obj1, object obj2, object obj3);
        void LogDebug(string format, object obj1, object obj2, object obj3, object obj4);

        void Log(string format, params object[] objs);
        void Log(string format);
        void Log(string format, object obj1);
        void Log(string format, object obj1, object obj2 );
        void Log(string format, object obj1, object obj2, object obj3);
        void Log(string format, object obj1, object obj2, object obj3, object obj4);
        
        void LogWarning(string format, params object[] objs);
        void LogWarning(string format);
        void LogWarning(string format, object obj1);
        void LogWarning(string format, object obj1, object obj2);
        void LogWarning(string format, object obj1, object obj2, object obj3);
        void LogWarning(string format, object obj1, object obj2, object obj3, object obj4);
        void LogError(string format, params object[] objs);
        void LogError(string format);
        void LogError(string format, object obj1);
        void LogError(string format, object obj1, object obj2);
        void LogError(string format, object obj1, object obj2, object obj3);
        void LogError(string format, object obj1, object obj2, object obj3, object obj4);

        //void LogException(Exception e, string format, object obj1);

        ILog ToThreadSafe();

        void Flush();
    }
    
}
