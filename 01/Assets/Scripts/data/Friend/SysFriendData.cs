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
    //MemoryData持有的
    public class SysFriendData
    {
        private int _mMaxFriendCount ;
        private List<long> _mFriendList = new List<long>();


        /// <summary>
        /// Gets the friend list.
        /// </summary>
        /// <value>The friend list.</value>
        public List<long> FriendList {
            get { return _mFriendList ;}
        }

        public bool IsFriendByUserId(long friendId)
        {
            if(FriendList != null && FriendList.Count > 0)
            {
                return FriendList.IndexOf(friendId) >= 0;
            }
            return false;
        }

        /// <summary>
        /// Adds the friend.
        /// </summary>
        /// <param name="player">Player.</param>
        public void AddFriend(Msg.MyFriend player) {
            if (player.UserID <= 0)
            {
                QLoger.ERROR("AddFriend 添加好友数据时发现有UserId为0的。。。。。。。。。。。。。");
                return;
            }

            var playInfo = MemoryData.PlayerData.get(player.UserID);
            playInfo.SetProtoMyFriend(player);
            if (_mFriendList.IndexOf(player.UserID) == -1)
            {
                _mFriendList.Add(player.UserID);
            }

            if(player.IsTemp)
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_FriendListData_NewAdd,player.UserID);
            }
        }

        public void RemoveFriend(long userid)
        {
            _mFriendList.Remove(userid);
            MemoryData.PlayerData.get(userid).FriendPlayerInfo.IsTemp = true;
            MemoryData.PlayerData.get(userid).PlayerDataBase.isAddFriend = false;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_PlayerData_Update,userid, SysPlayerDataHandle.PlayerDataType.All);
        }

        /// <summary>
        /// 好友最大数量
        /// </summary>
        /// <value>The max friend count.</value>
        public int maxFriendCount {
            get { 
                return _mMaxFriendCount ;
            }
            set { 
                this._mMaxFriendCount = value;
            }
        }


        public int friendCount {
            get { 
                return this._mFriendList.Count ;
            }
        }

        public void Sort()
        {
            FriendList.Sort(friendSortFunc);
        }
        private int friendSortFunc(long a,long b)
        {
            var friendA = MemoryData.PlayerData.get(a);
            var friendB = MemoryData.PlayerData.get(b);
            //判空
            if (friendA == null)
                return 1;
            else if (friendB == null)
                return -1;

            //好友状态 1 掉线 2游戏中 3牌桌等待中 4空闲
            if (friendA.FriendPlayerInfo.state != friendB.FriendPlayerInfo.state)
            {
                if (friendA.FriendPlayerInfo.state == 1)
                    return 1;
                else if (friendB.FriendPlayerInfo.state == 1)
                    return -1;
            }

            //热度
            if(friendA.FriendPlayerInfo.hotPoint != friendB.FriendPlayerInfo.hotPoint)
            {
                friendB.FriendPlayerInfo.hotPoint.CompareTo(friendA.FriendPlayerInfo.hotPoint);
            }


            //名字
            return friendA.PlayerDataBase.Name.CompareTo(friendB.PlayerDataBase.Name) ;
        }
    }
}