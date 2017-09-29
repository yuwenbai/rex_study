/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    /// <summary>
    /// 好友邀请列表
    /// </summary>
    public class FriendInviteListData
    {
        /// <summary>
        /// 好友列表
        /// </summary>
        public List<FriendInviteData> InviteListData;
        /// <summary>
        /// 累计邀请好友
        /// </summary>
        public int InviteCount;
        /// <summary>
        /// 累计领取桌卡
        /// </summary>
        public int RoomCardCount;
        /// <summary>
        /// 应答码
        /// 0 成功 1 失败
        /// </summary>
        public int ResultCode;
    }
    
    /// <summary>
    /// 单个好友邀请信息
    /// </summary>
    public class FriendInviteData
    {
        /// <summary>
        /// 被邀请人的Id
        /// </summary>
        public long FriendInviteID;
        /// <summary>
        /// 被邀请人的昵称
        /// </summary>
        public string FriendInviteName;
        /// <summary>
        /// 被邀请人注册日期
        /// </summary>
        public string FriendInviteTime;
        /// <summary>
        /// 领取奖励状态
        /// 0:未领取, 1:领取
        /// </summary>
        public int FriendInviteAwardState;
    }

    public class SysFriendInviteData
    {
        private FriendInviteListData _FInviteListData = new FriendInviteListData();
        public FriendInviteListData FInviteListData
        {
            get{
                return _FInviteListData;
            }
        }

        /// <summary>
        /// 好友邀请列表的状态变化
        /// </summary>
        public void FriendInviteListUpdate(long inviteId , int btnState)
        {
            foreach (FriendInviteData inviteData in FInviteListData.InviteListData)
            {
                if (inviteData.FriendInviteID == inviteId)
                {
                    inviteData.FriendInviteAwardState = btnState;
                }
            }
        }

        /// <summary>
        /// 初始化邀请好友列表
        /// </summary>
        public void FriendInviteListDataInit(Msg.GetInviteeListRsp inviteList)
        {
            _FInviteListData = new FriendInviteListData();
            _FInviteListData.InviteCount = inviteList.InviteeCount;
            _FInviteListData.RoomCardCount = inviteList.TotalTickets;
            _FInviteListData.ResultCode = inviteList.ResultCode;

            _FInviteListData.InviteListData = new List<FriendInviteData>();
            _FInviteListData.InviteListData = GetFriendInviteData(inviteList.InviteeList);            
        }

        public List<FriendInviteData> GetFriendInviteData(List<Msg.InviteeFriendInfo> inviteListInfo)
        {
            List<FriendInviteData> FriendInviteList = new List<FriendInviteData>();
                        
            foreach (Msg.InviteeFriendInfo listInfo in inviteListInfo)
            {
                FriendInviteData invite = new FriendInviteData();
                invite.FriendInviteID = listInfo.UserID;
                invite.FriendInviteName = listInfo.NickName;
                invite.FriendInviteTime = listInfo.RegDate;
                invite.FriendInviteAwardState = (int)listInfo.PickupStatus;

                FriendInviteList.Add(invite);
            }

            return FriendInviteList;
        }
    }

    #region 内存数据------------------------------------------------------------------------------

    public partial class MKey
    {
        public const string USER_FRIEND_INVITE_DATA = "USER_FRIEND_INVITE_DATA";
    }

    public partial class MemoryData
    {
        static public SysFriendInviteData FriendInviteData
        {
            get
            {
                SysFriendInviteData friendInviteData = MemoryData.Get<SysFriendInviteData>(MKey.USER_FRIEND_INVITE_DATA);
                if (friendInviteData == null)
                {
                    friendInviteData = new SysFriendInviteData();
                    MemoryData.Set(MKey.USER_FRIEND_INVITE_DATA, friendInviteData);
                }
                return friendInviteData;
            }
        }
    }

    #endregion -----------------------------------------------------------------------------------
}