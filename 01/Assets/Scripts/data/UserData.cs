/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UserInfo
    {
        public long UserID;
        public string Name;
        public int Level;
        //public int TotalBouts; // 玩过的游戏记录：总盘数
        //public int WinBouts; // 历史胜利盘数
        public int RegionID;
        public string PhoneNo;
        public int Money;
        public int Diamond;
        public int BindTickets;    // 绑定桌卡：只可自己消耗，不可转让
        public int Tickets;        // 普通桌卡：正常使用、转让
        public string HeadURL;
		public int Sex ;

        public int TotalTicket
        {
            get { return BindTickets + Tickets; }
        }
    }
    public class SysUserData
    {
        //自己的基本信息
        private UserInfo m_userInfo;
        public UserInfo UserInfo
        {
            set {
                m_userInfo = value;
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.EReciveedUserData);
            }
            get {
                if (m_userInfo == null)
                    m_userInfo = new UserInfo();
                return m_userInfo;
            }
        }

        //别人的基本信息
        private Dictionary<long, UserInfo> other_userInfo = new Dictionary<long, UserInfo>();


        //取得其他人的基本信息
        public UserInfo GetOtherUserInfo(long userid)
        {
            if(other_userInfo.ContainsKey(userid))
            {
                return other_userInfo[userid];
            }
            return null;
        }
        public void AddOtherUserInfo(UserInfo info)
        {
            if (other_userInfo.ContainsKey(info.UserID))
                other_userInfo[info.UserID] = info;
            else
                other_userInfo.Add(info.UserID, info);
        }

        /// <summary>
        /// 转换方法
        /// </summary>
        public static UserInfo ProtoToData(Msg.UserInfo userInfo)
        {
            UserInfo result = new UserInfo();
            result.UserID = userInfo.UserID;
            result.Name = userInfo.Name;
            result.Level = userInfo.Level;
            //result.TotalBouts = userInfo.TotalBouts;
            //result.WinBouts = userInfo.WinBouts;
            result.RegionID = userInfo.RegionID;
            result.PhoneNo = userInfo.PhoneNo;
            result.Money = userInfo.Money;
            result.Diamond = userInfo.Diamond;
            result.BindTickets = userInfo.BindTickets;
            result.Tickets = userInfo.Tickets;
            result.HeadURL = userInfo.HeadURL;
			result.Sex = userInfo.Sex;
            return result;
        }
    }
}