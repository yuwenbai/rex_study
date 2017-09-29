using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;
using System;
namespace projectQ
{
    public partial class MahjongUIManager
    {
        //断线重连之后刷新数据
        private void EMjControlReconnedRefresh(object[] obj)
        {
            CloseUISystem("UIMahjongCloseState");
            ClearWhenGameContinue();

            //刷新手的信息
            ResetPlayerHand();
            SetResultState();


            //tableMessage
            int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            MahongDataModel.GameProcessReconned processData = MjDataManager.Instance.MjData.ProcessData.processReconned;


            if (processData.gameState == (int)MjGameState.MjGameState_GameIngIdle)
            {
                ///空闲状态
                RefreshZuobi(true);
                _static.IniRolls(2);
            }
            else
            {
                // 庄家
                if (!MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISDuoHu))
                {
                    int dealerID = processData.dealerSeat;
                    int uiDealerID = CardHelper.GetMJUIPosByServerPos(dealerID, selfSeatID);
                    SetPlayerDealer(uiDealerID, false);
                }

                //表演
                EMjControlReconnedPerformance(processData.performData);

                //包次
                if (processData.showBaoci)
                {
                    MjDataManager.Instance.MjData.ProcessData.processBaociTip.CheckAndShowBaociTip();
                }

                //code & hand
                int gangNum = 0;
                bool hasControl = false;
                for (int i = 1; i < 5; i++)
                {
                    int readIndex = 0;
                    List<MjPai> paiList = processData.handCardArray[i];
                    List<int> handList = new List<int>();
                    int uiseatID = CardHelper.GetMJUIPosByServerPos(i, selfSeatID);

                    for (int j = 0; j < paiList.Count; j++)
                    {
                        if (j < readIndex)
                        {
                            continue;
                        }

                        MjPai pai = paiList[j];
                        EnumMjCodeType action = (EnumMjCodeType)pai.mjCodeType;
                        switch (action)
                        {
                            case EnumMjCodeType.Code_Niu:
                                EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_LogicReconnect.ToString());
                                break;
                            case EnumMjCodeType.Code_Hands:
                                {
                                    handList.Add(pai.mjCode);
                                }
                                break;
                            case EnumMjCodeType.Code_Gang_An:
                                {
                                    if (_ui3D)
                                    {
                                        _ui3D.ReconnedAnGang(uiseatID, pai.mjCode, -1);
                                    }
                                    MjDataManager.Instance.SetPlayerHandAnGang(i, pai.mjCode, 0, true);
                                    gangNum++;
                                    hasControl = true;
                                }
                                break;
                            case EnumMjCodeType.Code_Gang_Zhi:
                                {
                                    if (_ui3D)
                                    {
                                        int uiPut = CardHelper.GetMJUIPosByServerPos(pai.seatID, selfSeatID);
                                        int showType = CardHelper.GetPengGangShowType(uiseatID, uiPut);
                                        _ui3D.ReconnedZhiGang(uiseatID, pai.mjCode, showType);
                                    }
                                    MjDataManager.Instance.SetPlayerHandZhiGang(i, pai.mjCode, 0, true);
                                    gangNum++;
                                    hasControl = true;
                                }
                                break;
                            case EnumMjCodeType.Code_Gang_Bu:
                                {
                                    if (_ui3D)
                                    {
                                        int uiPut = CardHelper.GetMJUIPosByServerPos(pai.seatID, selfSeatID);
                                        int showType = CardHelper.GetPengGangShowType(uiseatID, uiPut);
                                        _ui3D.ReconnedBuGang(uiseatID, pai.mjCode, showType);
                                    }
                                    MjDataManager.Instance.SetPlayerHandBuGang(i, pai.mjCode, 0, true);
                                    gangNum++;
                                    hasControl = true;
                                }
                                break;
                            case EnumMjCodeType.Code_Peng:
                                {
                                    if (_ui3D)
                                    {
                                        int uiPut = CardHelper.GetMJUIPosByServerPos(pai.seatID, selfSeatID);
                                        int showType = CardHelper.GetPengGangShowType(uiseatID, uiPut);
                                        _ui3D.ReconnedPeng(uiseatID, pai.mjCode, showType);
                                    }
                                    MjDataManager.Instance.SetPlayerHandPeng(i, pai.mjCode, 0, true);
                                    hasControl = true;
                                }
                                break;
                            case EnumMjCodeType.Code_Chi:
                                {
                                    List<int> chiList = new List<int>();
                                    chiList.Add(pai.mjCode);
                                    chiList.Add(paiList[j + 1].mjCode);
                                    chiList.Add(paiList[j + 2].mjCode);
                                    if (_ui3D)
                                    {
                                        _ui3D.ReconnedChiList(uiseatID, chiList);
                                    }

                                    MjDataManager.Instance.SetPlayerHandChi(i, chiList);
                                    readIndex += 3;
                                    hasControl = true;
                                    continue;
                                }
                                break;
                            case EnumMjCodeType.Code_Mao:
                                {
                                    List<int> maoList = new List<int>();
                                    maoList.Add(pai.mjCode);
                                    maoList.Add(paiList[j + 1].mjCode);
                                    maoList.Add(paiList[j + 2].mjCode);
                                    if (_ui3D)
                                    {
                                        _ui3D.ReconnedMaoList(uiseatID, maoList);
                                    }
                                    MjDataManager.Instance.SetPlayerHandMao(i, maoList);
                                    readIndex += 3;
                                    continue;
                                }
                                break;
                        }

                        readIndex++;
                    }
                    processData.downListArray[i] = handList;
                }

