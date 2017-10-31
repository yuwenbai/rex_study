using System;
using System.Collections.Generic;
using UnityEngine;
namespace LitEngine
{
    public class UpdateBase
    {
        protected Action<float> mOneDelegate = null;
        protected Action mZeroDelegate = null;
        protected float mMaxTime = 0.1f;
        protected float mTimer = 0;
        protected bool mIsUseTimer = true;
        protected float mDeltatime;
        public float UpdateTimer
        {
            get { return mTimer; }
            set { mTimer = value; }
        }

        public float MaxTime
        {
            get
            {
                return mMaxTime;
            }
            set
            {
                mMaxTime = value;
                if (mMaxTime > 0)
                    mIsUseTimer = true;
                else
                    mIsUseTimer = false;
            }
        }
        public UpdateBase(Action<float> _delegate)
        {
            mOneDelegate = _delegate;
        }

        public UpdateBase(Action _delegate)
        {
            mZeroDelegate = _delegate;
        }
        public UpdateBase()
        {
 
        }
        virtual public void RunDelgete()
        {

        }

        virtual public void CallMethod()
        {
            if (mOneDelegate != null)
                mOneDelegate(mDeltatime);
            else if (mZeroDelegate != null)
                mZeroDelegate();
        }

        virtual public bool IsTimeOut()
        {
            if (mIsUseTimer)
            {
                if (Time.realtimeSinceStartup < mTimer) return false;
                mDeltatime = Time.realtimeSinceStartup - mTimer + mMaxTime;
                mTimer = Time.realtimeSinceStartup + mMaxTime;
            }
            return true;
        }
    }
    public class UpdateObject : UpdateBase
    {
        public UpdateObject(Action<float> _delegate):base(_delegate)
        {

        }

        public UpdateObject(Action _delegate) : base(_delegate)
        {

        }
        override public void RunDelgete()
        {
            if (!IsTimeOut()) return;
            CallMethod();
        }

    }

    public class UpdateGroup : UpdateBase
    {
        private List<UpdateObject> GroupList;

        private int mUpdateCount = 0;
        private int mNowUpdateCount = 0;
        private int mUpdateCountEveryFrame = 0;
        public int UpdateCountEveryFrame
        {
            get { return mUpdateCountEveryFrame; }
            set { mUpdateCountEveryFrame = value < 0 ? 0 : value;}
        }
        public int Count
        {
            get;
            private set;
        }
        public UpdateGroup()
        {
            GroupList = new List<UpdateObject>();
            Count = 0;
            UpdateCountEveryFrame = 0;
        }
        public void AddObject(UpdateObject _obj)
        {
            GroupList.Add(_obj);
            Count++;
        }
        public void Remove(UpdateObject _obj)
        {
            if (GroupList.Contains(_obj))
            {
                GroupList.Remove(_obj);
                Count--;
            }
        }


        override public void RunDelgete()
        {
            if (!IsTimeOut()) return;
            if (UpdateCountEveryFrame == 0)
                UpdateAll();
            else
                UpdatePerCount();
        }

        private void UpdateAll()
        {
            short i = 0;
            for (; i < Count; i++)
            {
                GroupList[i].RunDelgete();
            }
        }
        private void UpdatePerCount()
        {
            int i = mUpdateCount;
            for (; i < Count; i++)
            {
                GroupList[i].RunDelgete();
                mUpdateCount++;
                mNowUpdateCount++;
                if (mNowUpdateCount == UpdateCountEveryFrame)
                {
                    mNowUpdateCount = 0;
                    break;
                }

            }
            if (mUpdateCount >= Count)
            {
                mUpdateCount = 0;
                mNowUpdateCount = 0;
            }
        }
    }
}


