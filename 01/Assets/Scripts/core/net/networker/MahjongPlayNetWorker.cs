/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg;

namespace projectQ
{
    // 麻将玩法 消息
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfMahjongPlay()
        {
            ModelNetWorker.Regiest<MjRulerConfigRsp>(MjRulerConfigRsp);
        }
        public void MjRulerConfigReq()
        {
            var req = new MjRulerConfigReq();
            this.send(req);
        }

        private void MjRulerConfigRsp(object obj)
        {
            var rsp = obj as MjRulerConfigRsp;
            MemoryData.MahjongPlayData.ProtoToData(rsp.RulerTypeList);
            MemoryData.MahjongPlayData.ProtoToData(rsp.MjGameList);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoginSouccessRsp, "MjRulerConfig");
        }
        
    }
}