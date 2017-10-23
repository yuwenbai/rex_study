///王庆伟
using System;
using System.Collections.Generic;
namespace Game
{
    //public delegate void EventProsessDatas<T>(IEnumerable<T> datas);
    //public delegate void EventProsessDatas<T>(Queue<T> datas);

    public class ThreadSafeQueue<T>
    {
        public Action<Queue<T>> ProsessCallBack;
        public object mMutex = new object();
        //public object mMutexExc = new object();

        Queue<T> mDatas = new Queue<T>();
        Queue<T> mDatasTemp = new Queue<T>();
        public int AddData(T data)
        {
            lock (mMutex)
            {
                mDatas.Enqueue(data);
                return mDatas.Count;
            }
        }
        public int AddData(IEnumerable<T> datas)
        {
            lock (mMutex)
            {
                foreach(var data in datas)
                {
                    mDatas.Enqueue(data);
                }
                return mDatas.Count;
            }
        }
        public int dataCount{
            get {
                return mDatas.Count;
            }
        }

        public void Clear()
        {
            lock (mMutex)
            {
                mDatas.Clear();
                mDatasTemp.Clear();
            }
        }
        
        public void ProcessDatas( Action<Queue<T> > _process_fun )
        {
            if (mDatas.Count == 0)
                return;
            lock (mMutex)
            {
                if (mDatasTemp.Count == 0)
                {
                    var temp = mDatas;
                    mDatas = mDatasTemp;
                    mDatasTemp = temp;
                }
            }
            
            if (_process_fun != null)
            {
                _process_fun(mDatasTemp);
            }
            mDatasTemp.Clear();
        }

        public void ProseccDatas()
        {
            ProcessDatas(ProsessCallBack);
        }
    }
}
