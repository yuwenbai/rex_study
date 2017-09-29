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
    public class UIFriendListData
    {
        public UIFriendListModel UIModel;
        public PlayerDataModel FriendData;
        //邀请房间ID
        public int InviteDeskId = 0;

        public Action<UIFriendListItem, int> onButtonClick;
        public Action<UIFriendListItem> onItemClick;
    }
    public class UIFriendListItem : ScrollViewItemBase<UIFriendListData>
    {
        public enum State
        {
            NotFriend = 0,    //不是好友
            //固定服务器发过来的 
            Offline = 1,    //离线
            InGame = 2,    //游戏中
            DeskWait = 3,    //牌桌等待中
            Idle = 4,    //空闲


            Enter = 5,    //进桌
            Invite = 6,    //邀请
            Invited = 7,    //邀请
            Prepare = 8,    //准备
            UnPrepare = 9,    //考虑中
            Entered = 10,    //已申请进桌
        }
        public UITexture TextureHead;
        public UILabel UserName;
        //public UILabel UserWinBouts;
        public UILabel UserHeat;
        public List<GameObject> Buttons;
        public UILabel LabelState;
        public TweenPosition TweenPos;

        public GameObject AddFriendGroup;
        public GameObject FriendGroup;

        private string TextureHeadUrl;

        public override void Refresh()
        {
            if (TextureHeadUrl != this.UIData.FriendData.PlayerDataBase.HeadURL)
            {
                if (!string.IsNullOrEmpty(TextureHeadUrl))
                    ResetHeadTexture();

                DebugPro.Log(DebugPro.EnumLog.HeadUrl, "UIFriendListItem__Refresh", this.UIData.FriendData.PlayerDataBase.HeadURL);
                TextureHeadUrl = this.UIData.FriendData.PlayerDataBase.HeadURL;
                DownHeadTexture.Instance.WeChat_HeadTextureGet(this.UIData.FriendData.PlayerDataBase.HeadURL, HeadTexCallBack);
            }
            this.UserName.text = this.UIData.FriendData.PlayerDataBase.Name;
            //this.UserWinBouts.text = "胜:" + this.UIData.FriendData.PlayerDataBase.WinBouts;
            UpdateButtonAndText();
        }


        private void HeadTexCallBack(Texture2D headTexture, string headName)
        {
            if (headName == DownHeadTexture.Instance.Texture_HeadNameSet(this.UIData.FriendData.PlayerDataBase.HeadURL))
                this.TextureHead.mainTexture = headTexture;
        }
        //重置头像Texture
        private void ResetHeadTexture()
        {
            this.TextureHead.mainTexture = DownHeadTexture.Instance.Texture_DefaultSet(GameAssetCache.Texture_Hand_Path);
        }

        //取得当前状态
        public State GetState()
        {
            var fd = this.UIData.FriendData;

            //好友请求
            if (!this.UIData.UIModel.UI.UIState)
            {
                return State.NotFriend;
            }

            //当前在准备界面 && 我的好友在桌子里边
            if (MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame)
            {
                var MjDeskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID/*MemoryData.Get<int>(MKey.USER_CURR_DESK_ID)*/);
                if (MjDeskInfo != null)
                {
                    var allPlayer = MjDataManager.Instance.GetAllPlayerInfoByDeskID(MjDeskInfo.deskID);
                    var allPlayerId = MjDeskInfo.GetAllPlayerID();
                    for (int i = 0; i < allPlayer.Length; i++)
                    {
                        if (allPlayer[i] != null && (ulong)allPlayerId[i] == (ulong)this.UIData.FriendData.PlayerDataBase.UserID)
                        {
                            return allPlayer[i].hasReady ? State.Prepare : State.UnPrepare;
                        }

                    }
                }
            }
            //进桌  我在主页面 |或者| 麻将馆页面 &并且& 好友是桌主
            if (fd.FriendPlayerInfo.state == (int)State.DeskWait && (MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.MainIn))
            {
                if (this.UIData.UIModel.IsJoinDeskReq(fd.PlayerDataBase.UserID))
                    return State.Entered;
                return State.Enter;
            }
            //邀请
            if (fd.FriendPlayerInfo.state == (int)State.Idle && MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.PrepareGame
                && MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerDeskID >= 0)
            {
                if (this.UIData.UIModel.IsInvited(fd.PlayerDataBase.UserID))
                    return State.Invited;
                return State.Invite;
            }

            return (State)fd.FriendPlayerInfo.state;
        }

        //返回删除动画
        public UITweener DeleteTween()
        {
            this.TweenPos.from = transform.localPosition;
            this.TweenPos.to = transform.localPosition - new Vector3(GetComponent<UIWidget>().width, 0, 0);
            return this.TweenPos;
        }

        //更新按钮和文字
        private void UpdateButtonAndText()
        {
            State tempState = GetState();

            //不是好友
            if (GetState() == State.NotFriend)
            {
                AddFriendGroup.SetActive(true);
                FriendGroup.SetActive(false);
            }
            else
            {
                LabelState.gameObject.SetActive(false);
                Buttons[0].SetActive(false);

                AddFriendGroup.SetActive(false);
                FriendGroup.SetActive(true);

                this.UserHeat.text = "热度:" + this.UIData.FriendData.FriendPlayerInfo.hotPoint;
                switch (tempState)
                {
                    case State.Offline:
                        LabelState.gameObject.SetActive(true);
                        LabelState.text = "离线";
                        break;
                    case State.InGame:
                        LabelState.gameObject.SetActive(true);
                        LabelState.text = "游戏中";
                        break;
                    case State.Idle:
                        LabelState.gameObject.SetActive(true);
                        LabelState.text = "空闲中";
                        break;
                    case State.DeskWait:
                        LabelState.gameObject.SetActive(true);
                        LabelState.text = "等待开桌中";
                        break;
                    case State.Enter:
                        Buttons[0].SetActive(true);
                        Buttons[0].GetComponentInChildren<UILabel>().text = "进桌";
                        break;
                    case State.Invite:
                        Buttons[0].SetActive(true);
                        Buttons[0].GetComponentInChildren<UILabel>().text = "邀请";
                        break;
                    case State.Entered:
                        LabelState.gameObject.SetActive(true);
                        LabelState.text = "已申请";
                        break;
                    case State.Invited:
                        LabelState.gameObject.SetActive(true);
                        LabelState.text = "已邀请";
                        break;
                    case State.Prepare:
                        LabelState.gameObject.SetActive(true);
                        LabelState.text = "已准备";
                        break;
                    case State.UnPrepare:
                        LabelState.gameObject.SetActive(true);
                        LabelState.text = "考虑中";
                        break;
                }
            }
        }

        #region Event
        //按钮点击
        private void OnButtonClick(GameObject go)
        {
            if (UIData.onButtonClick != null)
                UIData.onButtonClick(this, Buttons.IndexOf(go));
        }

        //Item全身点击 弹出用户详情用
        private void OnItemClick(GameObject go)
        {
            if (this.UIData.onItemClick != null)
                this.UIData.onItemClick(this);
        }
        #endregion

        #region 生命周期
        private void Awake()
        {
            for (int i = 0; i < Buttons.Count; ++i)
            {
                UIEventListener.Get(Buttons[i]).onClick = OnButtonClick;
            }
            UIEventListener.Get(gameObject).onClick = OnItemClick;

        }
        #endregion
    }
}