using System.Threading;
using System.Collections.Generic;
namespace LitEngine
{
    public class SafeQueue<T>
    {
        private AutoResetEvent mLock = new AutoResetEvent(true);

        private Queue<T> mQueue = new Queue<T>();
        public void Enqueue(T _obj)
        {
            Lock();
            if (_obj == null)
            {
                DLog.LOG(DLogType.Error, "Enqueue 不能添加空的对象到队列!");
                return;
            }
            mQueue.Enqueue(_obj);
            UnLock();
        }
        public T Dequeue()
        {
            Lock();
            if (mQueue.Count == 0)
            {
                DLog.LOG(DLogType.Error, "Dequeue 已经是空队列了!");
                UnLock();
                return default(T);
            }
            T ret = mQueue.Dequeue();
            UnLock();
            return ret;
        }
        public T Peek()
        {
            Lock();
            if (mQueue.Count == 0)
            {
                DLog.LOG(DLogType.Error, "Peek 已经是空队列了!");
                UnLock();
                return default(T);
            }
            T ret = mQueue.Peek();
            UnLock();
            return ret;
        }
        public int Count
        {
            get
            {
                Lock();
                int ret = mQueue.Count;
                UnLock();
                return ret;
            }
        }
        public void Clear()
        {
            Lock();
            mQueue.Clear();
            UnLock();
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
