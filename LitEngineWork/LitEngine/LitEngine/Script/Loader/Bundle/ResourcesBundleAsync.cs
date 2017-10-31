using UnityEngine;
namespace LitEngine
{
    namespace Loader
    {
        public class ResourcesBundleAsync : BaseBundle
        {
            private ResourceRequest mReq = null;
            private bool mLoadFinished = false;
            public ResourcesBundleAsync(string _assetsname, string _resname) : base(_assetsname, _resname)
            {

            }

            public override void Load()
            {
                mPathName = LoaderManager.GetResourcesDataPath(mAssetName);
                mReq = Resources.LoadAsync(mPathName);
            }
            override public bool IsDone()
            {
                if (mLoadFinished) return true;
                if (mReq == null)
                {
                    DLog.LOG(DLogType.Error,mAssetName + "-erro Resources->loadasync.载入过程中，错误的调用了清除函数。");
                    mLoadFinished = true;
                    return false;
                }

                if (!mReq.isDone)
                {
                    mProgress = mReq.progress;
                    return false;
                }
                mProgress = mReq.progress;
                mAssetsBundle = mReq.asset;
                mAsset = mReq.asset;
                mReq = null;
                if (mAsset == null)
                    DLog.LOG(DLogType.Error,mAssetName + "-erro Resources->loadasync.载入失败!");
                LoadEnd();
                return true;
            }

            public override void LoadEnd()
            {
                mLoadFinished = true;
                base.LoadEnd();
            }
            public override void ClearGC(bool _clear)
            {
                if (_clear && mAssetsBundle != null)
                    Resources.UnloadAsset(mAssetsBundle);
                mReq = null;
                base.ClearGC(_clear);
            }


            public override void Destory(bool _clear)
            {
                base.Destory(_clear);
            }
        }
    }
}