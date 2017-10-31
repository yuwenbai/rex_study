using UnityEngine;
namespace LitEngine
{
    namespace Loader
    {
        public class AssetsBundleFromFile : BaseBundle
        {
            //只支持bundle 类型，其他种类的读取请使用异步或者直接stream读取
            public AssetsBundleFromFile(string _assetsname, string _resname) : base(_assetsname, _resname)
            {

            }
            public override void Load()
            {
                mPathName = LoaderManager.GetFullPath(mAssetName);

                mAssetsBundle = AssetBundle.LoadFromFile(mPathName);
                if (mAssetsBundle != null)
                {
                    mAsset = ((AssetBundle)mAssetsBundle).mainAsset;
                }
                else
                    DLog.LOG(DLogType.Error,"AssetsBundleFromFile打开文件失败,请检查资源是否存在-" + mPathName);
                LoadEnd();
            }
        }
    }
}

