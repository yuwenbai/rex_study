/**
 * @Author Xin.Wang
 *归类为山东地区的麻将玩法网络接收
 *
 */

using System.Collections.Generic;
using Msg;


namespace projectQ
{
    
    public partial class ModelNetWorker
    {  
        public void initDefaultHandleOfMahjongSD()
        {
            #region 赌暗杠
            ModelNetWorker.Regiest<MjDuAnGangNotify>(DuAnGangNotify);
            ModelNetWorker.Regiest<MjRspDuAnGang>(ResponseDuAnGang);
            #endregion

            #region 明楼通知
            ModelNetWorker.Regiest<TingKouInfoList>(TingKouInfoList);
            #endregion

            #region 漂胡跟漂
            ModelNetWorker.Regiest<MjPiaoHuNotify>(MjPiaoHuNotify);
            ModelNetWorker.Regiest<MjRspPiaoHu>(MjRspPiaoHu);
            #endregion

            #region 扭牌（山东）
            ModelNetWorker.Regiest<MjNiuPaiNotify>(NiuPaiNotify);
            ModelNetWorker.Regiest<MjRspNiuPai>(NiuPaiResponse);
            #endregion

        }

        #region 赌暗杠
        private void DuAnGangNotify(object resp)
        {
            MjDuAnGangNotify duAnGangNotify = resp as MjDuAnGangNotify;
            if (NullHelper.IsObjectIsNull(duAnGangNotify))
            {
                return;
            }
            MahjongPlayType.NotifyDuAnGangData data = new MahjongPlayType.NotifyDuAnGangData();
            data.State = duAnGangNotify.bSeatStat;
            data.SeatID = duAnGangNotify.nSeatID;
            data.MjCodeList = new List<int>();
            if (duAnGangNotify.nMjCodeList != null)
            {
                for (int i = 0; i < duAnGangNotify.nMjCodeList.Count; i++)
                {
                    data.MjCodeList.Add(duAnGangNotify.nMjCodeList[i]);
                }
            }
            else
            {
                DebugPro.DebugInfo("赌暗杠牌型数据为空");
            }
            MjDataManager.Instance.MjData.ProcessData.processDuAnGang.NotifyDuAnGangData = data;
            //通知逻辑层数据更新
            EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_LogicNotify.ToString());
        }
        /// <summary>
        /// 上传赌暗杠结果
        /// </summary>
        /// <param name="deskID">桌号</param>
        /// <param name="seatID">座位号</param>
        /// <param name="selectType">选择类型：1赌 2过</param>
        public void DuAnGangRequest(int deskID, int seatID, int selectType, int mjCode)
        {
            Msg.MjReqDuAnGang duAnGangReq = new Msg.MjReqDuAnGang();
            duAnGangReq.nDeskID = deskID;
            duAnGangReq.nSeatID = seatID;
            duAnGangReq.nSelectID = selectType;
            duAnGangReq.nMjCode = mjCode;
            ModelNetWorker.Send(duAnGangReq);
        }

        private void ResponseDuAnGang(object resp)
        {
            MjRspDuAnGang response = resp as MjRspDuAnGang;
            if (response.nResultCode == 1)
            {
                MahjongPlayType.RspDuAnGangData data = new MahjongPlayType.RspDuAnGangData();
                data.DeskID = response.nDeskID;
                data.SeatID = response.nSeatID;
                data.ResultCode = response.nDuResultCode;
                MjDataManager.Instance.MjData.ProcessData.processDuAnGang.RspDuAnGangData = data;
                //通知逻辑层数据更新
                EventDispatcher.FireEvent(MJEnum.DuAnGangEvents.DAG_RspNotify.ToString());
            }
            else
            {
                DebugPro.DebugError("MjRspDuAnGang error, error code:", response.nResultCode);
            }
        }

        #endregion

        #region 明楼

        private List<int> SetReconnedMinglouTingkou(Msg.TingKouInfoList prsp)
        {
            List<int> seatList = new List<int>();
            if (prsp != null && prsp.PlayerTingKouInfo != null && prsp.PlayerTingKouInfo.Count > 0)
            {
                List<MjTingInfo> info = GetTingkouInfo(out seatList, prsp.PlayerTingKouInfo);
                SetMinglouTingInfo(false, seatList, info);
            }

            return seatList;
        }

        private void TingKouInfoList(object rsp)
        {
            var prsp = rsp as TingKouInfoList;
            if (prsp.PlayerTingKouInfo != null && prsp.PlayerTingKouInfo.Count > 0)
            {
                List<int> seatID = new List<int>();
                List<MjTingInfo> info = GetTingkouInfo(out seatID, prsp.PlayerTingKouInfo);

                SetMinglouTingInfo(true, seatID, info);
            }
        }

