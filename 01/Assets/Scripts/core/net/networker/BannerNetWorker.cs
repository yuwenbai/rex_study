using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg;
namespace projectQ
{
    /// <summary>
    /// 公告网络处理，暂定每天首次登陆时弹出 由服务器控制
    /// </summary>
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfBanner()
        {
            ModelNetWorker.Regiest<GetBannerListRsp>(GetBannerListRsp);
        }
        /// <summary>
        /// 公告发送请求
        /// </summary>
        /// <param name="data"></param>
        public void BannerReq(string chatMsg)
        {
            var req = new GetBannerListReq();
            req.UserID = MemoryData.UserID;
            this.send(req);

        }
        /// <summary>
        /// 公告消息接收
        /// </summary>
        /// <param name="data"></param>
        public void GetBannerListRsp(object data)
        {
            var rsp = data as GetBannerListRsp;
            
            QLoger.LOG("GetBannerListRsp = " + rsp + "####################################");
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenBanner, rsp.ResultCode,rsp.ActivityList);
        }
    }

}
