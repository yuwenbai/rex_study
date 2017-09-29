/**
 * @Author lyb
 * 活动NetWorker
 *
 */

namespace projectQ
{
    //注册数据处理 战绩数据处理
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfActivityNetWorker()
        {
            ModelNetWorker.Regiest<Msg.SyncActivityListNotify>(S2CActivityListRsp);
            ModelNetWorker.Regiest<Msg.GetAwardConfigRsp>(S2CAwardConfigRsp);
            ModelNetWorker.Regiest<Msg.LotteryRsp>(S2CLotteryRsp);
            ModelNetWorker.Regiest<Msg.PickupAwardRsp>(S2CPickupAwardRsp);
        }

        #region Get Message ------------------------------------------------

        /// <summary>
        /// 获得活动中心消息
        /// </summary>
        public void S2CActivityListRsp(object obj)
        {
            var rsp = obj as Msg.SyncActivityListNotify;
            if (rsp.ActivityList != null)
            {
                for (int i = 0; i < rsp.ActivityList.Count; i++)
                {
                    MemoryData.SysActivityData.AddActivity(ActivityData.ProtoToData(rsp.ActivityList[i]));
                }
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_Activity_Rsp);
            }
        }

        /// <summary>
        /// 获得抽奖配置信息
        /// </summary>
        public void S2CAwardConfigRsp(object obj)
        {
            var rsp = obj as Msg.GetAwardConfigRsp;
            for (int i = 0; i < rsp.AwardList.Count; i++)
            {
                MemoryData.SysActivityData.AddAwardConfig(AwardConfigData.ProtoToData(rsp.AwardList[i]));
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_AwardConfig_Rsp);
        }

        /// <summary>
        /// 获得抽奖结果
        /// </summary>
        public void S2CLotteryRsp(object obj)
        {
            var rsp = obj as Msg.LotteryRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_Lottery_Rsp, rsp.AwardID);
            EventDispatcher.FireEvent(GEnum.NamedEvent.SysTools_FunctionPromptRefresh);
        }

        /// <summary>
        /// 领取奖励成功返回
        /// </summary>
        public void S2CPickupAwardRsp(object data)
        {
            Msg.PickupAwardRsp rsp = data as Msg.PickupAwardRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Other_PickupAward_Rsp, rsp.ResultCode);
        }

        #endregion ---------------------------------------------------------

        #region Send Message -----------------------------------------------

        /// <summary>
        /// 请求抽奖配置
        /// </summary>
        public void C2SAwardConfigReq(Msg.LotteryTypeDef type)
        {
            var req = new Msg.GetAwardConfigReq();
            req.LotteryType = type;
            this.send(req);
        }

        /// <summary>
        /// 抽奖请求
        /// </summary>
        public void C2SLotteryReq(Msg.LotteryTypeDef type)
        {
            var req = new Msg.LotteryReq();
            req.LotteryType = type;
            this.send(req);
        }

        /// <summary>
        /// 领取奖励请求
        /// </summary>
        public void C2SPickupAwardReq(int awardID)
        {
            var req = new Msg.PickupAwardReq();
            req.AwardID = awardID;
            this.send(req);
        }

        #endregion ---------------------------------------------------------
    }
}