        private List<MjTingInfo> GetTingkouInfo(out List<int> seatID, List<TingKouInfo> infoList)
        {
            seatID = new List<int>();
            List<MjTingInfo> tingInfo = new List<MjTingInfo>();
            if (infoList != null && infoList.Count > 0)
            {
                for (int i = 0; i < infoList.Count; i++)
                {
                    TingKouInfo originalInfo = infoList[i];
                    seatID.Add(originalInfo.SeatID);
                    MjTingInfo newInfo = MjGetTingInfo(originalInfo.TingInfo);
                    tingInfo.Add(newInfo);
                }
            }

            return tingInfo;
        }


        private void SetMinglouTingInfo(bool needFire, List<int> seatID, List<MjTingInfo> tingInfo)
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.UIMinglouTingInfo, seatID, tingInfo, needFire);
        }

        #endregion


        #region 漂胡 跟漂
        /// <summary>
        /// 重连
        /// </summary>
        /// <param name="rsp"></param>
        private void MjPiaoHuNotify(object rsp)
        {
            if (rsp != null)
            {
                MjPiaoHuNotify prsp = rsp as MjPiaoHuNotify;
                SetPiaoHuDataToMjData(prsp.PiaoData);
            }
        }

        /// <summary>
        /// 正常流程下
        /// </summary>
        /// <param name="rsp"></param>
        private void MjRspPiaoHu(object rsp)
        {
            MjRspPiaoHu prsp = rsp as MjRspPiaoHu;
            int resultCode = prsp.nResultCode;
            if (resultCode == 1)
            {
                int curShowSeatID = prsp.nSeatID;
                SetPiaoHuDataToMjData(prsp.PiaoData);

                //show
                MjDataManager.Instance.SetHuPiaoGenPiaoShow(curShowSeatID);
            }
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="dataList"></param>
        private void SetPiaoHuDataToMjData(List<Msg.MjPiaoHuNum> dataList)
        {
            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    int curValue = dataList[i].nPiaoNum;
                    int curSeat = dataList[i].nSeatID;

                    MjDataManager.Instance.SetHuPiaoGenPiaoData(curSeat, curValue);
                }
            }
        }


        /// <summary>
        /// 发送漂胡跟漂
        /// </summary>
        /// <param name="deskID"></param>
        /// <param name="seatID"></param>
        /// <param name="mjCode"></param>
        public void MjReqPiaoHu(int deskID, int seatID, int mjCode)
        {
            MjReqPiaoHu req = new Msg.MjReqPiaoHu();
            req.nSeatID = seatID;
            req.nDeskID = deskID;
            req.nMjCode = mjCode;

            this.send(req);
        }


        #endregion

        #region 扭牌(青岛)
        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="notify"></param>
        private void NiuPaiNotify(object notify)
        {
            MjNiuPaiNotify data = notify as MjNiuPaiNotify;
            if (NullHelper.IsObjectIsNull(data))
            {
                return;
            }
            if (NullHelper.IsObjectIsNull(data.NiuPaiData))
            {
                return;
            }
            //正常扭牌过程
            MahjongPlayType.NotifyNiuPai dataNiuPai = new MahjongPlayType.NotifyNiuPai();
            MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData = dataNiuPai;
            dataNiuPai.IsNiuPai = data.bIsNiuPai;
            dataNiuPai.DeskID = data.nDeskID;
            dataNiuPai.SeatID = data.nSeatID;
            dataNiuPai.RuleID = data.nRulerID;
            dataNiuPai.Data = new List<MahjongPlayType.NiuPaiGroupItem>();
            if (!FillNiuPaiData(data.NiuPaiData,  dataNiuPai.Data))
            {
                return;
            }
            EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_LogicNiuPaiNotify.ToString());
        }
        private bool FillNiuPaiData(List<MjNiuPaiList> srcData,  List<MahjongPlayType.NiuPaiGroupItem> desData)
        {
            for (int i = 0; i < srcData.Count; i++)
            {
                MahjongPlayType.NiuPaiGroupItem item = new MahjongPlayType.NiuPaiGroupItem();
                item.GroupType = (MahjongPlayType.NiuPaiGroupType)srcData[i].nNiuType;
                if (item.GroupType == MahjongPlayType.NiuPaiGroupType.None||NullHelper.IsObjectIsNull(srcData[i].nMjCode) || srcData[i].nMjCode.Count <= 0)
                {
                    DebugPro.DebugError("数据有误");
                    return false;
                }
                item.MjCodes = new List<int>();
                item.MjCodes.AddRange(srcData[i].nMjCode);
                desData.Add(item);
            }
            return true;
        }
        public void NiuPaiRequest()
        {
            MahjongPlayType.RequestNiuPai reqData = MjDataManager.Instance.MjData.ProcessData.processNiuPai.RequestNiuPai;
            //向服务器对应的消息中赋值
            MjReqNiuPai serverReq = new MjReqNiuPai();
            serverReq.DeskID = reqData.DeskID;
            serverReq.SeatID = reqData.SeatID;
            serverReq.bIsNiuPai = reqData.IsNiuPai;
            serverReq.nSelectID = reqData.SelectedID;
            if (NullHelper.IsObjectIsNull(serverReq.NiuPaiData))
            {
                DebugPro.DebugError("发送扭牌消息 失败");
                return;
            }
            for (int i = 0; i < reqData.Data.Count; i++)
            {
                MjNiuPaiList item = new MjNiuPaiList();
                item.nNiuType = (int)reqData.Data[i].GroupType;
                if (NullHelper.IsObjectIsNull(item.nMjCode))
                {
                    DebugPro.DebugError("发送扭牌消息 失败");
                    return;
                }
                item.nMjCode.AddRange(reqData.Data[i].MjCodes);
                serverReq.NiuPaiData.Add(item);
            }
            DebugPro.DebugInfo("发送扭牌消息");
            ModelNetWorker.Send(serverReq);
        }
        private void NiuPaiResponse(object response)
        {
            MjRspNiuPai data = response as MjRspNiuPai;
            if (NullHelper.IsObjectIsNull(data))
            {
                return;
            }
            if (NullHelper.IsObjectIsNull(data.NiuPaiData))
            {
                return;
            }
            MahjongPlayType.ResponseNiuPai dataNiuPai = new MahjongPlayType.ResponseNiuPai();
            MjDataManager.Instance.MjData.ProcessData.processNiuPai.ResponseNiuPai = dataNiuPai;
            dataNiuPai.IsNiuPai = data.bIsNiuPai;
            dataNiuPai.DeskID = data.DeskID;
            dataNiuPai.SeatID = data.SeatID;
            dataNiuPai.SelectedID = data.nSelectID;
            dataNiuPai.Data = new List<MahjongPlayType.NiuPaiGroupItem>();
            if (!FillNiuPaiData(data.NiuPaiData, dataNiuPai.Data))
            {
                return;
            }
            EventDispatcher.FireEvent(MJEnum.NiuPaiEvent.NPE_LogicResponse.ToString());
            //if (!dataNiuPai.IsNiuPai)
            //{
            //    //这里模仿服务器逻辑
            //    //补花消息下发
            //    int seatID = MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData.SeatID; ;
            //    List<MjRoundGetBuPai> changeFlower = new List<MjRoundGetBuPai>();
            //    MahjongPlayType.NotifyNiuPai data = MjDataManager.Instance.MjData.ProcessData.processNiuPai.NotifyNiuPaiData;
            //    List<int> paiData = CardHelper.SwitchToOneDimension(data.Data);
            //    MjRoundGetBuPai data1 = new MjRoundGetBuPai(paiData, paiData, 3);
            //    changeFlower.Add(data1);
            //    EventDispatcher.FireEvent(GEnum.NamedEvent.EMjGetFlowerNotify, seatID, changeFlower);
            //}
        }
        private void NiuPaiReconnect(List<MjNiuPaiReconnectNotify> conRsp)
        {
            if (NullHelper.IsObjectIsNull(conRsp))
            {
                return;
            }
            List<MahjongPlayType.NiuPaiHistoryRecords> historyReds = new List<MahjongPlayType.NiuPaiHistoryRecords>();
            for (int i = 0; i < conRsp.Count; i++)
            {
                MahjongPlayType.NiuPaiHistoryRecords records = new MahjongPlayType.NiuPaiHistoryRecords();
                records.NiuPaiData = new List<MahjongPlayType.NiuPaiGroupItem>();
                if (!FillNiuPaiData(conRsp[i].NiuPaiData, records.NiuPaiData))
                {
                    return;
                }
                records.BuNiuData = new List<MahjongPlayType.NiuPaiGroupItem>();
                if (!FillNiuPaiData(conRsp[i].BuNiuData, records.BuNiuData))
                {
                    return;
                }
                records.SeatID = conRsp[i].nSeatID;
                records.DeskID = conRsp[i].nDeskID;
                records.BuNiuPaiCountRecords.AddRange(conRsp[i].BuNiuPaiRecrodCount);
                records.NiuPaiCountRecords.AddRange(conRsp[i].NiuPaiRecrodCount);
                historyReds.Add(records);
            }
            MjDataManager.Instance.MjData.ProcessData.processNiuPai.HistoryRecords = historyReds;

        }
        #endregion

    }
}
