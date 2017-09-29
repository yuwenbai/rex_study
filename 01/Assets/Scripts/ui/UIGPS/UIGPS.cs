/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;
using System;

namespace projectQ
{
    public class UIGPS : UIViewBase {
        public GameObject ButtonRefresh;
        public GameObject ButtonClear;
        public UILabel LabelMemo;
        [Tooltip("列表的ScrollView优化脚本")]
        public ScrollViewWrapContent ScrollViewContent;
        [Tooltip("列表的ScrollView优化脚本")]
        public ScrollViewMgr<UIGPSItemData> ScrollViewMgr = new ScrollViewMgr<UIGPSItemData>();


        public void RefreshUI()
        {
            var dataList = GPSManager.Instance.GetAllGpsData();
            List<UIGPSItemData> itemList = new List<UIGPSItemData>();
            for (int i = dataList.Count - 1; i >= 0; i--)
            {
                var item = new UIGPSItemData(i + 1, dataList[i]);
                item.onClick = OnItemClick;
                itemList.Add(item);
            }
            ScrollViewMgr.RefreshScrollView(itemList);
        }
        private void SetMemoText(string text)
        {
            LabelMemo.text = text;
        }

        #region Event
        private void OnItemClick(UIGPSItemData data)
        {
            this.SetMemoText(data.ToStringLine(true));
        }
        private void OnButtonRefreshClick(GameObject go)
        {
            RefreshUI();
        }
        private void OnButtonClearClick(GameObject go)
        {
            GPSManager.Instance.ClearAllGpsData();
        }
        #endregion

        #region override
        public override void Init()
        {
            ScrollViewMgr.Init(ScrollViewContent);
            UIEventListener.Get(ButtonRefresh).onClick = OnButtonRefreshClick;
            UIEventListener.Get(ButtonClear).onClick = OnButtonClearClick;
        }

        public override void OnHide()
        {
            
        }

        public override void OnShow()
        {
            RefreshUI();
        }
        #endregion
    }
}