/**
 * @Author YQC
 * 消息面板
 *
 */

using System.Collections.Generic;
using Msg;

namespace projectQ
{
    //注册数据处理 好友网络数据处理
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfMessage()
        {
            ModelNetWorker.Regiest<MyMailListRsp>(MyMailListRsp);
            ModelNetWorker.Regiest<MailReadRsp>(MailReadRsp);
            ModelNetWorker.Regiest<MailPickupAttachRsp>(MailPickupAttachRsp);
            ModelNetWorker.Regiest<SystemNotice>(SystemNotice);
        }

        #region Get Message

        /// <summary>
        /// 获得消息列表
        /// </summary>
        public void MyMailListRsp(object rsp)
        {
            var prsp = rsp as MyMailListRsp;
            MemoryData.MessageData.MailMessage_Init(prsp.MailList);

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_Message_Succ);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoginSouccessRsp, "MyMailList");
        }

        /// <summary>
        /// 已读邮件
        /// </summary>
        public void MailReadRsp(object rsp)
        {
            var prsp = rsp as MailReadRsp;
            MemoryData.MessageData.IsReadState(prsp.MailID);
        }


        /// <summary>
        /// 获取道具结果反馈
        /// </summary>
        public void MailPickupAttachRsp(object rsp)
        {
            var prsp = rsp as MailPickupAttachRsp;
        }

        /// <summary>
        /// 获取走马灯信息
        /// </summary>
        public void SystemNotice(object rsp)
        {
            var prsp = rsp as SystemNotice;

            //MemoryData.OtherData.NoticeMsgList.Clear(); //Todo 要不要清除
            if (prsp.NoticeList != null && prsp.NoticeList.Count > 0)
            {
                for (int i = 0; i < prsp.NoticeList.Count; i++)
                {
                    MemoryData.OtherData.AddNoticeMsg(NoticeMsg.ProtoToData(prsp.NoticeList[i]));
                }
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Broadcast_Update);
            }
        }

        #endregion

        #region Send Message


        /// <summary>
        /// 邮件列表
        /// </summary>
        public void MyMailListReq()
        {
            var msg = new MyMailListReq();
            msg.UserID = MemoryData.UserID;
            this.send(msg);
        }


        /// <summary>
        /// 读邮件
        /// </summary>
        public void MailReadReq(long uid)
        {
            var msg = new MailReadReq();
            msg.MailID = uid;
            msg.UserID = MemoryData.UserID;

            this.send(msg);
        }

        /// <summary>
        /// 获取邮件附件
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="item">Item. 不传List为全部领取</param>
        public void MailPickupAttachReq(long uid, List<int> item = null)
        {
            var msg = new MailPickupAttachReq();
            msg.MailID = uid;
            msg.UserID = MemoryData.UserID;
            if (item != null)
            {
                msg.ItemID.AddRange(item);
            }
            this.send(msg);
        }

        #endregion
    }
}