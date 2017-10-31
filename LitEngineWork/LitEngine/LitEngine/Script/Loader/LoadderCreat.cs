using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace LitEngine
{
    namespace Loader
    {

        public class LoaderCreatBase
        {
            protected LoaderManager mLManager = null;
            public string mkey;
            public System.Action<string> mCreatCall;
            public LoaderCreatBase(LoaderManager _lmanager)
            {
                mLManager = _lmanager;
            }
            virtual public void StartLoad()
            {

            }

            virtual public void RemoveAssets(bool _clear)
            {

            }
        }
        public class LoaderCreatObjectBase : LoaderCreatBase
        {

            protected bool mStarted = false;
            protected string mAssetsKey;
            protected string mResname;
            public string AssetsKey
            {
                get
                {
                    return mAssetsKey;
                }
            }
            public bool Started
            {
                get
                {
                    return mStarted;
                }
            }

            public LoaderCreatObjectBase(LoaderManager _lmanager):base(_lmanager)
            {
            }

        }

        public class LoaderCreatObject : LoaderCreatObjectBase
        {
            private string mObjectKey;

            private object mTar;
            private System.Action<string, object, object> mDelgate;
            public LoaderCreatObject(LoaderManager _lmanager,string objkey, string _assetsKey,string _resname, System.Action<string, object, object> _callback, object tar) : base(_lmanager)
            {
                
                mObjectKey = objkey;
                mAssetsKey = _assetsKey;
                mResname = _resname;
                mDelgate = _callback;
                mTar = tar;
            }


            override public void StartLoad()
            {
                if (mStarted) return;
                mStarted = true;
                mLManager.LoadAssetsBundleByRelativePathNameAsync(mObjectKey, mAssetsKey, mResname, LoadCallBack, null, mTar);

            }

            protected void LoadCallBack(string ObjectKey, object _Res, object _Tar)
            {
                if (mDelgate != null)
                    mDelgate(mObjectKey, _Res, _Tar);
                mCreatCall(mkey);
            }

            override public void RemoveAssets(bool _clear)
            {
                mLManager.RemoveBundle(AssetsKey, _clear);
            }
        }


        public class LoaderCreat : LoaderCreatBase
        {
            private Dictionary<string, LoaderCreatBase> mList = new Dictionary<string, LoaderCreatBase>();

            private int mIndex = 0;
            private System.Action<string> mCallFinised;
            private System.Action<string, float> mProgressCallBack;
            private bool mFinished = false;
            private int mLoadedCount = 0;
            private bool mStarted = false;
            private float mProgress = 0;
            private float mOldProgress = 0;
            private int mMaxCount = 0;
            private int mListCount = 0;
            public LoaderCreat(LoaderManager _lmanager, string _key, System.Action<string> _callfinised, System.Action<string, float> _progress) : base(_lmanager)
            {
                mkey = _key;
                mCallFinised = _callfinised;
                mProgressCallBack = _progress;
            }
            /// <summary>
            /// 异步分组加载资源
            /// </summary>
            public void Add(string _key, string _relativePathName,string _resname, System.Action<string, object, object> _callback, object _target)
            {
                if (mStarted)
                {
                    DLog.LOG(DLogType.Error,"队列已经开始载入,不可中途插入");
                    return;
                }
                LoaderCreatObject tcobj = new LoaderCreatObject(mLManager, _key, _relativePathName, _resname, _callback, _target);
                tcobj.mkey = mIndex.ToString();
                tcobj.mCreatCall = LoadCallBack;
                mList.Add(tcobj.mkey, tcobj);
                mIndex++;
                mListCount++;
            }

            public LoaderCreat AddCreat(System.Action<string> _callfinised, System.Action<string, float> _progress)
            {
                if (mStarted)
                {
                    DLog.LOG(DLogType.Error,"队列已经开始载入,不可中途插入");
                    return null;
                }
                LoaderCreat tcobj = new LoaderCreat(mLManager,mIndex.ToString(), _callfinised, _progress);
                tcobj.mkey = mIndex.ToString();
                tcobj.mCreatCall = LoadCallBack;
                mList.Add(tcobj.mkey, tcobj);
                mIndex++;
                mListCount++;
                return tcobj;
            }
            override public void StartLoad()
            {
                if (mStarted)
                {
                    DLog.LOG(DLogType.Error,"队列已经开始载入-" + mkey);
                    return;
                }
                mStarted = true;
                mMaxCount = mList.Count;
                if (mMaxCount == 0)
                {
                    Finished();
                    return;
                }
                List<string> tlist = new List<string>(mList.Keys);
                for (int i = 0; i < mMaxCount; i++)
                {
                    string tkey = tlist[i];
                    if (mList.ContainsKey(tkey))
                        mList[tkey].StartLoad();
                }

            }
            protected void LoadCallBack(string _key)
            {
                mLoadedCount++;
                if (mFinished) return;
                mProgress = Mathf.Clamp01(((float)mLoadedCount) / ((float)mMaxCount));
                CallProgress(mProgress);
                if (mLoadedCount < mMaxCount) return;
                Finished();
            }
            void Finished()
            {
                if (mProgressCallBack != null)
                    mProgressCallBack(mkey, 1);
                mFinished = true;
                if (mCallFinised != null)
                    mCallFinised(mkey);
                if (mCreatCall != null)
                    mCreatCall(mkey);
            }

            private void CallProgress(float _progress)
            {
                if (mProgressCallBack == null || (_progress - mOldProgress) < 0.01f) return;
                mProgressCallBack(mkey, _progress);
                mOldProgress = _progress;
            }

            override public void RemoveAssets(bool _clear)
            {
                ArrayList tlist = new ArrayList(mList.Keys);
                foreach (string tkey in tlist)
                {
                    if (mList.ContainsKey(tkey))
                        mList[tkey].RemoveAssets(_clear);
                }
                mList.Clear();
            }

            #region 属性
            public bool isFinished
            {
                get
                {
                    return mFinished;
                }
            }
            #endregion
        }
    }
}


