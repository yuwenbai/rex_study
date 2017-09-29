

using System;
/**
* @Author YQC
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIFriendsParlorApply : UIViewBase
    {
        private UIFriendsParlorApplyModel Model
        {
            get { return _model as UIFriendsParlorApplyModel; }
        }
        #region UI
        public UILabel LabelInfo;
        public UIInput InputSearch;
        public GameObject ButtonSearch;//直接关联
        [Tooltip("进入麻将馆")]
        public GameObject ButtonJoinParlor;//搜索好友棋牌室

        public GameObject ButtonClose;
        #endregion
        private bool isCanClick = true;
        private bool showInfo = true;
        public void RefreshInfo(string info)
        {
            return;

            if (info != null) {
                info = info.Replace("\\n", "\r\n");
                LabelInfo.text = info;
            }
        }

        //收到搜索结果
        public void ReceiveSearchResult(List<MjRoom> mjHallList)
        {
            if(mjHallList == null || mjHallList.Count == 0)
            {
                //没有找到
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "未搜索到棋牌室", new string[] { "确定" }, delegate (int index)
                {
                    if (index == 0)
                    {

                    }
                });
            }
            else
            {
                if (showInfo)
                {
                    this.LoadUIMain("UIParlorInfo", mjHallList[0]);
                }
            }
        }
        private float timer = 0f;
        private void Update()
        {
            if (!isCanClick)
            {
                timer += Time.deltaTime;
                if (timer >= 3.5f)
                {
                    isCanClick = true;
                    timer = 0f;
                }
            }
        }
        #region Event
        //搜索按钮点击
        private void OnButtonSearchClick(GameObject go)
        {
            string value = InputSearch.value;
            value = value.Trim();
            if (value.Length == 0) {
                this.LoadTip("请输入要搜索的内容");
                //this.LoadUIMain("UIParlorInfo");
            }
            else
            {
                Model.OnSearchHall(value);
            }
            isCanClick = false;
        }

        //进入好友麻将馆
        private void OnButtonJoinParlorClick(GameObject go)
        {
            showInfo = false;
            if (isCanClick)
            {
                this.LoadUIMain("UIParlorList");
                this.Hide();
            }
            else
            {
                LoadTip("每次搜索的时间间隔为3秒");
                return;
            }
           
        }

        private void OnButtonClose(GameObject go)
        {
            //this.Close();
           
            this.Hide();
        }
        #endregion

        #region override
        public override void Init()
        {
            UIEventListener.Get(ButtonSearch).onClick = OnButtonSearchClick;
            UIEventListener.Get(ButtonJoinParlor).onClick = OnButtonJoinParlorClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonClose;
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
            showInfo = true;
            this.InputSearch.value = "";
        }
        #endregion
    }
}
