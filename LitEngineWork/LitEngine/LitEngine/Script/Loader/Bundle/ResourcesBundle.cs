using UnityEngine;
namespace LitEngine
{
    namespace Loader {
        public class ResourcesBundle : BaseBundle
        {
            public ResourcesBundle(string _assetsname, string _resname) : base(_assetsname, _resname)
            {

            }

            public override void Load()
            {
                mPathName = LoaderManager.GetResourcesDataPath(mAssetName);
                mAssetsBundle = Resources.Load(mPathName);
                if (mAssetsBundle == null)
                    DLog.LOG(DLogType.Error,"ResourcesBundle打开文件失败,请检查资源是否存在-" + mPathName);
                mAsset = mAssetsBundle;
                LoadEnd();
            }

            override public void ClearGC(bool _clear)
            {
                if(_clear && mAssetsBundle!=null)
                    Resources.UnloadAsset(mAssetsBundle);
                base.ClearGC(_clear);
            }
        }
    }  
}
