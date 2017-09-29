using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Msg;
using System;

namespace projectQ
{

    //好友麻将馆
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfFriendMjHall()
        {
            //ModleNetWorker.Regiest<FMjRoomPlayerInfoReq>(FMjRoomPlayerInfoReq);
            ModelNetWorker.Regiest<FMjRoomPlayerInfoRsp>(FMjRoomPlayerInfoRsp);

            ModelNetWorker.Regiest<FMjRoomDescInfoRsp>(FMjRoomDescInfoRsp);

            ModelNetWorker.Regiest<FMjRoomSearchRsp>(FMjRoomSearchRsp);

            ModelNetWorker.Regiest<FMjRoomTuijianRsp>(FMjRoomTuijianRsp);

            ModelNetWorker.Regiest<FMjRoomBindRsp>(FMjRoomBindRsp);

            ModelNetWorker.Regiest<FMjRoomUnBindRsp>(FMjRoomUnBindRsp);

            ModelNetWorker.Regiest<FMjRoomUnBindNotify>(FMjRoomUnBindNotifyRsp);

            ModelNetWorker.Regiest<FMjRoomEnterRsp>(FMjRoomEnterRsp);

            ModelNetWorker.Regiest<FMjRoomLeaveRsp>(FMjRoomLeaveRsp);

            ModelNetWorker.Regiest<FMjRoomChangeBoardRsp>(FMjRoomChangeBoardRsp);

            ModelNetWorker.Regiest<FMjRoomChangeBoardNotify>(FMjRoomChangeBoardNotify);
            //查看麻将馆
            ModelNetWorker.Regiest<MjFMjRoomRsp>(MjFMjRoomRsp);
            ModelNetWorker.Regiest<MyFMjRoomUpdateNotify>(MyFMjRoomUpdateNotify);
            //查看牌桌战绩
            ModelNetWorker.Regiest<FMjRoomViewDeskRecordRsp>(FMjRoomViewDeskRecordRsp);
            //关联的客户信息
            ModelNetWorker.Regiest<RoomPlayersReq>(RoomPlayersReq);
            ModelNetWorker.Regiest<RoomPlayersRsp>(RoomPlayersRsp);

        }
        /// <summary>
        /// 麻将馆消息请求
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomPlayerInfoReq(long userId)
        {
            var req = new FMjRoomPlayerInfoReq();
            req.UserID = userId;
            this.send(req);
        }

        /// <summary>
        /// 麻将馆消息接收
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomPlayerInfoRsp(object data)
        {
            var rsp = data as FMjRoomPlayerInfoRsp;
            if (rsp.PlayerInfo != null)
            {
                if (rsp.UserID == MemoryData.UserID)
                {
                    MemoryData.PlayerData.MyPlayerModel.SetMjRoomPlayerInfo(rsp.PlayerInfo);
                    int hallId = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.MjRoomId;
                    //if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID > 0)
                    //    hallId = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID;
                    //else if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomID > 0)
                    //    hallId = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomID;


                    if (hallId > 0)
                    {
                        var hallData = MemoryData.MjHallData.GetMjHallById(hallId);
                        if (hallData == null)
                            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageRoomSearch, hallId.ToString(), 0, Msg.FMjSortType.FMjSort_RenQi);
                        //FMjRoomSearchReq(hallId.ToString(), 0, Msg.FMjSortType.FMjSort_Null);
                    }
                }
                else
                {
                    MemoryData.PlayerData.get(rsp.UserID).SetMjRoomPlayerInfo(rsp.PlayerInfo);
                }
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_User_MjHallInfoUpdate, rsp.UserID);