                int zhengNum = 0;
                //put
                for (int i = 1; i < 5; i++)
                {
                    List<int> putList = processData.putCardArray[i];
                    int uiseatID = CardHelper.GetMJUIPosByServerPos(i, selfSeatID);
                    if (putList != null)
                    {
                        zhengNum += putList.Count;
                    }
                }


                //code == gamestart
                if (processData.gameState >= (int)MjGameState.MjGameState_StandingCards)
                {
                    int allCount = processData.allCount;
                    int curCount = processData.curRestCount;
                    int buNum = gangNum;

                    this.SetCurRestCount(curCount);

                    int startGet = CardHelper.GetMJUIPosByServerPos(processData.getMjSeat, selfSeatID);
                    int leftCount = processData.getOffset;

                    int dealerSeatID = processData.dealerSeat;
                    bool isMoreOne = MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsDaiCaiShen);
                    int[] everyCode = CardHelper.GetMJCodeNumArray(allCount, dealerSeatID, selfSeatID, isMoreOne);

                    if (_ui3D)
                    {
                        _ui3D.ReconnedCodeCard(startGet, leftCount, everyCode, zhengNum, buNum);
                    }

                    if (processData.gameState < (int)MjGameState.MjGameState_GameIng || zhengNum == 0)
                    {
                        processData.lastHandCard = -1;
                    }
                }
                else
                {
                    int allCount = processData.allCount;
                    this.SetCurRestCount(allCount);
                }


                // standing code
                if (processData.standingData != null && processData.standingData.Count > 0)
                {
                    for (int i = 0; i < processData.standingData.Count; i++)
                    {
                        MjStandingPlateData dataItem = processData.standingData[i];
                        MjDataManager.Instance.SetProcessSpecial(dataItem, false);

                        if (dataItem.standingOffset >= 0 && !dataItem.standingIsFromBegin)
                        {
                            SetOpenAcardInCode(dataItem.standingOffset, false, dataItem.standingMjCode, dataItem.standingIsCanget);
                        }
                    }

                    PROBASIC_SPECIALShowCard_LogicToView(new object[] { false });
                }

                _ui3D.ReconnedCodeCut(zhengNum, gangNum);

                //put
                for (int i = 1; i < 5; i++)
                {
                    List<int> putList = processData.putCardArray[i];
                    MjDataManager.Instance.RefreshPlayerHandPut(i, putList);
                    int uiseatID = CardHelper.GetMJUIPosByServerPos(i, selfSeatID);
                    int tingPutIndex = processData.putCloseIndexArray[i];
                    _ui3D.ReconnedPutList(uiseatID, putList, tingPutIndex);
                }

                if (processData.gameState == (int)MjGameState.MjGameState_GameIng && zhengNum == 0 && !hasControl)
                {
                    MjDataManager.Instance.SetDealerPutState(true);
                }

