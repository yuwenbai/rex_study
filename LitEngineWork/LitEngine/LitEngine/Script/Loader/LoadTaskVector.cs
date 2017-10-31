using UnityEngine;
using System.Collections.Generic;
namespace LitEngine
{
    namespace Loader
    {
        public class LoadTaskVector
        {
            private Dictionary<long, LoadTask> mList = null;
            private long mNowIndex = 0;
            public Dictionary<long, LoadTask> TaskList
            {
                get { return mList; }
            }
            public LoadTaskVector()
            {
                mList = new Dictionary<long, LoadTask>();
            }

            public LoadTask this[long key]
            {
                get
                {
                    return mList[key];
                }
                set
                {
                    mList[key] = value;
                }
            }

            public Dictionary<long, LoadTask>.ValueCollection values
            {
                get
                {
                    return mList.Values;
                }
            }

            public Dictionary<long, LoadTask>.KeyCollection Keys
            {
                get
                {
                    return mList.Keys;
                }
            }

            public int Count
            {
                get
                {
                    return mList.Count;
                }
            }
            public bool ContainsKey(long _key)
            {
                return mList.ContainsKey(_key);
            }
            public void Add(LoadTask _task)
            {
                _task.Parent = this;
                _task.mIndexKey = mNowIndex;
                mList.Add(mNowIndex, _task);
                mNowIndex++;
            }

            public void Remove(LoadTask _task)
            {
                if (_task == null)
                {
                    Debug.LogWarning("can not remove null");
                    return;
                }
                if (!mList.ContainsKey(_task.mIndexKey))
                    return;
                mList.Remove(_task.mIndexKey);
            }
            public void RemoveByKey(long _key)
            {
                if (!mList.ContainsKey(_key))
                    return;
                mList.Remove(_key);
            }

            public void Clear()
            {
                mList.Clear();
                mNowIndex = 0;
            }
        }
    }
}


