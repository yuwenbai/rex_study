///王庆伟
using System;
using System.Threading;

namespace Game
{
#if SERVER
    //只保证单线程的顺序
    public class ThreadSafeBlockQuere<T>
    {
        System.Collections.Concurrent.ConcurrentQueue<T> mQueue = new System.Collections.Concurrent.ConcurrentQueue<T>();
        //public int index = 0;
        public ThreadSafeBlockQuere(int _BlockSize)
        {
        }
        public void Add(T v)
        {
            mQueue.Enqueue(v);
        }


        //此函数是线程不安全的，只能由一个线程去处理
        public void ProcessDatas(Action<T> _process_fun)
        {
            while (mQueue.Count>0)
            {
                T v;
                if( mQueue.TryDequeue( out v ))
                {
                    _process_fun(v);
                }
            }
        }
        
        public void Clear()
        {
            mQueue = new System.Collections.Concurrent.ConcurrentQueue<T>();
        }
    }
#else
    public class ThreadSafeBlockQuere<T>
    {
        ThreadSafeQueue<T> mQueue = new ThreadSafeQueue<T>();
        //public int index = 0;
        public ThreadSafeBlockQuere(int _BlockSize)
        {
        }
        public void Add(T v)
        {
            mQueue.AddData(v);
        }


        //此函数是线程不安全的，只能由一个线程去处理
        public void ProcessDatas(Action<T> _process_fun)
        {
            mQueue.ProcessDatas((q)=>
            {
                while(q.Count>0)
                {
                    _process_fun(q.Dequeue());
                }
            });
        }

        public void Clear()
        {
            mQueue.Clear();
        }
    }
#endif
}
