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
    /// <summary>
    /// 黑白名单类
    /// </summary>
    [System.Serializable]
    public class UIBlackWhiteData
    {
        public string UIName;
        public bool IsShow;
    }
    public class UIFriendListModel : UIModelBase
    {
        public UIFriendList UI
        {
            get { return this._ui as UIFriendList; }
        }

        //已经是好友列表
        public List<UIFriendListData> FriendList = new List<UIFriendListData>();
        //不是好友的列表
        public List<UIFriendListData> AddFriendList = new List<UIFriendListData>();

        public List<long> NewAddFriendList = new List<long>();

        [Tooltip("黑名单列表")]
        public List<UIBlackWhiteData> Blacklist;
        [Tooltip("白名单列表")]
        public List<UIBlackWhiteData> Whitelist;

        #region 邀请好友功能(邀请别人)
        //发送邀请列表   用来保证不会连续发送邀请
        private List<long> InviteReqList;
        public void ClearInviteReqList()
        {
            if (InviteReqList != null)
                InviteReqList.Clear();
        }
        /// <summary>
        /// 是否已经邀请了
        /// </summary>
        public bool IsInvited(long friendUserId)
        {
            return InviteReqList != null && InviteReqList.IndexOf(friendUserId) != -1;
        }

        /// <summary>
        /// 邀请好友
        /// </summary>
        /// <param name="friendUserId"></param>
        public void InviteReq(long friendUserId)
        {
            if (InviteReqList == null)
                InviteReqList = new List<long>();

            if (!IsInvited(friendUserId))
            {
                InviteReqList.Add(friendUserId);
                ModelNetWorker.Instance.InviteFriendGameReq(friendUserId, MjDataManager.Instance.MjData.curUserData.selfDeskID);
            }
        }

        /// <summary>
        ///  删除邀请过列表
        /// </summary>
        public void RemoveInvite(long friendUserId)
        {
            if (InviteReqList != null)
            {
                int index = InviteReqList.IndexOf(friendUserId);
                if (index >= 0)
                    InviteReqList.RemoveAt(index);
            }
        }

        /// <summary>
        /// 刷新邀请列表
        /// </summary>
        public void RefreshInviteListByDeskInfo()
        {
            if (InviteReqList == null) return;
            //牌桌内的状态更新
            var MjDeskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            if (MjDeskInfo != null)
            {
                var allPlayer = MjDataManager.Instance.GetAllPlayerInfoByDeskID(MjDeskInfo.deskID);
                var allPlayerId = MjDeskInfo.GetAllPlayerID();
                for (int i = 0; i < allPlayer.Length; i++)
                {
                    if (allPlayer[i] != null && IsInvited(allPlayerId[i]))
                    {
                        RemoveInvite(allPlayerId[i]);
                    }
                }
            }


            //不在牌桌内的状态更新
            for (int i = 0; i < FriendList.Count; i++)
            {
                if (IsInvited(FriendList[i].FriendData.PlayerDataBase.UserID))
                {
                    if (FriendList[i].FriendData.FriendPlayerInfo.state == 2
                    || FriendList[i].FriendData.FriendPlayerInfo.state == 3)
                    {
                        RemoveInvite(FriendList[i].FriendData.PlayerDataBase.UserID);
                    }
                }
            }
        }

        /// <summary>
        /// 同意或拒绝好友添加请求
        /// </summary>
        public void ApplyFriendReq(long userId, bool isApply)
        {
            for (int i = 0; i < AddFriendList.Count; ++i)
            {
                if (AddFriendList[i].FriendData.PlayerDataBase.UserID == userId)
                {
                    AddFriendList.RemoveAt(i);
                    break;
                }
            }
            ModelNetWorker.Instance.ApplyFriendReq(userId, isApply);
        }
        #endregion

        #region 被邀请功能
        #endregion

        #region 进桌申请
        private List<string> joinDeskReqList;
        private void AddJoinDeskReqList(long userId,int deskId)
        {
            if (joinDeskReqList == null)
                joinDeskReqList = new List<string>();
            joinDeskReqList.Add(userId+"_"+deskId);
        }

        private void RemoveJoinDeskReq(long userId)
        {
            RemoveJoinDeskReqList(GetJoinDeskReqIndex(userId + "_"));
        }

        private void RemoveJoinDeskReq(int deskId)
        {
            RemoveJoinDeskReqList(GetJoinDeskReqIndex("_" + deskId));
        }

        private void RemoveJoinDeskReqList(List<string> removeList)
        {
            if(removeList != null && removeList.Count > 0 && joinDeskReqList != null && joinDeskReqList.Count > 0)
            {
                for (int i = 0; i < removeList.Count; i++)
                {
                    joinDeskReqList.Remove(removeList[i]);
                }
            }
        }

        private void ClearJoinDeskReqList()
        {
            if (joinDeskReqList != null)
                joinDeskReqList.Clear();
        }

        private List<string> GetJoinDeskReqIndex(string key)
        {
            if (joinDeskReqList != null && joinDeskReqList.Count > 0)
            {
                List<string> result = new List<string>();
                for (int i = 0; i < joinDeskReqList.Count; i++)
                {
                    if (joinDeskReqList[i].IndexOf(key) >= 0)
                    {
                        result.Add(joinDeskReqList[i]);
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 是否已经申请过进桌
        /// </summary>
        /// <returns></returns>
        public bool IsJoinDeskReq(long userId)
        {
            if(joinDeskReqList != null)
            {
                var result = GetJoinDeskReqIndex(userId + "_");
                if (result != null && result.Count > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 进桌申请
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deskId"></param>
        public void JoinFriendDeskReq(long userId, int deskId)
        {
            if (!IsJoinDeskReq(userId))
            {
                AddJoinDeskReqList(userId,deskId);
                ModelNetWorker.Instance.JoinFriendMjDeskReq(userId, deskId);
            }
        }
        #endregion

        #region 接受进桌请求
        public bool isOpenInviteGamePop = false;
        Queue<object[]> BeInviteQueue = new Queue<object[]>();
        private void PushBeInviteData(object[] data)
        {
            BeInviteQueue.Enqueue(data);
            UI.BeInviteGame();
        }
        public object[] PopBeInviteData()
        {
            object[] result = null;
            while (BeInviteQueue.Count > 0)
            {
                result = BeInviteQueue.Dequeue();
                if (result != null)
                    break;
            }
            return result;
        }

        public void ClearBeInviteData()
        {
            isOpenInviteGamePop = false;
            BeInviteQueue.Clear();
        }
        #endregion

        #region 数据刷新
        /// <summary>
        /// 刷新好友数据
        /// </summary>
        public void RefreshFriendListData()
        {
            FriendList.Clear();
            AddFriendList.Clear();

            MemoryData.FriendData.Sort();
            var tempFriendList = MemoryData.FriendData.FriendList;
            for (int i = 0; i < tempFriendList.Count; ++i)
            {
                var tempUIData = new UIFriendListData();
                tempUIData.UIModel = this;
                tempUIData.FriendData = MemoryData.PlayerData.get(tempFriendList[i]);
                tempUIData.onButtonClick = UI.OnScrollViewButtonClick;
                tempUIData.onItemClick = UI.OnScrollViewItemClick;
                if (tempUIData.FriendData.FriendPlayerInfo.IsTemp)
                {
                    AddFriendList.Add(tempUIData);
                }
                else
                {
                    FriendList.Add(tempUIData);
                }
            }

            ////牌桌内的状态更新
            //var MjDeskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
            //if (MjDeskInfo != null)
            //{
            //    var allPlayer = MjDataManager.Instance.GetAllPlayerInfoByDeskID(MjDeskInfo.deskID);
            //    var allPlayerId = MjDeskInfo.GetAllPlayerID();
            //    int proNum = 0;
            //    for (int i = 0; i < allPlayer.Length; i++)
            //    {
            //        if (allPlayer[i] != null)
            //            proNum++;
            //    }

            //    if (allPlayerId.Length != proNum)
            //        return;

            //    for (int i = allPlayer.Length - 1; i >= 0; i--)
            //    {
            //        if (i >= proNum)
            //            continue;

            //        int num = -1;
            //        for (int n = 0; n < FriendList.Count; n++)
            //        {
            //            if (allPlayer[i] != null && allPlayerId[i] == FriendList[n].FriendData.PlayerDataBase.UserID)
            //            {
            //                num = i;
            //            }
            //        }
            //        if (num < 0)
            //        {
            //            PlayerDataModel data = MemoryData.PlayerData.get(allPlayerId[i]);
            //            if (data != null)
            //            {
            //                data.FriendPlayerInfo.IsTemp = true;
            //                data.PlayerDataBase.isAddFriend = false;
            //            }
            //        }
        }
        #endregion

        #region 显隐
        /// <summary>
        /// 当前游戏状态发生改变时 切换显隐
        /// </summary>
        public void SignUpdate()
        {
            if (MemoryData.GameStateData.CurrUISign < SysGameStateData.EnumUISign.MainIn
                            || MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame_NotOpenDesk
                            || MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.GameIn)
                UI.OpenUI(false);
            else
            {
                UI.OpenUI(true);
            }
            UIOn_Off();
            ClearInviteReqList();
            ClearBeInviteData();
            ClearJoinDeskReqList();
            UI.RefreshUI(false);
        }

        public bool FindBlackWhiteList(List<UIBlackWhiteData> list,string uiName,bool isShow)
        {
            var FindData = list.Find((find) => {
                return (find != null && find.UIName == uiName && find.IsShow == isShow);
            });
            return FindData != null;
        }


        private void UIOn_Off()
        {
            List<string> showUINames = _R.ui.GetUIShowList();
            if (showUINames == null) return;

            bool flag = UI.Root.cachedGameObject.activeSelf;
            if(MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.GameIn
                || MemoryData.GameStateData.CurrUISign <= SysGameStateData.EnumUISign.LoginSucceed
                )
            {
                flag = false;
            }
            else
            {
                for (int i = 0; i < showUINames.Count; i++)
                {
                    if (FindBlackWhiteList(this.Blacklist, showUINames[i], true))
                    {
                        flag = false;
                        break;
                    }
                    else if (FindBlackWhiteList(this.Whitelist, showUINames[i], true))
                    {
                        flag = true;
                    }
                }
            }
            if(UI.Root.cachedGameObject.activeSelf != flag)
                UI.OpenUI(flag);
        }

        #endregion

        #region 生命周期
        public override void OnEnable()
        {
            base.OnEnable();
            if (FriendList.Count == 0 && AddFriendList.Count == 0)
                UI.RefreshUI(true);
        }
        #endregion

        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                GEnum.NamedEvent.SysData_FriendStatus_Updata,
                GEnum.NamedEvent.SysData_FriendListData_Updata,
                GEnum.NamedEvent.SysGame_UI_Sign_Update,
                GEnum.NamedEvent.SysData_Friend_InviteFriendGame,
                GEnum.NamedEvent.SysData_Friend_BeInviteGame,
                GEnum.NamedEvent.SysData_Friend_JoinFriendDeskRsp,
                GEnum.NamedEvent.SysData_Friend_BeJoinFriendDeskNotify,
                GEnum.NamedEvent.SysData_FriendListData_NewAdd,


                GEnum.NamedEvent.EMahjongDeskPlayer,
                GEnum.NamedEvent.EMahjongQuit,
                GEnum.NamedEvent.EMahjongReady,
                GEnum.NamedEvent.EMahjongJoinDesk,
                GEnum.NamedEvent.SysUI_FriendList_OpenClose_Req,
                GEnum.NamedEvent.SysUI_StateUpdate_Open,
                GEnum.NamedEvent.SysUI_StateUpdate_CloseOrHide,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                //好友数据刷新
                case GEnum.NamedEvent.SysData_FriendListData_Updata:
                    UI.RefreshUI(false);
                    break;

                case GEnum.NamedEvent.SysUI_StateUpdate_Open:
                    {
                        UIOn_Off();
                        //string openUI = data[0] as string;
                        //if (FindBlackWhiteList(this.Blacklist, openUI,true))
                        //{
                        //    UI.OpenUI(false);
                        //}
                        //else if (FindBlackWhiteList(this.Whitelist, openUI, true))
                        //{
                        //    UI.OpenUI(true);
                        //}
                    }
                    break;
                case GEnum.NamedEvent.SysUI_StateUpdate_CloseOrHide:
                    {
                        UIOn_Off();
                        //string uiName = data[0] as string;
                        //if (FindBlackWhiteList(this.Blacklist, uiName,false))
                        //{
                        //    UI.OpenUI(false);
                        //}
                        //else if (FindBlackWhiteList(this.Whitelist, uiName, false))
                        //{
                        //    UI.OpenUI(true);
                        //}
                    }
                    break;
                //当前UI标记
                case GEnum.NamedEvent.SysGame_UI_Sign_Update:
                    {
                        SignUpdate();
                        this.NewAddFriendList.Clear();
                    }
                    break;
                //好友状态更新
                case GEnum.NamedEvent.SysData_FriendStatus_Updata:
                    RefreshInviteListByDeskInfo();
                    UI.ScrollViewMgr.RefreshData();
                    break;
                //邀请好友进桌反馈
                case GEnum.NamedEvent.SysData_Friend_InviteFriendGame:
                    {
                        if (data.Length >= 2 && (int)data[0] != 0)
                        {
                            RemoveInvite((long)data[1]);
                        }
                        UI.InviteFriendGame((int)data[0]);
                        UI.ScrollViewMgr.RefreshData();
                    }
                    break;
                //被邀请进桌的接收
                case GEnum.NamedEvent.SysData_Friend_BeInviteGame:
                    //被邀请进桌 必须要在 大厅和未准备中
                    if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.MainIn && MemoryData.GameStateData.CurrUISign < SysGameStateData.EnumUISign.PrepareGame)
                        this.PushBeInviteData(data);
                    break;
                //请求进桌回复
                case GEnum.NamedEvent.SysData_Friend_JoinFriendDeskRsp:
                    {
                        int resultCode = (int)data[0];
                        //如果拒绝我进入则删除请求申请
                        if(resultCode != 0)
                        {
                            if(data.Length >= 2)
                            {
                                this.RemoveJoinDeskReq((long)data[1]);
                            }
                        }
                        UI.JoinFriendDesk(resultCode);
                        UI.ScrollViewMgr.RefreshData();
                    }
                    break;
                //好友 请求进入自己的牌桌 
                case GEnum.NamedEvent.SysData_Friend_BeJoinFriendDeskNotify:
                    //只有在准备中才能接受
                    if (MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame)
                        UI.BeJoinDesk((long)data[0], (int)data[1]);
                    break;

                //牌桌有变动时候
                case GEnum.NamedEvent.EMahjongDeskPlayer:
                case GEnum.NamedEvent.EMahjongQuit:
                case GEnum.NamedEvent.EMahjongReady:
                case GEnum.NamedEvent.EMahjongJoinDesk:
                    {
                        if (MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame)
                        {
                            RefreshInviteListByDeskInfo();
                            UI.ScrollViewMgr.RefreshData();
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysUI_FriendList_OpenClose_Req:
                    {
                        bool isShow = (bool)data[0];
                        if(isShow && !UI.Root.cachedGameObject.activeSelf)
                        {
                            UI.OpenUI(true);
                        }

                        if (UI.OnButtonOpenClick(isShow))
                        {
                            UI.RefreshUI(true);
                        }
                    }
                    break;

                case GEnum.NamedEvent.SysData_FriendListData_NewAdd://新好友添加
                    {
                        long userId = (long)data[0];
                        if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.PrepareGame_NotOpenDesk && NewAddFriendList.IndexOf(userId) == -1)
                        {
                            NewAddFriendList.Add(userId);

                            var playerInfo = MemoryData.PlayerData.get(userId);
                            if (playerInfo != null)
                            {
                                UI.LoadPop(WindowUIType.SystemPopupWindow, "提示", "用户" + playerInfo.PlayerDataBase.Name + "想要加您为游戏好友，是否同意他的请求？", new string[] { "拒绝", "接受" }, (index) =>
                                {
                                    if (index == 0)
                                    {
                                        ApplyFriendReq(playerInfo.PlayerDataBase.UserID, false);
                                    }
                                    else if (index == 1)
                                    {
                                        ApplyFriendReq(playerInfo.PlayerDataBase.UserID, true);
                                    }
                                    NewAddFriendList.Remove(userId);
                                }, true);
                            }
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}