                MemoryData.SysPlayerDataHandle.PlayerRsp(rsp.UserID, SysPlayerDataHandle.PlayerDataType.MjRoomData);
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoginSouccessRsp, "FMjRoomPlayerInfo");
        }

        /// <summary>
        /// 麻将馆介绍请求
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomDescInfoReq()
        {
            var req = new FMjRoomDescInfoReq();
            this.send(req);
        }

        /// <summary>
        /// 麻将馆介绍接收
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomDescInfoRsp(object data)
        {
            var rsp = data as FMjRoomDescInfoRsp;
            var mjHall = MemoryData.MjHallData;
            mjHall.MjHallDescInfo = rsp.DescInfo;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoginSouccessRsp, "FMjRoomDescInfo");
        }


        /// <summary>
        /// 麻将馆搜索请求
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomSearchReq(string likeKey, int regionId, FMjSortType sortType)
        {
            var req = new FMjRoomSearchReq();
            req.RoomKey = likeKey;
            req.RegionID = regionId;
            req.SortType = sortType;

            this.send(req);
        }

        /// <summary>
        /// 麻将馆搜索接收
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomSearchRsp(object data)
        {
            var rsp = data as FMjRoomSearchRsp;
            var result = FMjRoomListToMjHallList(rsp.RoomList);

            //MemoryData.MjHallData.MjHallList = result;
            MemoryData.MjHallData.AddMjHallListValue(rsp.RoomKey, result, rsp.IsFinish);

            //接受信息中，打开转圈提示
            //EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "FMjRoomSearchRsp");

            //TODO-SZZ 获得麻将馆列表后，派发麻将馆数据获取结束的事件

            if (rsp.IsFinish)
            {
                //EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill, "FMjRoomSearchRsp");
                EventDispatcher.FireEvent(GEnum.NamedEvent.SysData_MjJall_SearchOver, result);
            }

            //END TODO-SZZ

        }

        /// 麻将馆推荐列表请求
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomTuijianReq(object data)
        {
            var req = new FMjRoomTuijianReq();
            this.send(req);
        }

        /// <summary>
        /// 麻将馆推荐列表接收
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomTuijianRsp(object data)
        {
            var rsp = data as FMjRoomTuijianRsp;
            var result = FMjRoomListToMjHallList(rsp.RoomList);
            MemoryData.MjHallData.MjHallListRecommend = result;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoginSouccessRsp, "FMjRoomTuijian");
        }


        private List<MjRoom> FMjRoomListToMjHallList(List<FMjRoom> roomList)
        {
            List<MjRoom> result = new List<MjRoom>();
            for (int i = 0; i < roomList.Count; ++i)
            {
                MjRoom temp = MjRoom.ProtoToData(roomList[i]);
                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// 关联麻将馆请求
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomBindReq(int roomId)
        {
            var info = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase;
            //if (info.BindRoomID != roomId && info.BindRoomState != 2)
            {
                var req = new FMjRoomBindReq();
                req.UserID = MemoryData.UserID;
                req.RoomID = roomId;
                this.send(req);
            }
        }

        /// <summary>
        /// 关联麻将馆接收
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomBindRsp(object data)
        {
            var rsp = data as FMjRoomBindRsp;
            int isOK = rsp.ResultCode;
            bool isFirst = rsp.IsFirst;
            if (isOK == 0)
            {
                MemoryData.SysPlayerDataHandle.PlayerReq(MemoryData.UserID, SysPlayerDataHandle.PlayerDataType.MjRoomData, true);
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_BindUpdate, isOK, isFirst);
        }


        /// <summary>
        /// 解除关联麻将馆请求
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomUnBindReq(object data)
        {
            var req = new FMjRoomUnBindReq();
            req.UserID = MemoryData.UserID;
            req.RoomID = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomID;
            this.send(req);
        }

        /// <summary>
        /// 解除关联麻将馆接收 , 进入关联解除中状态
        /// </summary>
        public void FMjRoomUnBindRsp(object data)
        {
            var rsp = data as FMjRoomUnBindRsp;
            int isOK = rsp.ResultCode;
            if (isOK == 0)
            {
                //var info = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomID = 0;
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "解除关联申请已提交" + Environment.NewLine + "馆长同意后即可与棋牌室解除关联", new string[] { "确定" }
                , delegate (int index)
                {
                    //_R.ui.OpenUI("UIMain", UIMain.EnumUIMainState.NormalMain);
                });
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_BindUpdate, isOK);
        }

        /// <summary>
        /// 解除关联成功，12小时后解除关联成功消息通知
        /// </summary>
        public void FMjRoomUnBindNotifyRsp(object data)
        {
            var rsp = data as FMjRoomUnBindNotify;

            if (rsp.UserID == MemoryData.UserID)
            {
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow,
                    "解除关联成功", "您跟原棋牌室关联关系已解除", new string[] { "确定" }, delegate (int index)
               {
                   _R.ui.OpenUI("UIMain", UIMain.EnumUIMainState.NormalMain);
               });
            }
        }

        /// <summary>
        /// 进入麻将馆请求
        /// </summary>
        /// <param name="data"></param>

        public void FMjRoomEnterReq(int roomId)
        {
            var req = new FMjRoomEnterReq();
            req.UserID = MemoryData.UserID;
            req.RoomID = roomId;
            this.send(req);
        }

        /// <summary>
        /// 进入麻将馆接收
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomEnterRsp(object data)
        {
            var rsp = data as FMjRoomEnterRsp;
            int isOK = rsp.ResultCode;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_JoinMjHall, isOK);
        }

        /// <summary>
        /// 退出麻将馆请求
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomLeaveReq(int roomId)
        {
            var req = new FMjRoomLeaveReq();
            req.UserID = MemoryData.UserID;
            req.RoomID = roomId;
            this.send(req);
        }

        /// <summary>
        /// 退出麻将馆接收
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomLeaveRsp(object data)
        {
            var rsp = data as FMjRoomLeaveRsp;
            int isOK = rsp.ResultCode;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_ExitMjHall, isOK);
        }




        /// <summary>
        /// 切换牌匾 请求
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomChangeBoardReq(string boardKey)
        {
            var req = new FMjRoomChangeBoardReq();
            req.UserID = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.UserID;
            req.RoomID = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID;
            req.BoardKey = boardKey;
            MemoryData.MjHallData.GetMjHallById(MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID).BoardID = boardKey;
            this.send(req);
        }

        /// <summary>
        /// 切换牌匾 接收
        /// </summary>
        /// <param name="data"></param>
        public void FMjRoomChangeBoardRsp(object obj)
        {
            var rsp = obj as FMjRoomChangeBoardRsp;
            if (rsp.ResultCode != 0)
            {
                WindowUIManager.Instance.CreateByErrorCode(rsp.ResultCode);
            }
        }

        /// <summary>
        /// 广播通知其他在麻将馆大厅的玩家，牌匾改变更新
        /// </summary>
        /// <param name="obj"></param>
        public void FMjRoomChangeBoardNotify(object obj)
        {
            var rsp = obj as FMjRoomChangeBoardNotify;

            var hall = MemoryData.MjHallData.GetMjHallById(rsp.RoomID);
            hall.BoardID = rsp.BoardKey;

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_BoardUpdata, rsp.RoomID);
        }


        //请求查看麻将馆 
        public void MyFMjRoomReq(long userID)
        {
            MyFMjRoomReq req = new Msg.MyFMjRoomReq();
            req.UserID = userID;

            this.send(req);
        }

        //查看麻将馆返回
        public void MjFMjRoomRsp(object obj)
        {
            var rsp = obj as MjFMjRoomRsp;
            if (rsp.ResultCode == 0)
            {
                Msg.FMjRoom fMjRoomInfo = rsp.MjRoomInfo;
                int eventRoomID = 0;
                List<int> eventDeskID = new List<int>();

                if (fMjRoomInfo != null)
                {
                    MjRoom temp = MjRoom.ProtoToData(fMjRoomInfo);
                    //add
                    eventRoomID = temp.RoomID;
                    MemoryData.MjHallData.AddMjHallMap(temp);
                }

                //set deskInfo
                List<Msg.MjDeskData> deskDataList = rsp.MjDeskList;
                if (deskDataList != null && deskDataList.Count > 0)
                {
                    for (int i = 0; i < deskDataList.Count; i++)
                    {
                        Msg.MjDeskData deskData = deskDataList[i];
                        MjDeskInfo info = new MjDeskInfo();
                        info.deskID = deskData.DeskID;
                        info.mjGameType = /*(EnumMjType)*/deskData.MjType;
                        info.maxDouble = deskData.OddsLimit;
                        info.bouts = deskData.CurBouts;
                        info.rounds = deskData.MaxBouts;
                        info.viewScore = deskData.ViewRecord;
                        info.ownerUserID = deskData.OwnerUserID;

                        List<Msg.MjDeskPlayerData> deskPlayerData = deskData.PlayerList;
                        if (deskPlayerData != null && deskPlayerData.Count > 0)
                        {
                            for (int j = 0; j < deskPlayerData.Count; j++)
                            {
                                PlayerDataModel playerModel = MemoryData.PlayerData.get(deskPlayerData[j].UserID);
                                Msg.MjDeskPlayerData data = deskPlayerData[j];

                                MjPlayerInfo playerInfo = playerModel.playerDataMj = new MjPlayerInfo(data.UserID, data.Score, data.SeatID, data.IsReady);
                                playerModel.SetMjPlayerInfoData(playerInfo, data.UserID, data.HeadUrl, -1, data.Name, data.Sex, data.ClientIp);
                                info.SetPlayerInfo(deskPlayerData[j].UserID);
                            }
                        }

                        eventDeskID.Add(info.deskID);
                        MemoryData.DeskData.AddOrUpdateDeskInfo(info.deskID, info);
                    }
                }

                MemoryData.MjHallData.RefreshHallDeskList(eventRoomID, eventDeskID);
                //event
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_CheckMjHall, eventRoomID);
            }
        }


        public void MyFMjRoomUpdateNotify(object obj)
        {
            var rsp = obj as MyFMjRoomUpdateNotify;
            int deskCount = rsp.DeskCount;
            int members = rsp.Members;
            int onlineNum = rsp.OnlineNum;

            //event
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_MjHallUpdate, deskCount, members, onlineNum);
        }



        //请求查看战绩
        public void FMjRoomViewDeskRecordReq(long userID, int roomID, int deskID)
        {
            FMjRoomViewDeskRecordReq req = new Msg.FMjRoomViewDeskRecordReq();
            req.UserID = userID;
            req.RoomID = roomID;
            req.DeskID = deskID;

            this.send(req);
        }


        //查看战绩返回
        public void FMjRoomViewDeskRecordRsp(object obj)
        {
            var rsp = obj as FMjRoomViewDeskRecordRsp;

            if (rsp.ResultCode == 0)
            {
                List<Msg.MjDeskRecord> recordList = rsp.RecordList;
                if (recordList != null && recordList.Count > 0)
                {
                    for (int i = 0; i < recordList.Count; i++)
                    {
                        Msg.MjDeskRecord info = recordList[i];
                        int deskID = info.DeskID;
                        int gameType = info.MjGameType;
                        int gameSub = info.ConfigID;
                        int oddCount = info.OddsLimit;
                        int showType = info.ShowType;
                        List<MjTitleInfo> titleInfo = this.GetMjTitleInfoList(info.TitleInfo);

                        //set bureau
                        List<Msg.MjBureauDetialInfo> detailInfo = info.BoutsRecord;
                        List<MjBureauDetialInfo> detailList = this.GetMjBureauDetialInfo(detailInfo);

                        //set player
                        List<GameResultPlayer> playerInfoList = new List<GameResultPlayer>();
                        int selfSeatID = -1;
                        if (info.PlayerList != null)
                        {
                            for (int k = 0; k < info.PlayerList.Count; k++)
                            {
                                long userID = info.PlayerList[k].UserID;
                                string nickName = info.PlayerList[k].Name;
                                string headUrl = info.PlayerList[k].HeadUrl;

                                int seatID = info.PlayerList[k].SeatID;
                                int score = info.PlayerList[k].Score;
                                int winBouts = info.PlayerList[k].WinBouts;

                                if (userID == MemoryData.UserID)
                                {
                                    selfSeatID = seatID;
                                }
                                GameResultPlayer player = new GameResultPlayer(userID, nickName, headUrl);
                                player.SetResultData(seatID, score, winBouts);
                                playerInfoList.Add(player);
                            }
                        }

                        GameResult result = new GameResult(deskID, gameType, gameSub, oddCount, showType, detailList, titleInfo, selfSeatID);
                        result.SetPlayerInfo(playerInfoList, MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.UserID);
                        result.maxBouts = info.MaxBouts;
                        result.recordTime = info.RecordTime;
                        result.ownerUserID = info.OwnerUserID;


                        MemoryData.ResultData.CheckAndUpdateData(result);

                        List<GameResultCostData> resultCostList = GetResultCostDataList(info.URInfo);
                        int resultType = info.DeskType;
                        MemoryData.ResultData.SetResultCostData(deskID, resultCostList, resultType);

                        //event
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_MjHall_MjDeskViewRecord, deskID);
                    }
                }
            }
        }



        public void RoomPlayersReq(object obj)
        {
            var rsp = obj as RoomPlayersReq;

            if (rsp != null)
            {
                long userid = rsp.UserID;
            }
        }

        //关联客户信息请求
        public void SendRoomPlayersReq()
        {
            var req = new RoomPlayersReq();
            req.UserID = MemoryData.UserID;
            this.send(req);
        }

        //关联客户信息返回
        public void RoomPlayersRsp(object obj)
        {
            var rsp = obj as RoomPlayersRsp;

            if (rsp != null)
            {
                long id = rsp.UserID;
                List<SimplePlayerInfo> playerData = rsp.OnlinePlayers;
                List<SimplePlayerInfo> relevance = rsp.Members;

                List<long> onlinePlayer = new List<long>();
                for (int i = 0; i < playerData.Count; i++)
                {
                    SimplePlayerInfo data = playerData[i];
                    onlinePlayer.Add(data.UserID);
                    PlayerDataModel player = MemoryData.PlayerData.get(data.UserID);

                    player.PlayerDataBase.Name = data.NickName;
                    player.PlayerDataBase.HeadURL = data.HeadIcon;

                    player.ClienteleDataState.SetGameState(data.State);
                    player.ClienteleDataState.SetCurDeskID(data.DeskID);
                }

                List<long> relevancePlayer = new List<long>();
                for (int i = 0; i < relevance.Count; i++)
                {
                    SimplePlayerInfo data = relevance[i];
                    relevancePlayer.Add(relevance[i].UserID);
                    PlayerDataModel player = MemoryData.PlayerData.get(data.UserID);

                    player.PlayerDataBase.Name = data.NickName;
                    player.PlayerDataBase.HeadURL = data.HeadIcon;

                    player.ClienteleDataState.SetGameState(data.State);
                    player.ClienteleDataState.SetCurDeskID(data.DeskID);
                }

                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_RoomPlayers_Over, onlinePlayer, relevancePlayer);
            }
        }


    }
}
