/**
 * @Author lyb
 * 官方试玩群数据处理
 *
 */

namespace projectQ
{
    //注册数据处理 战绩数据处理
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfOfficialGroupNetWorker()
        {
            ModelNetWorker.Regiest<Msg.GetOfficialGroupRsp>(S2COfficialGroupRsp);
        }

        #region Get Message ------------------------------------------------

        /// <summary>
        /// 服务器返回官方试玩群列表消息
        /// </summary>
        public void S2COfficialGroupRsp(object rsp)
        {
            var prsp = rsp as Msg.GetOfficialGroupRsp;

            if (prsp.ResultCode == 0)
            {
                MemoryData.OfficialGroupData.OffcialPageIndex = prsp.OffcialPageIndex;

                MemoryData.OfficialGroupData.OfficialGroup_Update(prsp.GroupList);

                DebugPro.Log(" C2COfficialGroupSuccEvent ");

                //GameDelegateCache.C2COfficialGroupSuccEvent();
            }            
        }

        #endregion ---------------------------------------------------------

        #region Send Message -----------------------------------------------

        /// <summary>
        /// 跟服务器请求官方试玩群列表
        /// </summary>
        public void C2SOfficialGroupReq()
        {
            var msg = new Msg.GetOfficialGroupReq();
            msg.UserID = MemoryData.UserID;
            this.send(msg);
        }
        
        #endregion ---------------------------------------------------------
    }
}