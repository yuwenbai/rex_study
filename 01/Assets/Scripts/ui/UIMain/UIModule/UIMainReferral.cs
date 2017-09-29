/**
 * @Author YQC
 *
 *
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.ScrollViewTool;

namespace projectQ
{
    public class UIMainReferral : UIMainBase
    {
        //好友麻将馆按钮
        //public GameObject ButtonParlor;

        //麻将馆ScrollView
        public ScrollViewWrapContent ParlorScrollView;
        private ScrollViewMgr<ParlorItemData> scrollViewMgr = new ScrollViewMgr<ParlorItemData>();

        private List<ParlorItemData> DataList;
        private Action<ParlorItemData, GameObject> OnItemClick;
        private Action OnParlorClick;

        public void SetData(List<MjRoom> hallList, Action<ParlorItemData, GameObject>  onItemClick,Action onParlorClick)
        {
            if (hallList == null) return;
            DataList = new List<ParlorItemData>(hallList.Count);
            for (int i = 0; i < hallList.Count; ++i)
            {
                ParlorItemData data = new ParlorItemData();
                data.hall = hallList[i];
                data.index = i;
                data.OnClick = OnParlorItemClick;
                DataList.Add(data);
            }
            this.OnItemClick = onItemClick;
            this.OnParlorClick = onParlorClick;
        }
        public override void RefreshUI(UIMain.EnumUIMainState state)
        {
            if(state == UIMain.EnumUIMainState.NormalMain)
            {
                // 修改 lyb 隐藏，由麻将试玩群代替
                gameObject.SetActive(false);
                scrollViewMgr.RefreshScrollView(DataList);
            }
            else
            {   
                gameObject.SetActive(false);
            }
        }


        //好友麻将列表点击
        public void OnParlorItemClick(ParlorItemData data, GameObject go)
        {
            if(OnItemClick == null)
            {
                //打开信息
                this.ui.LoadUIMain("UIParlorInfo", data.hall);
            }
            else
            {
                this.OnItemClick(data,go);
            }
        }

        //打开好友麻将管页面
        //public void OnButtonParlorClick(GameObject go)
        //{
        //    if (this.OnParlorClick == null)
        //    {
        //        //打开麻将馆界面
        //        this.ui.LoadUIMain("UIFriendsParlorApply");
        //    }
        //    else
        //    {
        //        this.OnParlorClick();
        //    }
        //}

        private void Awake()
        {
            scrollViewMgr.Init(ParlorScrollView);
            //UIEventListener.Get(ButtonParlor).onClick = OnButtonParlorClick;
        }
    }
}