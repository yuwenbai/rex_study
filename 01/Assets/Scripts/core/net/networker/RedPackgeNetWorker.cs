/**
 * 作者：周腾
 * 作用：
 * 日期：
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg;
namespace projectQ
{
    public partial class ModelNetWorker
    {

        public void initDefaultHandleOfPushGift()
        {
            ModelNetWorker.Regiest<PushMoneyBagRsp>(PushMoneyBagRsp);
            ModelNetWorker.Regiest<PushMoneyBagResultRsp>(PushMoneyBagResultRsp);
            //ModleNetWorker.Regiest<FRoomPushGiftReq>(PushGiftReq);
            //ModleNetWorker.Regiest<FRoomPushGiftRsp>(PushGiftRsp);
            //ModleNetWorker.Regiest<FRoomPushGiftNotify>(FRoomPushGiftNotify);
            //ModleNetWorker.Regiest<FRoomRecveGiftRsp>(FRoomRecveGiftRsp);
            //ModleNetWorker.Regiest<FRoomRecvieGiftNotify>(FRoomRecvieGiftNotify);
        }
        public void SendRedPacket(long userId, int ticket, int num, string msg)
        {
            var req = new PushMoneyBagReq();
            req.UserID = userId;
            req.MaxTickets = ticket;
            req.MaxCount = num;
            req.BagMsg = msg;
            this.send(req);
        }

        public void PushMoneyBagRsp(object data)
        {
            var rsp = data as PushMoneyBagRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_PushMoney_Rsp, rsp.ResultCode, rsp.BagID, rsp.BagMsg);
        }

        /// <summary>
        /// 红包分享结果上报
        /// </summary>
        public void PushMoneyBagResultReq(bool isBol)
        {
            var req = new PushMoneyBagResultReq();
            req.UserID = MemoryData.UserID;
            req.PushOK = isBol;
            this.send(req);
        }

        /// <summary>
        /// 红包分享结果上报返回
        /// </summary>
        public void PushMoneyBagResultRsp(object data)
        {
            var rsp = data as PushMoneyBagResultRsp;
            if (rsp.UserID == MemoryData.UserID)
            {
                QLoger.LOG(" 红包分享结果上报返回 =  " + rsp.ResultCode);
            }
            //EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_PushMoney_Rsp, rsp.ResultCode, rsp.BagID, rsp.BagMsg);
        }

        //public void FRoomRecvieGiftNotify(object data)
        //{
        //    var rsp = data as FRoomRecvieGiftNotify;
        //    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_ReceiveGift_Notify, rsp.IsApply,rsp.SrcUserID,rsp.DstUserID,rsp.GiftID,rsp.SrcName);

        //}

        //public void FRoomRecveGiftRsp(object data)
        //{
        //    var rsp = data as FRoomRecveGiftRsp;
        //    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_ReceiveGift_Rsp, rsp.ResultCode);
        //}

        //public void FRoomReceiveGiftReq(bool isApply,long srcUserId,long dstUserId,int giftId)
        //{
        //    FRoomReciveGiftReq req = new FRoomReciveGiftReq();
        //    req.IsApply = isApply;
        //    req.SrcUserID = srcUserId;
        //    req.DstUserID = dstUserId;
        //    req.GiftID = giftId;
        //}

        //public void FRoomPushGiftNotify(object data)
        //{
        //    var rsp = data as FRoomPushGiftNotify;
        //    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_PushGift_Notify, rsp.SrcUserID, rsp.DstUserID, rsp.GiftID, rsp.SrcName, rsp.HasInBag);
        //}

        //public void PushGiftReq(long dstId,int giftId)
        //{

        //    var req = new FRoomPushGiftReq();
        //    req.SrcUserID = (long)dstId;
        //    req.DstUserID = MemoryData.UserID;
        //    req.GiftID = (int)giftId;
        //    this.send(req);
        //}

        //public void PushGiftRsp(object data)
        //{
        //    var rsp = data as FRoomPushGiftRsp;
        //    int result = rsp.ResultCode;
        //    EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_PushGift_Rsp, result);
        //}


    }
}
