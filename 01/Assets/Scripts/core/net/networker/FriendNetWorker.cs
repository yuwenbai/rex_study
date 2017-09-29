/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Msg ;

namespace projectQ
{
    //注册数据处理 好友网络数据处理
    public partial class ModelNetWorker{

        public void initDefaultHandleOfFriend()
        {
            ModelNetWorker.Regiest<OtherPlayerInfoRsp>(OtherPlayerInfoRsp);

            ModelNetWorker.Regiest<MyFriendsListRsp>(MyFriendsListRsp);
            ModelNetWorker.Regiest<DelFriendRsp>(DelFriendRsp);
            ModelNetWorker.Regiest<AddFriendRsp>(AddFriendRsp);
            ModelNetWorker.Regiest<ApplyFriendRsp>(ApplyFriendRsp);
            ModelNetWorker.Regiest<DelFriendNotify>(DelFriendNotify);


            ModelNetWorker.Regiest<UpdateFriendStatus>(UpdateFriendStatus);
            ModelNetWorker.Regiest<InviteFriendGameRsp>(InviteFriendGameRsp);
            ModelNetWorker.Regiest<InviteFriendGameNotify>(InviteFriendGameNotify);


            ModelNetWorker.Regiest<JoinFriendMjDeskRsp>(JoinFriendMjDeskRsp);
            ModelNetWorker.Regiest<JoinFriendMjDeskNotify>(JoinFriendMjDeskNotify);


            ModelNetWorker.Regiest<IsRefuseApplyRsp>(IsRefuseApplyRsp);
        }

        #region Get Message

