/**
 * @Author Xin.Wang
 *
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Msg;

namespace projectQ
{
    //注册数据处理
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfMahjong()
        {
            //base
            ModelNetWorker.Regiest<MjNewDeskRsp>(MjNewDeskRsp);
            ModelNetWorker.Regiest<MjJoinDeskRsp>(MjJoinDeskRsp);
            ModelNetWorker.Regiest<MjDeskUsersNotify>(MjDeskUsersNotify);

            ModelNetWorker.Regiest<MjOpActionNotify>(MjOpActionNotify);
            ModelNetWorker.Regiest<MjOpPutMjNotify>(MjOpPutMjNotify);
            ModelNetWorker.Regiest<MjOpActionRsp>(MjOpActionRsp);

            ModelNetWorker.Regiest<MjDeskActionNotify>(MjDeskActionNotify);
            ModelNetWorker.Regiest<MjDeskActionRsp>(MjDeskActionRsp);

            ModelNetWorker.Regiest<MjRoomGameStartNotify>(MjRoomGameStartNotify);
            ModelNetWorker.Regiest<MjGameInitMjListNotify>(MjGameInitMjListNotify);
            ModelNetWorker.Regiest<MjDealerBeginNotify>(MjDealerBeginNotify);

            ModelNetWorker.Regiest<MjSyncPlayerStateNotify>(MjSyncPlayerStateNotify);
            //ModelNetWorker.Regiest<MjHuPaiNotify>(MjHuPaiNotify);
            ModelNetWorker.Regiest<MjPutDownHandsCodeNotify>(MjPutDownHandsCodeNotify);
            //ModelNetWorker.Regiest<MjBalanceNotify>(MjBalanceNotify);
            ModelNetWorker.Regiest<MjGameOverNotify>(MjGameOverNotify);
            ModelNetWorker.Regiest<MjBalanceNewNotify>(MjBalanceNewNotify);
            ModelNetWorker.Regiest<MjWinnerCostCardInfoRsp>(MjWinnerCostCardInfoRsp);

            //new
            ModelNetWorker.Regiest<MjBeforeDispatch>(MjBeforeDispatch);
            ModelNetWorker.Regiest<MjStandingPlates>(MjStandingPlates);

            //换三张 
            ModelNetWorker.Regiest<MjAskReqChangeThree>(MjAskReqChangeThree);
            ModelNetWorker.Regiest<MjRspChangeThree>(MjRspChangeThree);
            ModelNetWorker.Regiest<MjChangeThreeNotify>(MjChangeThreeNotify);

            //定缺
            ModelNetWorker.Regiest<MjAskReqConfirm>(MjAskReqConfirm);
            ModelNetWorker.Regiest<MjRspConfirm>(MjRspConfirm);
            ModelNetWorker.Regiest<MjConfirmNotify>(MjConfirmNotify);

            //下跑
            ModelNetWorker.Regiest<MjAskReqPao>(MjAskReqPao);
            ModelNetWorker.Regiest<MjRspPao>(MjRspPao);
            ModelNetWorker.Regiest<MjPaoNotify>(MjPaoNotify);

            //下鱼
            ModelNetWorker.Regiest<MjAskReq258TiaoYu>(MjAskReq258TiaoYu);
            ModelNetWorker.Regiest<MjRsp258TiaoYu>(MjRsp258TiaoYu);
            ModelNetWorker.Regiest<Mj258TiaoYuNotify>(Mj258TiaoYuNotify);


            //翻牌
            ModelNetWorker.Regiest<MjObligate>(MjObligate);
            //买马 
            ModelNetWorker.Regiest<MjBuyHorseCountNotify>(MjBuyHorseCountNotify);
            ModelNetWorker.Regiest<MjBuyHorseNotify>(MjBuyHorseNotify);
            //补花
            ModelNetWorker.Regiest<MjGetFlowerNotify>(MjGetFlowerNotify);
            //明楼
            ModelNetWorker.Regiest<MjGetMingLouNotify>(MjGetMingLouNotify);
            //表演
            ModelNetWorker.Regiest<MjBiaoYanNotify>(MjBiaoYanNotify);

            //下炮子
            ModelNetWorker.Regiest<MjAskReqPaoZi>(MjAskReqPaoZi);
            ModelNetWorker.Regiest<MjRspPaoZi>(MjRspPaoZi);
            ModelNetWorker.Regiest<MjPaoZiNotify>(MjPaoZiNotify);


            //分数变化通知
            ModelNetWorker.Regiest<MjScoreChangeNotify>(MjScoreChangeNotify);
            //跟妆
            //ModelNetWorker.Regiest<MjFollowDealer>(MjFollowDealer);
            //时间
            ModelNetWorker.Regiest<MjTimeNotify>(MjTimeNotify);
            //重连
            ModelNetWorker.Regiest<MjDeskReconectRsp>(MjDeskReconectRsp);
            //在线状态
            ModelNetWorker.Regiest<MjOnLineOffLine>(MjOnLineOffLine);

            ModelNetWorker.Regiest<MjBalanceRsp>(MjBalanceRsp);

            //大结算打开关闭消息接收
            ModelNetWorker.Regiest<GameResultBoardStateRsp>(GameResultBoardStateRsp);

            //坎牌 闹庄 末留
            ModelNetWorker.Regiest<MjRspNaoZhuangMoLiuKanPai>(MjRspNaoZhuangMoLiuKanPai);
            ModelNetWorker.Regiest<MjNaoZhuangMoLiuKanPaiNotify>(MjNaoZhuangMoLiuKanPaiNotify);

            //选飘
            ModelNetWorker.Regiest<MjPiaoJinPaioSuNotify>(MjPiaoJinPaioSuNotify);
            ModelNetWorker.Regiest<MjRspGameingRulerSet>(MjRspGameingRulerSet);

            //杠后拿吃
            ModelNetWorker.Regiest<MjGangHouNaChiNotify>(MjGangHouNaChiServerNotify);
            ModelNetWorker.Regiest<MjRspGameingNaChi>(MjRspGameingNaChiResult);
            //包次提示
            ModelNetWorker.Regiest<MjTipInfoNotify>(MjTipInfoNotify);

            ////亮一张
            ModelNetWorker.Regiest<MjLiangYiZhangNotify>(MjLiangYiZhangNotify);
            ModelNetWorker.Regiest<MjRspLiangYiZhang>(MjRspLiangYiZhang);

            //ruler change
            ModelNetWorker.Regiest<MjRulerChangeNotify>(MjRulerChangeNotify);

            //亮四打一 
            ModelNetWorker.Regiest<MjLiangSiDaYiNotify>(MjLiangSiDaYiNotify);

            //出牌限制
            ModelNetWorker.Regiest<MjPutLimitNotify>(MjPutLimitNotify);

            //亮杠头
            ModelNetWorker.Regiest<MjLiangGangTouNotify>(MjLiangGangTouNotify);

            //补花
            ModelNetWorker.Regiest<MjBuHuaNotify>(MjBuHuaNotify);
            ModelNetWorker.Regiest<MjSelfBuHuaNotify>(MjSelfBuHuaNotify);
        }

        #region Get Message

        //开房
        public void MjNewDeskRsp(object rsp)
        {
            var prsp = rsp as MjNewDeskRsp;
            if (prsp.ResultCode == 0)
            {
                Msg.MjDeskInfo MjDeskInfo = prsp.DeskInfo;
                //List<EnumMjGameRuler> gamerulerList = new List<EnumMjGameRuler>();
                //if (MjDeskInfo.MjRuler != null && MjDeskInfo.MjRuler.Count > 0)
                //{
                //    for (int i = 0; i < MjDeskInfo.MjRuler.Count; i++)
                //    {
                //        EnumMjGameRuler ruler = (EnumMjGameRuler)((int)MjDeskInfo.MjRuler[i]);
                //        gamerulerList.Add(ruler);
                //    }
                //}
                //EnumMjType mjType = (EnumMjType)((int)MjDeskInfo.MjGameType);
                //MjDeskInfo deskInfo = new MjDeskInfo(MjDeskInfo.Bouts, MjDeskInfo.MaxDouble, mjType, gamerulerList,
                //    MjDeskInfo.DeskID, MjDeskInfo.Rounds, MjDeskInfo.ViewScore, MjDeskInfo.DeskAdvert);
                MjDeskInfo deskInfo = this.MjGetDeskInfo(MjDeskInfo);

                EnumMjNewDeskResult resultCode = (EnumMjNewDeskResult)prsp.ResultCode;
                ulong userID = prsp.UserID;

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjNewDeskRsp, resultCode, deskInfo, userID);
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongNewDesk, prsp.ResultCode);
        }

        //加入房间
        public void MjJoinDeskRsp(object rsp)
        {
            var prsp = rsp as MjJoinDeskRsp;

            MemoryData.GameStateData.JoinDeskRoomId = prsp.RoomID;

            if (prsp.ResultCode == 0)
            {
                Msg.MjDeskInfo MjDeskInfo = prsp.DeskInfo;

                MjDeskInfo deskInfo = this.MjGetDeskInfo(MjDeskInfo);

                EnumMjNewDeskResult resultCode = (EnumMjNewDeskResult)prsp.ResultCode;
                ulong userID = prsp.UserID;

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjJoinDeskRsp, resultCode, deskInfo, userID);
                MemoryData.GameStateData.IsExternalLinkJoinDesk = false;
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongJoinDesk, prsp.ResultCode);
            }
            if (MemoryData.InitData.GetKey() == WXOpenParaEnum.SHARE_INVITE_PLAY)
                MemoryData.InitData.Clear();
        }


        //加入房间的通知
        public void MjDeskUsersNotify(object rsp)
        {
            var prsp = rsp as MjDeskUsersNotify;

            List<Msg.MjPlayerInfo> originalList = prsp.Players;
            List<MjPlayerInfo> mjPlayerInfo = this.MjGetPlayerInfoList(originalList, 0);
            //List<long> mjIDInfo = this.MjGetPlayerIDList(originalList);
            int roomID = prsp.RoomID;

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjDeskUsersNotify, mjPlayerInfo, roomID);
        }

        public void MjOpActionRsp(object rsp)
        {
            var prsp = rsp as MjOpActionRsp;
            int resultCode = prsp.ResultCode;
            ulong userID = prsp.UserID;

            //MahjongLogicNew.Instance.MjActionRsp(userID, resultCode);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjOpActionRsp, userID, resultCode);
        }

        public void MjOpPutMjNotify(object rsp)
        {
            var prsp = rsp as MjOpPutMjNotify;
            int errorCode = prsp.ResultCode;
            if (errorCode == 1)
            {
                bool isDark = prsp.IsDark;
                bool isIndependent = prsp.IsIndependent;
                int mjCode = prsp.MjCode;
                int seatID = prsp.SeatID;
                ulong userID = prsp.UserID;
                bool isAutoPut = prsp.AutoPut;

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjOpactionPutNotify, seatID, mjCode, isIndependent, isDark, isAutoPut);
            }

        }


        //麻将行为通知 
        public void MjOpActionNotify(object rsp)
        {
            var prsp = rsp as MjOpActionNotify;
            List<int> chiList = prsp.ChiList;
            EnumMjHuType huType = (EnumMjHuType)((int)prsp.HuType);
            int lastPutSeat = prsp.LastPutSeatID;
            int mjCode = prsp.MjCode;
            int mjGangType = ((int)prsp.MjGangType);
            int mjRulerResult = prsp.MjRulerResult;
            int nGangSeatID = prsp.nGangSeatID;         //被抢杠的玩家
            int nMjCodeType = prsp.nMjCodeType;         //胡牌的牌型
            EnumMjOpAction mjAction = (EnumMjOpAction)((int)prsp.OpCode);
            List<int> seatIDlist = prsp.SeatID;
            List<MjTingInfo> tingInfoList = MjGetTingInfoList(prsp.TingList);
            ulong userID = prsp.UserID;

            //new
            List<int> maoList = prsp.MaoList;
            List<MjRoundGetBuPai> mjChangeFlower = null;
            if (prsp.BuPaiList != null)
            {
                List<Msg.MjRoundGetBuPai> changeFlower = prsp.BuPaiList.Round;
                if (changeFlower != null && changeFlower.Count > 0)
                {
                    mjChangeFlower = new List<MjRoundGetBuPai>();
                    for (int i = 0; i < changeFlower.Count; i++)
                    {
                        MjRoundGetBuPai round = new MjRoundGetBuPai(changeFlower[i].nPut, changeFlower[i].nValue, changeFlower[i].nType);
                        mjChangeFlower.Add(round);
                    }
                }
            }
            bool isIndependent = prsp.IsIndependent;
            int independentCount = prsp.IndependentCount;
            List<int> gangList = prsp.GangList;
            List<int> handList = prsp.CodeList;
            bool canFangmao = prsp.IsHaveMao;
            bool isQiangGang = prsp.IsQiangGang;
            bool isDark = prsp.IsDark;
            int moCode = prsp.nMoCode;
            MjDataManager.Instance.Base_RulerLimit(prsp.nPlayType);//阻塞玩家操作选项
            List<int> ciList = prsp.CiList;
            MJSetPaikouChangeList(prsp.ChangeList);


            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjOpActionNotify,
                chiList, huType, lastPutSeat, mjCode, mjGangType, mjRulerResult, nGangSeatID,
            mjAction, seatIDlist, tingInfoList, userID,
            maoList, mjChangeFlower, isIndependent, independentCount, gangList, handList, canFangmao, isQiangGang, isDark, moCode, ciList);
        }

        //游戏开始通知
        public void MjRoomGameStartNotify(object rsp)
        {
            var prsp = rsp as MjRoomGameStartNotify;
            int dealerID = prsp.DealerID;
            int getMjSeatID = prsp.GetMjSeatID;
            int getOffset = prsp.GetOffset;
            List<int> rollNum = prsp.RollNo;
            int deskID = prsp.DeskID;
            int allCount = prsp.nMjAllCount;

            //mao
            List<List<int>> maoList = null;
            List<MjMaoGroup> originalGroup = prsp.MaoGroup;
            if (originalGroup != null && originalGroup.Count > 0)
            {
                maoList = new List<List<int>>();
                for (int i = 0; i < originalGroup.Count; i++)
                {
                    maoList.Add(originalGroup[i].MaoValue);
                }
            }

            //change 
            if (prsp.ChangeList != null)
            {
                for (int i = 0; i < prsp.ChangeList.Count; i++)
                {
                    int cardID = prsp.ChangeList[i].nMjCode;
                    int type = prsp.ChangeList[i].Type;

                    MjDataManager.Instance.SetCurStartChange(cardID, type);
                }
            }

            //MahjongLogicNew.Instance.MjGameStartNotify(dealerID, getMjSeatID, getOffset, rollNum, roomID);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjRoomGameStartNotify, dealerID, getMjSeatID, getOffset, rollNum, allCount, deskID, maoList);
        }

        //麻将发牌通知
        public void MjGameInitMjListNotify(object rsp)
        {
            var prsp = rsp as MjGameInitMjListNotify;
            List<int> mjList = prsp.MjList;
            int seatID = prsp.SeatID;
            List<int>[] independentList = null;

            //MJTODO : 内部加上座位号 
            if (prsp.MjIndependentList != null && prsp.MjIndependentList.Count > 0)
            {
                independentList = new List<int>[4];
                for (int i = 0; i < prsp.MjIndependentList.Count; i++)
                {
                    independentList[i] = (prsp.MjIndependentList[i].MjList);
                }
            }

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjGameInitMjListNotify, mjList, seatID, independentList);
        }

        //麻将胡牌通知(废弃)
        //public void MjHuPaiNotify(object rsp)
        //{
        //    var prsp = rsp as MjHuPaiNotify;
        //    int roomID = prsp.DeskID;
        //    List<Msg.MjScore> originalList = prsp.ScoreList;
        //    List<MjScore> mjScoreList = null;
        //    if (originalList != null && originalList.Count > 0)
        //    {
        //        mjScoreList = new List<MjScore>();
        //        for (int i = 0; i < originalList.Count; i++)
        //        {
        //            Msg.MjScore originalScore = originalList[i];
        //            MjScore score = new MjScore(originalScore.SeatID, originalScore.Score);
        //            mjScoreList.Add(score);
        //        }
        //    }
        //    else
        //    {
        //        QLoger.ERROR("MjHuPaiNotify : score is null or count is 0", originalList);
        //    }
        //    int winseatID = prsp.WinSeatID;
        //}

        /// <summary>
        /// 推牌强
        /// </summary>
        /// <param name="rsp"></param>
        public void MjPutDownHandsCodeNotify(object rsp)
        {
            var prsp = rsp as MjPutDownHandsCodeNotify;

            List<int> dajiaoSeats = prsp.DaJiaoSeatID;
            List<int> huazhuSeats = prsp.HuaZhuSeatID;

            //tuishui
            List<Msg.MjScoreChange> tuishuiChange = prsp.TuiShuiScore;
            List<MjChangeScore> tuiShuiList = null;
            if (tuishuiChange != null && tuishuiChange.Count > 0)
            {
                tuiShuiList = new List<MjChangeScore>();

                for (int i = 0; i < tuishuiChange.Count; i++)
                {
                    MjScoreChange proItem = tuishuiChange[i];
                    int showType = proItem.ShowType;
                    List<MjScore> scoreList = new List<MjScore>();
                    if (proItem.score != null && proItem.score.Count > 0)
                    {
                        for (int j = 0; j < proItem.score.Count; j++)
                        {
                            MjScore scoreItem = new MjScore(proItem.score[j].SeatID, proItem.score[j].Score);
                            scoreList.Add(scoreItem);
                        }
                    }
                    MjChangeScore changeItem = new MjChangeScore(scoreList, showType);
                    tuiShuiList.Add(changeItem);
                }
            }

            List<List<MjPai>> handPai = new List<List<MjPai>>();
            List<List<int>> independentPai = new List<List<int>>();


            if (prsp.HandMjList != null && prsp.HandMjList.Count > 0)
            {
                for (int i = 0; i < prsp.HandMjList.Count; i++)
                {
                    List<MjPai> paiList = MjGetHandList(prsp.HandMjList[i].MjList);
                    handPai.Add(paiList);

                    List<int> independentList = prsp.HandMjList[i].IndependentList;
                    independentPai.Add(independentList);
                }
            }

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjPutDownNotify, dajiaoSeats, huazhuSeats, tuiShuiList, handPai, independentPai);
        }


        ///// <summary>
        ///// 麻将单局结算
        ///// </summary>
        ///// <param name="rsp"></param>
        //public void MjBalanceNotify(object rsp)
        //{
        //    //var prsp = rsp as MjBalanceNotify;
        //    //int allScore = prsp.AllScore;                                           //详细信息人的面板总分 
        //    //int deskID = prsp.DeskID;

        //    //List<Msg.MjDetaildedScore> originalDetailList = prsp.DetailScoreList;   //详细信息人的的分总计
        //    //List<MjDetaildedScore> detailList = new List<MjDetaildedScore>();
        //    //if (originalDetailList != null && originalDetailList.Count > 0)
        //    //{
        //    //    for (int i = 0; i < originalDetailList.Count; i++)
        //    //    {
        //    //        Msg.MjDetaildedScore original = originalDetailList[i];
        //    //        List<MjDetaildedSpecial> specialList = new List<MjDetaildedSpecial>();
        //    //        if (original.SpType != null && original.SpType.Count > 0)
        //    //        {
        //    //            for (int j = 0; j < original.SpType.Count; j++)
        //    //            {
        //    //                MjDetaildedSpecial specialItem = new MjDetaildedSpecial();
        //    //                specialItem.specialType = original.SpType[j].Type;
        //    //                specialItem.value = original.SpType[j].Value;
        //    //                specialList.Add(specialItem);
        //    //            }
        //    //        }

        //    //        MjDetaildedScore detail = new MjDetaildedScore(original.Score, original.ScoreType, original.SeatID,
        //    //            original.HuType, original.PaiType, specialList);
        //    //        detailList.Add(detail);
        //    //    }
        //    //}

        //    //BestMjRecord bestRecord = this.GetBestMjRecord(prsp.MjRecords);


        //    //List<Msg.MjScore> originalScore = prsp.ScoreList;                        //其余三人的的分总计
        //    //List<MjScore> scoreList = new List<MjScore>();
        //    //if (originalScore != null && originalScore.Count > 0)
        //    //{
        //    //    for (int i = 0; i < originalScore.Count; i++)
        //    //    {
        //    //        MjScore score = new MjScore(
        //    //            originalScore[i].SeatID, originalScore[i].Score, originalScore[i].IsDianPao, originalScore[i].IsChengBao);
        //    //        scoreList.Add(score);
        //    //    }
        //    //}

        //    //int showType = prsp.ShowType;                                           //展示类型 1为普通 2为血流血战
        //    //int winSeatID = prsp.WinSeatID;                                         //赢家座位ID 血流血战不需要 是自己

        //    //bool isNoWiner = prsp.IsDraw;

        //    //EventDispatcher.FireEvent(GEnum.NamedEvent.EMjBalanceNotify, deskID, allScore, detailList, bestRecord, scoreList, showType, winSeatID, isNoWiner);

        //}

        /// <summary>
        /// 小结算新消息
        /// </summary>
        /// <param name="rsp"></param>
        public void MjBalanceNewNotify(object rsp)
        {
            var prsp = rsp as MjBalanceNewNotify;

            MjBalanceNew info = GetBalanceNewInfo(prsp);
            MemoryData.BalanceData.AddOrUpdateData(info.deskID, info.curBureau, prsp);

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjBalanceNewNotify, true, info);
        }


        //小结算数据推送 
        public void MjBalanceRsp(object rsp)
        {
            var prsp = rsp as MjBalanceRsp;
            int resultCode = prsp.ResultCode;
            MjBalanceNew info = null;
            if (resultCode == 0)
            {
                info = GetBalanceNewInfo(prsp.BalanceInfo);
                MemoryData.BalanceData.AddOrUpdateData(info.deskID, info.curBureau, prsp.BalanceInfo);
            }
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjBalanceNewNotify, false, info);
        }



        public MjBalanceNew GetBalanceNewInfoByMemory(object obj)
        {
            var prsp = obj as MjBalanceNewNotify;
            return GetBalanceNewInfo(prsp);
        }


        /// <summary>
        /// 获取本地小结算数据
        /// </summary>
        /// <param name="prsp"></param>
        /// <returns></returns>
        public MjBalanceNew GetBalanceNewInfo(Msg.MjBalanceNewNotify prsp)
        {
            MjBalanceNew info = new MjBalanceNew();
            info.showType = prsp.ShowType;
            info.gameType = prsp.nGameType;
            info.gameTypeSub = prsp.ConfigID;
            info.gameRulerType = prsp.nRulerType;
            info.deskID = prsp.DeskID;
            info.dealerSeatID = prsp.DealerSeatID;
            info.ownerSeatID = prsp.DeskMgrSeatID;
            info.isDraw = prsp.IsDraw;

            info.curBureau = prsp.nCurBureau;
            info.maxBureau = prsp.nMaxBureau;
            info.showTime = prsp.nTime;
            info.gameEndTime = prsp.TimeStamp;

            //playerinfo
            if (prsp.Info != null && prsp.Info.Count > 0)
            {
                for (int i = 0; i < prsp.Info.Count; i++)
                {
                    MjBalanceNew.BalancePlayerInfo playerInfo = GetBalanceNewPlayerInfo(prsp.Info[i]);
                    info.playerInfoList.Add(playerInfo);
                }
            }

            //马信息
            if (prsp.HorseInfo != null)
            {
                //奖马、抓码
                if (prsp.HorseInfo.JiangMa != null)
                {
                    SetBalanceDataBuyHorse(info, prsp.HorseInfo.JiangMa);

                }
                if (prsp.HorseInfo.ZhuaMa != null)
                {
                    SetBalanceDataBuyHorse(info, prsp.HorseInfo.ZhuaMa);
                }


                //四家买马 
                if (prsp.HorseInfo.SijiaMaiMa != null)
                {
                    info.SetHorseDataIni();
                    if (info.horseInfo != null)
                    {
                        if (prsp.HorseInfo.SijiaMaiMa.DataCell != null && prsp.HorseInfo.SijiaMaiMa.DataCell.Count > 0)
                        {
                            bool needForShow = true;
                            int gameType = prsp.HorseInfo.SijiaMaiMa.nRulerID;
                            info.horseInfo.buyHorseSiJiaData = new MjBalanceNew.MjHorseInfo.BuyHorseSiJiangNotifyData(gameType);
                            for (int i = 0; i < prsp.HorseInfo.SijiaMaiMa.DataCell.Count; i++)
                            {
                                int seatID = prsp.HorseInfo.SijiaMaiMa.DataCell[i].ComnonValue.SeatID;
                                List<int> hitSeatList = prsp.HorseInfo.SijiaMaiMa.DataCell[i].DataConfirm.nHitSeatID;
                                List<int> mjCodeList = prsp.HorseInfo.SijiaMaiMa.DataCell[i].DataConfirm.nMjCode;
                                List<int> mjTypeList = prsp.HorseInfo.SijiaMaiMa.DataCell[i].DataConfirm.nType;
                                List<int> mjScoreList = prsp.HorseInfo.SijiaMaiMa.DataCell[i].DataConfirm.nScore;

                                needForShow = needForShow &&
                                    info.horseInfo.buyHorseSiJiaData.SetDataItme(seatID, mjCodeList, mjTypeList, hitSeatList, mjScoreList);
                            }
                            if (needForShow)
                            {
                                info.horseInfo.buyHorseSiJiaData.SetDataForShow();
                            }
                        }
                    }


                }
            }


            return info;
        }

        /// <summary>
        /// 设置单人马结算数据 
        /// </summary>
        private void SetBalanceDataBuyHorse(MjBalanceNew info, Msg.MjBuyHorseNotify prsp)
        {
            info.SetHorseDataIni();

            if (info.horseInfo != null)
            {
                List<int> mjcodeList = new List<int>();
                List<int> getTypeList = new List<int>();

                if (prsp.Horse != null && prsp.Horse.Count > 0)
                {
                    for (int i = 0; i < prsp.Horse.Count; i++)
                    {
                        mjcodeList = prsp.Horse[i].nValue;
                        getTypeList = prsp.Horse[i].GetType;
                        info.horseInfo.SetHorseDataList(prsp.nType, mjcodeList, getTypeList);
                    }
                }

                List<int> scoreList = new List<int>();
                List<int> scoreSeatList = new List<int>();
                if (prsp.Score != null && prsp.Score.Count > 0)
                {
                    for (int i = 0; i < prsp.Score.Count; i++)
                    {
                        scoreList.Add(prsp.Score[i].Score);
                        scoreSeatList.Add(prsp.Score[i].SeatID);
                    }
                }
                info.horseInfo.SetHorseDataScoreList(prsp.nType, scoreSeatList, scoreList);
            }
        }



        private MjBalanceNew.BalancePlayerInfo GetBalanceNewPlayerInfo(MjBalancePlayerInfo originalInfo)
        {
            MjBalanceNew.BalancePlayerInfo info = new MjBalanceNew.BalancePlayerInfo();
            info.userID = originalInfo.UserID;
            info.userSeat = originalInfo.SeatID;
            info.userNick = originalInfo.Name;
            info.userHead = originalInfo.HeadUrl;
            info.scoreAmount = originalInfo.nScore;
            info.scoreCur = originalInfo.nBureauScore;

            info.handRecord = this.GetBestMjRecord(originalInfo.MjRecords);
            info.huList = originalInfo.nHuCode;

            info.huNum = originalInfo.nZiMo;
            info.paoNum = originalInfo.nDianPao;
            info.isChengbao = originalInfo.bChengbao;

            //详细信息人的的分总计
            List<Msg.MjDetaildedScore> originalDetailList = originalInfo.DetailScoreList;
            List<MjDetaildedScore> detailList = new List<MjDetaildedScore>();
            List<MjDetaildedScore> detailCutList = new List<MjDetaildedScore>();
            if (originalDetailList != null && originalDetailList.Count > 0)
            {
                for (int i = 0; i < originalDetailList.Count; i++)
                {
                    Msg.MjDetaildedScore original = originalDetailList[i];
                    List<MjDetaildedSpecial> specialList = new List<MjDetaildedSpecial>();
                    if (original.SpType != null && original.SpType.Count > 0)
                    {
                        for (int j = 0; j < original.SpType.Count; j++)
                        {
                            MjDetaildedSpecial specialItem = new MjDetaildedSpecial();
                            specialItem.specialType = original.SpType[j].Type;
                            specialItem.value = original.SpType[j].Value;
                            specialList.Add(specialItem);
                        }
                    }

                    MjDetaildedScore detail = new MjDetaildedScore(original.Score, original.ScoreType, original.SeatID,
                        original.HuType, original.PaiType, specialList);

                    if (original.Score >= 0)
                    {
                        detailList.Add(detail);
                    }
                    else
                    {
                        detailCutList.Add(detail);
                    }

                }
            }

            info.detailList = detailList;
            info.detailListCut = detailCutList;

            //checkshow
            List<MjBalanceNew.MjCheckShow> checkList = new List<MjBalanceNew.MjCheckShow>();
            if (originalInfo.nCheck != null && originalInfo.nCheck.Count > 0)
            {
                for (int i = 0; i < originalInfo.nCheck.Count; i++)
                {
                    MjBalanceNew.MjCheckShow checkItem = new MjBalanceNew.MjCheckShow();
                    checkItem.checkType = (EnumMjSpecialCheck)originalInfo.nCheck[i].nCheckType;
                    checkItem.checkValue = originalInfo.nCheck[i].nValue;
                    checkList.Add(checkItem);
                }
            }

            info.checkShowList = checkList;

            return info;
        }



        //总结算
        public void MjGameOverNotify(object rsp)
        {
            var prsp = rsp as MjGameOverNotify;

            List<MjBureauDetialInfo> bureauList = this.GetMjBureauDetialInfo(prsp.BureauInfo);

            int gameType = prsp.GameType;
            int oddsCount = prsp.OddsCount;
            int showType = prsp.ShowType;

            List<Msg.MjTitleInfo> originalTitleList = prsp.TitleInfo;
            List<MjTitleInfo> titleInfoList = GetMjTitleInfoList(originalTitleList);
            int gameSubType = prsp.ConfigID;
            int createTime = prsp.RecordTime;

            bool isShow = prsp.IsShow;
            long ownerID = prsp.DeskMgrID;

            List<GameResultCostData> resultCostList = GetResultCostDataList(prsp.URInfo);
            int reusltType = prsp.DeskType;

            MjDataManager.Instance.SetGameOverResultData(isShow, gameSubType, gameType, oddsCount, showType, bureauList, titleInfoList, createTime, ownerID, resultCostList, reusltType);
        }

        private void MjWinnerCostCardInfoRsp(object rsp)
        {
            MjWinnerCostCardInfoRsp prsp = rsp as MjWinnerCostCardInfoRsp;
            int costNum = prsp.CostTicket;
            int restNum = prsp.OddTicket;
            bool isLast = prsp.IsLastUser;

            MjDataManager.Instance.SetCostData(costNum, restNum, isLast);
        }


        private List<GameResultCostData> GetResultCostDataList(List<MjUserCostInfo> prsp)
        {
            List<GameResultCostData> costList = new List<GameResultCostData>();
            if (prsp != null && prsp.Count > 0)
            {
                for (int i = 0; i < prsp.Count; i++)
                {
                    GameResultCostData item = new GameResultCostData();
                    item.seatID = prsp[i].SeatId;
                    item.costCardNum = prsp[i].CostTicket;
                    item.joinRoomTime = prsp[i].JoinRoomTime;

                    costList.Add(item);
                }
            }


            return costList;
        }




        //麻将房间行为通知
        public void MjDeskActionNotify(object rsp)
        {
            var prsp = rsp as MjDeskActionNotify;
            EnumMjDeskAction roomAction = (EnumMjDeskAction)((int)prsp.DeskAction);
            int seatID = prsp.SeatID;
            ulong userID = prsp.UserID;
            int closeTime = prsp.nCloseTime;
            int gamestate = prsp.nGameState;

            //MahjongLogicNew.Instance.MjDeskActionNotify(roomAction, seatID, userID);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjDeskActionNotify, roomAction, seatID, userID, closeTime, gamestate);
        }

        //麻将房间内行为回执
        public void MjDeskActionRsp(object rsp)
        {
            var prsp = rsp as MjDeskActionRsp;
            int resultCode = prsp.ResultCode;
            ulong userID = prsp.UserID;

            //MahjongLogicNew.Instance.MjDeskActionRsp(userID, resultCode);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjDeskActionRsp, userID, resultCode);
        }


        //麻将发牌通知
        public void MjSyncPlayerStateNotify(object rsp)
        {
            var prsp = rsp as MjSyncPlayerStateNotify;
            int mjCode = prsp.MjCode;
            int mjRulerResult = prsp.MjRulerResult;
            int seatID = prsp.SeatID;
            List<MjTingInfo> tingInfoList = MjGetTingInfoList(prsp.TingList);
            int getType = (int)prsp.nGetType;
            List<int> gangList = prsp.GangList;
            bool canFangMao = prsp.IsHaveMao;

            List<MjRoundGetBuPai> mjChangeFlower = null;
            if (prsp.BuPaiList != null)
            {
                List<Msg.MjRoundGetBuPai> changeFlower = prsp.BuPaiList.Round;
                if (changeFlower != null && changeFlower.Count > 0)
                {
                    mjChangeFlower = new List<MjRoundGetBuPai>();
                    for (int i = 0; i < changeFlower.Count; i++)
                    {
                        MjRoundGetBuPai round = new MjRoundGetBuPai(changeFlower[i].nPut, changeFlower[i].nValue, changeFlower[i].nType);
                        mjChangeFlower.Add(round);
                    }
                }
            }

            MjDataManager.Instance.Base_RulerLimit(prsp.nPlayType);  //阻塞玩家操作选项
            List<int> ciList = prsp.CiList;
            MJSetPaikouChangeList(prsp.ChangeList);

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjSyncPlayerStateNotify, mjCode, seatID, mjRulerResult,
                tingInfoList, gangList, getType, canFangMao, mjChangeFlower, ciList);
        }





        //庄家的第一轮开始的时候的消息
        public void MjDealerBeginNotify(object rsp)
        {
            var prsp = rsp as MjDealerBeginNotify;
            int mjRuler = prsp.MjRulerResult;
            int seatID = prsp.SeatID;
            ulong userID = prsp.UserID;
            List<MjTingInfo> tingInfoList = MjGetTingInfoList(prsp.TingList);
            List<int> gangList = prsp.GangList;
            bool canFangMao = prsp.IsHaveMao;

            List<MjRoundGetBuPai> mjChangeFlower = null;
            if (prsp.BuPaiList != null)
            {
                List<Msg.MjRoundGetBuPai> changeFlower = prsp.BuPaiList.Round;
                if (changeFlower != null && changeFlower.Count > 0)
                {
                    mjChangeFlower = new List<MjRoundGetBuPai>();
                    for (int i = 0; i < changeFlower.Count; i++)
                    {
                        MjRoundGetBuPai round = new MjRoundGetBuPai(changeFlower[i].nPut, changeFlower[i].nValue, changeFlower[i].nType);
                        mjChangeFlower.Add(round);
                    }
                }
            }

            MjDataManager.Instance.Base_RulerLimit(prsp.nPlayType);//阻塞玩家操作选项
            List<int> ciList = prsp.CiList;
            MJSetPaikouChangeList(prsp.ChangeList);


            //MahjongLogicNew.Instance.MjDealerBeginNotify(seatID, mjRuler, userID, tingInfoList);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjDealerBeginNotify, seatID, mjRuler, userID, tingInfoList, gangList, canFangMao, mjChangeFlower, ciList);
        }


        //游戏开始前的一些状态通知 
        public void MjBeforeDispatch(object rsp)
        {
            var prsp = rsp as MjBeforeDispatch;
            List<bool> checkValue = prsp.CheckValue;

            //MahjongLogicNew.Instance.MjBeforeDispatch(hasXiaPao, hasGangDaiPao, hasGaozhuang, hasLisi, changeFlower);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjBeforeDispatch, checkValue);
        }


        //在立牌阶段需要进行的一些操作（混，金，次）
        public void MjStandingPlates(object rsp)
        {
            var prsp = rsp as MjStandingPlates;

            MjStandingPlateData standingData = MjGetStandingData(prsp);
            if (standingData != null)
            {
                MjDataManager.Instance.SetProcessSpecial(standingData, true);
            }
        }



        #region 换三张

        //换三张

        //通知客户端进行换三张 
        public void MjAskReqChangeThree(object rsp)
        {
            var prsp = rsp as MjAskReqChangeThree;
            List<int> mjList = prsp.nMjCode;            //服务器推荐的三张 
            int clockType = (int)prsp.nType;            //本次换牌的方式 

            //MahjongLogicNew.Instance.MjAskReqChangeThree(mjList, clockType);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjAskReqChangeThree, mjList, clockType);
        }

        //个人换三张的结果
        public void MjRspChangeThree(object rsp)
        {
            var prsp = rsp as MjRspChangeThree;

            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                int deskID = prsp.DeskID;
                int seatID = prsp.SeatID;
                List<int> mjList = prsp.nMjCode;            //换出去的三张 

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjRspChangeThree, deskID, seatID, mjList);
            }

        }


        //所有人换三张结果通知
        public void MjChangeThreeNotify(object rsp)
        {
            var prsp = rsp as MjChangeThreeNotify;
            int deskID = prsp.DeskID;
            int seatID = prsp.SeatID;
            int clockType = (int)prsp.nType;
            List<int> mjList = prsp.nMjCode;            //换到的三张 
            Dictionary<int, MjChangeThreeData> changeDataDic = new Dictionary<int, MjChangeThreeData>();

            if (FakeReplayManager.Instance.ReplayState)
            {
                changeDataDic = FakeReplayManager.Instance.GetChangeThreeCardCache();
            }
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjChangeThreeNotify, deskID, seatID, clockType, mjList, changeDataDic);
        }




        #endregion

        #region 定缺

        //定缺
        //通知客户端进行定缺 
        public void MjAskReqConfirm(object rsp)
        {
            var prsp = rsp as MjAskReqConfirm;
            int confirmType = (int)prsp.nDefaultType;       //服务器默认定缺的类型

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjAskReqConfirm, confirmType);
        }
        //个人定缺的结果
        public void MjRspConfirm(object rsp)
        {
            var prsp = rsp as MjRspConfirm;
            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                int deskID = prsp.DeskID;
                int seatID = prsp.SeatID;
                int nType = (int)prsp.nType;

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjRspConfirm, deskID, seatID, nType);
            }
        }
        //所有人定缺结果
        public void MjConfirmNotify(object rsp)
        {
            var prsp = rsp as MjConfirmNotify;
            int deskID = prsp.DeskID;
            List<int> seatList = prsp.SeatID;
            List<int> confirmType = new List<int>();
            if (prsp.nType == null || prsp.nType.Count < 3)
            {
                QLoger.ERROR("其他玩家的定缺数量不对");
                return;
            }
            for (int i = 0; i < prsp.nType.Count; i++)
            {
                confirmType.Add((int)prsp.nType[i]);
            }

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjConfirmNotify, deskID, seatList, confirmType);
        }



        #endregion

        #region 下跑
        //下跑
        //通知客户端进行下跑
        public void MjAskReqPao(object rsp)
        {
            var prsp = rsp as MjAskReqPao;
            int defaultValue = prsp.nDefaultValue;      //默认下多少
            List<int> value = prsp.nValue;              //总共有几种下的数可以选

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjAskReqPao, defaultValue, value);
        }

        //客户端下跑的结果
        public void MjRspPao(object rsp)
        {
            var prsp = rsp as MjRspPao;
            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                int deskID = prsp.DeskID;
                int seatID = prsp.SeatID;
                int nvalue = prsp.nValue;

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjRspPao, deskID, seatID, nvalue);
            }
        }

        //所有人下跑的结果
        public void MjPaoNotify(object rsp)
        {
            var prsp = rsp as MjPaoNotify;
            int deskID = prsp.DeskID;
            if (prsp.SeatID == null || prsp.SeatID.Count < 3)
            {
                QLoger.ERROR("玩家座位号数目不对");
                return;
            }
            List<int> seatIDlist = prsp.SeatID;
            if (prsp.nValue == null || prsp.nValue.Count < 3)
            {
                QLoger.ERROR("玩家下跑数目不对");
                return;
            }
            List<int> valueList = prsp.nValue;

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjPaoNotify, deskID, seatIDlist, valueList);
        }

        #endregion

        #region 下鱼
        //ModelNetWorker.Regiest<MjAskReq258TiaoYu>(MjAskReq258TiaoYu);
        //    ModelNetWorker.Regiest<MjRsp258TiaoYu>(MjRsp258TiaoYu);
        //    ModelNetWorker.Regiest<Mj258TiaoYuNotify>(Mj258TiaoYuNotify);


        private void MjAskReq258TiaoYu(object rsp)
        {
            var prsp = rsp as MjAskReq258TiaoYu;
            int defaultValue = prsp.nDefaultValue;                                      //默认下多少
            List<int> value = prsp.nValue;                                              //总共有几种下的数可以选

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjServerYu, EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect, defaultValue, value);
        }

        private void MjRsp258TiaoYu(object rsp)
        {
            var prsp = rsp as MjRsp258TiaoYu;
            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                int seatID = prsp.SeatID;
                int nvalue = prsp.nValue;

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjServerYu, EnumMjSelectSubType.MjSelectSubType_WAIT_Select, seatID, nvalue);
            }
        }

        private void Mj258TiaoYuNotify(object rsp)
        {
            var prsp = rsp as Mj258TiaoYuNotify;
            List<int> seatIDlist = prsp.SeatID;
            List<int> valueList = prsp.nValue;

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjServerYu, EnumMjSelectSubType.MjSelectSubType_RESULT_Select, seatIDlist, valueList);
        }

        #endregion


        #region  坎牌 闹庄 末留
        public void MjNaoZhuangMoLiuKanPaiNotify(object rsp)
        {
            var prsp = rsp as MjNaoZhuangMoLiuKanPaiNotify;

            MjNaoZhuangMoLiuKanPaiSetData(prsp, true);
        }


        private bool MjNaoZhuangMoLiuKanPaiSetData(MjNaoZhuangMoLiuKanPaiNotify prsp, bool needSend)
        {
            int nSubType = prsp.nSelectSubType;
            bool countainOne = false;
            if (nSubType != 0)
            {
                //naozhuang
                bool isStart = nSubType == 1;
                if (prsp.NaoZhuangInfo != null && prsp.NaoZhuangInfo.IsExist)
                {
                    countainOne = true;
                    MjNaoZhuangData(prsp.NaoZhuangInfo, isStart);
                }

                //kanpai
                if (prsp.KanPaiInfo != null && prsp.KanPaiInfo.IsExist)
                {
                    countainOne = true;
                    MjKanPaiData(prsp.KanPaiInfo, isStart);
                }

                //moliu
                if (prsp.MoLiuInfo != null && prsp.MoLiuInfo.IsExist)
                {
                    countainOne = true;
                    MjMoLiuData(prsp.MoLiuInfo, isStart);
                }

                if (!needSend && countainOne)
                {
                    MjDataManager.Instance.MjData.ProcessData.processReconned.kannaomoState = nSubType;
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjKanNaoMoNotify, nSubType, needSend);
            }

            return countainOne;
        }



        private void MjNaoZhuangData(Msg.MjParseNaoZhuangInfo originalInfo, bool isStart)
        {
            MahjongPlayType.MjCanNaoMoServerData.EnumMjNaozhuangType naoType = (MahjongPlayType.MjCanNaoMoServerData.EnumMjNaozhuangType)originalInfo.NaoType;

            bool naoDefault = false;
            if (naoType == MahjongPlayType.MjCanNaoMoServerData.EnumMjNaozhuangType.NaoType_XianNone)
            {
                //闲家都不闹
                naoDefault = true;
            }

            if (originalInfo.DataCell != null && originalInfo.DataCell.Count > 0)
            {
                List<MahjongPlayType.MjCanNaoMoServerData> dataList = new List<MahjongPlayType.MjCanNaoMoServerData>();
                for (int i = 0; i < originalInfo.DataCell.Count; i++)
                {
                    MahjongPlayType.MjCanNaoMoServerData itemData = new MahjongPlayType.MjCanNaoMoServerData();
                    itemData.curType = MahjongPlayType.MjCanNaoMoData.EnumCommonType.NaoZhuang;
                    itemData.seatID = originalInfo.DataCell[i].ComnonValue.SeatID;
                    itemData.curState = isStart ? originalInfo.DataCell[i].DataConfirm.bDefaultValue : originalInfo.DataCell[i].DataConfirm.bValue;
                    if (naoDefault)
                    {
                        itemData.curState = false;
                    }
                    itemData.valueList = originalInfo.DataCell[i].DataConfirm.nMjCode;
                    itemData.chooseDefault = naoDefault;
                    itemData.haveChoose = originalInfo.DataCell[i].ComnonValue.bOpSet;

                    dataList.Add(itemData);
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjKanNaoMoData, dataList);
            }

        }

        private void MjKanPaiData(Msg.MjParseKanPaiInfo originalInfo, bool isStart)
        {
            if (originalInfo.DataCell != null && originalInfo.DataCell.Count > 0)
            {
                List<MahjongPlayType.MjCanNaoMoServerData> dataList = new List<MahjongPlayType.MjCanNaoMoServerData>();
                for (int i = 0; i < originalInfo.DataCell.Count; i++)
                {

                    MahjongPlayType.MjCanNaoMoServerData itemData = new MahjongPlayType.MjCanNaoMoServerData();
                    itemData.curType = MahjongPlayType.MjCanNaoMoData.EnumCommonType.CanPai;
                    itemData.seatID = originalInfo.DataCell[i].ComnonValue.SeatID;
                    itemData.curState = isStart ? originalInfo.DataCell[i].DataConfirm.bDefaultValue : originalInfo.DataCell[i].DataConfirm.bValue;
                    itemData.valueList = originalInfo.DataCell[i].DataConfirm.nMjCode;
                    itemData.haveChoose = originalInfo.DataCell[i].ComnonValue.bOpSet;

                    dataList.Add(itemData);
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjKanNaoMoData, dataList);
            }


        }

        private void MjMoLiuData(Msg.MjParseMoLiuInfo originalInfo, bool isStart)
        {
            if (originalInfo.DataCell != null && originalInfo.DataCell.Count > 0)
            {
                List<MahjongPlayType.MjCanNaoMoServerData> dataList = new List<MahjongPlayType.MjCanNaoMoServerData>();
                for (int i = 0; i < originalInfo.DataCell.Count; i++)
                {

                    MahjongPlayType.MjCanNaoMoServerData itemData = new MahjongPlayType.MjCanNaoMoServerData();
                    itemData.curType = MahjongPlayType.MjCanNaoMoData.EnumCommonType.MoLiu;
                    itemData.seatID = originalInfo.DataCell[i].ComnonValue.SeatID;
                    itemData.curState = isStart ? originalInfo.DataCell[i].DataConfirm.bDefaultValue : originalInfo.DataCell[i].DataConfirm.bValue;
                    itemData.valueList = originalInfo.DataCell[i].DataConfirm.nMjCode;
                    itemData.haveChoose = originalInfo.DataCell[i].ComnonValue.bOpSet;

                    dataList.Add(itemData);
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjKanNaoMoData, dataList);
            }
        }


        public void MjRspNaoZhuangMoLiuKanPai(object rsp)
        {
            var prsp = rsp as MjRspNaoZhuangMoLiuKanPai;
            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                // success
                int seatID = prsp.SeatID;
                bool kanState = prsp.IsKanPai;
                bool moState = prsp.IsMoLiu;
                bool naoState = prsp.IsNaoZhuang;

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjKanNaoMoRsp, seatID, kanState, naoState, moState);
            }

        }

        #endregion


        #region 选飘

        public void MjPiaoJinPaioSuNotify(object rsp)
        {
            MjPiaoJinPaioSuNotify prsp = rsp as MjPiaoJinPaioSuNotify;

            MJSetXuanPiaoData(prsp, true);
        }


        private void MJSetXuanPiaoData(MjPiaoJinPaioSuNotify prsp, bool sendLogic)
        {
            if (sendLogic && false)//CheckIsMingDaNotify(prsp))
            {
                MJMingDaNotify(prsp, sendLogic);
            }
            else
            {
                MJXuanPiaoNotify(prsp, sendLogic);
            }
        }

        private bool CheckIsMingDaNotify(MjPiaoJinPaioSuNotify prsp)
        {
            bool result = false;

            for (int i = 0; i < prsp.PiaoList.Count; i++)
            {
                if (!result)
                    result = MjDataManager.Instance.CheckHaveMingDa(prsp.PiaoList[i].RulerSetist.nRulerID);

                if (result)
                    break;
            }

            return result;
        }

        private void MJXuanPiaoNotify(MjPiaoJinPaioSuNotify prsp, bool sendLogic)
        {
            int subType = prsp.nSelectSubType;

            if (subType != 0)
            {
                bool isStart = subType == 1;
                if (prsp.PiaoList != null && prsp.PiaoList.Count > 0)
                {
                    List<MahjongPlayType.MjXuanPiaoServerData> serverDataList = new List<MahjongPlayType.MjXuanPiaoServerData>();

                    for (int i = 0; i < prsp.PiaoList.Count; i++)
                    {
                        MjParsePiaoInfo oriInfo = prsp.PiaoList[i];
                        if (oriInfo.IsExist)
                        {
                            MahjongPlayType.MjXuanPiaoServerData serverData = new MahjongPlayType.MjXuanPiaoServerData();
                            //set commondata
                            serverData.SetCommonData(oriInfo.RulerSetist.nRulerID, oriInfo.RulerSetist.nRulerSeletShowType
                                , oriInfo.RulerSetist.nValue);

                            //set playerData
                            if (oriInfo.DataCell != null && oriInfo.DataCell.Count > 0)
                            {
                                for (int j = 0; j < oriInfo.DataCell.Count; j++)
                                {
                                    int seatID = oriInfo.DataCell[j].ComnonValue.SeatID;
                                    int curValue = isStart ? oriInfo.DataCell[j].DataConfirm.nDefaultValue :
                                        oriInfo.DataCell[j].DataConfirm.nValue;
                                    bool haveChoose = oriInfo.DataCell[j].ComnonValue.bOpSet;
                                    serverData.AddPlayerData(seatID, curValue, haveChoose);
                                }
                            }

                            serverDataList.Add(serverData);
                        }

                    }

                    EventDispatcher.FireEvent(GEnum.NamedEvent.EMjXuanPiaoDataNotify, subType, serverDataList);

                    if (sendLogic)
                    {
                        EventDispatcher.FireEvent(GEnum.NamedEvent.EMjXuanpiaoNotify, subType);
                    }
                    else
                    {
                        MjDataManager.Instance.MjData.ProcessData.processReconned.xuanPiaoState = subType;
                    }

                }
            }
        }

        private void MJMingDaNotify(MjPiaoJinPaioSuNotify prsp, bool sendLogic)
        {
            int subType = prsp.nSelectSubType;

            if (subType != 0)
            {
                bool isStart = subType == 1;
                if (prsp.PiaoList != null && prsp.PiaoList.Count > 0)
                {
                    List<MahjongPlayType.MjMingDaServerData> serverDataList = new List<MahjongPlayType.MjMingDaServerData>();

                    for (int i = 0; i < prsp.PiaoList.Count; i++)
                    {
                        MjParsePiaoInfo oriInfo = prsp.PiaoList[i];
                        if (oriInfo.IsExist)
                        {
                            MahjongPlayType.MjMingDaServerData serverData = new MahjongPlayType.MjMingDaServerData();
                            //set commondata
                            serverData.SetCommonData(oriInfo.RulerSetist.nRulerID, oriInfo.RulerSetist.nRulerSeletShowType
                                , oriInfo.RulerSetist.nValue);

                            //set playerData
                            if (oriInfo.DataCell != null && oriInfo.DataCell.Count > 0)
                            {
                                for (int j = 0; j < oriInfo.DataCell.Count; j++)
                                {
                                    int seatID = oriInfo.DataCell[j].ComnonValue.SeatID;
                                    MjParsePiaoData ppData = oriInfo.DataCell[j].DataConfirm;
                                    int curValue = 0;
                                    if (ppData != null)
                                        curValue = isStart ? ppData.nDefaultValue : ppData.nValue; ;

                                    bool haveChoose = oriInfo.DataCell[j].ComnonValue.bOpSet;
                                    serverData.AddPlayerData(seatID, curValue, haveChoose);
                                }
                            }

                            serverDataList.Add(serverData);
                        }
                    }

                    MjDataManager.Instance.SetAllMingDaData(subType, serverDataList);

                    if (sendLogic)
                    {
                        EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_LogicNotify.ToString(), subType);
                        EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_RspNotify.ToString(), subType);
                    }
                    else
                    {
                        MjDataManager.Instance.isOutMingDa = true;
                    }
                }
            }
        }

        public void MjRspGameingRulerSet(object rsp)
        {
            if (false)//CheckIsMingDaRsp(rsp))
            {
                MJMingDaRsp(rsp);
            }
            else
            {
                MJXuanPiaoRsp(rsp);
            }
        }

        private bool CheckIsMingDaRsp(object prsp)
        {
            MjRspGameingRulerSet rsp = prsp as MjRspGameingRulerSet;
            bool result = false;

            for (int i = 0; i < rsp.SelectData.Count; i++)
            {
                if (!result)
                    result = MjDataManager.Instance.CheckHaveMingDa(rsp.SelectData[i].nRulerID);

                if (result)
                    break;
            }

            return result;
        }

        private void MJXuanPiaoRsp(object rsp)
        {
            MjRspGameingRulerSet prsp = rsp as MjRspGameingRulerSet;
            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                int seatID = prsp.SeatID;
                List<int> rulerID = new List<int>();
                List<int> valueLis = new List<int>();

                if (prsp.SelectData != null && prsp.SelectData.Count > 0)
                {
                    for (int i = 0; i < prsp.SelectData.Count; i++)
                    {
                        rulerID.Add(prsp.SelectData[i].nRulerID);
                        valueLis.Add(prsp.SelectData[i].nValue);
                    }
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjXuanPiaoDataNotify, 2, seatID, rulerID, valueLis);
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjXuanpiaoNotify, 2, seatID);
            }
        }

        private void MJMingDaRsp(object rsp)
        {
            MjRspGameingRulerSet prsp = rsp as MjRspGameingRulerSet;
            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                int seatID = prsp.SeatID;
                List<int> rulerID = new List<int>();
                List<int> valueLis = new List<int>();

                if (prsp.SelectData != null && prsp.SelectData.Count > 0)
                {
                    for (int i = 0; i < prsp.SelectData.Count; i++)
                    {
                        rulerID.Add(prsp.SelectData[i].nRulerID);
                        valueLis.Add(prsp.SelectData[i].nValue);
                    }
                }

                EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_LogicNotify.ToString(), 2, seatID, rulerID, valueLis);
                EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_RspNotify.ToString(), 2, seatID);
            }
        }

        #endregion


        public void MjObligate(object rsp)
        {
            var prsp = rsp as MjObligate;
            int offset = prsp.Offset;
            int nMjCode = prsp.nMjCode;

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjObligate, offset, nMjCode);
        }



        //通知客户端进行买马 
        public void MjBuyHorseCountNotify(object rsp)
        {
            var prsp = rsp as MjBuyHorseCountNotify;

            List<int> maNumList = prsp.HorseCount;

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjBuyHorseCountNotify, maNumList);
        }


        //通知买马结果（可以没有买的过程）(扎马，一码全中，抓鸟)
        public void MjBuyHorseNotify(object rsp)
        {
            MjJiangMaNotify(rsp);
            return;

            var prsp = rsp as MjBuyHorseNotify;

            int maType = prsp.nType;
            List<Msg.MjHorse> horseResult = prsp.Horse;
            List<MjHorse> mjHorseResult = new List<MjHorse>();
            if (horseResult != null && horseResult.Count > 0)
            {
                for (int i = 0; i < horseResult.Count; i++)
                {
                    MjHorse horseData = new MjHorse(horseResult[i].SeatID, horseResult[i].nValue, horseResult[i].GetType);
                    mjHorseResult.Add(horseData);
                }
            }
            List<Msg.MjScore> scoreResult = prsp.Score;
            List<MjScore> mjScoreResult = GetEasyScoreList(scoreResult);


            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjOpenMaNotify, maType, mjHorseResult, mjScoreResult);
        }


        //表演
        public void MjBiaoYanNotify(object rsp)
        {
            var prsp = rsp as MjBiaoYanNotify;
            List<Msg.MjBiaoYanData> originalList = prsp.BYData;
            if (originalList != null && originalList.Count > 0)
            {
                List<int> seatList = new List<int>();
                List<int> valueList = new List<int>();
                for (int i = 0; i < originalList.Count; i++)
                {
                    int seatID = originalList[i].SeatID;
                    int value = originalList[i].nValue;

                    seatList.Add(seatID);
                    valueList.Add(value);
                }

                bool isPut = prsp.IsPut;
                int curSeatID = prsp.SeatID;
                bool isHunDiao = prsp.IsKeBiaoYan;
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjPerformanceNotify, seatList, valueList, isPut, curSeatID, isHunDiao);
            }


        }


        //通知补花
        public void MjGetFlowerNotify(object rsp)
        {
            var prsp = rsp as MjGetFlowerNotify;

            int seatID = prsp.SeatID;

            List<Msg.MjRoundGetBuPai> changeFlower = prsp.Round;
            List<MjRoundGetBuPai> mjChangeFlower = null;

            if (changeFlower != null && changeFlower.Count > 0)
            {
                mjChangeFlower = new List<MjRoundGetBuPai>();
                for (int i = 0; i < changeFlower.Count; i++)
                {
                    MjRoundGetBuPai round = new MjRoundGetBuPai(changeFlower[i].nPut, changeFlower[i].nValue, changeFlower[i].nType);
                    mjChangeFlower.Add(round);
                }
            }

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjGetFlowerNotify, seatID, mjChangeFlower);
        }


        //通知明楼
        public void MjGetMingLouNotify(object rsp)
        {
            var prsp = rsp as MjGetMingLouNotify;
            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                List<int> handList = prsp.nMjCode;
                int seatID = prsp.SeatID;

                //fire
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjMinglouNotify, seatID, handList);
            }
        }


        //下炮子
        public void MjAskReqPaoZi(object rsp)
        {
            var prsp = rsp as MjAskReqPaoZi;

            int defaultValue = prsp.nDefaultValue;      //默认下多少
            List<int> value = prsp.nValue;              //总共有几种下的数可以选

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjAskReqPaoZi, defaultValue, value);
        }

        public void MjRspPaoZi(object rsp)
        {
            var prsp = rsp as MjRspPaoZi;
            int resultCode = prsp.ResultCode;

            if (resultCode == 1)
            {
                int deskID = prsp.DeskID;
                int seatID = prsp.SeatID;
                int nvalue = prsp.nValue;

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjRspPaoZi, deskID, seatID, nvalue);
            }
        }


        public void MjPaoZiNotify(object rsp)
        {
            var prsp = rsp as MjPaoZiNotify;
            int deskID = prsp.DeskID;
            if (prsp.SeatID == null || prsp.SeatID.Count < 3)
            {
                QLoger.ERROR("玩家座位号数目不对");
                return;
            }

            List<int> seatIDlist = prsp.SeatID;
            if (prsp.nValue == null || prsp.nValue.Count < 3)
            {
                QLoger.ERROR("玩家下炮子数目不对");
                return;
            }

            List<int> valueList = prsp.nValue;

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjPaoNotifyZi, deskID, seatIDlist, valueList);
        }


        //桌上积分变化通知
        public void MjScoreChangeNotify(object rsp)
        {
            var prsp = rsp as MjScoreChangeNotify;

            bool isUpdate = prsp.IsUpdate;
            int showSeatID = prsp.ShowSeatID;
            List<Msg.MjScore> scoreList = prsp.scoreChange.score;
            if (scoreList != null && scoreList.Count > 0)
            {
                int showType = prsp.scoreChange.ShowType;
                List<MjScore> changeList = new List<MjScore>();
                for (int i = 0; i < scoreList.Count; i++)
                {
                    MjScore score = new MjScore(scoreList[i].SeatID, scoreList[i].Score);
                    changeList.Add(score);
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjScoreChangeNotify, changeList, showType, isUpdate, showSeatID);
            }

        }

        ////通知客户端是否产生了跟庄
        //public void MjFollowDealer(object rsp)
        //{
        //    //var prsp = rsp as MjFollowDealer;
        //    //bool isFollow = prsp.IsFollow;

        //    //EventDispatcher.FireEvent(GEnum.NamedEvent.EMjFollowDealer, isFollow);
        //}


        //客户端时间通知
        public void MjTimeNotify(object rsp)
        {
            var prsp = rsp as MjTimeNotify;
            int showTime = prsp.Time;

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjTimeNotify, showTime);
        }

        //客户端在线离线 
        public void MjOnLineOffLine(object rsp)
        {
            var prsp = rsp as MjOnLineOffLine;

            int seatID = prsp.SeatID;
            bool onlineState = prsp.IsOnLine;

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjOnlineStateNotify, seatID, onlineState);
        }



        //重连
        public void MjDeskReconectRsp(object rsp)
        {
            var prsp = rsp as MjDeskReconectRsp;
            DebugPro.Log(DebugPro.EnumLog.MemoryData, "断线重连 MjDeskReconectRsp", CommonTools.ReflactionObject(prsp));
            //QLoger.LOG("[MjDeskReconectRsp] 接收消息 {0}", CommonTools.ReflactionObject(prsp));
            bool isInGame = prsp.nGameState != 0;
            if (prsp.IsIndesk)
            {

                //MARK-JEFF 预解析重连用户数据， 这里需要给开桌界面设置用户数据
                MahongDataModel.GameProcessReconned processData = new MahongDataModel.GameProcessReconned();
                //base
                processData.userID = prsp.UserID;
                processData.gameState = prsp.nGameState;
                processData.deskInfo = this.MjGetDeskInfo(prsp.DeskInfo);
                processData.playerInfoList = this.MjGetPlayerInfoList(prsp.Players, prsp.nGameState);
                // processData.playerIDList = this.MjGetPlayerIDList(prsp.Players);
                //before dispach
                processData.beforeDispatchValues = prsp.CheckValue;

                MjDataManager.Instance.SetReconnedData(processData);
                //END MARK-JEFF

                if (isInGame)
                {
                    //game start
                    if (prsp.GameStart != null)
                    {
                        processData.dealerSeat = prsp.GameStart.DealerID;
                        processData.rollNums = prsp.GameStart.RollNo;
                        processData.getMjSeat = prsp.GameStart.GetMjSeatID;
                        processData.getOffset = prsp.GameStart.GetOffset;
                        processData.allCount = prsp.GameStart.nMjAllCount;
                        processData.curRestCount = prsp.nMjRemainCount;
                        List<List<int>> maoList = null;
                        List<MjMaoGroup> originalGroup = prsp.GameStart.MaoGroup;
                        if (originalGroup != null && originalGroup.Count > 0)
                        {
                            maoList = new List<List<int>>();
                            for (int i = 0; i < originalGroup.Count; i++)
                            {
                                maoList.Add(originalGroup[i].MaoValue);
                            }
                        }
                        processData.maoGroup = maoList;


                        //change 
                        if (prsp.GameStart.ChangeList != null)
                        {
                            for (int i = 0; i < prsp.GameStart.ChangeList.Count; i++)
                            {
                                int cardID = prsp.GameStart.ChangeList[i].nMjCode;
                                int type = prsp.GameStart.ChangeList[i].Type;

                                MjDataManager.Instance.SetCurStartChange(cardID, type);
                            }
                        }
                    }


                    //standing Data
                    if (prsp.StandingPlates != null)
                    {
                        MjStandingPlateData data = this.MjGetStandingData(prsp.StandingPlates);
                        if (data != null)
                        {
                            processData.standingData.Add(data);
                        }
                    }


                    //special
                    if (prsp.TypeCheckValue != null && prsp.TypeCheckValue.Count > 0)
                    {
                        for (int i = 0; i < prsp.TypeCheckValue.Count; i++)
                        {
                            Msg.MjCheckValue originalCheck = prsp.TypeCheckValue[i];
                            MjReconnedCheck check = new MjReconnedCheck(originalCheck.nBaseType, originalCheck.nSubType, originalCheck.TypeValue);
                            processData.checkSpecial.Add(check);
                        }
                    }


                    //handelist
                    if (prsp.HandMjList != null && prsp.HandMjList.Count > 0)
                    {
                        for (int i = 0; i < prsp.HandMjList.Count; i++)
                        {
                            int seatID = i + 1;
                            processData.putCardArray[seatID] = prsp.HandMjList[i].HePaiList;
                            int tingIndex = prsp.HandMjList[i].TingIndex;
                            processData.putCloseIndexArray[seatID] = tingIndex > 0 ? tingIndex - 1 : -1;
                            processData.huCardArray[seatID] = prsp.HandMjList[i].HuCodeList;
                            processData.flowerArray[seatID] = prsp.HandMjList[i].BuPaiList;
                            processData.independentArray[seatID] = prsp.HandMjList[i].IndependentList;

                            List<MjPai> handePaiList = new List<MjPai>();
                            if (prsp.HandMjList[i].MjList != null && prsp.HandMjList[i].MjList.Count > 0)
                            {
                                for (int j = 0; j < prsp.HandMjList[i].MjList.Count; j++)
                                {
                                    Msg.MjPai originalPai = prsp.HandMjList[i].MjList[j];
                                    MjPai mjPai = new MjPai(originalPai.MjCode, originalPai.CodeType);
                                    mjPai.seatID = originalPai.nSeatID;
                                    handePaiList.Add(mjPai);
                                }
                            }
                            processData.handCardArray[seatID] = handePaiList;
                        }
                    }
                    processData.hasFlyOne = prsp.IsFeiYi;
                    processData.lastHandCard = prsp.LastMjCode;

                    //waite data
                    processData.waitType = prsp.WaitType;
                    processData.waitSeatID = prsp.nWaitSeatID;
                    processData.waitTime = prsp.nTime;
                    processData.waitLastPutSeatID = prsp.nLastSeatID;

                    //ruler info
                    processData.canFangMao = prsp.IsHaveMao;
                    processData.rulerResult = prsp.MjRulerResult;
                    processData.tingInfo = this.MjGetTingInfoList(prsp.TingList);
                    processData.chiList = prsp.ChiList;
                    processData.gangList = prsp.GangList;
                    processData.nMjCode = prsp.nCurCode;
                    processData.ciList = prsp.CiList;

                    //ting info
                    processData.tingSeatID = prsp.TingSeatIDList;
                    processData.confirmTingInfo = this.MjGetTingInfo(prsp.TingInfo);


                    //close
                    processData.isAskClose = prsp.IsAskClose;
                    processData.isAnswer = prsp.IsAnswer;
                    processData.startAskSeatID = prsp.nAskSeatID;
                    processData.isAgree = prsp.IsAgree;
                    processData.closeTime = prsp.nCloseTime;

                    //表演
                    processData.performData = GetPerformanceData(prsp.BiaoYan);

                    ///坎牌 闹庄 末留
                    SetReconnedKanNaoMo(prsp.NaoZhuang);

                    //明打下跑的
                    //SetReconnedMingDa(prsp.PreUniversalNotify);

                    //选飘
                    SetReconnedXuanPiao(prsp.PiaoJinPiaoSu);

                    //杠后拿吃
                    MjGangHouNaChiServerNotify(prsp.GangHouNaChi);
                    //明楼听口
                    processData.minglouTingList = SetReconnedMinglouTingkou(prsp.TingKouInfo);
                    //赌暗杠
                    DuAnGangNotify(prsp.DuAnGang);
                    //扭牌
                    NiuPaiNotify(prsp.NiuPaiInfo);
                    //扭牌历史数据
                    NiuPaiReconnect(prsp.NiuPaiRecInfo);

                    //亮一张
                    SetLiangyiData(prsp.LyzInfo, false);
                    //漂胡 跟漂
                    MjPiaoHuNotify(prsp.PiaoHuNotify);

                    //出门断
                    SetPutLimit(prsp.PutlimitInfo);

                    //亮杠头
                    SetLiangGangTou(prsp.LgtInfo);

                    //四家买马
                    ParserSiJiaMaiMaData(prsp.SjmmInfo, false);

                    //风圈
                    ParserFengQuanData(prsp.FengQuan, false);

                    MjDataManager.Instance.Base_RulerLimit(prsp.nPlayType);//阻塞玩家操作选项
                    MJSetPaikouChangeList(prsp.ChangeList);
                    if (prsp.RulerChange != null)
                    {
                        MJSetPaikouChangeList(prsp.RulerChange.ChangeList);
                    }


                    processData.showBaoci = SetMjTipInfoNotify(prsp.TipInfo, false);
                    MjLiangSiDaYiNotify(prsp.LsdyInfo);
                }
            }
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjReconned, prsp.IsIndesk, isInGame);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoginSouccessRsp, "MjDeskReconect");
        }

        private void SetReconnedMingDa(List<MjPiaoJinPaioSuNotify> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                MjPiaoJinPaioSuNotify prsp = list[i] as MjPiaoJinPaioSuNotify;

                if (CheckIsMingDaNotify(prsp))
                {
                    MJMingDaNotify(prsp, false);
                }
            }
        }

        private int SetReconnedKanNaoMo(Msg.MjNaoZhuangMoLiuKanPaiNotify prsp)
        {
            int countainNum = 0;
            if (prsp != null)
            {
                MjNaoZhuangMoLiuKanPaiSetData(prsp, false);
            }

            return countainNum;
        }

        private int SetReconnedXuanPiao(Msg.MjPiaoJinPaioSuNotify prsp)
        {
            int counTainNum = 0;
            if (prsp != null)
            {
                MJSetXuanPiaoData(prsp, false);
            }
            return counTainNum;
        }

        private MjPerformanceData GetPerformanceData(Msg.MjBiaoYanNotify prsp)
        {
            MjPerformanceData biaoyanData = null;
            if (prsp != null && prsp.BYData != null && prsp.BYData.Count > 0)
            {
                List<int> seatID = new List<int>();
                List<int> valueID = new List<int>();
                for (int i = 0; i < prsp.BYData.Count; i++)
                {
                    seatID.Add(prsp.BYData[i].SeatID);
                    valueID.Add(prsp.BYData[i].nValue);
                }
                biaoyanData = new MjPerformanceData(seatID, valueID, prsp.IsPut, prsp.SeatID);
            }

            return biaoyanData;
        }

        /// <summary>
        /// 大结算打开关闭的接收
        /// </summary>
        /// <param name="rsp"></param>
        public void GameResultBoardStateRsp(object rsp)
        {
            var prsp = rsp as GameResultBoardStateRsp;
        }


        private void MjRulerChangeNotify(object rsp)
        {
            var prsp = rsp as MjRulerChangeNotify;
            MJSetPaikouChangeList(prsp.ChangeList);
        }


        /// <summary>
        /// 亮四打一
        /// </summary>
        /// <param name="rsp"></param>
        private void MjLiangSiDaYiNotify(object rsp)
        {
            if (rsp != null)
            {
                MjLiangSiDaYiNotify prsp = rsp as MjLiangSiDaYiNotify;
                if (prsp.DataCell != null && prsp.DataCell.Count > 0)
                {
                    for (int i = 0; i < prsp.DataCell.Count; i++)
                    {
                        int seatID = prsp.DataCell[i].ComnonValue.SeatID;
                        bool havePut = prsp.DataCell[i].ComnonValue.bOpSet;
                        List<int> cardList = prsp.DataCell[i].DataConfirm.nMjCode;

                        MjDataManager.Instance.LSDY_SetData(seatID, cardList, havePut);
                    }
                }
            }
        }


        #endregion

        #region Send Message
        /// <summary>
        /// 请求新开房间
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roomInfo"></param>
        public void MjNewDeskReq(MjDeskInfo roomInfo, int roomId)
        {
            Msg.MjNewDeskReq newRoom = new Msg.MjNewDeskReq();
            newRoom.UserID = (ulong)MemoryData.UserID;
            newRoom.DeskInfo = MjGetProtoDeskInfo(roomInfo);
            newRoom.DeskAdvert = newRoom.DeskInfo.DeskAdvert;
            newRoom.RoomID = roomId;

            ModelNetWorker.Send(newRoom);
        }

        /// <summary>
        /// 请求加入房间
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roomID"></param>
        public void MjJoinDeskReq(ulong userID, int roomID)
        {
            if (!FakeReplayManager.Instance.ReplayState)
            {
                MemoryData.GameStateData.JoinDeskRoomId = 0;
                Msg.MjJoinDeskReq joinRoom = new Msg.MjJoinDeskReq();
                joinRoom.DeskID = roomID;
                joinRoom.UserID = userID;

                ModelNetWorker.Send(joinRoom);
            }
        }


        public void MjOpactionReq_FromEC2S(object[] obj)
        {
            ulong userID = (ulong)obj[0];
            int mjCode = (int)obj[1];
            EnumMjOpAction opCode = (EnumMjOpAction)obj[2];
            int seatID = (int)obj[3];
            int deskID = (int)obj[4];
            List<int> chiList = (List<int>)obj[5];
            bool isIndependent = (bool)obj[6];
            List<int> handList = (List<int>)obj[7];
            bool isLast = (bool)obj[8];


            MjOpActionReq(userID, mjCode, opCode, seatID, deskID, chiList, null, isIndependent, handList, isLast);
        }


        /// <summary>
        /// 发送游戏内行为
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="mjCode"></param>
        /// <param name="opCode"></param>
        /// <param name="seatID"></param>
        public void MjOpActionReq(ulong userID, int mjCode, EnumMjOpAction opCode, int seatID, int roomID, List<int> chiList = null,
            List<int> maoList = null, bool isIndependent = false, List<int> handList = null, bool isLast = false)
        {
            Msg.MjOpActionReq mjOpAction = new Msg.MjOpActionReq();
            mjOpAction.UserID = userID;
            mjOpAction.OpCode = (Msg.MjOpAction)((int)opCode);
            mjOpAction.SeatID = seatID;
            mjOpAction.MjCode = mjCode;
            mjOpAction.RoomID = roomID;
            mjOpAction.IsLastGet = isLast;
            if (chiList != null && chiList.Count > 0)
            {
                for (int i = 0; i < chiList.Count; i++)
                {
                    mjOpAction.ChiList.Add(chiList[i]);
                }
            }

            if (maoList != null && maoList.Count > 0)
            {
                for (int i = 0; i < maoList.Count; i++)
                {
                    mjOpAction.MaoList.Add(maoList[i]);
                }
            }
            mjOpAction.IsIndependent = isIndependent;
            if (handList != null && handList.Count > 0)
            {
                for (int i = 0; i < handList.Count; i++)
                {
                    mjOpAction.CodeList.Add(handList[i]);
                }
            }

            ModelNetWorker.Send(mjOpAction);
        }

        /// <summary>
        /// 发送补花消息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="seatID"></param>
        /// <param name="deskID"></param>
        /// <param name="cardID"></param>
        /// <param name="isDependent"></param>
        public void MjReqBuHuaZhang(long UserID, int seatID, int deskID, int cardID, bool isDependent)
        {
            MjReqBuHuaZhang req = new Msg.MjReqBuHuaZhang();
            req.UserID = UserID;
            req.DeskID = deskID;
            req.SeatID = seatID;
            req.nMjCode = cardID;
            req.IsIndependent = isDependent;
            Send(req);
        }


        /// <summary>
        /// 发送房间内行为
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="seatID"></param>
        /// <param name="actionCode"></param>
        public void MjDeskActionReq(ulong userID, int seatID, int roomID, EnumMjDeskAction actionCode)
        {
            Msg.MjDeskActionReq roomAction = new Msg.MjDeskActionReq();
            roomAction.UserID = userID;
            roomAction.SeatID = seatID;
            roomAction.DeskAction = (Msg.MjDeskAction)((int)actionCode);
            roomAction.DeskID = roomID;

            ModelNetWorker.Send(roomAction);
        }

        /// <summary>
        /// 发送换三张的请求
        /// </summary>
        /// <param name="deskID"></param>
        /// <param name="seatID"></param>
        /// <param name="changeList"></param>
        public void MjReqChangeThree(int deskID, int seatID, List<int> changeList)
        {
            if (changeList == null || changeList.Count < 3)
            {
                QLoger.ERROR("换三张的牌的数不符合要求 ");
                return;
            }

            Msg.MjReqChangeThree changeThree = new Msg.MjReqChangeThree();
            changeThree.DeskID = deskID;
            changeThree.SeatID = seatID;
            for (int i = 0; i < changeList.Count; i++)
            {
                changeThree.nMjCode.Add(changeList[i]);
            }

            ModelNetWorker.Send(changeThree);
        }

        /// <summary>
        /// 发送定缺请求
        /// </summary>
        /// <param name="deskID"></param>
        /// <param name="seatID"></param>
        /// <param name="nType"></param>
        public void MjReqConfirm(int deskID, int seatID, int nType)
        {
            Msg.MjReqConfirm confirm = new Msg.MjReqConfirm();
            confirm.DeskID = deskID;
            confirm.SeatID = seatID;
            confirm.nType = (Msg.ConfirmType)nType;

            ModelNetWorker.Send(confirm);
        }

        /// <summary>
        /// 发送下跑请求
        /// </summary>
        /// <param name="deskID"></param>
        /// <param name="seatID"></param>
        /// <param name="nValue"></param>
        public void MjReqPao(int deskID, int seatID, int nValue)
        {
            Msg.MjReqPao xiaPao = new Msg.MjReqPao();
            xiaPao.DeskID = deskID;
            xiaPao.SeatID = seatID;
            xiaPao.nValue = nValue;

            ModelNetWorker.Send(xiaPao);
        }


        /// <summary>
        /// 发送明楼请求
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="handList"></param>
        public void MjMingLouReq(int deskID, int seatID, List<int> handList)
        {
            Msg.MjGetMingLouReq minglou = new Msg.MjGetMingLouReq();
            minglou.SeatID = seatID;
            minglou.DeskID = deskID;
            if (handList != null && handList.Count > 0)
            {
                for (int i = 0; i < handList.Count; i++)
                {
                    minglou.nMjCode.Add(handList[i]);
                }
            }

            ModelNetWorker.Send(minglou);
        }


        /// <summary>
        /// 发送下炮子请求
        /// </summary>
        /// <param name="deskID"></param>
        /// <param name="seatID"></param>
        /// <param name="value"></param>
        public void MjReqPaoZi(int deskID, int seatID, int value)
        {
            MjReqPaoZi xiapaozi = new Msg.MjReqPaoZi();
            xiapaozi.DeskID = deskID;
            xiapaozi.SeatID = seatID;
            xiapaozi.nValue = value;

            ModelNetWorker.Send(xiapaozi);
        }

        /// <summary>
        /// 发送下鱼请求
        /// </summary>
        /// <param name="deskID"></param>
        /// <param name="seatID"></param>
        /// <param name="value"></param>
        public void MjReqXiayu(int deskID, int seatID, int value)
        {
            Msg.MjReq258TiaoYu xiayu = new Msg.MjReq258TiaoYu();
            xiayu.DeskID = deskID;
            xiayu.SeatID = seatID;
            xiayu.nValue = value;

            ModelNetWorker.Send(xiayu);
        }



        /// <summary>
        /// 牌桌断线重连的请求
        /// </summary>
        public void MjDeskReconectReq()
        {
            DebugPro.Log(DebugPro.EnumLog.MemoryData, "断线重连 MjDeskReconectReq");
            var req = new MjDeskReconectReq();
            req.UserID = MemoryData.UserID;
            ModelNetWorker.Send(req);
        }


        /// <summary>
        /// 麻将请求小结算数据
        /// </summary>
        public void MjBalanceReq(long userID, int deskID, int curBureau)
        {
            var req = new MjBalanceReq();
            req.DeskID = deskID;
            req.CurBouts = curBureau;
            req.UserID = userID;
            ModelNetWorker.Send(req);
        }

        /// <summary>
        /// 大结算打开和关闭调用
        /// </summary>
        /// <param name="isBigBalanceOpen"></param>
        public void GameResultBoardStateReq(bool isBoardOpen)
        {
            var req = new GameResultBoardStateReq();
            req.UserID = MemoryData.UserID;
            req.BoardOpen = isBoardOpen;
            ModelNetWorker.Send(req);
        }

        /// <summary>
        /// 发送坎牌 闹庄 末留 结果
        /// </summary>
        /// <param name="deskID"></param>
        /// <param name="seatID"></param>
        /// <param name="kanState"></param>
        /// <param name="moState"></param>
        /// <param name="naoState"></param>
        public void MjReqNaoZhuangMoLiuKanPai(int deskID, int seatID, bool kanState, bool moState, bool naoState)
        {
            var req = new MjReqNaoZhuangMoLiuKanPai();
            req.DeskID = deskID;
            req.SeatID = seatID;
            req.IsKanPai = kanState;
            req.IsNaoZhuang = naoState;
            req.IsMoLiu = moState;

            ModelNetWorker.Send(req);
        }

        /// <summary>
        /// 发送选飘界面数据
        /// </summary>
        public void MjReqGameingRulerSet(int deskID, int seatID, List<int> rulerIDList, List<int> valueList)
        {
            var req = new MjReqGameingRulerSet();
            req.SeatID = seatID;
            req.DeskID = deskID;

            if (rulerIDList != null && valueList != null && rulerIDList.Count == valueList.Count)
            {
                for (int i = 0; i < rulerIDList.Count; i++)
                {
                    MjRulerSetSelectData dataItem = new MjRulerSetSelectData();
                    dataItem.nRulerID = rulerIDList[i];
                    dataItem.nValue = valueList[i];
                    req.SelectData.Add(dataItem);
                }

            }

            ModelNetWorker.Send(req);
        }

        public void MjReqSidunSiding(int deskID, int seatID, bool state)
        {



            //send
        }

        #endregion

        #region Self func

        #region GetMjSpecialData


        #region Common
        public MjReconnedParseCommmon GetParseCommon(Msg.MjParseCommon prsp)
        {
            int seatID = prsp.SeatID;
            bool chooseSata = prsp.bOpSet;

            MjReconnedParseCommmon commonData = new MjReconnedParseCommmon(seatID, chooseSata);
            return commonData;
        }


        #endregion

        #region 下跑

        public MjReconnedParsePao GetParsePao(Msg.MjParsePaoData prsp)
        {
            MjReconnedParsePao parseData = new MjReconnedParsePao(prsp.nDefaultValue, prsp.nValue);
            return parseData;
        }


        public List<MjReconnedCellPao> GetCellPao(List<Msg.MjParsePaoCell> dataCell)
        {
            List<MjReconnedCellPao> cells = new List<MjReconnedCellPao>();

            if (dataCell != null && dataCell.Count > 0)
            {
                for (int i = 0; i < dataCell.Count; i++)
                {
                    MjReconnedCellPao cellInfo = new MjReconnedCellPao();
                    cellInfo.commonData = GetParseCommon(dataCell[i].ComnonValue);
                    cellInfo.parseData = GetParsePao(dataCell[i].DataPao);
                    cells.Add(cellInfo);
                }
            }

            return cells;
        }


        public MjReconnedInfoPao GetInfoPao(Msg.MjParsePaoInfo prsp)
        {
            int subType = prsp.nSelectSubType;
            List<int> values = prsp.nValue;

            List<MjReconnedCellPao> cells = GetCellPao(prsp.DataCell);

            MjReconnedInfoPao info = new MjReconnedInfoPao(subType, values);

            if (cells != null && cells.Count > 0)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    info.cellList.Add(cells[i]);
                }
            }

            return info;
        }

        #endregion

        #region 下炮

        public List<MjReconnedCellPaozi> GetCellPaozi(List<Msg.MjParsePaoZiCell> dataCell)
        {
            List<MjReconnedCellPaozi> cells = new List<MjReconnedCellPaozi>();

            if (dataCell != null && dataCell.Count > 0)
            {
                for (int i = 0; i < dataCell.Count; i++)
                {
                    MjReconnedCellPaozi cellInfo = new MjReconnedCellPaozi();
                    cellInfo.commonData = GetParseCommon(dataCell[i].ComnonValue);
                    cellInfo.parseData = GetParsePao(dataCell[i].DataPao);
                    cells.Add(cellInfo);
                }
            }

            return cells;
        }


        public MjReconnedInfoPaozi GetInfoPaozi(Msg.MjParsePaoZiInfo prsp)
        {
            int subType = prsp.nSelectSubType;
            List<int> values = prsp.nValue;

            List<MjReconnedCellPaozi> cells = GetCellPaozi(prsp.DataCell);

            MjReconnedInfoPaozi info = new MjReconnedInfoPaozi(subType, values);
            if (cells != null && cells.Count > 0)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    info.cellList.Add(cells[i]);
                }
            }


            return info;
        }
        #endregion

        #region 下鱼

        public List<MjReconnedCellXiayu> GetCellXiaYu(/*List<Msg.MjParsePaoZiCell> dataCell*/)
        {
            List<MjReconnedCellXiayu> cells = new List<MjReconnedCellXiayu>();

            //if (dataCell != null && dataCell.Count > 0)
            //{
            //    for (int i = 0; i < dataCell.Count; i++)
            //    {
            //        MjReconnedCellPaozi cellInfo = new MjReconnedCellPaozi();
            //        cellInfo.commonData = GetParseCommon(dataCell[i].ComnonValue);
            //        cellInfo.parseData = GetParsePao(dataCell[i].DataPao);
            //        cells.Add(cellInfo);
            //    }
            //}

            return cells;
        }


        //public MjReconnedInfoXiayu GetInfoXiaYu(/*Msg.MjParsePaoZiInfo prsp*/)
        //{
        //    int subType = prsp.nSelectSubType;
        //    List<int> values = prsp.nValue;

        //    List<MjReconnedCellPaozi> cells = GetCellPaozi(prsp.DataCell);

        //    MjReconnedInfoPaozi info = new MjReconnedInfoPaozi(subType, values);
        //    if (cells != null && cells.Count > 0)
        //    {
        //        for (int i = 0; i < cells.Count; i++)
        //        {
        //            info.cellList.Add(cells[i]);
        //        }
        //    }


        //    return info;
        //}

        #endregion

        #region 定缺
        public MjReconnedParseQue GetParseQue(Msg.MjParseConfirmData prsp)
        {
            MjReconnedParseQue queInfo = new MjReconnedParseQue((int)prsp.nDefaultType, (int)prsp.nValue);
            return queInfo;
        }


        public List<MjReconnedCellQue> GetCellQue(List<Msg.MjParseConfirmCell> dataCell)
        {
            List<MjReconnedCellQue> cells = new List<MjReconnedCellQue>();
            if (dataCell != null && dataCell.Count > 0)
            {
                for (int i = 0; i < dataCell.Count; i++)
                {
                    MjReconnedCellQue cellInfo = new MjReconnedCellQue();
                    cellInfo.commonData = GetParseCommon(dataCell[i].ComnonValue);
                    cellInfo.parseData = GetParseQue(dataCell[i].DataConfirm);
                    cells.Add(cellInfo);
                }
            }

            return cells;
        }



        public MjReconnedInfoQue GetInfoQue(Msg.MjParseConfirmInfo prsp)
        {
            int subType = prsp.nSelectSubType;

            MjReconnedInfoQue info = new MjReconnedInfoQue(subType);
            List<MjReconnedCellQue> cells = GetCellQue(prsp.DataCell);
            if (cells != null && cells.Count > 0)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    info.cellList.Add(cells[i]);
                }
            }

            return info;
        }

        #endregion


        #region 换三张


        public MjReconnedParseChangeThree GetParseChangeThree(MjParseChangeThreeData prsp)
        {
            MjReconnedParseChangeThree info = new MjReconnedParseChangeThree(prsp.nDefaultMjCode);
            return info;
        }

        public List<MjReconnedCellChangeThree> GetCellChangeThree(List<Msg.MjParseChangeThreeCell> dataCell)
        {
            List<MjReconnedCellChangeThree> cells = new List<MjReconnedCellChangeThree>();

            if (dataCell != null && dataCell.Count > 0)
            {
                for (int i = 0; i < dataCell.Count; i++)
                {
                    MjReconnedCellChangeThree cellInfo = new MjReconnedCellChangeThree();

                    cellInfo.commonData = GetParseCommon(dataCell[i].ComnonValue);
                    cellInfo.parseData = GetParseChangeThree(dataCell[i].DataConfirm);

                    cells.Add(cellInfo);
                }
            }

            return cells;
        }


        public MjReconnedInfoChangeThree GetInfoChangeThree(Msg.MjParseChangeThreeInfo prsp)
        {
            int subType = prsp.nSelectSubType;
            int clockType = (int)prsp.nType;

            MjReconnedInfoChangeThree info = new MjReconnedInfoChangeThree(subType, clockType);
            List<MjReconnedCellChangeThree> cells = GetCellChangeThree(prsp.DataCell);
            if (cells != null && cells.Count > 0)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    info.cellList.Add(cells[i]);
                }
            }

            return info;
        }

        #endregion


        #region 下鱼



        #endregion

        #endregion


        private void MJSetPaikouChangeList(List<MjRulerChange> changeList)
        {
            if (changeList != null && changeList.Count > 0)
            {
                for (int i = 0; i < changeList.Count; i++)
                {
                    int rulerID = changeList[i].RulerType;
                    int showType = changeList[i].Type;

                    MjDataManager.Instance.SetPaikouShowData(rulerID, showType);
                }
            }
        }


        private List<MjPlayerInfo> MjGetPlayerInfoList(List<Msg.MjPlayerInfo> originalList, int gameState)
        {
            List<MjPlayerInfo> mjPlayerInfo = null;
            if (originalList != null && originalList.Count > 0)
            {
                mjPlayerInfo = new List<MjPlayerInfo>();
                Msg.MjPlayerInfo originalInfo = null;
                for (int i = 0; i < originalList.Count; i++)
                {
                    originalInfo = originalList[i];

                    bool isReady = originalInfo.IsReady;
                    if (gameState != 0 && gameState != 10)
                    {
                        isReady = false;
                    }

                    PlayerDataModel playerModel = MemoryData.PlayerData.get(originalInfo.UserID);

                    MjPlayerInfo playerInfo = playerModel.playerDataMj = new MjPlayerInfo(originalInfo.UserID, originalInfo.Score, originalInfo.SeatID, isReady);
                    playerModel.SetMjPlayerInfoData(playerInfo, originalInfo.UserID, originalInfo.HeadUrl, -1, originalInfo.Name, originalInfo.Sex, originalInfo.ClientIp);
                    mjPlayerInfo.Add(playerInfo);
                }
            }
            return mjPlayerInfo;
        }


        private MjDeskInfo MjGetDeskInfo(Msg.MjDeskInfo MjDeskInfo)
        {
            //List<int> gamerulerList = new List<int>();
            //if (MjDeskInfo.MjRuler != null && MjDeskInfo.MjRuler.Count > 0)
            //{
            //    for (int i = 0; i < MjDeskInfo.MjRuler.Count; i++)
            //    {
            //        int ruler = MjDeskInfo.MjRuler[i];
            //        gamerulerList.Add(ruler);
            //    }
            //}
            List<RulerItem> rulerList = new List<RulerItem>();
            if (MjDeskInfo.RulerSelected != null && MjDeskInfo.RulerSelected.Count > 0)
            {
                for (int i = 0; i < MjDeskInfo.RulerSelected.Count; i++)
                {
                    RulerItem temp = new RulerItem();// MjDeskInfo.RulerSelected[i].RulerConfigID, MjDeskInfo.RulerSelected[i].RulerValue);
                    temp.RulerConfigID = MjDeskInfo.RulerSelected[i].RulerConfigID;
                    temp.RulerValue = MjDeskInfo.RulerSelected[i].RulerValue;
                    rulerList.Add(temp);
                }
            }

            int mjType = (int)MjDeskInfo.MjGameType;

            MjDeskInfo deskInfo = new MjDeskInfo(MjDeskInfo.ConfigID, MjDeskInfo.Bouts, MjDeskInfo.MaxDouble, mjType, null,
                MjDeskInfo.DeskID, MjDeskInfo.Rounds, MjDeskInfo.ViewScore, MjDeskInfo.DeskAdvert, rulerList, MjDeskInfo.OwnerUserID);
            MemoryData.MahjongPlayData.SavePlaySet(deskInfo.mjGameConfigId, deskInfo.viewScore, null, deskInfo.mjRulerSelected);
            return deskInfo;
        }


        private Msg.MjDeskInfo MjGetProtoDeskInfo(MjDeskInfo roomInfo)
        {
            Msg.MjDeskInfo newRoomInfo = new Msg.MjDeskInfo();
            newRoomInfo.ConfigID = roomInfo.mjGameConfigId;
            newRoomInfo.Bouts = roomInfo.bouts;
            newRoomInfo.DeskID = roomInfo.deskID;
            newRoomInfo.MaxDouble = roomInfo.maxDouble;
            newRoomInfo.MjGameType = roomInfo.mjGameType;
            //if (roomInfo.mjRulerList != null && roomInfo.mjRulerList.Count > 0)
            //{
            //    for (int i = 0; i < roomInfo.mjRulerList.Count; i++)
            //    {
            //        newRoomInfo.MjRuler.Add((int)roomInfo.mjRulerList[i]);
            //    }
            //}
            newRoomInfo.Rounds = roomInfo.rounds;
            newRoomInfo.ViewScore = roomInfo.viewScore;
            newRoomInfo.DeskAdvert = roomInfo.DeskAdvert;
            if (roomInfo.mjRulerSelected != null && roomInfo.mjRulerSelected.Count > 0)
            {
                for (int i = 0; i < roomInfo.mjRulerSelected.Count; i++)
                {
                    var item = new Msg.RulerItem();
                    item.RulerConfigID = roomInfo.mjRulerSelected[i].RulerConfigID;
                    item.RulerValue = roomInfo.mjRulerSelected[i].RulerValue;

                    //带星星的选项时 传星星ID
                    if (roomInfo.mjRulerSelected[i].StarConfigId > 0)
                    {
                        item.RulerConfigID = roomInfo.mjRulerSelected[i].StarConfigId;
                    }

                    newRoomInfo.RulerSelected.Add(item);
                }
            }
            return newRoomInfo;
        }


        private List<MjTingInfo> MjGetTingInfoList(List<Msg.MjTingInfo> tingList)
        {
            List<MjTingInfo> tingInfoList = null;
            if (tingList != null && tingList.Count > 0)
            {
                tingInfoList = new List<MjTingInfo>();
                Msg.MjTingInfo originalInfo = null;
                for (int i = 0; i < tingList.Count; i++)
                {
                    originalInfo = tingList[i];
                    MjTingInfo tingInfo = this.MjGetTingInfo(originalInfo);
                    tingInfoList.Add(tingInfo);
                }
            }
            return tingInfoList;
        }

        private MjTingInfo MjGetTingInfo(Msg.MjTingInfo originalInfo)
        {
            MjTingInfo tingInfo = null;
            if (originalInfo != null)
            {
                tingInfo = new MjTingInfo(originalInfo.TingCode, originalInfo.HuCode, originalInfo.HuCodeNum, originalInfo.SomeNumber);
            }
            return tingInfo;
        }


        private BestMjRecord GetBestMjRecord(Msg.BestMjRecord originalBest)
        {
            //Msg.BestMjRecord originalBest = prsp.MjRecords;
            BestMjRecord bestRecord = null;
            if (originalBest != null)
            {
                List<Msg.MjPai> originalCodeList = originalBest.MjList;
                List<MjPai> mjCodeList = new List<MjPai>();
                if (originalCodeList != null && originalCodeList.Count > 0)
                {
                    for (int i = 0; i < originalCodeList.Count; i++)
                    {
                        MjPai code = new MjPai(originalCodeList[i].MjCode, originalCodeList[i].CodeType);
                        mjCodeList.Add(code);
                    }
                }
                List<Msg.MjPai> originalSpecialList = originalBest.MjSpecialList;
                List<MjPai> specialList = new List<MjPai>();
                if (originalSpecialList != null && originalSpecialList.Count > 0)
                {
                    for (int j = 0; j < originalSpecialList.Count; j++)
                    {
                        MjPai specialCode = new MjPai(originalSpecialList[j].MjCode, originalSpecialList[j].CodeType);
                        specialList.Add(specialCode);
                    }
                }
                bestRecord = new BestMjRecord(originalBest.MjType, originalBest.OddsCount, mjCodeList, specialList, originalBest.scoreChange, originalBest.PaiType);
            }
            else
            {
                bestRecord = new BestMjRecord();
            }

            return bestRecord;
        }


        private List<MjBureauDetialInfo> GetMjBureauDetialInfo(List<Msg.MjBureauDetialInfo> originalDetailList)
        {
            List<MjBureauDetialInfo> detailInfoList = new List<MjBureauDetialInfo>();
            if (originalDetailList != null && originalDetailList.Count > 0)
            {
                for (int i = 0; i < originalDetailList.Count; i++)
                {
                    MjBureauInfo bureauInfo = this.GetMjBureauInfo(originalDetailList[i].BureauInfo);
                    BestMjRecord bestRecord = this.GetBestMjRecord(originalDetailList[i].MjRecords);

                    MjBureauDetialInfo detaiInfo = new MjBureauDetialInfo(bureauInfo, bestRecord, originalDetailList[i].nDianPaoCount, originalDetailList[i].nZiMoCount, originalDetailList[i].nSomeNumSeatID, originalDetailList[i].nMaxSomeNum);

                    detailInfoList.Add(detaiInfo);
                }

            }

            return detailInfoList;
        }



        private MjBureauInfo GetMjBureauInfo(Msg.MjBureauInfo originalBureauInfo)
        {
            List<Msg.MjScore> originalScore = originalBureauInfo.nDetailScore;
            List<MjScore> scoreList = new List<MjScore>();
            if (originalScore != null && originalScore.Count > 0)
            {
                for (int j = 0; j < originalScore.Count; j++)
                {
                    MjScore score = new MjScore(
                originalScore[j].SeatID, originalScore[j].Score, originalScore[j].IsDianPao, originalScore[j].IsChengBao);
                    scoreList.Add(score);
                }
            }

            MjBureauInfo bureau = new MjBureauInfo(originalBureauInfo.nBureauCount, scoreList, originalBureauInfo.nWinSeatID);

            return bureau;
        }

        private List<MjBureauInfo> GetMjBureauInfoList(List<Msg.MjBureauInfo> originalBureauInfo)
        {
            List<MjBureauInfo> bureauList = new List<MjBureauInfo>();
            if (originalBureauInfo != null && originalBureauInfo.Count > 0)
            {
                for (int i = 0; i < originalBureauInfo.Count; i++)
                {
                    MjBureauInfo bureau = GetMjBureauInfo(originalBureauInfo[i]);
                    bureauList.Add(bureau);
                }
            }

            return bureauList;
        }

        private List<MjTitleInfo> GetMjTitleInfoList(List<Msg.MjTitleInfo> originalTitleList)
        {
            List<MjTitleInfo> titleInfoList = new List<MjTitleInfo>();
            if (originalTitleList != null && originalTitleList.Count > 0)
            {
                for (int i = 0; i < originalTitleList.Count; i++)
                {
                    MjTitleInfo titleInfo = new MjTitleInfo(originalTitleList[i].nScore, originalTitleList[i].nTitle, originalTitleList[i].SeatID);
                    titleInfoList.Add(titleInfo);
                }
            }
            return titleInfoList;
        }


        private List<MjPai> MjGetHandList(List<Msg.MjPai> originalList)
        {
            List<MjPai> paiList = null;
            if (originalList != null && originalList.Count > 0)
            {
                paiList = new List<MjPai>();
                for (int i = 0; i < originalList.Count; i++)
                {
                    MjPai paiItem = new MjPai(originalList[i].MjCode, originalList[i].CodeType);
                    paiList.Add(paiItem);
                }
            }

            return paiList;
        }



        private List<MjScore> GetEasyScoreList(List<Msg.MjScore> scoreList)
        {
            List<MjScore> changeList = new List<MjScore>();
            if (scoreList != null && scoreList.Count > 0)
            {
                for (int i = 0; i < scoreList.Count; i++)
                {
                    MjScore score = new MjScore(scoreList[i].SeatID, scoreList[i].Score);
                    changeList.Add(score);
                }
            }

            return changeList;
        }


        private MjStandingPlateData MjGetStandingData(Msg.MjStandingPlates prsp)
        {
            MjStandingPlateData standingData = null;
            if (prsp != null)
            {
                standingData = new MjStandingPlateData(prsp.nMjCode, prsp.nMjChangeCode, (int)prsp.ShowType);

                standingData.SetDataFromCode(prsp.Offset, prsp.isCanGet, prsp.isFromBegin);
                standingData.SetDataRoll(prsp.RollNo, prsp.WhoSeatID);
            }

            return standingData;
        }


        #endregion

        #region　杠后拿吃
        private void MjGangHouNaChiServerNotify(object rsp)
        {
            DebugPro.DebugInfo("MjGangHouNaChiServerNotify: ");
            MjGangHouNaChiNotify nachiNotify = rsp as MjGangHouNaChiNotify;
            if (!NullHelper.IsObjectIsNull(nachiNotify))
            {
                MahjongPlayType.GangHouNaChiNotifyData data = new MahjongPlayType.GangHouNaChiNotifyData();
                data.MjCodeList = new List<int>();
                if (nachiNotify.MjCodeList != null)
                {
                    for (int i = 0; i < nachiNotify.MjCodeList.Count; i++)
                    {
                        data.MjCodeList.Add(nachiNotify.MjCodeList[i]);
                    }
                }
                else
                {
                    DebugPro.DebugInfo("杠后拿吃的牌型数据为空");
                }
                data.RulerID = nachiNotify.nRulerID;
                data.SeatID = nachiNotify.nSeatID;
                data.SelectSubType = nachiNotify.nSelectSubType;
                MjDataManager.Instance.MjData.ProcessData.processGangHouNaChi.GangHouNaChiNotifyData = data;
                DebugPro.DebugInfo("GangHouNaChi self seat id:", MjDataManager.Instance.MjData.curUserData.selfSeatID);
                DebugPro.DebugInfo("GangHouNaChi nachi seat id:", data.SeatID);
                DebugPro.DebugInfo("GangHouNaChi SelectSubType:", data.SelectSubType);
                EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_LogicNotify.ToString());

            }
        }

        public void MjReqGameingNaChiResult(int deskID, int seatID, int cardID, int cardIndex)
        {
            Msg.MjReqGameingNaChi nachiReq = new Msg.MjReqGameingNaChi();
            nachiReq.DeskID = deskID;
            nachiReq.SeatID = seatID;
            nachiReq.nSelectID = cardIndex;
            ModelNetWorker.Send(nachiReq);
        }
        private void MjRspGameingNaChiResult(object rsp)
        {
            MjRspGameingNaChi result = rsp as MjRspGameingNaChi;
            DebugPro.DebugError("====result: ", result.ResultCode);
            if (result.ResultCode == 1)
            {
                EventDispatcher.FireEvent(MJEnum.GangHouNaChi.GHNC_LogicResponseResult.ToString());
            }
            else
            {
                DebugPro.DebugError("MjReqGameingNaChiResult error");
            }
        }
        #endregion


        #region 包次(new)

        private void MjTipInfoNotify(object rsp)
        {
            SetMjTipInfoNotify(rsp as MjTipInfoNotify, true);
        }

        private bool SetMjTipInfoNotify(Msg.MjTipInfoNotify info, bool needShow)
        {
            bool countain = false;
            if (info != null)
            {
                countain = true;
                MjDataManager.Instance.IniBaociTip(info.nSelectSubType, info.nRulerID, info.nParam, needShow);
            }
            return countain;
        }

        #endregion

        #region 亮一张

        private void MjLiangYiZhangNotify(object rsp)
        {
            MjLiangYiZhangNotify prsp = rsp as MjLiangYiZhangNotify;
            SetLiangyiData(prsp, true);
        }


        private void SetLiangyiData(MjLiangYiZhangNotify prsp, bool isNormalLogic)
        {
            if (prsp != null)
            {
                int selectType = prsp.nSelectSubType;
                if (selectType != 0)
                {
                    int rulerID = prsp.nRulerID;
                    List<MahjongPlayType.MjLiangyiData> dataList = new List<MahjongPlayType.MjLiangyiData>();
                    if (prsp.DataCell != null && prsp.DataCell.Count > 0)
                    {
                        for (int i = 0; i < prsp.DataCell.Count; i++)
                        {
                            int seatID = prsp.DataCell[i].ComnonValue.SeatID;
                            bool haveChoose = prsp.DataCell[i].ComnonValue.bOpSet;
                            int curValue = prsp.DataCell[i].DataConfirm.nSelectValue;
                            MahjongPlayType.MjLiangyiData item = new MahjongPlayType.MjLiangyiData(seatID);
                            item.haveChoose = haveChoose;
                            item.chooseCard = curValue;
                            dataList.Add(item);
                        }
                    }

                    MjDataManager.Instance.LiangyiNetSetData(selectType, dataList, isNormalLogic);
                }
            }
        }


        private void MjRspLiangYiZhang(object rsp)
        {
            MjRspLiangYiZhang prsp = rsp as MjRspLiangYiZhang;

            int resultCode = prsp.ResultCode;
            if (resultCode == 1)
            {
                int selectID = prsp.nSelectID;
                int chooseCode = prsp.nCodeID;
                int seatID = prsp.SeatID;
                MjDataManager.Instance.LiangyiNetUpdateData(seatID, chooseCode);
            }
        }


        public void MjReqLiangYiZhang(int deskID, int seatID, int indexID, int cardID)
        {
            MjReqLiangYiZhang req = new Msg.MjReqLiangYiZhang();
            req.DeskID = deskID;
            req.SeatID = seatID;
            req.nSelectID = indexID;
            req.nCodeID = cardID;

            this.send(req);
        }

        #endregion

        #region 出牌限制
        //出牌限制
        private void MjPutLimitNotify(object rsp)
        {
            if (rsp != null)
            {
                MjPutLimitNotify data = (MjPutLimitNotify)rsp;

                for (int i = 0; i < data.DataCell.Count; i++)
                {
                    var cellData = data.DataCell[i];
                    MjDataManager.Instance.SetInitLimitData(cellData.ComnonValue.SeatID, cellData.DataConfirm.nLackType, cellData.DataConfirm.nPaiType, cellData.DataConfirm.nMjCode);
                }
                EventDispatcher.FireEvent(MJEnum.MjDuanMenEvent.MjDM_LogicNotify.ToString());
            }
        }

        //重连
        private void SetPutLimit(object resp)
        {
            if (resp != null)
            {
                List<MjPutLimitNotify> list = (List<MjPutLimitNotify>)resp;

                for (int i = 0; i < list.Count; i++)
                {
                    MjPutLimitNotify data = list[i];

                    for (int n = 0; n < data.DataCell.Count; n++)
                    {
                        var cellData = data.DataCell[n];
                        MjDataManager.Instance.SetInitLimitData(cellData.ComnonValue.SeatID, cellData.DataConfirm.nLackType, cellData.DataConfirm.nPaiType, cellData.DataConfirm.nMjCode);
                    }
                }
            }
        }
        #endregion

        #region 亮杠头
        //亮杠头
        private void MjLiangGangTouNotify(object rsp)
        {
            if (rsp != null)
            {
                MjLiangGangTouNotify data = (MjLiangGangTouNotify)rsp;

                MjDataManager.Instance.SetInitLiangGangTouData(data.nSelectSubType, data.nSeatID, data.nRulerID, data.MjCodeList);
                EventDispatcher.FireEvent(MJEnum.MjLiangGangTouEvent.MJLGT_LogicNotify.ToString());
            }
        }

        //重连
        private void SetLiangGangTou(object resp)
        {
            if (resp != null)
            {
                MjLiangGangTouNotify data = (MjLiangGangTouNotify)resp;

                MjDataManager.Instance.SetInitLiangGangTouData(data.nSelectSubType, data.nSeatID, data.nRulerID, data.MjCodeList);
            }
        }
        #endregion

        #region 补花
        /// <summary>
        /// 出牌补花
        /// </summary>
        /// <param name="rsp"></param>
        private void MjSelfBuHuaNotify(object rsp)
        {
            MjSelfBuHuaNotify prsp = rsp as MjSelfBuHuaNotify;
            //ruler
            int seatID = prsp.SeatID;
            int mjRuler = prsp.MjRulerResult;
            List<MjTingInfo> tingInfoList = this.MjGetTingInfoList(prsp.TingList);
            List<int> gangList = prsp.GangList;
            List<int> ciList = prsp.CiList;
            MjDataManager.Instance.SetRulerResult(seatID, mjRuler, gangList, tingInfoList, ciList);
            //change
            MJSetPaikouChangeList(prsp.ChangeList);
            //changeFlower
            MjSetBuHuaNotifyData(prsp.BuHuaInfo, true, mjRuler);
        }


        /// <summary>
        /// 开局前补花
        /// </summary>
        /// <param name="rsp"></param>
        private void MjBuHuaNotify(object rsp)
        {
            MjBuHuaNotify prsp = rsp as MjBuHuaNotify;
            MjDataManager.Instance.SetChangeDataIni();
            MjSetBuHuaNotifyData(prsp, false, -1);
            MjDataManager.Instance.SetChangeDataOverWhenDeal();
        }


        private void MjSetBuHuaNotifyData(Msg.MjBuHuaNotify prsp, bool isPut, int mjRuler)
        {
            if (prsp != null)
            {
                if (prsp.DataCell != null && prsp.DataCell.Count > 0)
                {
                    for (int i = 0; i < prsp.DataCell.Count; i++)
                    {
                        MjParseBuHuaCell playerCell = prsp.DataCell[i];
                        int curSeatID = playerCell.nSeatID;
                        List<MjBuHuaRound> roundList = playerCell.RoundList;
                        if (roundList != null && roundList.Count > 0)
                        {
                            for (int indexIn = 0; indexIn < roundList.Count; indexIn++)
                            {
                                ///一轮
                                MjBuHuaRound roundItem = roundList[indexIn];
                                for (int indexIn_2 = 0; indexIn_2 < roundItem.MjCodeList.Count; indexIn_2++)
                                {
                                    MjBuHuaPai buItem = roundItem.MjCodeList[indexIn_2];
                                    if (isPut)
                                    {
                                        MjDataManager.Instance.SetChangeDataWhenPut(curSeatID, buItem.nMjCode, buItem.nHandType, buItem.nGetMjCode, buItem.nGetHandType, mjRuler);
                                    }
                                    else
                                    {
                                        MjDataManager.Instance.SetChangeDataWhenDeal(curSeatID, buItem.nMjCode, buItem.nHandType, buItem.nGetMjCode, buItem.nGetHandType);
                                    }
                                }

                            }
                        }


                    }

                }
            }
        }

        #endregion

        #region 奖马、抓马（new）

        private void MjJiangMaNotify(object rsp)
        {
            if (rsp != null)
            {
                MjBuyHorseNotify prsp = rsp as MjBuyHorseNotify;

                int maType = prsp.nType;
                List<Msg.MjHorse> horseResult = prsp.Horse;
                List<MjHorse> mjHorseResult = new List<MjHorse>();
                if (horseResult != null && horseResult.Count > 0)
                {
                    for (int i = 0; i < horseResult.Count; i++)
                    {
                        MjHorse horseData = new MjHorse(horseResult[i].SeatID, horseResult[i].nValue, horseResult[i].GetType);
                        mjHorseResult.Add(horseData);
                    }
                }
                List<Msg.MjScore> scoreResult = prsp.Score;
                List<MjScore> mjScoreResult = GetEasyScoreList(scoreResult);

                MjDataManager.Instance.InitSetJiangMaData(maType, mjHorseResult, mjScoreResult);
                EventDispatcher.FireEvent(MJEnum.MjJiangMaEvent.MJJM_ShowLogicNotify.ToString(), maType);
            }
        }

        #endregion

    }
}


