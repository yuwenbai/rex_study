/**
 * @Author Xin.Wang
 * 
 * 打扰了 这个是重构的第一步
 * 大概上呢 这个脚本就是把logicnew的特殊玩法给拆到这边来
 * 为什么要这样呢
 * 因为logicnew已经四五千行啦 这样大 以后可怎么把锅甩给别人啊
 * 就酱
 *
 */

using System.Collections.Generic;
using MahjongPlayType;

namespace projectQ
{

    public class MahjongLogicSpecial
    {
        #region Net
        #region Send

        /// <summary>
        /// 发送坎牌闹庄末留数据
        /// </summary>
        /// <param name="kanState"></param>
        /// <param name="moState"></param>
        /// <param name="naoState"></param>
        public void SendMjKanNaoMoResult(bool kanState, bool moState, bool naoState)
        {
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            ModelNetWorker.Instance.MjReqNaoZhuangMoLiuKanPai(deskID, seatID, kanState, moState, naoState);
        }


        /// <summary>
        /// 发送选飘数据
        /// </summary>
        /// <param name="rulerList"></param>
        /// <param name="valueList"></param>
        public void SendMjXuanPiaoResult(List<int> rulerList, List<int> valueList)
        {
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            ModelNetWorker.Instance.MjReqGameingRulerSet(deskID, seatID, rulerList, valueList);
        }

        /// <summary>
        /// 发送死蹲死顶数据
        /// </summary>
        /// <param name="state"></param>
        public void SendMjDundingResult(bool state)
        {
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            ModelNetWorker.Instance.MjReqSidunSiding(deskID, seatID, state);
        }

        #endregion


        #endregion

        #region Commmon


        /// <summary>
        /// 开始选择
        /// </summary>
        /// <param name="showTime"></param>
        /// <param name="animType"></param>
        /// <param name="defaultValue"></param>
        /// <param name="canChooseValue"></param>
        private void SetAnimChooseStart(float showTime, GEnum.NameAnim animType, int defaultValue, List<int> canChooseValue)
        {
            AnimPlayManager.Instance.PlayAnimCenter(showTime, animType, EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect, defaultValue, canChooseValue);
        }


        /// <summary>
        /// 自己选择
        /// </summary>
        /// <param name="showTime"></param>
        /// <param name="animType"></param>
        /// <param name="seatID"></param>
        /// <param name="chooseValue"></param>
        private void SetAnimChooseState(float showTime, GEnum.NameAnim animType, int chooseValue)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int uiSeatID = CardHelper.GetMJUIPosByServerPos(selfSeatID, selfSeatID);
            List<int> seatIDs = new List<int>() { uiSeatID };
            List<int> nums = new List<int>() { chooseValue };

            AnimPlayManager.Instance.PlayAnimCenter(showTime, animType, EnumMjSelectSubType.MjSelectSubType_WAIT_Select, seatIDs, nums);
        }


        /// <summary>
        /// 他人选择
        /// </summary>
        /// <param name="showTime"></param>
        /// <param name="animType"></param>
        /// <param name="seatID"></param>
        private void SetAnimChooseUpdate(float showTime, GEnum.NameAnim animType, int seatID)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int uiSeatID = CardHelper.GetMJUIPosByServerPos(seatID, selfSeatID);

