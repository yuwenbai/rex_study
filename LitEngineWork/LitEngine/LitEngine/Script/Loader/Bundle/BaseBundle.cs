using UnityEngine;
namespace LitEngine
{
    namespace Loader
    {

        public class BaseBundle
        {
            #region 类属性
            static public string sSuffixName = ".bytes";
            static public bool ClearWhenFinished = false;
            protected string mAssetName = "";
            protected string mResName = "";
            protected string mPathName = "";
            protected UnityEngine.Object mAssetsBundle;
            protected object mAsset;
            protected bool mIsLoaded = false;
            protected int mPCount = 0;
            protected float mProgress = 0;
            protected BundleVector mParent;
            #endregion
            public BaseBundle()
            {

            }

            public BaseBundle(string _assetname, string _resname)
            {
                mAssetName = _assetname;
                mResName = _resname;
            }

            #region 属性
            public float Progress
            {
                get
                {
                    return mProgress;
                }
            }
            public object Asset
            {
                get
                {
                    return mAsset;
                }
            }

            public BundleVector Parent
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

            public bool Loaded
            {
                get
                {
                    return mIsLoaded;
                }
            }

            public virtual UnityEngine.Object AssetsBundleObject
            {
                get
                {
                    return mAssetsBundle;
                }
            }

            public string AssetName
            {
                get
                {
                    return mAssetName;
                }
                set
                {
                    mAssetName = value;
                }
            }
            #endregion
            public virtual bool IsDone()
            {
                return false;
            }
            public virtual void Load()
            {
            }
            public virtual void LoadEnd()
            {
                if (ClearWhenFinished)
                    ClearGC(false);
                mIsLoaded = true;
            }

            #region 资源计数以及删除
            public virtual void Release()
            {
                if (!Loaded)
                {
                    DLog.LOG(DLogType.Error,"错误的释放时机，资源还没有载入完成");
                    return;
                }
                mPCount--;
                if (mPCount <= 0)
                    DestoryFormParent();
            }

            public virtual void Retain()
            {
                mPCount++;
            }

            //删除自身并且从父类列表删除
            protected virtual void DestoryFormParent()
            {
                if (Parent != null)
                    Parent.Remove(this, true);
            }

            //删除操作不从父类删除，不可单独调用
            public virtual void ClearGC(bool _clear)
            {
                if (mAsset != null && typeof(Object).IsInstanceOfType(mAsset))
                {
                    Resources.UnloadAsset((UnityEngine.Object)mAsset);
                    mAsset = null;
                }

                if (mAssetsBundle != null && mAssetsBundle.GetType() == typeof(AssetBundle))
                    ((AssetBundle)mAssetsBundle).Unload(_clear);
                mAssetsBundle = null;
                
                   
                //System.GC.Collect();
                // Resources.UnloadUnusedAssets();
            }
            public virtual void Destory(bool _clear)
            {
                ClearGC(_clear);
            }
            #endregion
        }
    }
}