                // independent
                for (int i = 1; i < 5; i++)
                {
                    int uiseatID = CardHelper.GetMJUIPosByServerPos(i, selfSeatID);
                    List<int> independentList = processData.independentArray[i];
                    bool isliangliu = MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISLiangLiu) ||
                       MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsLiangSiDaYi);

                    if (_ui3D)
                    {
                        EnumMjStandingType curStandingType = (isliangliu && uiseatID != 0) ? EnumMjStandingType.Open : EnumMjStandingType.Standing;
                        EnumMjSpecialType curSpecialType = isliangliu ? EnumMjSpecialType.LiangLiu : EnumMjSpecialType.Null;

                        _ui3D.ReconnedIndependent(uiseatID, independentList, curStandingType, curSpecialType);
                    }
                    MjDataManager.Instance.SetPlayerIndependentRefresh(i, independentList);

                    if (isliangliu)
                    {
                        if (i == selfSeatID && processData.hasFlyOne)
                        {
                            //yijingfeiyi
                            MjDataManager.Instance.MjData.ProcessData.HasFlyOne = true;
                            _ui3D.SetCardFlyOne(0);
                        }
                    }
                }


                bool needSort = true;
                if (processData.gameState < (int)MjGameState.MjGameState_GameIng)
                {
                    needSort = !MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsNoSortAfterDeal);
                }
                //shoupai  
                for (int i = 1; i < 5; i++)
                {
                    int uiseatID = CardHelper.GetMJUIPosByServerPos(i, selfSeatID);
                    List<int> handList = processData.downListArray[i];
                    if (_ui3D)
                    {
                        _ui3D.ReconnedDownList(uiseatID, handList, needSort);
                    }
                    MjDataManager.Instance.SetPlayerHandRefresh(i, handList);
                }

                //standing hand 
                if (processData.standingData != null && processData.standingData.Count > 0)
                {
                    EnumMjSpecialType refreshSpecial = MjDataManager.Instance.GetCurOpenSpecialType();
                    List<int> changeList = MjDataManager.Instance.GetCurOpenSpecialChangeList();

                    List<int> uiSeatList = MjDataManager.Instance.GetAllUiSeatCurDesk(false);
                    for (int i = 0; i < uiSeatList.Count; i++)
                    {
                        _ui3D.SetCardAfterSpecial(uiSeatList[i], refreshSpecial, changeList, false);
                    }
                }


                //special
                List<MjReconnedCheck> specialCheck = processData.checkSpecial;
                if (specialCheck != null && specialCheck.Count > 0)
                {
                    for (int i = 0; i < specialCheck.Count; i++)
                    {
                        this.CheckReconnedSpecial(specialCheck[i]);
                    }
                }
                //this.CheckReconnedSpecialNew(processData.specailCheck);

                if (processData.kannaomoState != 0)
                {
                    MjKanNaoMoResultReconned(processData.kannaomoState);
                }

                if (processData.xuanPiaoState != 0)
                {
                    MjXuanPiaoResultReconned(processData.xuanPiaoState);
                }

                if (processData.lastHandCard > 0)
                {
                    //刷新最后一张手牌 
                    _ui3D.ReconnedDownListLast(processData.lastHandCard);
                }

                //hu
                for (int i = 1; i < 5; i++)
                {
                    List<int> huList = processData.huCardArray[i];
                    if (huList != null && huList.Count > 0)
                    {
                        int uiseatID = CardHelper.GetMJUIPosByServerPos(i, selfSeatID);
                        _ui3D.ReconnedHuList(uiseatID, huList);
                        MjDataManager.Instance.SetPlayerHasHu(i);

                        int curDesk = MjDataManager.Instance.MjData.curUserData.selfDeskID;
                        int gameType = MemoryData.DeskData.GetOneDeskInfo(curDesk).mjGameType;

                        if (MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISDuoHu))
                        {
                            if (gameType == 50007 || gameType == 50060)
                            {
                                ShowTipsState(uiseatID);
                            }
                        }
                    }
                }


                bool isHaveHua = false;
                //补牌
                for (int i = 1; i < 5; i++)
                {
                    List<int> flowerList = processData.flowerArray[i];
                    if (flowerList.Count > 0)
                    {
                        isHaveHua = true;
                    }
                    MjDataManager.Instance.SetPlayerHandHua(i, flowerList);
                    this.SetFlowerSeatCount(i, false);
                }

                _ui3D.RemoveFlower(false);
                if (isHaveHua)
                {
                    //show ui
                    int allFlower = MjDataManager.Instance.GetPlayerHandHuaAllCount();
                    this.SetFlowerAmount(allFlower, false);
                }

                //ting 
                for (int i = 0; i < processData.tingSeatID.Count; i++)
                {
                    if (processData.tingSeatID[i] == 0)
                    {
                        continue;
                    }

                    int curSeatID = i + 1;
                    int uiseatID = CardHelper.GetMJUIPosByServerPos(curSeatID, selfSeatID);

                    int curValue = processData.tingSeatID[i];
                    MjPaiKou paikouStruct = MjDataManager.Instance.GetPaikouStruct(curValue);

                    if (paikouStruct.canTing)
                    {
                        //听啤
                        _ui3D.SetTingSate(uiseatID, false, null);

                        EnumMjOpAction showAction = MjDataManager.Instance.GetPaikouActionType(ConstDefine.MJ_PK_TING, EnumMjOpAction.MjOp_Ting);
                        this.ShowTingTip(uiseatID, showAction, false);
                        MjDataManager.Instance.SetPlayerHasTing(curSeatID);
                    }

                    if (paikouStruct.canMingLou)
                    {
                        List<int> handList = null;

                        bool isChekMinglouTing = MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsMingLouLiangTing);
                        if (isChekMinglouTing)
                        {
                            handList = null;
                        }
                        else
                        {
                            handList = processData.downListArray[curSeatID];
                            if (handList.Count % 3 == 2)
                            {
                                handList.RemoveAt(handList.Count - 1);
                            }
                        }
                        _ui3D.SetTingSate(uiseatID, false, handList);
                        EnumMjOpAction showAction = MjDataManager.Instance.GetPaikouActionType(ConstDefine.Mj_PK_MINGLOU, EnumMjOpAction.MjOp_Minglou);
                        this.ShowTingTip(uiseatID, showAction, false);
                        MjDataManager.Instance.SetPlayerHasMinglou(curSeatID);
                    }
                }


                //明楼听牌 
                if (processData.minglouTingList != null && processData.minglouTingList.Count > 0)
                {
                    object[] vars = new object[] { processData.minglouTingList };
                    AnimMingLouTing(vars);
                }


                MjDataManager.Instance.SetPaikouTingConfirm(processData.confirmTingInfo);

                EventDispatcher.FireEvent(GEnum.Table3DEvents.Table3D_PrepareData.ToString());
                if (processData.waitLastPutSeatID > 0)
                {
                    MjDataManager.Instance.MjData.ProcessData.lastPutSeatID = processData.waitLastPutSeatID;
                    int lastPlutUISeatID = CardHelper.GetMJUIPosByServerPos(processData.waitLastPutSeatID, selfSeatID);
                    //_ui3D.SetLastPutSeat(lastPlutUISeatID);
                    Vector3 latPos = _ui3D.GetLastPutSeatCardPos(-1, lastPlutUISeatID);
                    this.SetTipPos(latPos);
                }


                //ruler 
                if (processData.gameState == (int)MjGameState.MjGameState_GameIng)
                {
                    this.SetChangeHandListBtnState(true);
                }

                if (processData.canFangMao)
                {
                    //fangmao
                    MjDataManager.Instance.SetRulerResult(selfSeatID, processData.rulerResult, processData.gangList, processData.tingInfo, processData.ciList);
                    object[] objs = new object[] { selfSeatID, true };
                    //AnimShowFangMao(objs);
                    MjEventFangmao(objs);
                }
                else
                {
                    int rulerResult = processData.rulerResult;
                    if (rulerResult != ConstDefine.MJ_PK_NULL)
                    {
                        MjPaiKou paikouStruct = MjDataManager.Instance.CheckPaikou(rulerResult);

                        if (paikouStruct.canPeng)
                        {
                            //可以碰
                            MjDataManager.Instance.SetPaikouPeng(processData.nMjCode);
                        }

                        if (paikouStruct.canHu)
                        {
                            //可以胡
                            paikouStruct.canZiMo = processData.waitSeatID == selfSeatID;
                            if (paikouStruct.canZiMo)
                            {
                                paikouStruct.canZiMo = _ui3D.seatUIArray[0].GetHandMoveState();
                            }
                            if (paikouStruct.canZiMo)
                            {
                                MjDataManager.Instance.SetPaiKouHu(processData.lastHandCard);
                            }
                            else
                            {
                                MjDataManager.Instance.SetPaiKouHu(processData.nMjCode);
                            }
                        }

                        if (paikouStruct.canCiHu)
                        {
                            ///可以次胡
                            MjDataManager.Instance.SetPaikouCiList(processData.ciList);
                            if (paikouStruct.canGang && processData.gangList != null)
                            {
                                //剔除gang的list同ci
                                for (int i = 0; i < processData.ciList.Count; i++)
                                {
                                    processData.gangList.Remove(processData.ciList[i]);
                                }
                                if (processData.gangList.Count == 0)
                                {
                                    paikouStruct.canGang = false;
                                }
                            }
                        }


                        if (paikouStruct.canGang)
                        {
                            //可以杠
                            MjDataManager.Instance.SetPaikouGang(processData.gangList);
                        }

                        if (paikouStruct.canChi)
                        {
                            //可以吃
                            List<int> handList = new List<int>();
                            if (_ui3D)
                            {
                                handList = _ui3D.GetPlayerHandList(0);
                            }
                            MjDataManager.Instance.SetPaikouChi(processData.nMjCode, handList);
                        }

                        if (paikouStruct.canPiao)
                        {
                            MjDataManager.Instance.SetPaiKouHu(processData.lastHandCard);
                        }

                        MjDataManager.Instance.SetPaikouTing(processData.tingInfo);

                        ShowPengGangHu(paikouStruct);
                        AnimShowControlerBase(null);
                    }
                    else
                    {
                        if (MjDataManager.Instance.MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISCheckTing))
                        {
                            MjDataManager.Instance.ClearPaikou();
                            MjDataManager.Instance.SetPaikouTing(processData.tingInfo);
                            List<int> tingInfoList = MjDataManager.Instance.GetPaikouTingIDList();
                            if (tingInfoList.Count > 0)
                            {
                                this.SetChangeHandListBtnState(false);
                            }
                            _ui3D.SetCardMark(tingInfoList, 1);
                        }
                    }
                }

                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSetTipTime, processData.waitTime);
                if (processData.waitSeatID > 0)
                {
                    ChangeCurOprate(processData.waitSeatID, selfSeatID);
                }

                if (processData.waitSeatID == selfSeatID)
                {
                    _ui3D.CheckClickStateReconed();
                }


                Vector3 timePos = Vector3.zero;
                if (_static != null)
                {
                    timePos = _static.GetTimePos();
                }
                _ui2D.ShowTime(true, timePos);
            }

            RefreshReady();

            UIModuleReconnect();

            //close
            if (processData.isAskClose)
            {
                bool selfChoose = false;
                int startSeat = processData.startAskSeatID;
                MjDataManager.Instance.SetCloseEnd();
                MjDataManager.Instance.SetCloseStart(startSeat);

                for (int i = 0; i < processData.isAnswer.Count; i++)
                {
                    int state = 0;
                    if (processData.isAnswer[i])
                    {
                        state = processData.isAgree[i] ? 1 : 2;

                        if (i + 1 == MjDataManager.Instance.MjData.curUserData.selfSeatID)
                        {
                            selfChoose = true;
                        }
                    }
                    MjDataManager.Instance.SetCloseState(i + 1, state);
                }

                if (selfChoose)
                {
                    //state
                    float closeTime = Convert.ToSingle(processData.closeTime);
                    EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlCloseState, MjDataManager.Instance.MjData.curUserData.selfSeatID, closeTime, -1);
                }
                else
                {
                    //ask
                    int closeTime = processData.closeTime;
                    EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlCloseAsk, startSeat, closeTime);
                }
            }


            //over
            AnimPlayManager.Instance.canPlayAnim = true;
            MjDataManager.Instance.MjData.isReconned = false;
        }
        #region 行牌记录
        /// <summary>
        /// 按照行牌操作记录，回复牌桌各个区域的信息
        /// </summary>
        private void DealPlayerRecords(int selfSeatID, ref int gangNum, ref bool hasControl)
        {
            MahongDataModel.GameProcessReconned processData = MjDataManager.Instance.MjData.ProcessData.processReconned;
            //code & hand

            for (int i = 1; i < 5; i++)
            {
                int readIndex = 0;
                List<MjPai> paiList = processData.handCardArray[i];
                List<int> handList = new List<int>();
                int uiseatID = CardHelper.GetMJUIPosByServerPos(i, selfSeatID);
                //if (i == 1)
                //{ paiList.Add(new MjPai(-1, 8)); }
                for (int j = 0; j < paiList.Count; j++)
                {
                    if (j < readIndex)
                    {
                        continue;
                    }
                    MjPai pai = paiList[j];
                    EnumMjCodeType action = (EnumMjCodeType)pai.mjCodeType;
                    switch (action)
                    {
                        case EnumMjCodeType.Code_Niu:
                            DebugPro.DebugError("===EnumMjCodeType.Code_Niu===");
                            EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_LogicReconnect.ToString());
                            break;
                        case EnumMjCodeType.Code_Hands:
                            {
                                handList.Add(pai.mjCode);
                            }
                            break;
                        case EnumMjCodeType.Code_Gang_An:
                            {
                                if (_ui3D)
                                {
                                    _ui3D.ReconnedAnGang(uiseatID, pai.mjCode, -1);
                                }
                                MjDataManager.Instance.SetPlayerHandAnGang(i, pai.mjCode, 0, true);
                                gangNum++;
                                hasControl = true;
                            }
                            break;
                        case EnumMjCodeType.Code_Gang_Zhi:
                            {
                                if (_ui3D)
                                {
                                    int uiPut = CardHelper.GetMJUIPosByServerPos(pai.seatID, selfSeatID);
                                    int showType = CardHelper.GetPengGangShowType(uiseatID, uiPut);
                                    _ui3D.ReconnedZhiGang(uiseatID, pai.mjCode, showType);
                                }
                                MjDataManager.Instance.SetPlayerHandZhiGang(i, pai.mjCode, 0, true);
                                gangNum++;
                                hasControl = true;
                            }
                            break;
                        case EnumMjCodeType.Code_Gang_Bu:
                            {
                                if (_ui3D)
                                {
                                    int uiPut = CardHelper.GetMJUIPosByServerPos(pai.seatID, selfSeatID);
                                    int showType = CardHelper.GetPengGangShowType(uiseatID, uiPut);
                                    _ui3D.ReconnedBuGang(uiseatID, pai.mjCode, showType);
                                }
                                MjDataManager.Instance.SetPlayerHandBuGang(i, pai.mjCode, 0, true);
                                gangNum++;
                                hasControl = true;
                            }
                            break;
                        case EnumMjCodeType.Code_Peng:
                            {
                                if (_ui3D)
                                {
                                    int uiPut = CardHelper.GetMJUIPosByServerPos(pai.seatID, selfSeatID);
                                    int showType = CardHelper.GetPengGangShowType(uiseatID, uiPut);
                                    _ui3D.ReconnedPeng(uiseatID, pai.mjCode, showType);
                                }
                                MjDataManager.Instance.SetPlayerHandPeng(i, pai.mjCode, 0, true);
                                hasControl = true;
                            }
                            break;
                        case EnumMjCodeType.Code_Chi:
                            {
                                List<int> chiList = new List<int>();
                                chiList.Add(pai.mjCode);
                                chiList.Add(paiList[j + 1].mjCode);
                                chiList.Add(paiList[j + 2].mjCode);
                                if (_ui3D)
                                {
                                    _ui3D.ReconnedChiList(uiseatID, chiList);
                                }

                                MjDataManager.Instance.SetPlayerHandChi(i, chiList);
                                readIndex += 3;
                                hasControl = true;
                                continue;
                            }
                        //break;
                        case EnumMjCodeType.Code_Mao:
                            {
                                List<int> maoList = new List<int>();
                                maoList.Add(pai.mjCode);
                                maoList.Add(paiList[j + 1].mjCode);
                                maoList.Add(paiList[j + 2].mjCode);
                                if (_ui3D)
                                {
                                    _ui3D.ReconnedMaoList(uiseatID, maoList);
                                }
                                MjDataManager.Instance.SetPlayerHandMao(i, maoList);
                                readIndex += 3;
                                continue;
                            }
                            //break;
                    }

                    readIndex++;
                }
                processData.downListArray[i] = handList;
            }
        }
        #endregion

    }

}
