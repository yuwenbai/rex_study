/**
* @Author Xin.Wang
*
*
*/

using System.Collections.Generic;
using MjCreateFactory;
using System;

namespace projectQ
{
    public class MahjongLogicNew : SingletonTamplate<MahjongLogicNew>
    {
        private MjMusicController _MusicController = new MjMusicController();
        private Dictionary<System.Type, BaseLogicModule> _LogicModules = new Dictionary<System.Type, BaseLogicModule>();

        public void ClearUp()
        {
            Dictionary<System.Type, BaseLogicModule>.Enumerator item = _LogicModules.GetEnumerator();
            while (item.MoveNext())
            {
                item.Current.Value.ClearUp();
            }
        }

        //游戏基本逻辑
        private void IniDisPatcherBase()
        {
            ResetLogicModules();
            _MusicController.AddEvents();

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMahjongJoinDesk, Set3DTableMessage);            //加入房间
            EventDispatcher.AddEvent(GEnum.Table3DEvents.Table3D_PrepareData.ToString(), Set3DTableMessage);
            //EventDispatcher.AddEvent(GEnum.InGameModelEvents.IGM_SetPaiKou.ToString(), SetPaiKou);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjNewDeskRsp, CreateNewDeskRsp);         //创建桌子
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjJoinDeskRsp, JoinDeskRsp);             //加入桌子
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjDeskUsersNotify, JoneDeskNotify);      //桌内玩家通知
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjOpActionNotify, MjActionNotify);       //基本操作通知
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjRoomGameStartNotify, MjGameStartNotify);   //游戏开始
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjBeforeDispatch, MjBeforeDispatch);         //游戏需要变量通知

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjSyncPlayerStateNotify, MjSyncPlayerStateNotify);//摸牌通知
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjDealerBeginNotify, MjDealerBeginNotify);       //庄家开始通知
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjChangeFlowerWhenDeal, LogicChangeFlowerWhenDeal);       //发牌后补花
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjChangeFlowerWhenPut, LogicChangeFlowerWhenPut);       //出牌后补花

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjOpactionPutNotify, MjOpactionPutNotify);   //出牌操作通知
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjGameInitMjListNotify, MjGameIniMjListNotify);//游戏发牌
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjPutDownNotify, MjPutDownNotify);       //游戏推牌
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjGameOverNotify, MjGameOverNotify);     //游戏战绩
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjBalanceNewNotify, MjBalanceNewNotify); //单局结算

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjTimeNotify, MjNotifyTime);             //时间通知

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjReconned, MjReconned);                 //麻将重连
        }

        private void ResetLogicModules()
        {
            _LogicModules.Clear();
            BaseLogicModuleCreateFactory.CreateBaseLogicModule(_LogicModules);
            UniversalLogicModuleCreateFactory.CreateUniversalLogicModule(_LogicModules);
            SpecialLogicModuleCreateFactory.CreateSpcialLogicModule(_LogicModules);
        }

        //游戏特殊玩法
        private void IniDisPatcherSpecial()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjObligate, MjObligate);                 //通知留牌

            //MJTODO : 重构
            //换三张
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjAskReqChangeThree, MjAskReqChangeThree);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjRspChangeThree, MjRspChangeThree);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjChangeThreeNotify, MjChangeThreeNotify);

            //定缺
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjAskReqConfirm, MjAskReqConfirm);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjRspConfirm, MjRspConfirm);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjConfirmNotify, MjConfirmNotify);

            //下跑
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjAskReqPao, MjAskReqPao);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjRspPao, MjRspPao);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjPaoNotify, MjPaoNotify);

            //下炮子
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjAskReqPaoZi, MjAskReqPaoZi);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjRspPaoZi, MjRspPaoZi);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjPaoNotifyZi, MjPaoNotifyZi);

            //下鱼
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjServerYu, MjServerYu);

            //买马
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjBuyHorseCountNotify, MjBuyHorseCountNotify);
            //EventDispatcher.AddEvent(GEnum.NamedEvent.EMjBuyHorseNotify, MjBuyHorseNotify);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjOpenMaNotify, MjOpenMaNotify);

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjPerformanceNotify, MjPerformanceNotify);


            //积分通知
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjScoreChangeNotify, MjScoreChangeNotify);


            //补花，明楼(废弃)
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjGetFlowerNotify, MjGetFlowerNotify);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjMinglouNotify, MjMinglouNotify);

            //坎牌 闹庄 末留

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjKanNaoMoNotify, MjKanNaoMoNotify);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjKanNaoMoRsp, MjKanNaoMoRsp);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjKanNaoMoData, MjKanNaoMoData);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendKanNaoMo, MjControlSendKanNaoMo);

            //选飘
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjXuanpiaoNotify, MjXuanpiaoNotify);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjXuanPiaoDataNotify, MjXuanPiaoDataNotify);
            EventDispatcher.AddEvent(GEnum.NamedEvent.UINumberFlyingSubmit, NumberFlyingSubmit);
            //EventDispatcher.AddEvent(GEnum.NamedEvent.EMjXuanPiaoDataNotify, MjXuanPiaoDataNotify);

            //明楼听口
            EventDispatcher.AddEvent(GEnum.NamedEvent.UIMinglouTingInfo, MinglouTingInfo);

            //包次区域
            EventDispatcher.AddEvent(GEnum.EnumMjOprateBaoci.BC_LogicShow.ToString(), SpecialController.ShowBaociTip);
        }

        //其他系统模块功能
        private void IniOtherSystem()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjOpActionRsp, MjActionRsp);             //游戏内操作回执
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjDeskActionNotify, MjDeskActionNotify); //游戏桌内操作通知
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjDeskActionRsp, MjDeskActionRsp);       //游戏桌内操作回执

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjOnlineStateNotify, MjOnlineStateNotify);   //玩家上下线

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMahjongLoadSceneSuccess, SetUIByIniOk);      //游戏场景加载
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMahjongOutScene, SetUIByOut);                //离开当前场景

            EventDispatcher.AddEvent(GEnum.NamedEvent.EYuyinState, MjYuyinState);                   //初始化语音
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlClickCloseResult, MjControlClickCloseResult);            //初始化语音
        }


        public void Ini()
        {
            IniDisPatcherBase();
            IniDisPatcherSpecial();
            IniOtherSystem();

            #region Control
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendOpaction, EControlOpAction);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendChangeThree, EControlChangeThree);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendMinglou, EControlSendMinglou);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendFangmao, EControlSendFangMao);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendChooseBtn, EControlSendChooseBtn);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendChooseQue, EControlSendChooseQue);

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendClose, EControlSendClose);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendCloseAnwser, EControlSendCloseAnwser);
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendCloseBalance, EControlSendCloseBalance);

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSendGetBalanceNew, MjControlSendGetBalanceNew);

            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSetRestCount, EControlRestCountChange);

            #endregion

        }

        #region 特殊玩法管理类
        private MahjongLogicSpecial _specailControler = null;
        private MahjongLogicSpecial SpecialController
        {
            get
            {
                if (_specailControler == null)
                {
                    _specailControler = new MahjongLogicSpecial();
                }
                return _specailControler;
            }
        }

        #endregion


        #region Net

        #region Receive
        //创建房间Rsp
        public void CreateNewDeskRsp(object[] obj)
        {
            EnumMjNewDeskResult resultCode = (EnumMjNewDeskResult)obj[0];
            MjDeskInfo deskInfo = (MjDeskInfo)obj[1];
            ulong userID = (ulong)obj[2];

            this.LogicCreateDeskRsp(resultCode, deskInfo, userID);
        }

        //加入房间Rsp
        public void JoinDeskRsp(object[] obj)
        {
            EnumMjNewDeskResult resultCode = (EnumMjNewDeskResult)obj[0];
            MjDeskInfo deskInfo = (MjDeskInfo)obj[1];
            ulong userID = (ulong)obj[2];

            this.LogicJoinDeskRsp(resultCode, deskInfo, userID);
        }

        //玩家加入桌子Notify
        public void JoneDeskNotify(object[] obj)
        {
            List<MjPlayerInfo> playerInfoList = (List<MjPlayerInfo>)obj[0];
            int deskID = (int)obj[1];
            //List<long> playerIDList = (List<long>)obj[2];
            this.LogicJoneDeskNotify(playerInfoList, deskID);
        }
        //麻将游戏行为回执
        public void MjActionRsp(object[] obj)
        {
            ulong userID = (ulong)obj[0];
            int resultCode = (int)obj[1];

            this.LogicMjActionRsp(userID, resultCode);
        }
        //麻将桌上行为回执
        public void MjDeskActionRsp(object[] obj)
        {
            ulong userID = (ulong)obj[0];
            int resultCode = (int)obj[1];

            this.LogicMjDeskActionRsp(userID, resultCode);
        }

        public void MjOpactionPutNotify(object[] obj)
        {
            int seatID = (int)obj[0];
            int mjCode = (int)obj[1];
            bool isIndependent = (bool)obj[2];
            bool isDark = (bool)obj[3];
            bool isAutoPut = (bool)obj[4];

            bool isSlef = seatID == MjDataManager.Instance.MjData.curUserData.selfSeatID;

            if (isSlef && isAutoPut == false && !FakeReplayManager.Instance.ReplayState)
            {
                return;
            }

            this.LogicMjPutNotify(seatID, mjCode, isIndependent, isDark);
        }


        //麻将桌上行为 
        public void MjActionNotify(object[] obj)
        {
            List<int> chiList = (List<int>)obj[0];
            EnumMjHuType huType = (EnumMjHuType)obj[1];
            int lastPutSeat = (int)obj[2];
            int mjCode = (int)obj[3];
            EnumMjCodeType mjGangType = (EnumMjCodeType)((int)obj[4]);
            int mjRulerResult = (int)obj[5];
            int nGangSeatID = (int)obj[6];
            EnumMjOpAction mjAction = (EnumMjOpAction)obj[7];
            List<int> seatIDlist = (List<int>)obj[8];
            List<MjTingInfo> tingInfoList = (List<MjTingInfo>)obj[9];
            ulong userID = (ulong)obj[10];

            //new
            List<int> maoList = (List<int>)obj[11];
            List<MjRoundGetBuPai> mjChangeFlower = (List<MjRoundGetBuPai>)obj[12];
            bool isIndependent = (bool)obj[13];
            int independentCount = (int)obj[14];
            List<int> gangList = (List<int>)obj[15];
            List<int> handList = (List<int>)obj[16];
            bool canFangmao = (bool)obj[17];
            bool isQiangGang = (bool)obj[18];
            bool isDark = (bool)obj[19];
            int moCode = (int)obj[20];
            List<int> ciList = (List<int>)obj[21];

            this.LogicMjActionNotify(chiList, huType, lastPutSeat, mjCode, mjGangType, mjRulerResult, nGangSeatID,
            mjAction, seatIDlist, tingInfoList, userID,
            maoList, mjChangeFlower, isIndependent, independentCount, gangList, handList, canFangmao, isQiangGang, isDark, moCode, ciList);
        }


        //通知麻将开始
        public void MjGameStartNotify(object[] obj)
        {
            int dealerID = (int)obj[0];
            int getMjSeatID = (int)obj[1];
            int getOffset = (int)obj[2];
            List<int> rollNum = (List<int>)obj[3];
            int allCount = (int)obj[4];
            int deskID = (int)obj[5];
            List<List<int>> maoGroup = (List<List<int>>)obj[6];

            this.LogicGameStartNotify(dealerID, getMjSeatID, getOffset, rollNum, allCount, deskID, maoGroup);
        }

        //通知麻将牌ini 也包括其他人的牌
        public void MjGameIniMjListNotify(object[] obj)
        {
            List<int> mjList = (List<int>)obj[0];
            int seatID = (int)obj[1];
            List<int>[] independentList = (List<int>[])obj[2];

            this.LogicIniMjListNotify(mjList, seatID, independentList);
        }

        public void MjPutDownNotify(object[] obj)
        {
            List<int> dajiaoSeats = (List<int>)obj[0];
            List<int> huazhuSeats = (List<int>)obj[1];
            List<MjChangeScore> tuishuiList = (List<MjChangeScore>)obj[2];
            List<List<MjPai>> handList = (List<List<MjPai>>)obj[3];
            List<List<int>> independentList = (List<List<int>>)obj[4];

            LogicMjPutDownNotify(dajiaoSeats, huazhuSeats, tuishuiList, handList, independentList);
        }

        //麻将新的单据结算
        public void MjBalanceNewNotify(object[] obj)
        {
            bool isInGame = (bool)obj[0];
            MjBalanceNew newInfo = null;
            if (obj.Length > 1)
            {
                newInfo = (MjBalanceNew)obj[1];
            }

            this.LogicMjBalanceNewNotify(newInfo, isInGame);
        }


        //麻将总局结算 
        public void MjGameOverNotify(object[] obj)
        {
            bool isShow = (bool)obj[0];

            this.LogicMjGameOverNotify(isShow);
        }


        //房间内行为通知
        public void MjDeskActionNotify(object[] obj)
        {
            EnumMjDeskAction roomAction = (EnumMjDeskAction)obj[0];
            int seatID = (int)obj[1];
            ulong userID = (ulong)obj[2];
            int closeTime = (int)obj[3];
            int gameState = (int)obj[4];

            this.LogicRoomActionNotify(roomAction, seatID, userID, closeTime, gameState);
        }
        //轮到谁摸牌通知 
        public void MjSyncPlayerStateNotify(object[] obj)
        {
            int mjCode = (int)obj[0];
            int seatID = (int)obj[1];
            int mjRulerResult = (int)obj[2];
            List<MjTingInfo> tingList = (List<MjTingInfo>)obj[3];
            List<int> gangList = (List<int>)obj[4];
            int getType = (int)obj[5];
            bool canFangMao = (bool)obj[6];
            List<MjRoundGetBuPai> mjChangeFlower = (List<MjRoundGetBuPai>)obj[7];
            List<int> ciList = (List<int>)obj[8];
            //if (FakeReplayManager.Instance.ReplayState && mjCode == -1)
            //{
            //    var st = FakeReplayManager.Instance.GetTimeGetCardCache(seatID);
            //    if (st != null)
            //    {
            //        mjCode = (int)st[0];
            //    }
            //}
            this.LogicTurnToPlayer(mjCode, seatID, mjRulerResult, tingList, gangList, getType, canFangMao, mjChangeFlower, ciList);
        }
        //庄家第一轮的行为通知
        public void MjDealerBeginNotify(object[] obj)
        {
            int seatID = (int)obj[0];
            int mjRulerResult = (int)obj[1];
            ulong userID = (ulong)obj[2];
            List<MjTingInfo> tingList = (List<MjTingInfo>)obj[3];
            List<int> gangList = (List<int>)obj[4];
            bool canFangMao = (bool)obj[5];
            List<MjRoundGetBuPai> mjChangeFlower = (List<MjRoundGetBuPai>)obj[6];
            List<int> ciList = (List<int>)obj[7];

            this.LogicDealerBeginNotify(seatID, mjRulerResult, userID, tingList, gangList, canFangMao, mjChangeFlower, ciList);
        }


        //new rule
        //设置当前对局的一些状态
        public void MjBeforeDispatch(object[] obj)
        {
            List<bool> checkValue = (List<bool>)obj[0];

            this.LogicBeforeDispatch(checkValue);
        }

        //换三张
        //通知开始换三张
        public void MjAskReqChangeThree(object[] obj)
        {
            List<int> changeThreeList = (List<int>)obj[0];
            int clockType = (int)obj[1];

            SpecialController.LogicAskChangeThree(changeThreeList, clockType);
        }

        //个人换三张的结果 
        public void MjRspChangeThree(object[] obj)
        {
            int deskID = (int)obj[0];
            int seatID = (int)obj[1];
            List<int> mjList = (List<int>)obj[2];

            SpecialController.LogicRspChangeThree(deskID, seatID, mjList);
        }

        //所有人换三张结束
        public void MjChangeThreeNotify(object[] obj)
        {
            int deskID = (int)obj[0];
            int seatID = (int)obj[1];
            int clockType = (int)obj[2];
            List<int> mjList = (List<int>)obj[3];
            Dictionary<int, MjChangeThreeData> changeList = (Dictionary<int, MjChangeThreeData>)obj[4];

            SpecialController.LogicChangeThreeNotify(deskID, seatID, clockType, mjList, changeList);
        }


        //定缺
        //通知定缺 
        public void MjAskReqConfirm(object[] obj)
        {
            int confirmType = (int)obj[0];

            SpecialController.LogicMjAskReqConfirm(confirmType);
        }

        public void MjRspConfirm(object[] obj)
        {
            int deskID = (int)obj[0];
            int seatID = (int)obj[1];
            int nType = (int)obj[2];

            SpecialController.LogicMjRspConfirm(deskID, seatID, nType);
        }

        public void MjConfirmNotify(object[] obj)
        {
            int deskID = (int)obj[0];
            List<int> seatIDList = (List<int>)obj[1];
            List<int> confirmTypeList = (List<int>)obj[2];

            SpecialController.LogicMjConfirmNotify(deskID, seatIDList, confirmTypeList);
        }

        //下跑
        public void MjAskReqPao(object[] obj)
        {
            int defaultValue = (int)obj[0];
            List<int> values = (List<int>)obj[1];

            SpecialController.LogicMjAskReqPao(defaultValue, values);
        }

        public void MjRspPao(object[] obj)
        {
            int deskID = (int)obj[0];
            int seatID = (int)obj[1];
            int nValue = (int)obj[2];

            SpecialController.LogicMjRspPao(deskID, seatID, nValue);
        }

        public void MjPaoNotify(object[] obj)
        {
            int deskID = (int)obj[0];
            List<int> seatIDList = (List<int>)obj[1];
            List<int> valueList = (List<int>)obj[2];

            SpecialController.LogicMjPaoNotify(deskID, seatIDList, valueList);
        }

        public void MjObligate(object[] obj)
        {
            int offset = (int)obj[0];
            int mjCode = (int)obj[1];

            this.LogicMjObligate(offset, mjCode);
        }


        public void MjBuyHorseCountNotify(object[] obj)
        {
            List<int> maNumList = (List<int>)obj[0];

            this.LogicMjBuyHorseCountNotify(maNumList);
        }

        private void MjOpenMaNotify(object[] obj)
        {
            int maType = (int)obj[0];
            List<MjHorse> horseResult = (List<MjHorse>)obj[1];
            List<MjScore> ScoreResult = (List<MjScore>)obj[2];

            SpecialController.LogicOpenMa(maType, horseResult, ScoreResult);
        }

        private void MjPerformanceNotify(object[] obj)
        {
            List<int> seatID = (List<int>)obj[0];
            List<int> nValue = (List<int>)obj[1];
            bool isPut = (bool)obj[2];
            int curSeatID = (int)obj[3];
            bool isHunDiao = (bool)obj[4];

            SpecialController.LogicMjPerformanceNotify(curSeatID, isPut, seatID, nValue, isHunDiao);
        }

        /// <summary>
        /// 放毛，补花，扭牌补花
        /// </summary>
        /// <param name="obj"></param>
        public void MjGetFlowerNotify(object[] obj)
        {
            int seatID = (int)obj[0];
            List<MjRoundGetBuPai> changeRound = (List<MjRoundGetBuPai>)obj[1];

            this.LogicMjGetFlowerNotify(seatID, changeRound);
        }

        public void MjMinglouNotify(object[] obj)
        {
            int seatID = (int)obj[0];
            List<int> handList = (List<int>)obj[1];

            this.LogicMjMinglouNotify(seatID, handList);
        }


        public void MjAskReqPaoZi(object[] obj)
        {
            int defaultValue = (int)obj[0];
            List<int> values = (List<int>)obj[1];

            SpecialController.LogicMjAskReqPaoZi(defaultValue, values);
        }

        public void MjRspPaoZi(object[] obj)
        {
            int deskID = (int)obj[0];
            int seatID = (int)obj[1];
            int nValue = (int)obj[2];

            SpecialController.LogicMjRspPaoZi(deskID, seatID, nValue);
        }


        public void MjPaoNotifyZi(object[] obj)
        {
            int deskID = (int)obj[0];
            List<int> seatIDList = (List<int>)obj[1];
            List<int> valueList = (List<int>)obj[2];

            SpecialController.LogicMjPaoNotifyZi(deskID, seatIDList, valueList);
        }


        //下鱼
        public void MjServerYu(object[] obj)
        {
            EnumMjSelectSubType curSubType = (EnumMjSelectSubType)obj[0];
            switch (curSubType)
            {
                case EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect:
                    {
                        int defautValue = (int)obj[1];
                        List<int> canChooseValue = (List<int>)obj[2];

                        SpecialController.LogicMjAskReqXiayu(curSubType, defautValue, canChooseValue);
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_WAIT_Select:
                    {
                        int seatID = (int)obj[1];
                        int chooseValue = (int)obj[2];

                        SpecialController.LogicMjRspXiayu(curSubType, seatID, chooseValue);
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_RESULT_Select:
                    {
                        List<int> seatIDList = (List<int>)obj[1];
                        List<int> chooseList = (List<int>)obj[2];

                        SpecialController.LogicMjNotifyXiayu(curSubType, seatIDList, chooseList);
                    }
                    break;
            }


        }

        public void MjScoreChangeNotify(object[] obj)
        {
            List<MjScore> socreChangeList = (List<MjScore>)obj[0];
            int showType = (int)obj[1];
            bool isUpdate = (bool)obj[2];
            int showSeatID = (int)obj[3];

            this.LogicMjScoreChangeNotify(socreChangeList, showType, isUpdate, showSeatID);
        }

        //当前桌面显示时间
        public void MjNotifyTime(object[] obj)
        {
            int time = (int)obj[0];

            this.LogicTimeNotify(time);
        }

        public void MjOnlineStateNotify(object[] obj)
        {
            int seatID = (int)obj[0];
            bool state = (bool)obj[1];

            LogicMjOnlineStateNotify(seatID, state);
        }



        //坎牌 闹庄 末留
        private void MjKanNaoMoNotify(object[] obj)
        {
            //sub state
            //1.开始 2.结束
            int subType = (int)obj[0];
            bool needSend = (bool)obj[1];


            SpecialController.LogicMjKanNaoMoNotify(subType, needSend);
        }

        private void MjKanNaoMoRsp(object[] obj)
        {
            //某个人的结果
            int seatID = (int)obj[0];
            bool kanState = (bool)obj[1];
            bool naoState = (bool)obj[2];
            bool moState = (bool)obj[3];

            SpecialController.LogicMjKanNaoMoRsp(seatID, kanState, naoState, moState);
        }


        private void MjKanNaoMoData(object[] obj)
        {
            //数据同步
            List<MahjongPlayType.MjCanNaoMoServerData> dataList = (List<MahjongPlayType.MjCanNaoMoServerData>)obj[0];

            SpecialController.LogicMjKanNaoMoData(dataList);
        }

        private void MjControlSendKanNaoMo(object[] obj)
        {
            SpecialController.LogicMjKanNaoMo();
        }

        private void MinglouTingInfo(object[] obj)
        {
            List<int> seatID = (List<int>)obj[0];
            List<MjTingInfo> tingInfo = (List<MjTingInfo>)obj[1];
            bool needSend = (bool)obj[2];

            LogicMjMingLouTingNotify(seatID, tingInfo, needSend);
        }


        #region 选飘
        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="obj"></param>
        private void MjXuanPiaoDataNotify(object[] obj)
        {
            int subType = (int)obj[0];
            EnumMjSelectSubType curSubType = (EnumMjSelectSubType)subType;
            if (curSubType == EnumMjSelectSubType.MjSelectSubType_WAIT_Select)
            {
                //每个人的选择
                if (obj.Length > 2)
                {
                    int seatID = (int)obj[1];
                    List<int> rulerID = (List<int>)obj[2];
                    List<int> valueList = (List<int>)obj[3];

                    SpecialController.LogicMjXuanPiaoRspData(seatID, rulerID, valueList);
                }
                else
                {
                    //重连
                    List<MahjongPlayType.MjXuanPiaoServerData> serverDataList = (List<MahjongPlayType.MjXuanPiaoServerData>)obj[1];
                    SpecialController.LogicMjXuanPiaoData(serverDataList);
                }
            }
            else
            {
                //通知开始或者结束
                List<MahjongPlayType.MjXuanPiaoServerData> serverDataList = (List<MahjongPlayType.MjXuanPiaoServerData>)obj[1];
                SpecialController.LogicMjXuanPiaoData(serverDataList);

            }

        }


        /// <summary>
        /// 逻辑
        /// </summary>
        /// <param name="obj"></param>
        private void MjXuanpiaoNotify(object[] obj)
        {
            int subType = (int)obj[0];
            EnumMjSelectSubType curSubType = (EnumMjSelectSubType)subType;
            if (curSubType == EnumMjSelectSubType.MjSelectSubType_WAIT_Select)
            {
                //每个人的选择
                int seatID = (int)obj[1];
                SpecialController.LogicMjXuanPiaoRsp(seatID);
            }
            else
            {
                //通知开始或者结束
                SpecialController.LogicMjXuanPiaoNotify(subType, true);
            }

        }

        /// <summary>
        /// UI选飘结果
        /// </summary>
        /// <param name="obj"></param>
        private void NumberFlyingSubmit(object[] obj)
        {
            SpecialController.LogicMjXuanPiaoResult();
        }


        #endregion

        #endregion

        #region Send

        public void SendMjJoinDesk(ulong userID, int deskInfo)
        {
            ModelNetWorker.Instance.MjJoinDeskReq(userID, deskInfo);
        }

        public void SendMjChangeAction(long userID, int deskID, int seatID, int cardID, bool isDependent)
        {
            ModelNetWorker.Instance.MjReqBuHuaZhang(userID, seatID, deskID, cardID, isDependent);
        }

        public void SendMjAction(ulong userID, int mjCode, EnumMjOpAction opCode, int seatID, int deskID, List<int> chiList,
            bool isIndependent, List<int> handList, bool isLast = false)
        {
            //if (opCode == EnumMjOpAction.MjOp_PutMj ||
            //    opCode == EnumMjOpAction.MjOp_Ting ||
            //    opCode == EnumMjOpAction.MjOp_Minglou)
            //{
            //    EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMahjongOpAction,
            //    userID, mjCode, opCode, seatID, deskID, chiList, isIndependent, handList, isLast);
            //}
            //else
            //{
            //    ModelNetWorker.Instance.MjOpActionReq(userID, mjCode, opCode, seatID, deskID, chiList, null, isIndependent, handList, isLast);
            //}

            ModelNetWorker.Instance.MjOpActionReq(userID, mjCode, opCode, seatID, deskID, chiList, null, isIndependent, handList, isLast);
        }

        public void SendMjActionFangMao(ulong userID, int mjCode, EnumMjOpAction opCode, int seatID, int deskID, List<int> maoList)
        {
            ModelNetWorker.Instance.MjOpActionReq(userID, mjCode, opCode, seatID, deskID, null, maoList);
        }

        public void SendMjRoomAction(ulong userID, int seatID, int deskID, EnumMjDeskAction actionCode)
        {
            ModelNetWorker.Instance.MjDeskActionReq(userID, seatID, deskID, actionCode);
        }

        public void SendMjChangeThree(int deskID, int seatID, List<int> ChangeList)
        {
            ModelNetWorker.Instance.MjReqChangeThree(deskID, seatID, ChangeList);
        }

        public void SendMjConfirm(int deskID, int seatID, int nType)
        {
            ModelNetWorker.Instance.MjReqConfirm(deskID, seatID, nType);
        }

        public void SendMjBtnChoose(EnumMjSpecialCheck chooseType, int deskID, int seatID, int nvalue)
        {
            switch (chooseType)
            {
                case EnumMjSpecialCheck.MjBaseCheckType_XiaPao:
                    {
                        ModelNetWorker.Instance.MjReqPao(deskID, seatID, nvalue);
                    }
                    break;
                case EnumMjSpecialCheck.MJBaseCheckType_XiaPaoZi:
                    {
                        ModelNetWorker.Instance.MjReqPaoZi(deskID, seatID, nvalue);
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaYu:
                    {
                        ModelNetWorker.Instance.MjReqXiayu(deskID, seatID, nvalue);
                    }
                    break;
                case EnumMjSpecialCheck.MjBaseCheckType_XiaMa:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaBang:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaYuNingXia:
                case EnumMjSpecialCheck.MjBaseCheckType_XiaPiao:
                case EnumMjSpecialCheck.MjBaseCheckType_XinXiaPiao:
                    {
                        //下漂,下码
                        SpecialController.LogicMjXuanPiaoResult(nvalue);
                    }
                    break;
            }

        }

        public void SenMjMinglou(int deskID, int seatID, List<int> nvalue)
        {
            ModelNetWorker.Instance.MjMingLouReq(deskID, seatID, nvalue);
        }

        public void SendMjBalanceNew(long userID, int deskID, int curBureau)
        {
            ModelNetWorker.Instance.MjBalanceReq(userID, deskID, curBureau);
        }

        public void SendMjResultState(bool state)
        {
            ModelNetWorker.Instance.GameResultBoardStateReq(state);
        }


        #endregion

        #endregion

        #region GameLogic

        private bool iniOk = false;

        private enum GameProcess
        {
            Idol,
            GameStart,
            Gaming
        }

        private enum ActionResult
        {
            MjActionResult_OK = 1,      //操作成功
            MjActionResult_Wait = 2     //等待
        }

        #region func for gameLogic

        //创建房间游戏逻辑 
        private void LogicCreateDeskRsp(EnumMjNewDeskResult resultCode, MjDeskInfo deskInfo, ulong userID)
        {
            switch (resultCode)
            {
                case EnumMjNewDeskResult.MjError_Success:
                    {
                        MemoryData.DeskData.AddOrUpdateDeskInfo(deskInfo.deskID, deskInfo);
                        QLoger.LOG("[Mahjong LogicLog] : 创建房间成功 + RoomID = {0} + UserID = {1}  ", deskInfo.deskID, userID);

                        //请求进入房间
                        this.SendMjJoinDesk(userID, deskInfo.deskID);
                    };
                    break;
            }
        }

        //加入桌子的游戏逻辑
        private void LogicJoinDeskRsp(EnumMjNewDeskResult resultCode, MjDeskInfo deskInfo, ulong userID)
        {
            switch (resultCode)
            {
                case EnumMjNewDeskResult.MjError_Success:
                    {
                        MemoryData.DeskData.AddOrUpdateDeskInfo(deskInfo.deskID, deskInfo);
                        int deskID = MjDataManager.Instance.SetPlayerEnterDesk(deskInfo, userID);
                        QLoger.LOG("[Mahjong LogicLog] : 加入房间成功 + RoomID = {0} + UserID = {1}  ", deskID, userID);
                    };
                    break;
            }

            MahjongPlay data = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(deskInfo.mjGameConfigId);
            data.OptionLogic.ImportSelectedList(deskInfo.mjRulerSelected);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongJoinDesk, resultCode, deskInfo);
        }


        //加入桌子的时候玩家的加入状态通知
        private void LogicJoneDeskNotify(List<MjPlayerInfo> playerInfoList, int deskID)
        {

            MjDataManager.Instance.SetPlayerInfosByDeskID(playerInfoList, deskID);
            if (playerInfoList != null && playerInfoList.Count > 0)
            {
                for (int i = 0; i < playerInfoList.Count; i++)
                {
                    if (playerInfoList[i].userID == MemoryData.UserID)
                    {
                        MjDataManager.Instance.MjData.curUserData.selfSeatID = playerInfoList[i].seatID;
                    }
                }
            }
            if (FakeReplayManager.Instance.ReplayState && playerInfoList.Count > 0)
            {
                for (int i = 0; i < playerInfoList.Count; i++)
                {
                    if (playerInfoList[i].userID == (long)MjDataManager.Instance.MjData.curUserData.selfUserID)
                    {
                        MjDataManager.Instance.MjData.curUserData.selfSeatID = playerInfoList[i].seatID;
                    }
                }
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongDeskPlayer, playerInfoList, deskID);
        }

        //游戏行为发送的回执
        private void LogicMjActionRsp(ulong userID, int resultCode)
        {
            //ActionResult actionResult = (ActionResult)resultCode;
            //switch (actionResult)
            //{
            //    case ActionResult.MjActionResult_OK:
            //        {

            //        }
            //        break;
            //    case ActionResult.MjActionResult_Wait:
            //        {

            //        }
            //        break;
            //}
        }
        //桌上行为发送回执
        private void LogicMjDeskActionRsp(ulong userID, int resultCode)
        {
            //ActionResult actionResult = (ActionResult)resultCode;
            //switch (actionResult)
            //{
            //    case ActionResult.MjActionResult_OK:
            //        {

            //        }
            //        break;
            //    case ActionResult.MjActionResult_Wait:
            //        {

            //        }
            //        break;
            //}
        }

        #region 游戏行为：出牌
        private void LogicMjPutNotify(int seatID, int mjCode, bool isIndependent, bool isDark)
        {
            MjDataManager.Instance.SetPlayerHandPut(seatID, mjCode, isIndependent);
            MjDataManager.Instance.LSDY_SetPutState(seatID);

            if (seatID == MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {
                MjDataManager.Instance.ClearPaikou();
                if (MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISLiangLiu) && isIndependent)
                {
                    MjDataManager.Instance.MjData.ProcessData.HasFlyOne = true;
                }
            }
            else
            {
                MjDataManager.Instance.LoseCardByNum(mjCode, 1, true, seatID);
            }

            //事件流
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimPutCard, seatID, mjCode, isDark);          //出牌前置
            EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_PutCard.ToString(), seatID, mjCode);                  //设置出牌声音
            MjDataManager.Instance.SetDealerPutState(false);
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_Put, GEnum.NameAnim.EMjPut, seatID, mjCode, isIndependent, isDark); //出牌动画
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_Flip, GEnum.NameAnim.EMjFlip, seatID, isIndependent); //插牌动画
        }


        /// <summary>
        /// 出牌ruler通知
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="mjCode"></param>
        /// <param name="ruler"></param>
        /// <param name="tingInfoList"></param>
        private void LogicOpActionPut(int seatID, int mjCode, int ruler, List<MjTingInfo> tingInfoList)
        {
            if (seatID == MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {
                MjTingInfo refreshTingInfo = null;
                if (tingInfoList != null && tingInfoList.Count > 0)
                {
                    refreshTingInfo = tingInfoList[0];
                }
                MjDataManager.Instance.SetPaikouTingConfirm(refreshTingInfo);
            }

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimPutTanban, ruler, mjCode, seatID);
            AnimPlayManager.Instance.PlayAnimFx(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjXingpaiChoose.XPC_AnimShow.ToString());
        }

        #endregion
        #region 游戏行为： 碰牌
        private void LogicMjPengNotify(int seatID, int mjCode, int independentCount, int lastPutSeat)
        {
            //展示碰的2D效果
            MjDataManager.Instance.SetPlayerHandPeng(seatID, mjCode, independentCount);
            if (seatID == MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {
                MjTingInfo refreshTingInfo = null;
                MjDataManager.Instance.SetPaikouTingConfirm(refreshTingInfo);
                this.SendCanPutCard();
            }
            else
            {
                MjDataManager.Instance.LoseCardByNum(mjCode, 2, false);
            }


            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimPeng, seatID);
            EventDispatcher.FireEvent(MJEnum.MusicEnum.ME_ActionFx.ToString(), MjDataManager.Instance.GetCurrentUserIDBySeatID(seatID), EnumMjOpAction.MjOp_Peng);
            AnimRemovePutCard(mjCode, false);
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_ControlCreate, GEnum.NameAnim.EMjPeng, seatID, mjCode, lastPutSeat, independentCount);
        }


        private void LogicOpActionPeng(List<MjRoundGetBuPai> mjChangeFlower, int seatID, bool canFangMao,
            int mjRulerResult, List<int> gangList, List<MjTingInfo> tingInfoList, int mjCode, List<int> ciList)
        {
            bool canFlower = mjChangeFlower != null && mjChangeFlower.Count > 0;
            bool canClick = MjDataManager.Instance.Base_GetRulerLimitClick(canFlower, canFangMao);
            SendTurnPlay(seatID, canClick);

            //补牌
            if (canFlower)
            {
                this.LogicMjGetFlowerNotify(seatID, mjChangeFlower);
            }

            if (canFangMao)
            {
                ShowFangMao(seatID, mjRulerResult, gangList, tingInfoList, ciList);
            }
            else
            {
                //show tanban
                ShowTanban(seatID, mjRulerResult, tingInfoList, gangList, mjCode, ciList);
            }
        }

        #endregion

        #region 游戏行为 ： 杠牌
        private void LogicMjGangNotify(EnumMjCodeType mjGangType, int seatID, int mjCode, int independentCount, int mjRulerResult, int lastPutSeat, bool isQiangGang)
        {
            int loseCard = 0;
            switch (mjGangType)
            {
                case EnumMjCodeType.Code_Gang_Bu:
                    {
                        MjDataManager.Instance.SetPlayerHandBuGang(seatID, mjCode, independentCount);
                        loseCard = 1;
                    }
                    break;
                case EnumMjCodeType.Code_Gang_An:
                    {
                        MjDataManager.Instance.SetPlayerHandAnGang(seatID, mjCode, independentCount);
                        loseCard = 4;
                    }
                    break;
                case EnumMjCodeType.Code_Gang_Zhi:
                    {
                        MjDataManager.Instance.SetPlayerHandZhiGang(seatID, mjCode, independentCount);
                        loseCard = 3;
                    }
                    break;
            }


            if (seatID != MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {
                if (loseCard == 4)
                {
                    //暗杠
                    bool needOpenOne = !MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsCloseAnGang);
                    if (needOpenOne)
                    {
                        MjDataManager.Instance.LoseCardByNum(mjCode, loseCard, false);
                    }
                }
                else
                {
                    MjDataManager.Instance.LoseCardByNum(mjCode, loseCard, false);
                }
            }



            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimGang, seatID, mjGangType, isQiangGang, mjRulerResult, mjCode);

            if (mjGangType == EnumMjCodeType.Code_Gang_Zhi)
            {
                AnimRemovePutCard(mjCode, false);
            }
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_ControlCreate, GEnum.NameAnim.EMjGang, seatID, mjGangType, mjCode, mjRulerResult, lastPutSeat, independentCount);
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjXingpaiChoose.XPC_AnimShow.ToString());
        }


        #endregion

        #region 游戏行为 ：听牌

        private void LogicMjTingNotify(int seatID, List<int> handList, int moCode)
        {
            LogicMjTingBase(seatID, handList, moCode);
            MjDataManager.Instance.SetPlayerHasTing(seatID);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimTing, seatID);
        }


        private void LogicMjMingLouNotify(int seatID, List<int> handList, int moCode)
        {
            LogicMjTingBase(seatID, handList, moCode);
            MjDataManager.Instance.SetPlayerHasMinglou(seatID);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimMingLou, seatID);
        }


        private void LogicMjTingBase(int seatID, List<int> handList, int moCode)
        {
            bool isCheckTing = MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsMingLouLiangTing);

            MjDataManager.Instance.SetMinglouData(seatID, handList, null);
            if (isCheckTing)
            {
                //只查听 亮牌
                handList = new List<int>();
            }
            else
            {
                handList.Sort();
                if (seatID != MjDataManager.Instance.MjData.curUserData.selfSeatID)
                {
                    MjDataManager.Instance.LoseCardByNum(handList);
                }
            }
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_Ting, GEnum.NameAnim.EMjTing, seatID, handList, moCode);
        }


        /// <summary>
        /// 明楼同步听口
        /// </summary>
        private void LogicMjMingLouTingNotify(List<int> seatID, List<MjTingInfo> info, bool needFire)
        {
            bool isCheckTing = MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsMingLouLiangTing);

            if (isCheckTing)
            {
                if (seatID != null && info != null && seatID.Count == info.Count && seatID.Count > 0)
                {
                    for (int i = 0; i < seatID.Count; i++)
                    {
                        MjDataManager.Instance.SetMinglouData(seatID[i], null, info[i]);
                    }
                }
                //fire
                if (needFire)
                {
                    EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimMingLouTing, seatID);
                }
            }
        }


        #endregion


        #region 游戏行为 ： 吃牌
        private void LogicMjChiNotify(int seatID, List<int> chiList, int chiCode)
        {
            //有人吃 
            int selfSeat = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            bool isSelf = seatID == selfSeat;
            int code1 = 0, code2 = 1;
            int index1 = -1, index2 = -1;

            if (isSelf)
            {
                MjActionCodeChi codeChi = MjDataManager.Instance.GetCurPaikouChi();
                if (codeChi != null)
                {
                    code1 = codeChi.selfCode1;
                    code2 = codeChi.selfCode2;
                    index1 = codeChi.selfCodeIndex1;
                    index2 = codeChi.selfCodeIndex2;
                }

                MjTingInfo refreshTingInfo = null;
                MjDataManager.Instance.SetPaikouTingConfirm(refreshTingInfo);
                this.SendCanPutCard();
            }
            else
            {
                MjDataManager.Instance.LoseCardByNum(chiList);
            }


            MjDataManager.Instance.SetPlayerHandChi(seatID, chiList);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimChi, seatID);
            AnimRemovePutCard(chiCode, false);
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_ControlCreate, GEnum.NameAnim.EMjChi, seatID, selfSeat, chiList, index1, index2, code1, code2, chiCode);
        }


        private void LogicOpActionChi(int seatID, List<MjRoundGetBuPai> mjChangeFlower, bool canFangMao,
            int mjRulerResult, List<int> gangList, List<MjTingInfo> tingInfoList, int mjCode, List<int> ciList)
        {
            bool canFlower = mjChangeFlower != null && mjChangeFlower.Count > 0;
            bool canClick = MjDataManager.Instance.Base_GetRulerLimitClick(canFlower, canFangMao);
            SendTurnPlay(seatID, canClick);

            //补牌
            if (canFlower)
            {
                this.LogicMjGetFlowerNotify(seatID, mjChangeFlower);
            }

            if (canFangMao)
            {
                ShowFangMao(seatID, mjRulerResult, gangList, tingInfoList, ciList);
            }
            else
            {
                //show tanban
                ShowTanban(seatID, mjRulerResult, tingInfoList, gangList, mjCode, ciList);
            }


        }

        #endregion

        #region 游戏行为 ： 胡牌
        private void LogicMjHuNotify(int nGangSeatID, List<int> seatIDlist, int lastPutSeat, int mjCode, EnumMjHuType huType)
        {
            if (nGangSeatID > 0)
            {
                //抢杠胡
                //setdata
            }
            else
            {
                //普通胡
                //set data
            }

            EnumMjOpAction baseHuType = lastPutSeat > 0 ? EnumMjOpAction.MjOp_HuPai : EnumMjOpAction.MjOp_Zimo;
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            for (int i = 0; i < seatIDlist.Count; i++)
            {
                MjDataManager.Instance.SetPlayerHasHu(seatIDlist[i]);
            }
            if (!seatIDlist.Contains(selfSeatID) && baseHuType == EnumMjOpAction.MjOp_Zimo)
            {
                MjDataManager.Instance.LoseCardByNum(mjCode, 1, false);
            }



            int curDesk = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            int gameType = MemoryData.DeskData.GetOneDeskInfo(curDesk).mjGameType;
            bool showPlayAnim = CardHelper.shouldPlayHuAnim(huType, gameType);

            float tipTime2 = showPlayAnim ? 1.4f : 0f;
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimHu, seatIDlist);
            MjDataManager.Instance.SetDealerPutState(false);


            //AnimPlayManager.Instance.PlayAnimFx(seatIDlist[0], 1.4f + tipTime2, GEnum.NameAnim.EMjControlAnimHu, huType, seatIDlist, lastPutSeat, nGangSeatID);
            AnimPlayManager.Instance.PlayAnim(seatIDlist[0], 1.4f + tipTime2, GEnum.NameAnim.EMjControlAnimHu, huType, seatIDlist, lastPutSeat, nGangSeatID);
            for (int i = 0; i < seatIDlist.Count; i++)
            {
                bool isSub = i > 0;

                if (baseHuType == EnumMjOpAction.MjOp_HuPai && !isSub)
                {
                    AnimRemovePutCard(mjCode, true);
                }
                AnimPlayManager.Instance.PlayAnim(seatIDlist[i], MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_Hu, GEnum.NameAnim.EMjHu, huType, lastPutSeat, nGangSeatID, seatIDlist[i], mjCode, isSub);
            }
        }

        #endregion

        #region 游戏行为 ： 放毛
        private void LogicMjFangMaoNotify(int seatID, List<int> maoList, bool hasBu)
        {
            //放毛
            MjDataManager.Instance.SetPlayerHandMao(seatID, maoList);
            if (seatID != MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {
                MjDataManager.Instance.LoseCardByNum(maoList);
            }

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimFangmao, seatID);
            SendTurnPlay(seatID);
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_Mao, GEnum.NameAnim.EMjFangMao, seatID, maoList, hasBu);
        }


        private void LogicOpActionFangMao(int seatID, List<MjRoundGetBuPai> mjChangeFlower, int mjRulerResult, List<int> gangList,
            List<MjTingInfo> tingInfoList, int mjCode, List<int> ciList)
        {
            //补牌
            if (mjChangeFlower != null && mjChangeFlower.Count > 0)
            {
                this.LogicMjGetFlowerNotify(seatID, mjChangeFlower);
            }

            //ruler
            ShowTanban(seatID, mjRulerResult, tingInfoList, gangList, mjCode, ciList);
        }


        #endregion

        #region 游戏行为 ： 摸牌（庄家开始）
        //轮到谁摸牌
        private void LogicTurnToPlayer(int mjCode, int seatID, int mjRulerResult, List<MjTingInfo> tingList, List<int> gangList, int getType,
            bool canFangMao, List<MjRoundGetBuPai> mjChangeFlower, List<int> ciList)
        {
            QLoger.LOG("[Mahjong LogicLog] : 麻将摸牌 + 座位号为  = {0} ， 摸得牌为 = {1} ",
                             seatID, mjCode);

            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            MjDataManager.Instance.SetPlayerHandAdd(seatID, mjCode);

            if (seatID == selfSeatID)
            {
                MjTingInfo refreshTingInfo = null;
                MjDataManager.Instance.SetPaikouTingConfirm(refreshTingInfo);
                MjDataManager.Instance.LoseCardByNum(mjCode, 1, false);
                this.SendCanPutCard();
            }

            bool hasFlower = mjChangeFlower != null && mjChangeFlower.Count > 0;
            //get code

            EventDispatcher.FireEvent(MJEnum.MjXingpaiChoose.XPC_EventClose.ToString());
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_Get, GEnum.NameAnim.EMjTurnGetCode, seatID, getType, mjCode, hasFlower);

            bool canClick = MjDataManager.Instance.Base_GetRulerLimitClick(hasFlower, canFangMao);
            SendTurnPlay(seatID, canClick);

            if (canFangMao)
            {
                ShowFangMao(seatID, mjRulerResult, gangList, tingList, ciList);
            }
            else
            {
                //change flower
                if (hasFlower)
                {
                    LogicMjGetFlowerNotify(seatID, mjChangeFlower);
                }
                ShowTanban(seatID, mjRulerResult, tingList, gangList, mjCode, ciList);
            }
        }


        //庄家开始
        private void LogicDealerBeginNotify(int seatID, int mjRulerResult, ulong userID, List<MjTingInfo> tingList,
            List<int> gangList, bool canFangMao, List<MjRoundGetBuPai> mjChangeFlower, List<int> ciList)
        {

            MjTingInfo refreshTingInfo = null;
            if (seatID != MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {
                if (tingList != null && tingList.Count > 0)
                {
                    refreshTingInfo = tingList[0];
                }
            }
            else
            {
                this.SendCanPutCard();
            }

            MjDataManager.Instance.SetPaikouTingConfirm(refreshTingInfo, false);

            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjDealerBegin, seatID);
            //做一下修饰
            bool hasFlower = mjChangeFlower != null && mjChangeFlower.Count > 0;
            MjDataManager.Instance.SetDealerPutState(true);
            bool canClick = MjDataManager.Instance.Base_GetRulerLimitClick(hasFlower, canFangMao);
            SendTurnPlay(seatID, canClick);

            if (canFangMao)
            {
                ShowFangMao(seatID, mjRulerResult, gangList, tingList, ciList, true);
            }
            else
            {
                //change flower
                if (hasFlower)
                {
                    LogicMjGetFlowerNotify(seatID, mjChangeFlower);
                }

                int mjCode = MjDataManager.Instance.GetPlayerFirstLastHand();
                ShowTanban(seatID, mjRulerResult, tingList, gangList, mjCode, ciList);
            }
        }

        #endregion


        //MJTODO : 重构 游戏行为的通知
        private void LogicMjActionNotify(
            List<int> chiList, EnumMjHuType huType, int lastPutSeat, int mjCode, EnumMjCodeType mjGangType,
            int mjRulerResult, int nGangSeatID, EnumMjOpAction mjAction, List<int> seatIDlist, List<MjTingInfo> tingInfoList, ulong userID,
            List<int> maoList, List<MjRoundGetBuPai> mjChangeFlower, bool isIndependent, int independentCount, List<int> gangList, List<int> handList, bool canFangMao,
            bool isQiangGang, bool isDark, int moCode, List<int> ciList)
        {
            switch (mjAction)
            {
                case EnumMjOpAction.MjOp_PutMj:
                    {
                        LogicOpActionPut(seatIDlist[0], mjCode, mjRulerResult, tingInfoList);
                    }
                    break;
                case EnumMjOpAction.MjOp_Peng:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 当前有玩家碰牌 + UserID = {0}  +  seatID  = {1}   +  打出的牌是  = {2} + 碰的牌有多少是独立的 = {3} ",
                            userID, seatIDlist[0], mjCode, independentCount);

                        LogicMjPengNotify(seatIDlist[0], mjCode, independentCount, lastPutSeat);
                        LogicOpActionPeng(mjChangeFlower, seatIDlist[0], canFangMao, mjRulerResult, gangList, tingInfoList, mjCode, ciList);
                    }
                    break;
                case EnumMjOpAction.MjOp_Gang:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 当前有玩家杠牌 + UserID = {0}  +  seatID  = {1}   +  打出的牌是  = {2} + 杠的牌有多少是独立的 = {3} ",
                            userID, seatIDlist[0], mjCode, independentCount);

                        LogicMjGangNotify(mjGangType, seatIDlist[0], mjCode, independentCount, mjRulerResult, lastPutSeat, isQiangGang);
                    }
                    break;
                case EnumMjOpAction.MjOp_Ting:
                    {
                        //change
                        QLoger.LOG("[Mahjong LogicLog] : 当前有玩家听牌 + UserID = {0}  +  seatID  = {1}   +  打出的牌是  = {2} ",
                            userID, seatIDlist[0], mjCode);

                        LogicMjTingNotify(seatIDlist[0], handList, mjCode);
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglou:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 当前有玩家明楼 + UserID = {0}  +  seatID  = {1}   +  打出的牌是  = {2} ",
                            userID, seatIDlist[0], mjCode);

                        LogicMjMingLouNotify(seatIDlist[0], handList, mjCode);
                    }
                    break;
                case EnumMjOpAction.MjOp_Chi:
                    {
                        LogicMjChiNotify(seatIDlist[0], chiList, mjCode);
                        LogicOpActionChi(seatIDlist[0], mjChangeFlower, canFangMao, mjRulerResult, gangList, tingInfoList, mjCode, ciList);
                    }
                    break;
                case EnumMjOpAction.MjOp_HuPai:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 当前有玩家胡牌 + UserID = {0}  +  seatID  = {1}   +  打出的牌是  = {2} ",
                           userID, seatIDlist[0], mjCode);


                        LogicMjHuNotify(nGangSeatID, seatIDlist, lastPutSeat, mjCode, huType);
                    }
                    break;
                case EnumMjOpAction.MjOp_Mao:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 当前有玩家放毛 + UserID = {0}  +  seatID  = {1}   +  放毛长度  = {2} ",
                           userID, seatIDlist[0], maoList.Count);

                        bool isBuNotify = mjChangeFlower != null && mjChangeFlower.Count > 0;
                        LogicMjFangMaoNotify(seatIDlist[0], maoList, isBuNotify);
                        LogicOpActionFangMao(seatIDlist[0], mjChangeFlower, mjRulerResult, gangList, tingInfoList, mjCode, ciList);
                    }
                    break;
                case EnumMjOpAction.MjOp_Pass:
                    {
                        if (isQiangGang)
                        {
                            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimPass, isQiangGang, seatIDlist[0]);
                        }
                    }
                    break;
            }

        }

        private void LogicGameStartNotify(int dealerID, int getMjSeatID, int getOffset, List<int> rollNum, int allCount, int roomID, List<List<int>> maogroup)
        {
            QLoger.LOG("[Mahjong LogicLog] : 当前一局游戏开始 + 庄家座位号为 = {0}  +  拿牌起始座位号为  = {1}   +  拿牌间隔为  = {2} ",
                            dealerID, getMjSeatID, getOffset);


            this.ClearGameReady();
            MjDataManager.Instance.ClearLastGame();
            ClearUp();
            MjDataManager.Instance.SetProcessGameStart(dealerID, getMjSeatID, getOffset, rollNum, allCount, roomID, maogroup);
            int selfDeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            MemoryData.DeskData.GetOneDeskInfo(selfDeskID).bouts += 1;

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongGameStart);
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlCloseMaskUI);
            if (iniOk)
            {
                bool needAnim = !MjDataManager.Instance.MjData.gameStartNum;
                if (needAnim)
                {
                    EventDispatcher.FireEvent(GEnum.NamedEvent.SysWebView_Close, true);
                    MjDataManager.Instance.MjData.gameStartNum = true;
                }

                float showTime = needAnim ? MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_GameStart : MahjongTimeConfig.Instance.MjSystemTime.Time_Notime;
                AnimPlayManager.Instance.PlayAnimCenter(showTime, GEnum.NameAnim.EMjGameStart, needAnim);
                MjDataManager.Instance.ProcessBasic_AnimPlayerTime(true, showTime);                 //压入时间
            }
        }


        private void LogicIniMjListNotify(List<int> mjList, int seatID, List<int>[] independentList)
        {
            QLoger.LOG("[Mahjong LogicLog] : 发送麻将数据 + 发送麻将的张数是  = {0}  +  发送的作为号为  = {1}  ",
                              mjList.Count, seatID);

            int dependentCount = MjDataManager.Instance.GetIndependentCount();

            int dealerID = MjDataManager.Instance.MjData.ProcessData.dealerSeatID;
            int getseatID = MjDataManager.Instance.MjData.ProcessData.getMjSeatID;
            int restCount = MjDataManager.Instance.MjData.ProcessData.getOffset;
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            int allCount = MjDataManager.Instance.MjData.ProcessData.curMjAmount;
            bool isMoreOne = MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsDaiCaiShen);
            int[] codeNum = CardHelper.GetMJCodeNumArray(allCount, dealerID, selfSeatID, isMoreOne);

            LogicRollWhenGameStart();
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_CreateCode, GEnum.NameAnim.EMjCreateCode, getseatID, restCount, codeNum, selfSeatID);
            MjDataManager.Instance.ProcessBasic_AnimPlayerTime(true, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_CreateCode);                 //压入时间
            LogicDealWhenGameStart(independentList, dependentCount, mjList);
        }


        private void LogicRollWhenGameStart()
        {
            List<int> rolls = MjDataManager.Instance.MjData.ProcessData.rollNums;
            MjDataManager.Instance.ProcessBasic_RollNums(rolls, -1);
        }


        private void LogicDealWhenGameStart(List<int>[] independentList, int dependentCount, List<int> mjList)
        {
            int[] selfCards = null;

            List<int> seatIDs = MjDataManager.Instance.GetAllSeatIDCurDesk(false);
            for (int i = 0; i < seatIDs.Count; i++)
            {
                int[] originalArray = MjDataManager.Instance.SetPlayerHandIni(seatIDs[i], mjList, dependentCount);
                if (originalArray != null)
                {
                    selfCards = originalArray;
                }
            }

            int dealerSeat = MjDataManager.Instance.MjData.ProcessData.dealerSeatID;
            EnumMjBeforeDispatch specialCheck = EnumMjBeforeDispatch.Null;

            if (MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISLiangLiu))
            {
                specialCheck = EnumMjBeforeDispatch.ISLiangLiu;
            }
            if (MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISlLiSi))
            {
                specialCheck = EnumMjBeforeDispatch.ISlLiSi;
            }
            if (MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsLiangSiDaYi))
            {
                specialCheck = EnumMjBeforeDispatch.IsLiangSiDaYi;
            }

            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_DealCard, GEnum.NameAnim.EMjDeal, selfCards, dealerSeat, independentList, dependentCount, specialCheck);
            MjDataManager.Instance.ProcessBasic_AnimPlayerTime(true, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_DealCard);                 //压入时间
        }


        /// <summary>
        ///  发牌后补花
        /// </summary>
        private void LogicChangeFlowerWhenDeal(object[] vars)
        {
            bool containChange = MjDataManager.Instance.CheckPlayDataOprateContain(MjPlayOprateType.OPRATE_CHANGEFLOWER);
            if (containChange)
            {
                //补花
                List<int> seatIDList = MjDataManager.Instance.GetAllSeatIDCurDesk(false);
                for (int i = 0; i < seatIDList.Count; i++)
                {
                    int curSeatID = seatIDList[i];
                    MahjongPlayType.MjPlayOprateChangeFlowerBase changeData = MjDataManager.Instance.MjData.ProcessData.processChangeFlower.CheckChangeContain(curSeatID);
                    if (changeData != null)
                    {
                        this.LogicMjChangeFlower(changeData);
                    }
                }
            }
        }

        /// <summary>
        /// 出牌后补花
        /// </summary>
        /// <param name="vars"></param>
        private void LogicChangeFlowerWhenPut(object[] vars)
        {
            MahjongPlayType.MjPlayOprateChangeFlowerBase changeData = MjDataManager.Instance.MjData.ProcessData.processChangeFlower.CheckChangeOne();
            if (changeData != null)
            {
                bool haveRuler = (bool)vars[0];
                this.LogicMjChangeFlower(changeData);
                GetTanBanRulerAndShow(false);
                SendCanPutCard();
            }
        }


        //桌上积分实时变化 
        private void LogicMjScoreChangeNotify(List<MjScore> changeList, int showType, bool isUpdate, int showSeatID)
        {
            if (changeList == null)
            {
                return;
            }
            int selfDeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;

            for (int i = 0; i < changeList.Count; i++)
            {
                int seatID = changeList[i].seatID;
                int scoreAll = changeList[i].score;
                if (isUpdate)
                {
                    MjDataManager.Instance.GetPlayerInfoByDeskIDAndSeatID(selfDeskID, seatID).score = scoreAll;
                }
                else
                {
                    MjDataManager.Instance.GetPlayerInfoByDeskIDAndSeatID(selfDeskID, seatID).score += scoreAll;
                }
            }


            EnumMjScroeChangeType scoreShowType = (EnumMjScroeChangeType)showType;
            if (isUpdate)
            {
                scoreShowType = EnumMjScroeChangeType.ScoreChangeTongbu;
            }

            switch (scoreShowType)
            {
                case EnumMjScroeChangeType.Null:
                    {
                        //瓢分
                        if (showSeatID > 0)
                        {
                            AnimPlayManager.Instance.PlayAnimFx(showSeatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_BalanceTuishui, GEnum.NameAnim.EMjScoreChangeNotify, changeList, scoreShowType);
                        }
                    }
                    break;
                case EnumMjScroeChangeType.ScoreChangeFollow:
                case EnumMjScroeChangeType.ScoreChangeTuishui:
                case EnumMjScroeChangeType.ScoreChangeTongbu:
                    {
                        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_BalanceTuishui, GEnum.NameAnim.EMjScoreChangeNotify, changeList, scoreShowType);
                    }
                    break;
            }

        }


        private void LogicTimeNotify(int showTime)
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSetTipTime, showTime);
        }


        private void LogicMjOnlineStateNotify(int seatID, bool state)
        {
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int uiSeatID = CardHelper.GetMJUIPosByServerPos(seatID, selfSeatID);

            long[] usrIDS = MjDataManager.Instance.GetAllPlayerIDByDeskID(deskID);
            if (usrIDS != null)
            {
                for (int i = 0; i < usrIDS.Length; i++)
                {
                    PlayerDataModel model = MemoryData.PlayerData.get(usrIDS[i]);
                    if (model.playerDataMj.seatID == seatID)
                    {
                        model.PlayerDataBase.IsOnline = state;
                    }
                }
            }

            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlOnlineState, seatID, state);
        }


        private void LogicMjPutDownNotify(List<int> dajiaoSeats, List<int> huazhuSeats, List<MjChangeScore> tuishuiList, List<List<MjPai>> handList, List<List<int>> independentList)
        {
            this.ClearGameReady();
            MjDataManager.Instance.SetPaikouTingConfirm(null);
            AnimPlayManager.Instance.waitSubStop = true;
            //tuipai
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_BalancePutdown, GEnum.NameAnim.EMjBalancePutdown, handList, independentList);
            //dajiao huazhu
            if ((dajiaoSeats != null && dajiaoSeats.Count > 0) || (huazhuSeats != null && huazhuSeats.Count > 0))
            {
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_BalanceHuaJiao, GEnum.NameAnim.EMjBalanceHuaJiao, dajiaoSeats, huazhuSeats);
            }

            // tuishui
            if (tuishuiList != null && tuishuiList.Count > 0)
            {
                for (int i = 0; i < tuishuiList.Count; i++)
                {
                    //int[] changeNums = GetScoreNums(tuishuiList[i].socreList);
                    AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_BalanceTuishui, GEnum.NameAnim.EMjBalanceTuishui, tuishuiList[i].socreList);
                }
            }
        }


        /// <summary>
        /// 麻将单局结算新
        /// </summary>
        /// <param name="newInfo"></param>
        private void LogicMjBalanceNewNotify(MjBalanceNew newInfo, bool isInGame)
        {
            if (newInfo != null)
            {
                if (!isInGame)
                {
                    newInfo.showByResult = true;
                }
            }

            if (isInGame)
            {
                ClearUp();
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjBalanceNewNotify, newInfo.deskID, newInfo.curBureau);
                EventDispatcher.FireEvent(GEnum.InGameModelEvents.ClearUI.ToString());
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMjBalanceDataRsp, newInfo);
            }
        }


        private void LogicMjGameOverNotify(bool isShow)
        {
            if (isShow)
            {
                SendMjResultState(true);
                MjDataManager.Instance.ClearLastGame();
                ClearUp();
            }
        }

        private void LogicRoomActionNotify(EnumMjDeskAction roomAction, int seatID, ulong userID, int closeTime, int gameState)
        {
            switch (roomAction)
            {
                case EnumMjDeskAction.MjRoom_Ready:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 有人准备 + 座位号  = {0} ",
                            seatID);

                        if (gameState == 0)
                        {
                            //配桌界面
                            var tempPlay = MjDataManager.Instance.GetPlayerInfoByDeskIDAndSeatID(MjDataManager.Instance.MjData.curUserData.selfDeskID, seatID);
                            tempPlay.hasReady = true;
                            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongReady, seatID);
                        }
                        else
                        {
                            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjDeskActionReady, seatID);
                        }
                    }
                    break;
                case EnumMjDeskAction.MjRoom_Quit:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 有人退出 + 座位号  = {0} ",
                            seatID);
                        var temp = MemoryData.DeskData.GetOneDeskInfo(MjDataManager.Instance.MjData.curUserData.selfDeskID);
                        temp.DeletePlayerInfo(seatID);
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongQuit, seatID);
                    }
                    break;
                case EnumMjDeskAction.MjRoom_Close:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 房间关闭 + 座位号  = {0} ",
                            seatID);

                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMahjongClose, seatID);
                    }
                    break;
                case EnumMjDeskAction.MjRoom_Trust:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 有人托管 + 座位号  = {0} ", seatID);
                    }
                    break;
                case EnumMjDeskAction.MjRoom_UnTrust:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 有人取消托管  + 座位号  = {0} ", seatID);
                    }
                    break;
                case EnumMjDeskAction.MjRoom_Agree:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 有人同意散桌  + 座位号  = {0} ", seatID);

                        MjDataManager.Instance.SetCloseState(seatID, 1);
                        //agree
                        float closeTimer = Convert.ToSingle(closeTime);
                        EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlCloseState, seatID, closeTimer, 1);
                    }
                    break;
                case EnumMjDeskAction.MjRoom_DisAgree:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 有人拒绝散桌  + 座位号  = {0} ", seatID);

                        MjDataManager.Instance.SetCloseState(seatID, 2);
                        //dis
                        float closeTimer = Convert.ToSingle(closeTime);
                        EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlCloseState, seatID, closeTimer, 0);
                    }
                    break;
                case EnumMjDeskAction.MjRoom_AskClose:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 有人请求散桌  + 座位号  = {0} ", seatID);

                        MjDataManager.Instance.SetCloseState(seatID, 1);
                        EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlCloseAsk, seatID, closeTime);
                    }
                    break;
                case EnumMjDeskAction.MjRoom_Continue:
                    {
                        QLoger.LOG("[Mahjong LogicLog] : 麻将桌上行为 : 游戏继续 + 座位号  = {0} ",
                            seatID);

                        EventDispatcher.FireEvent(GEnum.NamedEvent.EMahjongContinue, true);
                    }
                    break;
            }

        }


        //设置游戏开始时新规则状态 
        public void LogicBeforeDispatch(List<bool> checkValue)
        {
            this.SetCheckValueBeforeDispatch(checkValue);
        }

        public void LogicMjObligate(int offset, int mjCode)
        {
            string str = string.Format("通知客户端留牌 偏移{0} , 牌是 {1}", offset, mjCode);
            QLoger.ERROR(str);
        }


        //通知客户端进行买马 
        public void LogicMjBuyHorseCountNotify(List<int> maNumList)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_NormalTime, GEnum.NameAnim.EMjBuyHorse, maNumList, selfSeatID);
        }



        //通知客户端明楼
        public void LogicMjMinglouNotify(int seatID, List<int> handList)
        {
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_Ting, GEnum.NameAnim.EMjMinglou, seatID, handList);
        }
        private void LogicModuleReconnect()
        {
            Dictionary<System.Type, BaseLogicModule>.Enumerator item = _LogicModules.GetEnumerator();
            while (item.MoveNext())
            {
                if (!NullHelper.IsObjectIsNull(item.Current.Value))
                {
                    item.Current.Value.ModuleReconnect();
                }
            }
        }
        //断线重连 
        public void LogicMjReConned()
        {
            MahongDataModel.GameProcessReconned processData = MjDataManager.Instance.MjData.ProcessData.processReconned;
            if (processData != null)
            {
                AnimPlayManager.Instance.canPlayAnim = false;
                AnimPlayManager.Instance.waitSubStop = false;


                string messageProcess = CommonTools.ReflactionObject(processData);
                UserActionManager.AddLocalTypeLog("Log2", "开始真的走到断线重连");
                UserActionManager.AddLocalTypeLog("Log2", "重连的消息体 :" + messageProcess);


                int dealerID = processData.dealerSeat;
                int getMjSeatID = processData.getMjSeat;
                int getOffset = processData.getOffset;
                List<int> rollNum = processData.rollNums;
                int amount = processData.allCount;
                List<List<int>> maoGroup = processData.maoGroup;
                MjDataManager.Instance.SetProcessGameStart(dealerID, getMjSeatID, getOffset, rollNum, amount, processData.deskInfo.deskID, maoGroup);
                MjDataManager.Instance.MjData.gameStartNum = true;
                MjDataManager.Instance.SetCurCount(processData.curRestCount);

                //refresh UI && code
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlReconnedFresh);
            }

        }

        #endregion

        #region func for private



        //通知客户端补花 
        public void LogicMjGetFlowerNotify(int seatID, List<MjRoundGetBuPai> changeRound)
        {
            if (changeRound != null && changeRound.Count > 0)
            {
                bool isLastRound = false;
                for (int i = 0; i < changeRound.Count; i++)
                {
                    if (i == changeRound.Count - 1)
                    {
                        isLastRound = true;
                    }
                    AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.PartTime.Mj_Anim3D_ChangeFlower[0], GEnum.NameAnim.EMjChangeFlowerLose, seatID, changeRound[i].buType, changeRound[i].putCode, changeRound[i].putCodeDependent);
                    AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.PartTime.Mj_Anim3D_ChangeFlower[1], GEnum.NameAnim.EMjChangeFlowrGet, seatID, changeRound[i].changeCode, changeRound[i].getCodeDependent, isLastRound);
                }
            }
        }


        public void LogicMjChangeFlower(MahjongPlayType.MjPlayOprateChangeFlowerBase changeData)
        {
            if (changeData != null)
            {
                int curSeatID = changeData.seatID;
                List<MjRoundGetBuPai> roundList = new List<MjRoundGetBuPai>();
                for (int i = 0; i < changeData.roundList.Count; i++)
                {
                    MahjongPlayType.MjPlayOprateChangeFlowerRound roundData = changeData.roundList[i];
                    MjRoundGetBuPai roundItem = new MjRoundGetBuPai(roundData.handLoseList, roundData.handGetList, roundData.buType);
                    roundItem.SetDependentData(roundData.dependentLoseList, roundData.dependentGetList);
                    roundList.Add(roundItem);
                }
                LogicMjGetFlowerNotify(curSeatID, roundList);
            }
        }


        private void SendCanPutCard()
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlClearPutMessage);
        }


        private void GetTanBanRulerAndShow(bool showTurn)
        {
            //pass
            if (MjDataManager.Instance.MjData.ProcessData.processRulerResult != null)
            {
                int seatID = MjDataManager.Instance.MjData.ProcessData.processRulerResult.seatID;
                int mjRulerResult = MjDataManager.Instance.MjData.ProcessData.processRulerResult.rulerResult;
                List<MjTingInfo> tingList = MjDataManager.Instance.MjData.ProcessData.processRulerResult.tingList;
                List<int> gangList = MjDataManager.Instance.MjData.ProcessData.processRulerResult.gangList;
                List<int> ciList = MjDataManager.Instance.MjData.ProcessData.processRulerResult.ciList;

                //show tanban
                int cardID = MjDataManager.Instance.GetPlayerFirstLastHand();
                ShowTanban(seatID, mjRulerResult, tingList, gangList, cardID, ciList);

                if (showTurn)
                {
                    SendTurnPlay(seatID);
                }
            }
        }



        /// <summary>
        /// 展示弹板
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="mjRulerResult"></param>
        /// <param name="tingInfoList"></param>
        /// <param name="gangList"></param>
        /// <param name="mjCode"></param>
        private void ShowTanban(int seatID, int mjRulerResult, List<MjTingInfo> tingInfoList, List<int> gangList, int mjCode, List<int> ciList)
        {
            if (seatID == MjDataManager.Instance.MjData.curUserData.selfSeatID)
            {
                if (mjRulerResult != ConstDefine.MJ_PK_NULL)
                {
                    EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlAnimShowControl, seatID, mjRulerResult, tingInfoList, gangList, true, mjCode, ciList);
                    AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjXingpaiChoose.XPC_AnimShow.ToString());
                }
                else
                {
                    AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjXingpaiChoose.XPC_AnimShowTingTip.ToString(), tingInfoList);
                }
            }
        }

        private void AnimRemovePutCard(int cardID, bool needAnim)
        {
            int seatID = MjDataManager.Instance.MjData.ProcessData.lastPutSeatID;
            MjDataManager.Instance.SetPlayerHandRemovePut(seatID, cardID);
            MjDataManager.Instance.MjData.ProcessData.lastPutSeatID = -1;
            MjDataManager.Instance.MjData.ProcessData.lastPutCode = -1;
            AnimPlayManager.Instance.PlayAnimFx(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjRemovePutCard, seatID, cardID, needAnim);
        }


        private void SendTurnPlay(int seatID, bool canClick = true)
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlTurnPlayer, seatID, canClick);
        }


        private void ShowFangMao(int seatID, int mjRulerResult, List<int> gangList, List<MjTingInfo> tingInfoList, List<int> ciList, bool needAnim = false)
        {
            MjDataManager.Instance.SetRulerResult(seatID, mjRulerResult, gangList, tingInfoList, ciList);
            EventDispatcher.FireEvent(MJEnum.MjXingpaiChoose.XPC_EventShowMao.ToString(), seatID, false);

            //showfangmao
            if (needAnim)
            {
                AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjXingpaiChoose.XPC_AnimShowMao.ToString(), seatID);
            }
            else
            {
                AnimPlayManager.Instance.PlayAnimFx(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, MJEnum.MjXingpaiChoose.XPC_AnimShowMao.ToString(), seatID);
            }
        }


        private void ClearGameReady()
        {
            int curdeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            long[] users = MjDataManager.Instance.GetAllPlayerIDByDeskID(curdeskID);
            for (int i = 0; i < users.Length; i++)
            {
                MemoryData.PlayerData.get(users[i]).playerDataMj.hasReady = false;
            }

        }


        private void SetCheckValueBeforeDispatch(List<bool> checkValue)
        {
            MjDataManager.Instance.SetProcessStateInfo(checkValue);
        }

        //加载场景结束后
        private void SetUIByIniOk(object[] org)
        {
            //set Game Start
            iniOk = true;

            MjDataManager.Instance.MjData.isInMahjongScene = true;

            if (MjDataManager.Instance.MjData.isReconned)
            {
                this.LogicMjReConned();
                LogicModuleReconnect();
            }
            else
            {
                AnimPlayManager.Instance.canPlayAnim = true;
            }
        }

        //离开场景之前
        private void SetUIByOut(object[] org)
        {
            SendMjResultState(false);
            iniOk = false;
            AnimPlayManager.Instance.canPlayAnim = false;
            AnimPlayManager.Instance.ClearAllAnim();
            MjDataManager.Instance.ClearAllData();
            ClearUp();
        }


        //麻将重连
        private void MjReconned(object[] org)
        {
            bool isIndesk = (bool)org[0];
            bool isInGame = (bool)org[1];

            UserActionManager.AddLocalTypeLog("Log2", "IS InDesk :" + isIndesk + "Is InGame :" + isInGame);

            if (isIndesk)
            {
                if (isInGame)
                {
                    MjDataManager.Instance.MjData.isReconned = true;
                }
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoadMahjongScene, true, isInGame);

                if (iniOk && isInGame)
                {
                    AnimPlayManager.Instance.ClearAllAnim();
                    DG.Tweening.DOTween.KillAll(true);
                    //断线重连
                    this.LogicMjReConned();
                }
            }
            else
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoadMahjongScene, false, false);
            }
        }

        /// <summary>
        /// 语音SDK状态
        /// </summary>
        /// <param name="vars"></param>
        private void MjYuyinState(object[] vars)
        {
            bool state = (bool)vars[0];
            if (!state)
            {
                RecordManager.Instance.LogoutRecord();
            }
        }

        /// <summary>
        /// 关闭结算UI
        /// </summary>
        private void MjControlClickCloseResult(object[] vars)
        {
            //SendMjResultState(false);
        }

        #endregion


        #region func for ui call

        private void EControlOpAction(object[] objs)
        {
            ulong userID = MjDataManager.Instance.MjData.curUserData.selfUserID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int selfDeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;

            EnumMjOpAction action = (EnumMjOpAction)objs[0];
            switch (action)
            {
                case EnumMjOpAction.MjOp_PutMj:
                    {
                        int mjCode = (int)objs[1];
                        bool isIndependent = (bool)objs[2];
                        MahjongPlayType.MjChangeStart.EnumChangeStartSend type = MjDataManager.Instance.GetStartChangeSendType(mjCode);
                        switch (type)
                        {
                            case MahjongPlayType.MjChangeStart.EnumChangeStartSend.MjChangeType_Put:
                                {
                                    SendMjAction(userID, mjCode, EnumMjOpAction.MjOp_PutMj, seatID, selfDeskID, new List<int>(), isIndependent, new List<int>());
                                    LogicMjPutNotify(seatID, mjCode, isIndependent, false);
                                }
                                break;
                            case MahjongPlayType.MjChangeStart.EnumChangeStartSend.MjChangeType_Change:
                                {
                                    //发送手动补花
                                    SendMjChangeAction((long)userID, selfDeskID, seatID, mjCode, isIndependent);
                                }
                                break;
                        }
                    }
                    break;
                case EnumMjOpAction.MjOp_Peng:
                    {
                        int mjCode = MjDataManager.Instance.GetPaikouPeng();
                        SendMjAction(userID, mjCode, EnumMjOpAction.MjOp_Peng, seatID, selfDeskID, new List<int>(), false, new List<int>());
                    }
                    break;
                case EnumMjOpAction.MjOp_TingGang:
                    {
                        int mjCode = (int)objs[1];
                        SendMjAction(userID, mjCode, EnumMjOpAction.MjOp_TingGang, seatID, selfDeskID, new List<int>(), false, new List<int>());
                    }
                    break;
                case EnumMjOpAction.MjOp_Gang:
                    {
                        int mjCode = (int)objs[1];
                        SendMjAction(userID, mjCode, EnumMjOpAction.MjOp_Gang, seatID, selfDeskID, new List<int>(), false, new List<int>());
                    }
                    break;
                case EnumMjOpAction.MjOp_CiHu:
                    {
                        int mjCode = (int)objs[1];
                        SendMjAction(userID, mjCode, EnumMjOpAction.MjOp_Gang, seatID, selfDeskID, new List<int>(), false, new List<int>());
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglou:
                    {
                        int mjCode = (int)objs[1];
                        bool isIndependent = (bool)objs[2];
                        List<int> handList = (List<int>)objs[3];
                        if (handList == null)
                        {
                            handList = new List<int>();
                        }

                        SendMjAction(userID, mjCode, EnumMjOpAction.MjOp_Minglou, seatID, selfDeskID, new List<int>(), isIndependent, handList);
                    }
                    break;
                case EnumMjOpAction.MjOp_Ting:
                    {
                        int mjCode = (int)objs[1];
                        bool isIndependent = (bool)objs[2];
                        List<int> handList = (List<int>)objs[3];
                        if (handList == null)
                        {
                            handList = new List<int>();
                        }

                        SendMjAction(userID, mjCode, EnumMjOpAction.MjOp_Ting, seatID, selfDeskID, new List<int>(), isIndependent, handList);
                    }
                    break;
                case EnumMjOpAction.MjOp_Zimo:
                case EnumMjOpAction.MjOp_HuPai:
                    {
                        int mjCode = MjDataManager.Instance.GetPaikouHu();
                        SendMjAction(userID, mjCode, EnumMjOpAction.MjOp_HuPai, seatID, selfDeskID, new List<int>(), false, new List<int>());
                    }
                    break;
                case EnumMjOpAction.MjOp_Pass:
                    {
                        SendMjAction(userID, -1, EnumMjOpAction.MjOp_Pass, seatID, selfDeskID, new List<int>(), false, new List<int>());
                    }
                    break;
                case EnumMjOpAction.MjOp_Chi:
                    {
                        MjActionCodeChi chiCode = (MjActionCodeChi)objs[1];
                        MjDataManager.Instance.SetCurPaikouChi(chiCode);
                        SendMjAction(userID, chiCode.chiCode, EnumMjOpAction.MjOp_Chi, seatID, selfDeskID, chiCode.chiList, false, new List<int>());
                    }
                    break;
            }

        }

        private void EControlChangeThree(object[] objs)
        {
            List<int> chooseChangeThree = (List<int>)objs[0];

            if (chooseChangeThree.Count == 3)
            {
                int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
                int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

                this.SendMjChangeThree(deskID, seatID, chooseChangeThree);
            }
        }

        //请求解散桌子
        private void EControlSendClose(object[] objs)
        {
            ulong userID = MjDataManager.Instance.MjData.curUserData.selfUserID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;

            //MjDataManager.Instance.SetCloseStart(seatID);
            SendMjRoomAction(userID, seatID, deskID, EnumMjDeskAction.MjRoom_Close);
        }

        private void EControlSendCloseAnwser(object[] objs)
        {
            int index = (int)objs[0];

            EnumMjDeskAction action = index == 0 ? EnumMjDeskAction.MjRoom_Agree : EnumMjDeskAction.MjRoom_DisAgree;

            ulong userID = MjDataManager.Instance.MjData.curUserData.selfUserID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;

            SendMjRoomAction(userID, seatID, deskID, action);
        }


        /// <summary>
        /// 请求关闭小结算
        /// </summary>
        /// <param name="objs"></param>
        private void EControlSendCloseBalance(object[] objs)
        {
            //发送准备
            ulong userID = MjDataManager.Instance.MjData.curUserData.selfUserID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;

            SendMjRoomAction(userID, seatID, deskID, EnumMjDeskAction.MjRoom_Ready);
        }


        private void EControlSendMinglou(object[] objs)
        {
            List<int> handlist = (List<int>)objs[0];
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;

            SenMjMinglou(deskID, seatID, handlist);
        }

        private void EControlSendFangMao(object[] objs)
        {
            List<int> maoList = null;
            if (objs != null && objs.Length > 0)
            {
                maoList = (List<int>)objs[0];
            }

            if (maoList != null && maoList.Count == 3)
            {
                //send fangmao
                ulong userID = MjDataManager.Instance.MjData.curUserData.selfUserID;
                int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
                int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;

                SendMjActionFangMao(userID, -1, EnumMjOpAction.MjOp_Mao, seatID, deskID, maoList);
            }
            else
            {
                //pass
                GetTanBanRulerAndShow(true);
            }
        }





        private void EControlSendChooseBtn(object[] objs)
        {
            EnumMjSpecialCheck chooseType = (EnumMjSpecialCheck)objs[0];
            int paoNum = (int)objs[1];

            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            if (chooseType != EnumMjSpecialCheck.MjBaseCheckType_SiDunSiDing)
            {
                SendMjBtnChoose(chooseType, deskID, seatID, paoNum);
            }
            else
            {
                bool chooseValue = false;
                if (paoNum == 0)
                {
                    chooseValue = true;
                }

                //发送死蹲死顶
            }


        }


        private void EControlSendChooseQue(object[] obj)
        {
            int queType = (int)obj[0];
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            this.SendMjConfirm(deskID, seatID, queType);
        }


        private void EControlRestCountChange(object[] objs)
        {
            int count = (int)objs[0];
            MjDataManager.Instance.CutDownCurRestCount(count);
        }


        private void MjControlSendGetBalanceNew(object[] objs)
        {
            int deskID = (int)objs[0];
            int curBureau = (int)objs[1];

            long userID = MemoryData.UserID;
            this.SendMjBalanceNew(userID, deskID, curBureau);
        }

        #endregion


        #endregion


        #region 事件处理
        public void Set3DTableMessage(object[] param)
        {
            int selfDeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            if (selfDeskID <= 0)
            {
                DebugPro.DebugError("Set3DTableMessage error:selfDeskID,", selfDeskID);
                return;
            }
            MjDeskInfo deskInfo = MemoryData.DeskData.GetOneDeskInfo(selfDeskID);
            if (deskInfo == null)
            {
                DebugPro.DebugError("MjDeskInfo error:,", selfDeskID);
                return;
            }
            //List<int> rulerList = deskInfo.mjRulerList;
            int maxOdds = deskInfo.maxDouble;

            bool showTableNum = !MjDataManager.Instance.MjData.gameStartNum;

            string[] showArray = MemoryData.MahjongPlayData.GetMahjongPlayOptionStr(deskInfo.mjGameConfigId, deskInfo.mjRulerSelected);
            EventDispatcher.FireEvent(GEnum.Table3DEvents.Table3D_ShowInfo.ToString(), selfDeskID, maxOdds, showArray, showTableNum);
        }


        ///// <summary>
        ///// 出牌的弹板
        ///// </summary>
        ///// <param name="vars"></param>
        //private void SetPaiKou(object[] vars)
        //{
        //    int rulerResult = (int)vars[0];
        //    int mjCode = (int)vars[1];
        //    List<int> handList = (List<int>)vars[2];
        //    //有人出牌触发到我个人的逻辑
        //    if (rulerResult != ConstDefine.MJ_PK_NULL)
        //    {
        //        //目前有人出牌只能触发到我的碰 杠  胡 吃
        //        MjPaiKou paikouStruct = MjDataManager.Instance.CheckPaikou(rulerResult);
        //        if (paikouStruct.canPeng)
        //        {
        //            //可以碰
        //            MjDataManager.Instance.SetPaikouPeng(mjCode);
        //        }
        //        if (paikouStruct.canHu)
        //        {
        //            //可以胡
        //            paikouStruct.canZiMo = false;
        //            MjDataManager.Instance.SetPaiKouHu(mjCode);

        //        }
        //        if (paikouStruct.canGang)
        //        {
        //            //可以杠
        //            MjDataManager.Instance.SetPaikouGang(new List<int>() { mjCode });
        //        }

        //        if (paikouStruct.canChi)
        //        {
        //            MjDataManager.Instance.SetPaikouChi(mjCode, handList);
        //        }

        //        QLoger.LOG("[Mahjong LogicLog] : 当前玩家出牌触发到个人逻辑 + 碰 ： {0} + 杠 : {1} + 听 : {2} + 胡 : {3} + 吃 ： {4}",
        //            paikouStruct.canPeng, paikouStruct.canGang, paikouStruct.canTing, paikouStruct.canHu, paikouStruct.canChi);
        //        EventDispatcher.FireEvent(GEnum.MJUIManagerEvents.MJUIM_PengGangTingChi.ToString(), paikouStruct);
        //    }
        //}



        #endregion
    }

}
