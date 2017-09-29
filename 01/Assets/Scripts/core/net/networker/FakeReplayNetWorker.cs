/**
 * @Author Lyb
 *  邀请好友有礼
 *
 */

using Msg;

namespace projectQ
{
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfReplayData()
        {
            //这里注册回放消息的回调处理
            ModelNetWorker.Regiest<DeskInfoRsp>(GetRoundReplayDataRsp);
            ModelNetWorker.Regiest<DeskRecordRsp>(GetReplayDataListRsp);
            Regiest<ShowDeskCard>(GetShowDeskCardRsp);
            Regiest<ChangeThreeCard>(GetChangeThreeCardRsp);
        }

        #region Get Message --------------------------------------------------

        /// <summary>
        /// 某玩家所有回放发送请求
        /// </summary>
        /// <param name="data"></param>
        /// 
        public void C2SMessageReplayDataReq()
        {
            var req = new DeskInfoReq();
            req.UserID = MemoryData.UserID;
            this.send(req);
        }
        /// <summary>
        /// 某局发送请求
        /// </summary>
        /// <param name="data"></param>
        public void C2SMessageReplayRoundReq(int deskId, int roundId)
        {
            var req = new DeskRecordReq();
            req.DeskID = deskId;
            req.BoutID = roundId;
            this.send(req);
        }
        ///// <summary>
        ///// 指定玩家回放数据接收
        ///// </summary>
        ///// <param name="data"></param>
        public void GetReplayDataListRsp(object data)
        {
            var rsp = data as DeskRecordRsp;
            
            FakeReplayManager.Instance.ParseData(rsp);
        }
        ///// <summary>
        ///// 指定局数回放数据接收
        ///// </summary>
        ///// <param name="data"></param>
        public void GetRoundReplayDataRsp(object data)
        {
            var rsp = data as DeskInfoRsp;
            //var deskList = rsp.DeskList;
            FakeReplayManager.Instance.ParseRoundData(rsp);
        }
        ///// <summary>
        ///// 这里处理其他玩家手牌
        ///// </summary>
        ///// <param name="data"></param>
        public void GetShowDeskCardRsp(object data)
        {
            var rsp = data as ShowDeskCard;
            MemoryData.PlayerOtherData.RefreshData(rsp);
        }
        ///// <summary>
        ///// 这里处理换3张数据
        ///// </summary>
        ///// <param name="data"></param>
        public void GetChangeThreeCardRsp(object data)
        {
            var rsp = data as ChangeThreeCard;
            foreach (var item in rsp.ChangeThreed)
            {
                FakeReplayManager.Instance.PutChangeThreeCardCache(item.SeatID, item.nMjCode);
            }
        }

        #endregion ----------------------------------------------------------------------
    }
}