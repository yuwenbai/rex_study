/**
 * @Author lyb
 * 桌卡数据跟服务器交互
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace projectQ
{
    //注册数据处理 战绩数据处理
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfRoomCardNetWorker()
        {
            ModelNetWorker.Regiest<Msg.GetGoodsListRsp>(S2CRoomCardListSuccRsp);
			ModelNetWorker.Regiest<Msg.BuyGoodsRsp>(S2CRoomCardBuySuccRsp);
			ModelNetWorker.Regiest<Msg.PayFinishNotify>(S2CPayFinishRsp);
			ModelNetWorker.Regiest<Msg.PayStartNotify>(PayStartNotify);
        }

        #region Get Message ------------------------------------------------

        /// <summary>
        /// 服务器返回商品列表消息
        /// </summary>
        public void S2CRoomCardListSuccRsp(object rsp)
        {
            var prsp = rsp as Msg.GetGoodsListRsp;

            MemoryData.RoomCardData.GoodsData_Update(prsp.GoodsList);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_RoomCardList_Succ, prsp.ResultCode);
        }

        /// <summary>
        /// 服务器返回购买成功消息
        /// </summary>
        public void S2CRoomCardBuySuccRsp(object rsp)
        {
            var prsp = rsp as Msg.BuyGoodsRsp;

            MemoryData.RoomCardData.BuyParameterData_Update(prsp);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_RoomCardBuy_Succ, prsp.ResultCode);
        }


		public void PayStartNotify(object rsp)
		{
			var prsp = rsp as Msg.PayStartNotify;

			if (prsp != null )
			{
				//EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_PayFinish_Succ , prsp.GoodsID);
				//发消息唤醒微信支付

				var jdata = new JsonData ();
				jdata ["partnerId"] = prsp.PartnerID;
				jdata ["prepayId"] = prsp.PrePayID;
				jdata ["packageValue"] = prsp.Package;
				jdata ["nonceStr"] = prsp.NonceStr;
				jdata ["timeStamp"] = prsp.TimeStamp;
				jdata ["sign"] = prsp.Sign;

				QLoger.ERROR ("微信参数配置 " + CommonTools.ReflactionObject (prsp));

				EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_StarWXPay , jdata.ToJson());
			}
		}


        /// <summary>
        /// 走完h5界面之后完成支付消息通知
        /// </summary>
        public void S2CPayFinishRsp(object rsp)
        {
            var prsp = rsp as Msg.PayFinishNotify;

            if (prsp.ResultCode == 0)
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_PayFinish_Succ , prsp.GoodsID);
            }            
        }

        #endregion ---------------------------------------------------------

        #region Send Message -----------------------------------------------

        /// <summary>
        /// 跟服务器请求桌卡列表
        /// </summary>
        public void C2SRoomCardListReq()
        {
            var msg = new Msg.GetGoodsListReq();
            msg.UserID = MemoryData.UserID;
            this.send(msg);
        }

        /// <summary>
        /// 跟服务器请求购买桌卡
        /// </summary>
        public void C2SRoomCardBuyReq(int roomCardId , int roomCardNum = 1)
        {
            var msg = new Msg.BuyGoodsReq();
            msg.UserID = MemoryData.UserID;
            msg.GoodsID = roomCardId;
            msg.GoodsNum = roomCardNum;
            this.send(msg);
        }

        #endregion ---------------------------------------------------------
    }
}