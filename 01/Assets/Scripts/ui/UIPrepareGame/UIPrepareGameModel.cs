/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIPrepareGameModel : UIModelBase
    {
        private UIPrepareGame UI
        {
            get { return this._ui as UIPrepareGame; }
        }

        public UIPrepareGameData data;

        //准备消息发送
        public void OnPrepareGame()
        {
            if(!FakeReplayManager.Instance.ReplayState)
            SendDeskAction(EnumMjDeskAction.MjRoom_Ready);
        }
        //解散房间
        public void OnQuitDesk()
        {
            if (this.data.DeskInfo.deskID == 0)
            {
                UI.LoadUIMain("CreateRoomNew");
                UI.Hide();
            }
            else
            {
                if (this.data.MyPlayer.seatID == 1)
                {
                    SendDeskAction(EnumMjDeskAction.MjRoom_Close);
                }
                else
                {
                    SendDeskAction(EnumMjDeskAction.MjRoom_Quit);
                }
            }
        }

        private void SendDeskAction(EnumMjDeskAction actionEnum)
        {
            ModelNetWorker.Instance.MjDeskActionReq((ulong)MemoryData.UserID, data.MyPlayer.seatID, MjDataManager.Instance.MjData.curUserData.selfDeskID, actionEnum);
        }

        private void ExitDesk(int index)
        {

#if UNITY_ANDROID
            RecordManager.Instance.LogoutRecord();//leave();
#endif
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenMain);
            //MemoryData.Set(MKey.USER_CURR_DESK_ID, 0);
        }
        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.EMahjongJoinDesk,
                GEnum.NamedEvent.EMahjongDeskPlayer,
                GEnum.NamedEvent.EMahjongReady,
                GEnum.NamedEvent.EMahjongQuit,
                GEnum.NamedEvent.EMahjongGameStart,
                GEnum.NamedEvent.EMahjongClose,
                GEnum.NamedEvent.EMahjongNewDesk,
                GEnum.NamedEvent.SysUI_FriendList_OpenClose_Rsp,
                GEnum.NamedEvent.LoginSouccessRsp,
                GEnum.NamedEvent.SysUI_PrepareGame_RefreshPrepare,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            Debug.Log("桌子状态变更 消息"+msgEnum.ToString());
            switch (msgEnum)
            {
                //加入desk的玩家信息(也包括我自己) List<MjPlayerInfo> playerInfoList, int roomID
                case GEnum.NamedEvent.EMahjongDeskPlayer:
                    {
                        //if (this.data != null && this.data.GetPlayerCount() >= 4)
                        //{
                        //    StartCoroutine(UITools.WaitExcution(
                        //        () =>
                        //        {
                        //            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_CheatInfo_Open, true);
                        //        }, 1f));
                        //}
                    }
                    break;
                //有人准备
                case GEnum.NamedEvent.EMahjongReady:
                    {
                    }
                    break;
                //有人退出房间
                case GEnum.NamedEvent.EMahjongQuit:
                    {
                        int seatID = (int)data[0];
                        if (seatID == this.data.MyPlayer.seatID)
                        {
                            if (this.data.MyPlayer.seatID == 1)
                            {
                                EventDispatcher.FireSysEvent(GEnum.NamedEvent.OpenUI_UICreateRoom);
                                //UI.LoadUIMain("UICreateRoom");
                                UI.Hide();
                            }
                            else
                            {
                                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenMain);
                            }
                            return;
                        }

                        if (this.data != null)
                        {
                            int index = this.data.GetIndex(seatID);
                            this.data.PlayerDataList[index] = null;
                        }
                    }
                    break;
                case GEnum.NamedEvent.EMahjongClose: //解散房间
                    {
                        //当处于游戏中时 不参与解散房间消息处理
                        if (UI.CurrState == UIPrepareGame.UIState.GameIn) return;
                        if (this.data.MyPlayer != null && this.data.MyPlayer.seatID == 1)
                        {

                            ExitDesk(0);
                            return;
                        }
                        else
                        {
                            if (UI.CurrState < UIPrepareGame.UIState.GameIn)
                                UI.LoadPop(WindowUIType.SystemPopupWindow, "牌桌解散", "房主解散牌桌", new string[] { "确定" }, ExitDesk);
                        }
                    }
                    break;
                case GEnum.NamedEvent.EMahjongJoinDesk: //进入牌桌
                    {
                        int resultCode = (int)data[0];
                        if (resultCode == 0)
                        {
                            RefreshData();
                        }
                        break;
                    }
                case GEnum.NamedEvent.EMahjongGameStart://游戏开始
                    {
                        UI.OpenDesk();
                    }
                    break;
                case GEnum.NamedEvent.EMahjongNewDesk:
                    {
                        if ((int)data[0] == 0)
                        {
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysUI_FriendList_OpenClose_Rsp:
                    if (UI.CurrState == UIPrepareGame.UIState.NotPrepare || UI.CurrState == UIPrepareGame.UIState.Prepare)
                    {
                        UI.OpenDeskMgr.OnUpdateButton(!(bool)data[0]);
                    }
                    return;
                case GEnum.NamedEvent.LoginSouccessRsp:
                    {

                    }
                    break;
                case GEnum.NamedEvent.SysUI_PrepareGame_RefreshPrepare:
                    {
                        UI.RefreshUI_Prepare(false);
                    }
                    return;
                case GEnum.NamedEvent.SysUI_PrepareGame_ClearPrepare:
                    {
                        UI.RefreshUI_Prepare(true);
                    }
                    return;
            }
            UI.RefreshUI();
        }
        #endregion

        #region 生命周期
        private void Awake()
        {
            this.data = new UIPrepareGameData();
        }
        #endregion

        public void RefreshData()
        {
            this.data.DeskInfo = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);

            if (this.data.DeskInfo == null) return;

            var playerInfoList = MjDataManager.Instance.GetAllPlayerInfoByDeskID(data.DeskInfo.deskID);
            var playerInfoIDList = this.data.DeskInfo.GetAllPlayerID();

            if (playerInfoList == null || playerInfoIDList == null) return;


            for (int i = 0; i < playerInfoIDList.Length; i++)
            {
                if (playerInfoIDList[i] == (long)MjDataManager.Instance.MjData.curUserData.selfUserID)
                {
                    this.data.SetPlayer(playerInfoList[i], playerInfoIDList[i]);
                    break;
                }
            }

            for (int i = 0; i < playerInfoIDList.Length; i++)
            {
                this.data.SetPlayer(playerInfoList[i], playerInfoIDList[i]);
            }

            if (this.data.PlayerDataList != null)
            {
                for (int i = 0; i < this.data.PlayerDataList.Count; i++)
                {
                    if (this.data.PlayerDataList[i] != null)
                    {
                        if (playerInfoIDList == null || System.Array.IndexOf(playerInfoIDList, this.data.PlayerDataList[i].userID) < 0)
                        {
                            this.data.PlayerDataList[i] = null;
                        }
                    }
                }
            }


            /*if(MjDataManager.Instance.MjData.curUserData.selfSeatID > 0)
            {
				int idx = MjDataManager.Instance.MjData.curUserData.selfSeatID - 1;
				if(playerInfoList.Length > idx+1
					&& playerInfoIDList.Length > idx +1){
					this.data.SetPlayer(playerInfoList[idx], playerInfoIDList[idx]);
				}
            }*/
        }


    }
    public class UIPrepareGameData
    {
        public int Count
        {
            get
            {
                int result = 0;
                if (PlayerDataList != null)
                {
                    for (int i = 0; i < PlayerDataList.Count; i++)
                    {
                        if (PlayerDataList[i] != null) result++;
                    }
                }
                return result;
            }
        }
        //房间数据
        private MjDeskInfo _deskInfo;
        public MjDeskInfo DeskInfo
        {
            set
            {
                //MemoryData.DeskData.TempDeskData = null;
                _deskInfo = value;
            }
            get
            {
                if (_deskInfo == null)
                {
                    return MemoryData.DeskData.TempDeskData;
                }
                return _deskInfo;
            }
        }

        //座位数据
        public List<PlayerData> PlayerDataList;
        public PlayerData MyPlayer
        {
            get
            {
                if (PlayerDataList == null) return null;
                return PlayerDataList[0];
            }
        }
        public PlayerData RightPlayer
        {
            get
            {
                if (PlayerDataList == null) return null;
                return PlayerDataList[1];
            }
        }
        public PlayerData UpPlayer
        {
            get
            {
                if (PlayerDataList == null) return null;
                return PlayerDataList[2];
            }
        }
        public PlayerData LeftPlayer
        {
            get
            {
                if (PlayerDataList == null) return null;
                return PlayerDataList[3];
            }
        }

        public PlayerData DeskOwnerPlayer
        {
            get
            {
                if (PlayerDataList != null && DeskOwnerIndex >= 0)
                {
                    return PlayerDataList[DeskOwnerIndex];
                }
                return null;
            }
        }

        public int DeskOwnerIndex = -1;

        //玩家数据
        public class PlayerData
        {
            public long userID;
            public string headUrl;
            public int level;
            public string nickName;
            public int score;
            public int seatID;
            public bool hasReady;
        }

        public void SetPlayer(MjPlayerInfo data, long userID)
        {
            if (data == null) return;
            if (PlayerDataList == null)
            {
                PlayerDataList = new List<PlayerData>();
                for (int i = 0; i < 4; ++i)
                {
                    PlayerDataList.Add(null);
                }
            }
            PlayerData pd = ToPlayerData(data, userID);
            if (pd.userID == (long)MjDataManager.Instance.MjData.curUserData.selfUserID)
            {
                PlayerDataList[0] = pd;
                if (pd.seatID == 1)
                    DeskOwnerIndex = 0;
            }
            else
            {
                int index = this.GetIndex(pd.seatID);
                if (index > 0)
                {
                    PlayerDataList[index] = pd;
                }
                if (pd.seatID == 1)
                    DeskOwnerIndex = index;
            }
        }

        public int GetIndex(int seatID)
        {
            if (MyPlayer == null) return -1;

            int myPos = MyPlayer.seatID;
            int uiIndex = -1;
            uiIndex = seatID - myPos;
            if (uiIndex < 0)
            {
                uiIndex += 4;
            }
            return uiIndex;
        }

        //是否全部准备好
        public bool IsAllReady()
        {
            if (PlayerDataList == null || PlayerDataList.Count != 4)
            {
                return false;
            }
            for (int i = 0; i < PlayerDataList.Count; ++i)
            {
                if (PlayerDataList[i] == null || !PlayerDataList[i].hasReady)
                {
                    return false;
                }
            }
            return true;
        }

        //取得玩家人数
        public int GetPlayerCount()
        {
            if (DeskInfo != null && DeskInfo.playerIdList != null)
            {
                return DeskInfo.playerIdList.Count;
            }
            return 0;
        }

        private PlayerData ToPlayerData(MjPlayerInfo data, long userID)
        {
            DebugPro.Log(DebugPro.EnumLog.HeadUrl, "UIPrepareGameModel__ToPlayerData", MemoryData.PlayerData.get(userID).PlayerDataBase.HeadURL);

            var play = MemoryData.PlayerData.get(userID).PlayerDataBase;
            PlayerData result = new PlayerData();
            result.userID = play.UserID;
            result.headUrl = play.HeadURL;
            result.level = play.Level;
            result.nickName = play.Name;
            result.score = data.score;
            result.seatID = data.seatID;
            result.hasReady = data.hasReady;

            return result;
        }
    }
}