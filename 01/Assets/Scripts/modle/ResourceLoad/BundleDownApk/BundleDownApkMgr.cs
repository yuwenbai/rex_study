/****************************************************
*
*  创建下载Apk的提示框
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;

namespace projectQ
{
    public class BundleDownApkMgr
    {
        #region 创建提示框-------------------------
        
        private GameObject CreatBundleApkDownBox(GameObject obj)
        {
            if (obj == null)
            {
                obj = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI")).gameObject;
            }

            GameObject prefab = ResourcesDataLoader.Load<GameObject>(GameAssetCache.Prefab_BundleDownApk_Path);
            GameObject go = NGUITools.AddChild(obj, prefab);
            return go;
        }

        #endregion---------------------------------

        #region 创建下载提示框---------------------
        
        public GameObject Show(GameObject parent = null)
        {
            GameObject box = CreatBundleApkDownBox(parent);

            BundleDownApkUI downApk = box.GetComponent<BundleDownApkUI>();
            downApk.DownApk_Begin(GameConfig.Instance.ApkDownUrl);

            return box;
        }

        #endregion---------------------------------
    }
}