            AnimPlayManager.Instance.PlayAnimCenter(showTime, animType, uiSeatID);
        }



        private void SetAnimChooseResult(float showTime, GEnum.NameAnim animType, List<int> seatIDList, List<int> chooseList)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            List<int> uiSeatID = new List<int>();
            List<int> uiPaoNum = new List<int>();

            for (int i = 0; i < seatIDList.Count; i++)
            {
                int uiSeat = CardHelper.GetMJUIPosByServerPos(seatIDList[i], selfSeatID);
                if (uiSeat == 0)
                {
                    continue;
                }

                //other
                uiSeatID.Add(uiSeat);
                uiPaoNum.Add(chooseList[i]);
            }


            AnimPlayManager.Instance.PlayAnimCenter(showTime, animType, EnumMjSelectSubType.MjSelectSubType_RESULT_Select, uiSeatID, uiPaoNum);
        }



        #endregion

        #region 下鱼

        public void LogicMjAskReqXiayu(EnumMjSelectSubType curType, int defaultValue, List<int> canChooseValue)
        {
            SetAnimChooseStart(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimXiayu, defaultValue, canChooseValue);
        }

        public void LogicMjRspXiayu(EnumMjSelectSubType curType, int seatID, int chooseValue)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            if (selfSeatID == seatID)
            {
                SetAnimChooseState(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimXiayu, chooseValue);
            }
            else
            {
                SetAnimChooseUpdate(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimXiayuUpdate, seatID);
            }
        }

        public void LogicMjNotifyXiayu(EnumMjSelectSubType curType, List<int> seatIDList, List<int> chooseList)
        {
            SetAnimChooseResult(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimXiayu, seatIDList, chooseList);
        }

        #endregion

        #region 换三张

        //通知客户端换三张 
        public void LogicAskChangeThree(List<int> changeThreeList, int clockType)
        {
            EnumClockType curClockType = (EnumClockType)clockType;

            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_NormalTime, GEnum.NameAnim.EMjChangeThreeNotify, changeThreeList, curClockType);
        }

        public void LogicRspChangeThree(int deskID, int seatID, List<int> mjList)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            if (selfSeatID == seatID)
            {
                MjDataManager.Instance.SetPlayerHandChangeThreeLose(mjList);
                List<int> uiseat = new List<int>() { 0 };
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjChangeThreeLose, uiseat, mjList);
            }
            else
            {
                int uiSeatID = CardHelper.GetMJUIPosByServerPos(seatID, selfSeatID);
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjChangeThreeUpdate, uiSeatID, true, mjList);
            }

        }


        public void LogicChangeThreeNotify(int deskID, int seatID, int clockType, List<int> mjList, Dictionary<int, MjChangeThreeData> changeList)
        {
            MjDataManager.Instance.SetPlayerHandChangeThreeAdd(mjList);

            List<int> loseSeat = MjDataManager.Instance.GetAllUiSeatCurDesk(true);
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.PartTime.Mj_Anim3d_ChangeThree[0] + MahjongTimeConfig.Instance.MjSystemTime.PartTime.Mj_Anim3d_ChangeThree[1], GEnum.NameAnim.EMjChangeThreeGet, loseSeat, clockType, mjList, true, changeList);
        }

        #endregion

        #region 定缺

        //定缺 
        //通知客户端定缺 
        public void LogicMjAskReqConfirm(int confirmType)
        {
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjDingQueNotify, confirmType);
        }

        //定缺的个人结果
        public void LogicMjRspConfirm(int deskID, int seatID, int nType)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int uiSeatID = CardHelper.GetMJUIPosByServerPos(seatID, selfSeatID);

            List<int> uiSeat = new List<int>() { uiSeatID };
            List<int> queType = new List<int>() { nType };

            MjDataManager.Instance.SetQueType(uiSeatID, nType);

            if (uiSeatID == 0)
            {
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjDingQueResult, uiSeat, queType);
            }
            else
            {
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjDingQueUpdate, uiSeatID);
            }
        }

        //定缺所有人结果
        public void LogicMjConfirmNotify(int deskID, List<int> seatIDList, List<int> confirmType)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            List<int> uiSeatID = new List<int>();
            List<int> uiType = new List<int>();

            for (int i = 0; i < seatIDList.Count; i++)
            {
                int uiSeat = CardHelper.GetMJUIPosByServerPos(seatIDList[i], selfSeatID);
                MjDataManager.Instance.SetQueType(uiSeat, confirmType[i]);
                if (uiSeat == 0)
                {
                    continue;
                }

                uiSeatID.Add(uiSeat);
                uiType.Add(confirmType[i]);
            }

            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjDingQueResult, uiSeatID, uiType);
        }

        #endregion

        #region 下跑

        //下跑
        //通知客户端下跑
        public void LogicMjAskReqPao(int defaultValue, List<int> values)
        {
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjXiaPaoStart, values, defaultValue);
        }

        //下跑的单人结果
        public void LogicMjRspPao(int deskID, int seatID, int nValue)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int uiSeatID = CardHelper.GetMJUIPosByServerPos(seatID, selfSeatID);
            List<int> seatIDs = new List<int>() { uiSeatID };
            List<int> nums = new List<int>() { nValue };

            if (uiSeatID == 0)
            {
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjXiaPao, seatIDs, nums);
            }
            else
            {
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjXiaPaoUpdate, uiSeatID);
            }
        }


        //下跑所有人结果
        public void LogicMjPaoNotify(int deskID, List<int> seatIDList, List<int> valueList)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            List<int> uiSeatID = new List<int>();
            List<int> uiPaoNum = new List<int>();

            for (int i = 0; i < seatIDList.Count; i++)
            {
                int uiSeat = CardHelper.GetMJUIPosByServerPos(seatIDList[i], selfSeatID);
                if (uiSeat == 0)
                {
                    continue;
                }

                //other
                uiSeatID.Add(uiSeat);
                uiPaoNum.Add(valueList[i]);
            }

            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjXiaPao, uiSeatID, uiPaoNum);
        }

        #endregion

        #region 下炮子

        //下炮子
        public void LogicMjAskReqPaoZi(int defaultValue, List<int> values)
        {
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjXiaPaoZiStart, values, defaultValue);
        }

        public void LogicMjRspPaoZi(int deskID, int seatID, int nValue)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            int uiSeatID = CardHelper.GetMJUIPosByServerPos(seatID, selfSeatID);
            List<int> seatIDs = new List<int>() { uiSeatID };
            List<int> nums = new List<int>() { nValue };

            if (uiSeatID == 0)
            {
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_NormalTime, GEnum.NameAnim.EMjXiaPaoZi, seatIDs, nums);
            }
            else
            {
                AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_NormalTime, GEnum.NameAnim.EMjXiaPaoZiUpdate, uiSeatID);
            }

        }

        public void LogicMjPaoNotifyZi(int deskID, List<int> seatIDList, List<int> valueList)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            List<int> uiSeatID = new List<int>();
            List<int> uiPaoNum = new List<int>();

            for (int i = 0; i < seatIDList.Count; i++)
            {
                int uiSeat = CardHelper.GetMJUIPosByServerPos(seatIDList[i], selfSeatID);
                if (uiSeat == 0)
                {
                    continue;
                }

                //other
                uiSeatID.Add(uiSeat);
                uiPaoNum.Add(valueList[i]);
            }

            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_NormalTime, GEnum.NameAnim.EMjXiaPaoZi, uiSeatID, uiPaoNum);
        }

        #endregion

        #region 扎马，一码全中，抓鸟

        public void LogicOpenMa(int type, List<MjHorse> horseResult, List<MjScore> scoreResult)
        {
            AnimPlayManager.Instance.waitSubStop = true;
            EnumMjOpenMaType openType = (EnumMjOpenMaType)type;
            if (openType == EnumMjOpenMaType.MaiMa)
            {
                //买马s
                LogicMjBuyHorseNotify(horseResult);
            }
            else
            {
                if (horseResult != null && horseResult.Count > 0)
                {
                    float time = CardHelper.GetUITotalAimTime(horseResult.Count, ConstDefine.OpenMaFlippingTime, ConstDefine.OpenMaFlippingIntervalTime, ConstDefine.UICloseDlay);

                    AnimPlayManager.Instance.PlayAnimCenter(time, GEnum.NameAnim.EMjAnimOpenMaStepShow, openType, horseResult);
                }


                if (scoreResult != null && scoreResult.Count > 0)
                {
                    List<int> showSeatID = new List<int>();
                    for (int i = 0; i < scoreResult.Count; i++)
                    {
                        if (scoreResult[i].score < 0)
                        {
                            showSeatID.Add(scoreResult[i].seatID);
                        }
                    }

                    if (showSeatID.Count > 0)
                    {
                        AnimPlayManager.Instance.PlayAnimCenter(ConstDefine.Mj_AnimFx_MaFly, GEnum.NameAnim.EMjAnimOpenMaStepFly, openType, showSeatID);
                    }
                }

            }

        }


        //通知买马结果(可以先不买)
        public void LogicMjBuyHorseNotify(List<MjHorse> horseResult)
        {
            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Mj_Anim_BuyHorse, GEnum.NameAnim.EMjBuyHorseResult, horseResult);
        }

        #endregion

        #region 表演

        public void LogicMjPerformanceNotify(int curSeatID, bool isPut, List<int> seatID, List<int> actionValue, bool isHunDiao)
        {
            MjPerformanceData performData = new MjPerformanceData(seatID, actionValue, isPut, curSeatID);
            performData.isHunDiao = isHunDiao;
            AnimPlayManager.Instance.PlayAnim(curSeatID, MahjongTimeConfig.Instance.MjSystemTime.Part2DTime.Mj_Anim2d_BaseTips, GEnum.NameAnim.EMjAnimPerformance, performData);
        }


        #endregion

        #region 坎牌 末留 闹庄
        public void LogicMjKanNaoMo()
        {
            int seatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;

            bool kanState = MjDataManager.Instance.GetKanNaoMoDataState(seatID, MjCanNaoMoData.EnumCommonType.CanPai);

            bool moState = MjDataManager.Instance.GetKanNaoMoDataState(seatID, MjCanNaoMoData.EnumCommonType.MoLiu);

            bool naoState = MjDataManager.Instance.GetKanNaoMoDataState(seatID, MjCanNaoMoData.EnumCommonType.NaoZhuang);


            SendMjKanNaoMoResult(kanState, moState, naoState);
        }

        public void LogicMjKanNaoMoNotify(int subState, bool needSend)
        {
            EnumMjSelectSubType eSubType = (EnumMjSelectSubType)subState;
            if (needSend)
            {
                LogicKanNaoMoNotifyNormal(eSubType);
            }
            else
            {
                LogicKanNaoMoNotifyReconned(eSubType);
            }
        }

        private void LogicKanNaoMoNotifyNormal(EnumMjSelectSubType eSubType)
        {
            switch (eSubType)
            {
                case EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect:
                    {
                        //start
                        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimKanNaoMoNotify);
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_RESULT_Select:
                    {
                        List<int> seatList = MjDataManager.Instance.GetAllSeatIDCurDesk(false);
                        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimKanNaoMoResult, eSubType, seatList);
                    }
                    break;
            }
        }

        private void LogicKanNaoMoNotifyReconned(EnumMjSelectSubType eSubType)
        {
        }

        public void LogicMjKanNaoMoRsp(int seatID, bool kanState, bool naoState, bool moState)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            if (seatID == selfSeatID)
            {
                MjDataManager.Instance.SetKanPaiData(false, seatID, kanState, null, true);
                MjDataManager.Instance.SetNaozhuangData(false, seatID, naoState, null, true);
                MjDataManager.Instance.SetMoLiuData(false, seatID, moState, null, true);

                AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimKanNaoMoResult, EnumMjSelectSubType.MjSelectSubType_WAIT_Select, new List<int>() { selfSeatID });
            }
            else
            {
                AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimKanNaoMoUpdate, seatID);
            }
        }


        public void LogicMjKanNaoMoData(List<MjCanNaoMoServerData> dataList)
        {
            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    MjCanNaoMoServerData dataItem = dataList[i];
                    switch (dataItem.curType)
                    {
                        case MjCanNaoMoData.EnumCommonType.CanPai:
                            {
                                MjDataManager.Instance.SetKanPaiData(true, dataItem.seatID, dataItem.curState, dataItem.valueList, dataItem.haveChoose);
                            }
                            break;
                        case MjCanNaoMoData.EnumCommonType.NaoZhuang:
                            {
                                MjDataManager.Instance.SetNaozhuangData(true, dataItem.seatID, dataItem.curState,
                                    dataItem.valueList, dataItem.haveChoose, dataItem.chooseDefault);
                            }
                            break;
                        case MjCanNaoMoData.EnumCommonType.MoLiu:
                            {
                                MjDataManager.Instance.SetMoLiuData(true, dataItem.seatID, dataItem.curState, dataItem.valueList, dataItem.haveChoose);
                            }
                            break;
                    }
                }
            }
        }


        #endregion

        #region 选飘(胡飘 杠飘 飘金钓鱼 飘素自摸 跑头 跑杠)

        public void LogicMjXuanPiaoNotify(int subState, bool needSend)
        {
            EnumMjSelectSubType eSubType = (EnumMjSelectSubType)subState;
            if (needSend)
            {
                LogicMjXuanPiaoNotifyNormal(eSubType);
            }
        }


        public void LogicMjXuanPiaoNotifyNormal(EnumMjSelectSubType eSubType)
        {
            switch (eSubType)
            {
                case EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect:
                    {
                        //start
                        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimXuanPiao, EnumThreeStepSelect.Start);
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_RESULT_Select:
                    {
                        //end
                        List<int> seatList = MjDataManager.Instance.GetAllSeatIDCurDesk(true);
                        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimXuanPiao, EnumThreeStepSelect.Close, seatList);
                    }
                    break;
            }
        }

        /// <summary>
        /// 选择结果数据通知
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="curType"></param>
        /// <param name="curValue"></param>
        public void LogicMjXuanPiaoRspData(int seatID, List<int> curType, List<int> curValue)
        {
            if (curType != null && curType.Count > 0 && curValue != null && curValue.Count == curType.Count)
            {
                for (int i = 0; i < curType.Count; i++)
                {
                    MjDataManager.Instance.UpdateXuanPiaoData(seatID, curType[i], curValue[i]);
                }
            }
        }

        /// <summary>
        /// 选择结果通知
        /// </summary>
        /// <param name="seatID"></param>
        public void LogicMjXuanPiaoRsp(int seatID)
        {
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimXuanPiao, EnumThreeStepSelect.WaitOther, seatID);
        }


        public void LogicMjXuanPiaoData(List<MjXuanPiaoServerData> dataList)
        {
            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    MjXuanPiaoServerData dataItem = dataList[i];
                    int rulerID = dataItem.commonData.rulerID;
                    List<int> valueList = dataItem.commonData.valueList;

                    for (int j = 0; j < dataItem.playerData.Count; j++)
                    {
                        MjDataManager.Instance.SetXuanPiaoData(dataItem.playerData[j].seatID, rulerID, dataItem.playerData[j].curValue
                            , valueList, dataItem.playerData[j].haveChoose, dataItem.commonData.showType);
                    }
                }
            }
        }


        public void LogicMjXuanPiaoResult(int nVlaue = -1)
        {
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            MjXuanPiaoData data = MjDataManager.Instance.GetXuanPiaoDataBySeatID(selfSeatID);

            if (data.Data != null && data.Data.Count > 0)
            {
                List<int> rulerID = new List<int>();
                List<int> valueList = new List<int>();
                for (int i = 0; i < data.Data.Count; i++)
                {
                    int ru = (int)data.Data[i].enumType;
                    rulerID.Add(ru);
                    if (nVlaue >= 0)
                    {
                        data.Data[i].curChooseValue = nVlaue;
                        valueList.Add(nVlaue);
                    }
                    else
                    {
                        int va = data.Data[i].curChooseValue;
                        valueList.Add(va);
                    }

                }

                this.SendMjXuanPiaoResult(rulerID, valueList);
            }
        }

        #endregion

        #region 死蹲死顶
        /// <summary>
        /// 死蹲死顶通知
        /// </summary>
        /// <param name="subState"></param>
        /// <param name="needSend"></param>
        public void LogicMjSiDunSiDingNotify(int subState, bool needSend)
        {
            EnumMjSelectSubType eSubType = (EnumMjSelectSubType)subState;
            if (needSend)
            {
                LogicMjSiDunSiDingNotifyNormal(eSubType);
            }
        }

        /// <summary>
        /// 表现逻辑
        /// </summary>
        /// <param name="eSubType"></param>
        private void LogicMjSiDunSiDingNotifyNormal(EnumMjSelectSubType eSubType)
        {
            switch (eSubType)
            {
                case EnumMjSelectSubType.MjSelectSubType_WAIT_NoSelect:
                    {
                        //第一阶段
                        List<int> seatList = MjDataManager.Instance.GetAllSeatIDCurDesk(false);
                        int dealerID = MjDataManager.Instance.MjData.ProcessData.dealerSeatID;

                        for (int i = 0; i < seatList.Count; i++)
                        {
                            MjDataManager.Instance.UpdateDunDingData(seatList[i], dealerID, 0);
                        }
                        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimSiDunSiDing, EnumThreeStepSelect.Start);
                    }
                    break;
                case EnumMjSelectSubType.MjSelectSubType_RESULT_Select:
                    {
                        //第三阶段
                        List<int> seatList = MjDataManager.Instance.GetAllSeatIDCurDesk(true);
                        AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimSiDunSiDing, EnumThreeStepSelect.Close, seatList);
                    }
                    break;
            }
        }


        /// <summary>
        /// 个人选择结果
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="chooseState"></param>
        public void LogicMjSiDunSiDingRspData(int seatID, bool chooseState)
        {
            int dealerSeatID = MjDataManager.Instance.MjData.ProcessData.dealerSeatID;
            int stateValue = chooseState ? 1 : 2;
            MjDataManager.Instance.UpdateDunDingData(seatID, dealerSeatID, stateValue);
            AnimPlayManager.Instance.PlayAnim(seatID, MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.NameAnim.EMjAnimSiDunSiDing, EnumThreeStepSelect.WaitOther, seatID, chooseState);
        }


        #endregion


        #region 包次区域
        public void ShowBaociTip(object[] vars)
        {
            bool needShow = (bool)vars[0];
            string showStr = (string)vars[1];

            AnimPlayManager.Instance.PlayAnimCenter(MahjongTimeConfig.Instance.MjSystemTime.Time_Notime, GEnum.EnumMjOprateBaoci.BC_AnimShow.ToString(), needShow, showStr);
        }

        #endregion


    }
}

