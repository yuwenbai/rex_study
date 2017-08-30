using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonTamplate<T> where T : new()
{
    private static T m_instance;
#if NEED_ASYNC_LOCK
    private static object objLock = new System.Object();
#endif
    public static T Instance
    {
        get {
            if (m_instance == null)
            {
#if NEED_ASYNC_LOCK
                lock (objLock) {
#endif
                m_instance = MakeInstance();
#if NEED_ASYNC_LOCK
                }
#endif
            }
            return m_instance;
        }
    }

    protected static T MakeInstance()
    {
        return new T();
    }
}
