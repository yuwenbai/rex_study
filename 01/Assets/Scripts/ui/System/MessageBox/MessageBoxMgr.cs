using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class MessageBoxMgr
    {
        public delegate void OnCancel();
        public delegate void OnOK();

        #region 创建提示框-------------------------

        /// <summary>
        /// 创建该提示框获取其Message方法
        /// </summary>
        private GameObject CreatMessageCase(GameObject obj)
        {
            if (obj == null)
            {
                obj = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI")).gameObject;
            }

            GameObject prefab = ResourcesDataLoader.Load<GameObject>(GameAssetCache.Prefab_MessageBox_Path);
            GameObject go = NGUITools.AddChild(obj, prefab);
            return go;
        }

        #endregion---------------------------------

        #region 创建两个按钮的提示框---------------

        /// <summary>
        /// 初始化弹出框，有两个按钮
        /// </summary>
        public GameObject Show(string title, string desc, GameObject parent, OnOK ok, OnCancel cancel)
        {
            GameObject box = CreatMessageCase(parent);

            MessageBoxUI boxMgr = box.GetComponent<MessageBoxUI>();
            boxMgr.Title = title;
            boxMgr.Message = desc;
            boxMgr.mOnOK = ok;
            boxMgr.mOnCancel = cancel;

            return box;
        }

        #endregion---------------------------------

        #region 创建单个按钮的提示框---------------

        /// <summary>
        /// 创建单个按钮的提示框 - 可传ok按钮的响应事件代理
        /// </summary>
        public GameObject ShowSingleBtn(string title, string desc, GameObject parent, OnOK ok)
        {
            GameObject box = Show(title, desc, parent, ok, null);
            MessageBoxUI boxMgr = box.GetComponent<MessageBoxUI>();

            boxMgr.NoBtn.gameObject.SetActive(false);
            Vector3 button = boxMgr.OkBtn.transform.localPosition;
            button.x = 0.0f;
            boxMgr.OkBtn.transform.localPosition = button;

            return box;
        }

        #endregion---------------------------------       
    }
}