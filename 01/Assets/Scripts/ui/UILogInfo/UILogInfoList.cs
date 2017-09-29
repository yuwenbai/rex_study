/**
* @Author lyb
* 文件内容解析
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI.ScrollViewTool;
using UnityEngine;

namespace projectQ
{
    public class UILogInfoListData
    {
        public FileInfo InfoData;
        public UILogInfoList.LogInfoBtnClickDelegate LogClickCallBack;
    }

    public class UILogInfoList : ScrollViewItemBase<UILogInfoListData>
    {
        public delegate void LogInfoBtnClickDelegate(string infoName);
        public LogInfoBtnClickDelegate OnClickCallBack;

        public UILabel LogNameLab;

        private string LogName = "";

        void OnClick()
        {
            if (OnClickCallBack != null)
            {
                OnClickCallBack(LogName);
            }
        }

        public override void Refresh()
        {
            LogInfoInit(this.UIData.InfoData, this.UIData.LogClickCallBack);
        }

        /// <summary>
        /// 初始化文件按钮
        /// </summary>
        public void LogInfoInit(FileInfo infoData, LogInfoBtnClickDelegate callBack)
        {
            OnClickCallBack = callBack;
            LogName = infoData.Name;

            string sName = infoData.Name.Replace("log__2017_", "");
            string[] values = sName.Split(new char[] { '.' });
            LogNameLab.text = values[0];
        }
    }
}