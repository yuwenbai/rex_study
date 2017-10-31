using UnityEngine;
namespace LitEngine
{
    namespace Loader
    {
        public class AssetsBundleAsyncFromFile : BaseBundle
        {
            private AssetBundleCreateRequest mCreat = null;
            private AssetBundleRequest mLoadObjReq = null;
            private bool mLoadFinished = false;
            private bool mIsScene = false;
            private bool mBCreated = false;
            public AssetsBundleAsyncFromFile()
            {
            }

            public AssetsBundleAsyncFromFile(string _assetsname, string _resname) : base(_assetsname, _resname)
            {
            }

            public override void LoadEnd()
            {
                mLoadFinished = true;
                base.LoadEnd();
            }
            void CreatBundleReq()
            {
                AssetBundle tasbd = mAssetsBundle as AssetBundle;
                if (tasbd == null)
                {
                    DLog.LOG(DLogType.Error,"AssetBundle 转换失败.mAssetName = " + mAssetName);
                    return;
                }

                int tstartindex = mResName.LastIndexOf("/");
                int endindex = mResName.LastIndexOf(sSuffixName);
                string tresname = mResName.Substring(tstartindex + 1, endindex - tstartindex - 1);
                mLoadObjReq = ((AssetBundle)mAssetsBundle).LoadAssetAsync(tresname.ToLower());

            }
            override public bool IsDone()
            {
                if (mLoadFinished) return true;

                if (mCreat == null)
                {
                    DLog.LOG(DLogType.Error,mAssetName + "-erro loadasync.载入过程中，错误的调用了清除函数。");
                    LoadEnd();
                    return false;
                }
                if (!mCreat.isDone)
                {
                    mProgress = mCreat.progress;
                    return false;
                }
                if (!mBCreated)
                {
                    mProgress = mCreat.progress;
                    mAssetsBundle = mCreat.assetBundle;
                    if (mAssetsBundle == null)
                    {
                        DLog.LOG(DLogType.Error,"AssetsBundleAsyncFromFile-erro created。文件载入失败,请检查文件名:" + mPathName);
                        LoadEnd();
                        return true;
                    }

                    if (((AssetBundle)mAssetsBundle).isStreamedSceneAssetBundle)
                    {
                        mAsset = ((AssetBundle)mAssetsBundle).mainAsset;
                        mIsScene = true;
                    }
                    else
                    {
                        CreatBundleReq();
                    }

                    mBCreated = true;
                }

                if (!mIsScene)
                {
                    if (!mLoadObjReq.isDone) return false;
                    mAsset = mLoadObjReq.asset;
                    if (mAsset == null)
                    {
                        mAsset = ((AssetBundle)mAssetsBundle).mainAsset;
                        DLog.LOG(DLogType.Error,"在资源包 " + mAssetName + " 中找不到文件名:" + mResName.ToLower() + " 的资源。或者因为资源的命名不规范导致unity加载模块找不到该资源。例如:名字中代有多个.符号");
                    }

                }
                mCreat = null;
                mLoadObjReq = null;
                LoadEnd();

                return true;
            }

            public override void Load()
            {
                mPathName = LoaderManager.GetFullPath(mAssetName);
                mCreat = AssetBundle.LoadFromFileAsync(mPathName);
            }

            public override void ClearGC(bool _clear)
            {
                mCreat = null;
                mLoadObjReq = null;
                base.ClearGC(_clear);
            }

            public override void Destory(bool _clear)
            {
                base.Destory(_clear);
            }
        }
    }
}

