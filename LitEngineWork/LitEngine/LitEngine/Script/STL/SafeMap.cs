using System.Threading;
using System.Collections.Generic;
using UnityEngine;
namespace LitEngine
{
    public class SafeMap<K, T>
    {
        private AutoResetEvent mLock = new AutoResetEvent(true);
        private Dictionary<K, T> mDMap = new Dictionary<K, T>();

        public T this[K key]
        {
            get
            {
                Lock();
                T ret = mDMap[key];
                UnLock();
                return ret;
            }
            set
            {
                Lock();
                mDMap[key] = value;
                UnLock();
            }
        }

        public Dictionary<K, T>.ValueCollection values
        {
            get
            {
                Lock();
                Dictionary<K, T>.ValueCollection ret = mDMap.Values;
                UnLock();
                return ret;
            }
        }

        public Dictionary<K, T>.KeyCollection Keys
        {
            get
            {
                Lock();
                Dictionary<K, T>.KeyCollection ret = mDMap.Keys;
                UnLock();
                return ret;
            }
        }

        public bool ContainsKey(K _key)
        {
            Lock();
            bool ret = mDMap.ContainsKey(_key);
            UnLock();
            return ret;
        }

        public bool ContainsValue(T _value)
        {
            Lock();
            bool ret = mDMap.ContainsValue(_value);
            UnLock();
            return ret;
        }

        public void Add(K _key, T _value)
        {
            Lock();
            if (!mDMap.ContainsKey(_key))
                mDMap.Add(_key, _value);
            else
                DLog.LOG(DLogType.Error, string.Format("MsgMap关键字={0}:不能重复添加.", _key));
            UnLock();
        }

        public void Remove(K _key)
        {
            Lock();
            if (mDMap.ContainsKey(_key))
                mDMap.Remove(_key);
            else
                DLog.LOG(DLogType.Error, string.Format("MsgMap关键字={0}:找不到要删除的对象.", _key));
            UnLock();
        }

        public void Clear()
        {
            Lock();
            mDMap.Clear();
            UnLock();
        }

        public int Count()
        {
            Lock();
            int ret = mDMap.Count;
            UnLock();
            return ret;
        }

        private void Lock()
        {
            mLock.WaitOne();
        }

        private void UnLock()
        {
            mLock.Set();
        }
    }
}

