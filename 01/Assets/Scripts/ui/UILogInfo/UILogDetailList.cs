/**
* @Author lyb
* Log详细信息列表
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UI.ScrollViewTool;

namespace projectQ
{
    public class UILogDetailListData
    {
        public string LogDetailStr;
        public UILogDetailList.LogDetailListClickDelegate LogClickCallBack;
    }

    public class UILogDetailList : ScrollViewItemBase<UILogDetailListData>
    {
        public delegate void LogDetailListClickDelegate(string logStr);
        public LogDetailListClickDelegate OnClickCallBack;

        public UILabel LogDetailLab;

        private string LogStr = "";

        void OnClick()
        {
            if (OnClickCallBack != null)
            {
                OnClickCallBack(LogStr);
            }
        }

        public override void Refresh()
        {
            LogDetailInit(this.UIData.LogDetailStr, this.UIData.LogClickCallBack);
        }

        /// <summary>
        /// 初始化文件按钮
        /// </summary>
        public void LogDetailInit(string detailStr, LogDetailListClickDelegate callBack)
        {
            OnClickCallBack = callBack;
            LogStr = detailStr;

            LogDetailLab.text = detailStr;
        }
    }
}