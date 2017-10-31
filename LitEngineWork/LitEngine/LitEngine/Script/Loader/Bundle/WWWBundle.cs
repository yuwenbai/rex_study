using UnityEngine;
namespace LitEngine
{
    namespace Loader
    {
        class WWWBundle : BaseBundle
        {
            private WWW mCreat = null;
            private bool mLoadFinished = false;
            public WWWBundle()
            {
            }

            public WWWBundle(string _assetsname, string _resname) : base(_assetsname, _resname)
            {
            }

            public override void Load()
            {
                mPathName = mAssetName;
                if (!mPathName.Contains("file://"))
                    mPathName = "file://" + mPathName;
                mCreat = new WWW(mPathName);
            }
            public override void LoadEnd()
            {
                base.LoadEnd();
                mLoadFinished = true;
            }
            public override bool IsDone()
            {
                if (mLoadFinished) return true;
                if (!mCreat.isDone)
                {
                    mProgress = mCreat.progress;
                    return false;
                }
                mProgress = mCreat.progress;
                if (mCreat.error == null)
                    mAsset = mCreat;
                LoadEnd();
                return true;
            }



            public override void Destory(bool _clear)
            {
                base.Destory(_clear);
            }

            public override void ClearGC(bool _clear)
            {
                if (mCreat.assetBundle != null)
                    mCreat.assetBundle.Unload(_clear);
                mCreat.Dispose();
                mCreat = null;
                base.ClearGC(_clear);
            }
        }
    }
}

