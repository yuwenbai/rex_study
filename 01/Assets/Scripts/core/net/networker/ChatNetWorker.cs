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
    //注册数据处理，聊天网络数据处理
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfChat()
        {
            ModelNetWorker.Regiest<ChatNotify>(ChatNotify);
        }
        /// <summary>
        /// 聊天消息发送请求
        /// </summary>
        /// <param name="data"></param>
        public void ChatReq(string chatMsg)
        {
           
            var req = new ChatReq();
            req.UserID = MemoryData.UserID;
            req.SeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
            req.ChatMsg = chatMsg;
            req.DeskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            this.send(req);

        }
        /// <summary>
        /// 聊天消息接收
        /// </summary>
        /// <param name="data"></param>
        public void ChatNotify(object data)
        {
            var rsp = data as ChatNotify;
            
            QLoger.LOG("chatNotyfy = " + rsp + "####################################");
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.EMjChatNotify, rsp.SeatID,rsp.UserID,rsp.ChatMsg);
        }
    }

}
