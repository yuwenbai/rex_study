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
    public class UIFriendList : UIViewBase
    {
        private UIFriendListModel Model
        {
            get { return this._model as UIFriendListModel; }
        }
        // True好友列表 false不是好友的列表
        private bool _uiState = true;
        public bool UIState
        {
            private set { _uiState = value; }
            get { return _uiState; }
        }
        //初始位置
        private Vector3 InitPos;

        [Tooltip("根节点")]
        public UIWidget Root;

        [Tooltip("打开按钮")]
        public GameObject ButtonOpen;

        [Tooltip("好友按钮")]
        public UISprite ButtonFriend;
        [Tooltip("好友按钮文字")]
        public UILabel ButtonFriendLabel;

        [Tooltip("新好友按钮")]
        public UISprite ButtonAddFriend;
        [Tooltip("新好友按钮文字")]
        public UILabel ButtonAddFriendLabel;

        [Tooltip("准备界面的Title")]
        public UILabel LabelTitle;
        [Tooltip("好友/新好友的背景")]
        public UISprite BgTopButton;
        [Tooltip("好友/新好友的背景 大小")]
        public List<Vector2> BgTopSize;

        [Tooltip("关闭按钮")]
        public GameObject ButtonClose;


        public List<Color> TopButtonLabelColor;
        [Tooltip("添加好友的提示小圆点")]
        public List<GameObject> AddFriendPoint;
        [Tooltip("打开关闭按钮的<")]
        public UISprite OpenTag;

        [Tooltip("好友数量")]
        public UILabel LabelFriendNum;

        [Tooltip("列表的ScrollViewPanel")]
        public UIPanel ScrollViewPanel;
        [Tooltip("列表的ScrollView优化脚本")]
        public ScrollViewWrapContent ScrollViewContent;
        [Tooltip("列表的ScrollView优化脚本")]
        public ScrollViewMgr<UIFriendListData> ScrollViewMgr = new ScrollViewMgr<UIFriendListData>();
        [Tooltip("列表的ScrollView背景")]
        public UIWidget ScrollViewDragBg;
        public Vector4 ScrollViewNormal;
        public Vector4 ScrollViewPrepare;
        public GameObject ButtonWeiXin;

        [Tooltip("当列表数据为空时 所显示的Label")]
        public UILabel LabelEmpty;
        [Tooltip("打开的动画")]
        public UIPlayAnimation OpenAnimation;

        [Tooltip("点击区域")]
        public GameObject ClickZone;

        #region API
        public void OpenUI(bool isOpen)
        {
            this.Root.cachedGameObject.SetActive(isOpen);
            //撤回位置
            this.Root.cachedTransform.localPosition = InitPos;

            this.ListIsOpen = false;
        }
        public void RefreshUI(bool forceRefresh)
        {
            DebugPro.Log(DebugPro.EnumLog.UI, "好友列表刷新 ", (forceRefresh || IsCanRefresh()).ToString(), " 强制刷新 ", forceRefresh.ToString(), " 是否可以刷新 ", IsCanRefresh().ToString());
            if (!forceRefresh && !IsCanRefresh()) return;
            Model.RefreshFriendListData();
            LabelFriendNum.text = "好友:" + Model.FriendList.Count;
            if (MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame)
                UIState = true;

            if (UIState)
            {
                if (Model.FriendList.Count == 0)
                {
                    LabelEmpty.text = "当前无好友";
                    LabelEmpty.gameObject.SetActive(true);
                }
                else
                {
                    LabelEmpty.gameObject.SetActive(false);
                }
            }
            else
            {
                if (Model.AddFriendList.Count == 0)
                {
                    LabelEmpty.text = "未收到加好友邀请";
                    LabelEmpty.gameObject.SetActive(true);
                }
                else
                {
                    LabelEmpty.gameObject.SetActive(false);
                }
            }
            RefreshFriendButton();
            RefreshUIOpenButton();
            RefreshAddFriendPoint();
            UpdateSize();
            RefreshFriendList();
        }
        //是否可以刷新   //当处于打开界面时候是不能刷新的
        private bool IsCanRefresh()
        {
            return !this.ListIsOpen;
        }
        //刷新好友按钮
        private void RefreshFriendButton()
        {
            bool flag = MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame;

            ButtonFriend.gameObject.SetActive(!flag);
            ButtonAddFriend.gameObject.SetActive(!flag);
            ButtonWeiXin.SetActive(!flag);
            LabelTitle.gameObject.SetActive(flag);
            ButtonClose.SetActive(flag);
            BgTopButton.width = (int)BgTopSize[flag ? 1 : 0].x;
            BgTopButton.height = (int)BgTopSize[flag ? 1 : 0].y;

            if (!flag)
            {
                ButtonFriend.enabled = UIState;
                ButtonAddFriend.enabled = !UIState;

                ButtonFriendLabel.GetComponent<UIButton>().defaultColor = TopButtonLabelColor[UIState ? 0 : 1];
                ButtonFriendLabel.color = TopButtonLabelColor[UIState ? 0 : 1];
                ButtonAddFriendLabel.GetComponent<UIButton>().defaultColor = TopButtonLabelColor[UIState ? 1 : 0];
                ButtonAddFriendLabel.color = TopButtonLabelColor[UIState ? 1 : 0];
            }
        }
        //修改小红点
        private void RefreshAddFriendPoint()
        {
            //加好友的小圆点
            bool isActive = Model.AddFriendList != null && Model.AddFriendList.Count > 0;
            if (AddFriendPoint == null) return;
            for (int i = 0; i < AddFriendPoint.Count; ++i)
            {
                if (AddFriendPoint[i].activeSelf != isActive)
                {
                    AddFriendPoint[i].SetActive(isActive);
                }
            }
        }
        //修改 > 的按钮
        public void RefreshUIOpenButton()
        {
            if (MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame)
            {
                ButtonOpen.SetActive(false);
            }
            else
            {
                Root.alpha = 1;
                ButtonOpen.SetActive(true);
                Root.Invalidate(true);
            }
        }
        //修改大小
        public void UpdateSize()
        {
            Vector4 tempV4;
            if (MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame)
            {
                tempV4 = ScrollViewPrepare;
            }
            else
            {
                tempV4 = ScrollViewNormal;
            }

            if (ScrollViewPanel.baseClipRegion != tempV4)
            {
                ScrollViewPanel.baseClipRegion = tempV4;

                ScrollViewDragBg.ResetAnchors();
            }
        }
        #endregion

        #region ScrollView
        private void InitScrollView()
        {
            ScrollViewMgr.Init(ScrollViewContent);
            //ScrollViewMgr_Add.Init(ScrollViewContent_Add);
        }
        private void RefreshFriendList()
        {
            ScrollViewMgr.RefreshScrollView(UIState ? Model.FriendList : Model.AddFriendList);
        }

        /// <summary>
        /// Item点击弹出用户信息
        /// </summary>
        /// <param name="item"></param>
        public void OnScrollViewItemClick(UIFriendListItem item)
        {
            object[] data = new object[2];
            data[0] = item.UIData.FriendData.PlayerDataBase.UserID;
            data[1] = item.UIData.FriendData.PlayerDataBase.HeadURL;
            LoadUIMain("UIUserInfo", data);
            this.OnButtonOpenClick(false);
        }

        public void OnScrollViewButtonClick(UIFriendListItem item, int buttonIndex)
        {
            if (!this.UIState)
            {
                if (!ScrollViewMgr.DelItem(item, item.DeleteTween()))
                {
                    return;
                }
            }
            switch (item.GetState())
            {
                case UIFriendListItem.State.NotFriend:
                    //未添加好友 1同意 2拒绝
                    Model.ApplyFriendReq(item.UIData.FriendData.PlayerDataBase.UserID, buttonIndex == 1);
                    break;
                case UIFriendListItem.State.Enter: //进桌
                    Model.JoinFriendDeskReq(item.UIData.FriendData.PlayerDataBase.UserID, item.UIData.FriendData.PlayerDataBase.OwnerDeskID);
                    break;
                case UIFriendListItem.State.Invite: //邀请
                    Model.InviteReq(item.UIData.FriendData.PlayerDataBase.UserID);
                    break;
            }
            RefreshAddFriendPoint();
        }
        #endregion

        #region 邀请功能

        //好友进入游戏反馈
        public void InviteFriendGame(int resultCode)
        {
            if (resultCode == 0)
            {
                LoadTip("已发送好友请求，请等待对方同意。");
            }
            else
            {
                LoadTip(resultCode);
            }
        }

        public void BeInviteGame()
        {
            if (!Model.isOpenInviteGamePop)
            {
                object[] data = Model.PopBeInviteData();
                if (data != null)
                {
                    BeInviteGame((int)data[0], (long)data[1], data[2] == null ? null : (data[2] as MjRoom));
                }
            }
        }
        //被人邀请进桌
        private void BeInviteGame(int deskId, long userId, MjRoom mjRoom)
        {
            Model.isOpenInviteGamePop = true;
            var user = MemoryData.PlayerData.get(userId).PlayerDataBase;
            this.LoadPop(WindowUIType.SystemPopupWindow, "提示", "您的好友" + user.Name + "邀请你加入牌桌,是否同意", new string[] { "取消", "确认" }, (index) =>
            {
                Model.isOpenInviteGamePop = false;
                if (index == 0)
                {
                    //拒绝
                    ModelNetWorker.Instance.FefuseFriendReq(userId);
                    BeInviteGame();
                }
                else
                {
                    Model.ClearBeInviteData();
                    //同意
                    MahjongLogicNew.Instance.SendMjJoinDesk((ulong)MemoryData.UserID, deskId);


                    ////对方的牌桌不是麻将馆  或者  我已经有麻将馆了
                    //if (mjRoom == null || MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.IsHaveMjRoom())
                    //{
                    //    //进入牌桌
                    //}
                    //else
                    //{
                    //    Action<object> tempCallBack = ParlorInfoHideCallBack;
                    //    //麻将馆数据，true查看麻将馆 false关联麻将馆，ui关闭后的回调，回调所带回的数据
                    //this.LoadUIMain("UIParlorInfo", mjRoom, false, tempCallBack, deskId);
                    //}
                }
            });
        }


        ////关联麻将馆的回调函数
        //private void ParlorInfoHideCallBack(object deskId)
        //{
        //    MahjongLogicNew.Instance.SendMjJoinDesk((ulong)MemoryData.UserID, (int)deskId);
        //}
        #endregion

        #region 进桌功能
        //请求进桌反馈
        public void JoinFriendDesk(int resultCode)
        {
            if (resultCode == 0)
            {
                LoadTip("已向桌主发送进桌申请，请等待");
            }
            else
            {
                LoadTip(resultCode);
            }
        }
        //被请求进桌
        public void BeJoinDesk(long friendId, int deskId)
        {
            var user = MemoryData.PlayerData.get(friendId).PlayerDataBase;
            this.LoadPop(WindowUIType.SystemPopupWindow, "进桌请求", "好友" + user.Name + "请求加入牌桌", new string[] { "取消", "确认" }, (index) =>
            {
                //同意或者拒绝
                ModelNetWorker.Instance.RecvDeskJoinReq(friendId, deskId, index == 1);
            });
        }
        #endregion

        #region 点击区域

        private void UpdateClickZoneActive()
        {
            //if (MemoryData.GameStateData.CurrUISign > SysGameStateData.EnumUISign.MainIn)
            //{
                if (this.ClickZone.activeSelf)
                {
                    this.ClickZone.SetActive(false);
                }
            //}
            else if (this.ClickZone.activeSelf != this.ListIsOpen)
            {
                this.ClickZone.SetActive(this.ListIsOpen);
            }
        }
        private void OnClickZoneClick(GameObject go)
        {
            this.OnButtonOpenClick(false);
        }
        #endregion

        #region Event
        /// <summary>
        /// 新好友列表切换按钮
        /// </summary>
        /// <param name="go"></param>
        private void OnButtonAddFriendClick(GameObject go)
        {
            if (this.UIState != false)
            {
                this.UIState = false;
                this.RefreshUI(true);
            }
            else
            {
                this.RefreshUI(false);
            }
        }

        /// <summary>
        /// 好友列表切换按钮
        /// </summary>
        /// <param name="go"></param>
        private void OnButtonFriendClick(GameObject go)
        {
            if (this.UIState != true)
            {
                this.UIState = true;
                this.RefreshUI(true);
            }
            else
            {
                this.RefreshUI(false);
            }
        }


        public bool _listIsOpen = false;
        public bool ListIsOpen
        {
            set
            {
                if (_listIsOpen != value)
                {
                    _listIsOpen = value;
                    if (!value)
                    {
                        this.RefreshUI(true);
                    }
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_FriendList_OpenClose_Rsp, _listIsOpen);
                }
                UpdateClickZoneActive();
            }
            get { return _listIsOpen; }
        }

        /// <summary>
        /// UI打开关闭动画
        /// </summary>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        public bool OnButtonOpenClick(bool isOpen)
        {
            bool flag = false;
            if (/*isCanOpen &&*/ isOpen != ListIsOpen)
            {
                bool isCenterAnim = MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame;
                OpenTag.flip = ListIsOpen ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing;
                OpenAnimation.clipName = isCenterAnim ? "UIFriendListAnimationCenter" : "UIFriendListAnimation";
                if (ListIsOpen)
                {
                    OpenAnimation.Play(false, false);
                }
                else
                {
                    OpenAnimation.Play(true, false);
                    this.ScrollViewMgr.RefreshData();
                }
                flag = true;
                //EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_FriendList_OpenClose_Rsp, !ListIsOpen);
            }
            return flag;
        }

        /// <summary>
        /// 打开按钮
        /// </summary>
        /// <param name="go"></param>
        private void OnButtonOpenClick(GameObject go)
        {
            OnButtonOpenClick(!ListIsOpen);
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="go"></param>
        private void OnButtonCloseClick(GameObject go)
        {
            OnButtonOpenClick(false);
        }

        /// <summary>
        /// 微信邀请按钮
        /// </summary>
        /// <param name="go"></param>
        public void OnButtonWeiXinClick(GameObject go)
        {
            string InvitationNum = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.InviteCode;
            WXShareParams shareParams = new WXShareParams("WECHAT_SHARE_INVITE_FRIEND");
            shareParams.InsertUrlParams(new object[] { (int)WXOpenParaEnum.SHARE_INVITE_FRIEND, InvitationNum,
                "", "", "", "", "" });
            SDKManager.Instance.SDKFunction("WECHAT_SHARE_INVITE_FRIEND", shareParams);
        }

        /// <summary>
        /// 动画结束
        /// </summary>
        private void OnFinished()
        {
            Root.Invalidate(true);
            ListIsOpen = !ListIsOpen;
        }
        #endregion


        #region override
        public override void Init()
        {
            InitPos = new Vector3(-160f, 0, 0);
            //按钮
            UIEventListener.Get(ButtonAddFriend.transform.FindChild("Label").gameObject).onClick = OnButtonAddFriendClick;
            UIEventListener.Get(ButtonFriend.transform.FindChild("Label").gameObject).onClick = OnButtonFriendClick;
            UIEventListener.Get(ButtonOpen).onClick = OnButtonOpenClick;
            UIEventListener.Get(ButtonWeiXin).onClick = OnButtonWeiXinClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonCloseClick;
            UIEventListener.Get(ClickZone.gameObject).onClick = OnClickZoneClick;

            EventDelegate.Add(this.OpenAnimation.onFinished, OnFinished);

            //ScrollView
            InitScrollView();

            Model.SignUpdate();
        }

        public override void OnShow()
        {
            RefreshUI(true);
        }

        public override void OnHide()
        {
        }
        #endregion

        //string duifangID = "";
        //Rect text = new Rect(0, 0, 50, 100);
        //Rect btn = new Rect(100, 0, 50, 100);
        //private void OnGUI()
        //{
        //    duifangID = GUI.TextArea(text, duifangID);
        //    if (GUI.Button(btn, "添加"))
        //    {
        //        string[] ids = duifangID.Split('\n');
        //        for (int i = 0; i < ids.Length; i++)
        //        {
        //            ids[i] = ids[i].Trim();
        //            if (ids[i].Length > 0)
        //                ModelNetWorker.Instance.AddFriendReqTest(long.Parse(ids[i]));
        //        }
        //    }
        //}
    }
}