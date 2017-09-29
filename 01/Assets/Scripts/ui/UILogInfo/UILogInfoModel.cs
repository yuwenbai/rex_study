/**
* @Author lyb
*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace projectQ
{
    public class UILogInfoModel : UIModelBase
    {
        public UILogInfo UI
        {
            get { return _ui as UILogInfo; }
        }

        #region override-----------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] { };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                default:
                    break;
            }
        }

        #endregion------------------------------------------------------

        /// <summary>
        /// 当前类型页签下的所有的log日志
        /// 1.error  2.exception  3.log   4.warning   5.NetS  6.NetR
        /// </summary>
        private Dictionary<string, List<string>> _LogDataDic = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> LogDataDic
        {
            get { return _LogDataDic; }
            set { _LogDataDic = value; }
        }

        /// <summary>
        /// 当前选中的是第几个页签
        /// </summary>
        private int LogTypeIndex = 0;
        private string LogTypeName;

        #region lyb 清空数据--------------------------------------------

        public void LogData_Clear()
        {
            LogDataDicInit();

            UI.LogTypeBtnList[0].GetComponent<UIButton>().gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }

        #endregion------------------------------------------------------

        #region lyb 获取指定路径下的所有的文件--------------------------

        /// <summary>
        /// 获取指定路径下的所有的文件
        /// </summary>
        public void LogFile_Get(string filePath)
        {
            List<FileInfo> logList = LogInfoHelper.LogFile_AllGet(filePath);

            LogFile_Cerat(logList);
        }

        /// <summary>
        /// 根据文件夹下的文件个数创建列表
        /// </summary>
        void LogFile_Cerat(List<FileInfo> logList)
        {
            //GameObject obj = null;
            //int index = 0;

            logList.Reverse();
            List<UILogInfoListData> infoList = new List<UILogInfoListData>();
            foreach (FileInfo fInfo in logList)
            {
                UILogInfoListData iData = new UILogInfoListData();
                iData.InfoData = fInfo;
                iData.LogClickCallBack = LogBtnClickCallBack;

                infoList.Add(iData);
            }

            UI.RefreshLogLeftList(infoList);

            /*
            UITools.CreateChild<FileInfo>(UI.GridLeftObj.transform, null, logList, (go, logData) =>
            {
                if (index == 0)
                {
                    obj = go;
                }

                UILogInfoList infoList = go.GetComponent<UILogInfoList>();
                if (infoList != null)
                {
                    infoList.LogInfoInit(logData, LogBtnClickCallBack);
                }

                index++;
            });
            UI.GridLeftObj.Reposition();
            */

            Transform tran = UI.GridLeftObj.transform.GetChild(0);
            if (tran != null)
            {
                tran.GetComponent<UIButton>().gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
            }
        }

        #endregion------------------------------------------------------

        #region lyb 按钮点击事件回调------------------------------------

        public void LogBtnClickCallBack(string infoName)
        {
            ArrayList logDataList = LogInfoHelper.LogFile_LoadData(UserActionManager.SAVE_LOG_FILE_PATH, infoName);
            if (logDataList != null)
            {
                LogDataInit(logDataList);
            }

            UI.LogTypeBtnList[LogTypeIndex].GetComponent<UIButton>().gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// log类型页签按钮点击
        /// </summary>
        public void LogTypeBtnClickCallBack(string typeName)
        {
            LogTypeClickSave(typeName);

            UI.NoLogObj.SetActive(false);
            if (_LogDataDic[typeName].Count <= 0)
            {
                UI.NoLogObj.SetActive(true);
            }

            List<UILogDetailListData> logList = new List<UILogDetailListData>();
            foreach (string lStr in _LogDataDic[typeName])
            {
                UILogDetailListData lData = new UILogDetailListData();
                lData.LogDetailStr = lStr;
                lData.LogClickCallBack = LogDetailListClickCallBack;

                logList.Add(lData);
            }

            UI.RefreshLogList(logList);

            /*
            UITools.CreateChild<string>(UI.GridRightObj.transform, null, _LogDataDic[typeName], (go, logStr) =>
            {
                UILogDetailList detail = go.GetComponent<UILogDetailList>();
                detail.LogDetailInit(logStr, LogDetailListClickCallBack);
            });
            UI.GridRightObj.Reposition();
            */

            UI.ScrollViewRightObj.ResetPosition();
        }

        /// <summary>
        /// 把点击的log类型记录一下
        /// </summary>
        void LogTypeClickSave(string typeName)
        {
            for (int i = 0; i < UI.LogTypeBtnList.Length; i++)
            {
                if (typeName.Equals(UI.LogTypeBtnList[i].name))
                {
                    LogTypeIndex = i;
                    LogTypeName = typeName;
                }
            }
        }

        /// <summary>
        /// log详细信息列表点击回调
        /// </summary>
        public void LogDetailListClickCallBack(string detailStr)
        {
            UI.LogDetailBox.gameObject.SetActive(true);

            UI.LogDetailBox.LogBoxInit(detailStr, LogTypeName);
        }

        #endregion------------------------------------------------------

        #region lyb 把读取到的文件数据解析保存--------------------------

        /// <summary>
        /// 初始化本地存储字典
        /// </summary>
        void LogDataDicInit()
        {
            _LogDataDic = new Dictionary<string, List<string>>();

            for (int i = 0; i < UI.LogTypeBtnList.Length; i++)
            {
                List<string> logList = new List<string>();
                _LogDataDic.Add(UI.LogTypeBtnList[i].name, logList);
            }
        }

        /// <summary>
        /// 把读取到的文件数据解析保存
        /// </summary>
        void LogDataInit(ArrayList logDataList)
        {
            LogDataDicInit();

            logDataList.Reverse();
            foreach (string lData in logDataList)
            {
                string[] values = lData.Split(new char[] { '-' });

                switch (values[0])
                {
                    case "Error":
                    case "EError":
                        _LogDataDic["Error"].Add(lData);
                        break;
                    case "Log":
                    case "ELog":
                        _LogDataDic["Log"].Add(lData);
                        break;
                    default:
                        if (_LogDataDic.ContainsKey(values[0]))
                        {
                            _LogDataDic[values[0]].Add(lData);
                        }
                        break;
                }
            }
        }

        #endregion------------------------------------------------------
    }
}