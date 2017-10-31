namespace LitEngine
{
    namespace Loader
    {
        public class LoadTask
        {
            #region 属性
            public long mIndexKey;
            protected string mTaskKey;
            protected object mTargetObject;
            protected System.Action<string, object, object> mCallBack;
            protected System.Action<string, float> mProgressCall;
            protected BaseBundle mBundle;
            protected LoadTaskVector mParent;
            #endregion
            public LoadTask(string _key, BaseBundle _bundle, System.Action<string, object, object> _callThreeParmater, System.Action<string, float> _progress, object _Targetobj)
            {
                mTaskKey = _key;
                mBundle = _bundle;
                mCallBack = _callThreeParmater;
                mTargetObject = _Targetobj;
                mProgressCall = _progress;
            }

            public LoadTaskVector Parent
            {
                get
                {
                    return mParent;
                }

                set
                {
                    mParent = value;
                }
            }

            #region 执行任务

            public bool IsDone()
            {
                if (!mBundle.Loaded)
                {
                    if (mProgressCall != null)
                        mProgressCall(mTaskKey, mBundle.Progress);
                    return false;
                }
                if (mProgressCall != null)
                    mProgressCall(mTaskKey, mBundle.Progress);
                if (mCallBack != null)
                {
                    mBundle.Retain();
                    mCallBack(mTaskKey, mBundle.Asset, mTargetObject);

                }
                DestoryTaskFormParent();
                return true;

            }


            #endregion
            public virtual void DestoryTaskFormParent()
            {
                if (Parent != null)
                    Parent.Remove(this);
            }
        }

    }
}

