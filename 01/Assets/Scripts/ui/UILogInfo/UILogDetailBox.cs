/**
* @Author lyb
* Log详细信息面板
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace projectQ
{
    public class UILogDetailBox : MonoBehaviour
    {
        public UILabel LogTitleLab;
        public UILabel LogDetailLab;
        public GameObject CloseBtn;
        public UIScrollView ScrollViewObj;

        void OnEnable()
        {
            UIEventListener.Get(CloseBtn).onClick = OnCloseBtnClick;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void LogBoxInit(string detailStr, string logTypeName)
        {
            string dStr = detailStr.Replace("--换行--", "\n");

            LogDetailLab.text = dStr;

            LogTitleLab.text = logTypeName;

            ScrollViewObj.ResetPosition();
        }
        
        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void OnCloseBtnClick(GameObject go)
        {
            gameObject.SetActive(false);
        }
    }
}