        /// <summary>
        /// 获得好友列表
        /// </summary>
        /// <param name="rsp">Rsp.</param>
        public void MyFriendsListRsp(object rsp)
        {
            var prsp = rsp as MyFriendsListRsp;
            for (int i = 0; i < prsp.FriendList.Count; i++)
            {
                var p = prsp.FriendList[i];
                MemoryData.FriendData.AddFriend(p);
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_PlayerData_Update,p.UserID);
            }

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_FriendListData_Updata);
            //FIRE_EVENT 获取到好友信息
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.LoginSouccessRsp, "MyFriendsList");


        }


        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="rsp">Rsp.</param>
        public void DelFriendRsp(object rsp)
        {
            var prsp = rsp as DelFriendRsp;
            if(prsp.ResultCode != 0)
            {
            }
        }


        /// <summary>
        /// 增加好友反馈
        /// </summary>
        /// <param name="rsp">Rsp.</param>
        public void AddFriendRsp(object rsp)
        {
            var prsp = rsp as AddFriendRsp;
            WindowUIManager.Instance.CreateTip("已发送好友请求，请等待对方同意");
            //EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Friend_AddRsp);
        }

        /// <summary>
        /// 同意添加好友反馈
        /// </summary>
        /// <param name="obj"></param>
        public void ApplyFriendRsp(object obj)
        {
            var rsp = obj as ApplyFriendRsp;
            if(rsp.ResultCode == 0)
            {
                //MyFriendsListReq();
            }
        }


        public void DelFriendNotify(object obj)
        {
            var rsp = obj as DelFriendNotify;
            MemoryData.FriendData.RemoveFriend(rsp.DestID);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_FriendListData_Updata);
        }

        /// <summary>
        /// 接收其他用户信息
        /// </summary>
        /// <param name="obj"></param>
        public void OtherPlayerInfoRsp(object obj)
        {
            var rsp = obj as OtherPlayerInfoRsp;
            PlayerDataModel userDataBase;
            if (rsp.BaseInfo.UserID == MemoryData.UserID)
            {
                userDataBase = MemoryData.PlayerData.MyPlayerModel;
            }
            else
            {
                userDataBase = MemoryData.PlayerData.get(rsp.BaseInfo.UserID);
            }

            userDataBase.SetUserInfo(rsp.BaseInfo);
            userDataBase.SetMjRoomPlayerInfo(rsp.MjDataInfo);

            MemoryData.SysPlayerDataHandle.PlayerRsp(userDataBase.PlayerDataBase.UserID, SysPlayerDataHandle.PlayerDataType.All);
        }

        /// <summary>
        /// 好友状态更新
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateFriendStatus(object obj)
        {
            var rsp = obj as UpdateFriendStatus;
            MemoryData.PlayerData.get(rsp.Friend.UserID).SetProtoMyFriend(rsp.Friend);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_FriendStatus_Updata);
        }

        /// <summary>
        /// 邀请好友进桌的回复
        /// </summary>
        /// <param name="obj"></param>
        public void InviteFriendGameRsp(object obj)
        {
            var rsp = obj as InviteFriendGameRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Friend_InviteFriendGame,rsp.ResultCode,rsp.UserID);
        }

        /// <summary>
        /// 被邀请进桌的回复
        /// </summary>
        /// <param name="obj"></param>
        public void InviteFriendGameNotify(object obj)
        {
            var rsp = obj as InviteFriendGameNotify;
            MjRoom mjRoom = null;
            if(rsp.RoomInfo != null)
            {
                mjRoom = MjRoom.ProtoToData(rsp.RoomInfo);
                MemoryData.MjHallData.AddMjHallMap(mjRoom);
            }
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysStagingData_Friend_BeInviteGame, rsp.DeskID,rsp.UserID, mjRoom);
        }

        /// <summary>
        /// 申请进桌的回复
        /// </summary>
        /// <param name="obj"></param>
        public void JoinFriendMjDeskRsp(object obj)
        {
            var rsp = obj as JoinFriendMjDeskRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Friend_JoinFriendDeskRsp,rsp.ResultCode,rsp.FriendUserID);
        }

        /// <summary>
        /// 收到申请进桌消息
        /// </summary>
        /// <param name="obj"></param>
        public void JoinFriendMjDeskNotify(object obj)
        {
            var rsp = obj as JoinFriendMjDeskNotify;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Friend_BeJoinFriendDeskNotify,rsp.UserID, rsp.DeskID);
        }


        /// <summary>
        /// 添加好友 好友的反馈 true为拒绝
        /// </summary>
        /// <param name="obj"></param>
        public void IsRefuseApplyRsp(object obj)
        {
            var rsp = obj as IsRefuseApplyRsp;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Friend_RefuseApplyRsp, rsp.UserName, rsp.IsRefuse, rsp.DestID);
        }
        #endregion

        #region Send Message


        /// <summary>
        /// 查找好友
        /// </summary>
        /// <param name="uid">Uid.</param>
        public void MyFriendsListReq()
        {
            var msg = new MyFriendsListReq();
            msg.UserID = MemoryData.UserID ;
            this.send (msg);
        }

        /// <summary>
        /// 增加好友
        /// </summary>
        /// <param name="uid">Uid.</param>
        public void AddFriendReq(long uid)
        {
            var msg = new AddFriendReq();
            msg.DestID = uid;
            msg.UserID = MemoryData.UserID ;

            this.send (msg);
        }

        ///// <summary>
        ///// 别人请求加我好友 临时测试使用
        ///// </summary>
        ///// <param name="uid">Uid.</param>
        //public void AddFriendReqTest(long uid)
        //{
        //    var msg = new AddFriendReq();
        //    msg.DestID = MemoryData.UserID;
        //    msg.UserID = uid;

        //    this.send(msg);
        //}

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="uid">Uid.</param>
        public void DelFriendReq(long uid)
        {
            var msg = new DelFriendReq();
            msg.DestID = uid;
            msg.UserID = MemoryData.UserID ;

            MemoryData.FriendData.RemoveFriend(uid);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_FriendListData_Updata);
            this.send (msg);
        }

        /// <summary>
        /// 是否同意加好友
        /// </summary>
        /// <param name="deskId"></param>
        public void ApplyFriendReq(long deskId,bool isConfirm)
        {
            var req = new ApplyFriendReq();
            req.UserID = MemoryData.UserID;
            req.DestID = deskId;
            req.IsConfirm = isConfirm;
            this.send(req);
            if(!isConfirm)
            {
                MemoryData.FriendData.RemoveFriend(deskId);
            }
        }

        /// <summary>
        /// 请求其他用户信息
        /// </summary>
        /// <param name="userId"></param>
        public void OtherPlayerInfoReq(long userId)
        {
            var req = new OtherPlayerInfoReq();
            req.UserID = userId;
            this.send(req);
        }

        /// <summary>
        /// 邀请好友进桌
        /// </summary>
        public void InviteFriendGameReq(long friendUserId, int deskId)
        {
            var req = new InviteFriendGameReq();
            req.UserID = friendUserId;
            req.DeskID = deskId;
            this.send(req);
        }

        /// <summary>
        /// 拒绝邀请进桌
        /// </summary>
        /// <param name="inviteUserID">邀请人的UserID</param>
        public void FefuseFriendReq(long inviteUserID)
        {
            var req = new FefuseFriendReq();
            req.UserID = MemoryData.UserID;
            req.InviteUserID = inviteUserID;
            this.send(req);
        }


        /// <summary>
        /// 请求进入好友牌桌
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="deskId"></param>
        public void JoinFriendMjDeskReq(long friendId,int deskId)
        {
            var req = new JoinFriendMjDeskReq();
            req.UserID = MemoryData.UserID;
            req.FriendUserID = friendId;
            req.DeskID = deskId;
            this.send(req);
        }

        public void RecvDeskJoinReq(long friendId,int deskId,bool isApply)
        {
            var req = new RecvDeskJoinReq();
            req.UserID = MemoryData.UserID;
            req.FriendUserID = friendId;
            req.DeskID = deskId;
            req.IsApply = isApply;
            this.send(req);
        }
        #endregion

    }
}