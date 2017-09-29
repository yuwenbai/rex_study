/**
 * @Author lyb
 * 读取本地的Log日志文件，显示在面板上
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;

namespace projectQ
{
    public class UILogInfo : UIViewBase
    {
        public UILogInfoModel Model
        {
            get { return _model as UILogInfoModel; }
        }

        /// <summary>
        /// 关闭面板按钮
        /// </summary>
        public GameObject CloseBtn;
        /// <summary>
        /// 删除所有log文件按钮
        /// </summary>
        public GameObject DeleteAllBtn;
        /// <summary>
        /// 创建文件按钮的grild
        /// </summary>
        public UIGrid GridLeftObj;
        /// <summary>
        /// 左边的滑动控件
        /// </summary>
        public UIScrollView ScrollViewLeftObj;
        /// <summary>
        /// 创建文件按钮的grild
        /// </summary>
        public UIGrid GridRightObj;
        /// <summary>
        /// 右边的滑动控件
        /// </summary>
        public UIScrollView ScrollViewRightObj;
        /// <summary>
        /// log类型页签按钮
        /// </summary>
        public GameObject[] LogTypeBtnList;
        /// <summary>
        /// 当前没有该类型的log
        /// </summary>
        public GameObject NoLogObj;
        /// <summary>
        /// 详细信息弹框
        /// </summary>
        public UILogDetailBox LogDetailBox;

        public ScrollViewWrapContent ScrollViewContent;
        public ScrollViewMgr<UILogDetailListData> ScrollViewMgr = new ScrollViewMgr<UILogDetailListData>();

        public ScrollViewWrapContent ScrollViewLeft;
        public ScrollViewMgr<UILogInfoListData> ScrollViewLeftMgr = new ScrollViewMgr<UILogInfoListData>();

        #region override--------------------------------------

        public override void Init()
        {
            UIEventListener.Get(CloseBtn).onClick = OnCloseBtnClick;
            UIEventListener.Get(DeleteAllBtn).onClick = OnDeleteAllBtnClick;

            for (int i = 0; i < LogTypeBtnList.Length; i++)
            {
                UIEventListener.Get(LogTypeBtnList[i]).onClick = OnLogTypeBtnClick;
            }

            InitScrollView();
        }

        public override void OnHide() { }

        public override void OnShow()
        {
            Model.LogFile_Get(UserActionManager.SAVE_LOG_FILE_PATH);
        }

        #endregion--------------------------------------------

        #region 按钮点击事件回调------------------------------

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void OnCloseBtnClick(GameObject go)
        {
            this.Close();
        }

        /// <summary>
        /// 删除所有Log文件按钮点击
        /// </summary>
        private void OnDeleteAllBtnClick(GameObject go)
        {
            LogInfoHelper.LogFile_AllDelete(UserActionManager.SAVE_LOG_FILE_PATH);

            Model.LogFile_Get(UserActionManager.SAVE_LOG_FILE_PATH);

            Model.LogData_Clear();
        }

        /// <summary>
        /// Log类型页签按钮点击
        /// </summary>
        private void OnLogTypeBtnClick(GameObject go)
        {
            Model.LogTypeBtnClickCallBack(go.name);
        }

        #endregion--------------------------------------------

        #region ScrollView------------------------------------

        private void InitScrollView()
        {
            ScrollViewMgr.Init(ScrollViewContent);
            ScrollViewLeftMgr.Init(ScrollViewLeft);
        }

        public void RefreshLogList(List<UILogDetailListData> logList)
        {
            ScrollViewMgr.RefreshScrollView(logList);
        }

        public void RefreshLogLeftList(List<UILogInfoListData> logInfoList)
        {
            ScrollViewLeftMgr.RefreshScrollView(logInfoList);
        }

        #endregion--------------------------------------------
    }
}