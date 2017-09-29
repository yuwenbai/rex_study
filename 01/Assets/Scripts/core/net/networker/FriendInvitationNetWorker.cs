/**
 * @Author Lyb
 *  邀请好友有礼
 *
 */

using Msg ;

namespace projectQ
{
    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfFriendInvitation()
        {
            ModelNetWorker.Regiest<GetInviteeListRsp>(S2CInviteFriendListRsp);
            ModelNetWorker.Regiest<BindInviteCodeRsp>(S2CInviteCodeGetAwardRsp);
            ModelNetWorker.Regiest<PickInviteAwardRsp>(S2CInvitedAwardRsp);
        }

        #region Get Message --------------------------------------------------

        /// <summary>
        /// 请求好友列表 返回
        /// </summary>
        public void S2CInviteFriendListRsp(object rsp)
        {
            var prsp = rsp as GetInviteeListRsp;

            MemoryData.FriendInviteData.FriendInviteListDataInit(prsp);
        }

        /// <summary>
        /// 填写我的邀请人获取奖励 返回
        /// </summary>
        public void S2CInviteCodeGetAwardRsp(object rsp)
        {
            var prsp = rsp as BindInviteCodeRsp;

            if (prsp.ResultCode == 0)
            {
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "邀好友有礼",
                    "邀请码验证成功，邀请码验证成功，您已获得桌卡", new string[] { "确定" }, delegate (int index) { });
            }

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_FriendInvite_CodeAward_Get, prsp.ResultCode, prsp.InviteCode, prsp.OtherName);
        }
        
        /// <summary>
        /// 领取好友奖励 返回
        /// </summary>
        public void S2CInvitedAwardRsp(object rsp)
        {
            var prsp = rsp as PickInviteAwardRsp;

            if (prsp.ResultCode == 0)
            {
                // 领取成功                

                MemoryData.FriendInviteData.FriendInviteListUpdate(prsp.InviteeUserID , (int)prsp.InviteeStatus);

                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "邀好友有礼", "领取成功", new string[] { "确定" }
                    , delegate (int index) { });
            }

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_FriendInvite_PickUpAward_Get, prsp.ResultCode);
        }
        
        #endregion ------------------------------------------------------------------------

        #region Send Message --------------------------------------------------------------
        
        /// <summary>
        /// 请求好友列表
        /// </summary>
        public void C2SInviteFriendListReq()
        {
            var msg = new GetInviteeListReq();
            msg.UserID = MemoryData.UserID ;
            this.send (msg);
        }
        
        /// <summary>
        /// 填写我的邀请人获取奖励
        /// </summary>
        public void C2SInviteCodeGetAwardReq(string code)
        {
            var msg = new BindInviteCodeReq();
            msg.UserID = MemoryData.UserID;
            msg.InviteCode = code;
            this.send (msg);
        }

        /// <summary>
        /// 领取好友奖励
        /// </summary>
        public void C2SInvitedAwardReq(long InviteId)
        {
            var msg = new PickInviteAwardReq();
            msg.UserID = MemoryData.UserID;
            msg.InviteeUseID = InviteId;
            this.send (msg);
        }

        #endregion ----------------------------------------------------------------------
    }
}