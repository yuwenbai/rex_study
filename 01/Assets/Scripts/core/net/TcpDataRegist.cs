/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:CEPH
 *	创建时间：11/07/2015
 *	文件名：  TcpDataHandler.cs @ herocraft151104
 *	文件功能描述：
 *  创建标识：ceph.11/07/2015
 *	创建描述：网路连接功能类
 *
 *  修改标识：
 *  修改描述：
 *
 *
 *
 *****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Msg;
using System.Diagnostics;
using UnityEngine.Events;

namespace projectQ
{
    /// <summary>
    /// 消息Enum 与 proto对象 对应
    /// </summary>
    public static class TcpDataRegist
    {

        private static bool isInit = false;

        public static void Init()
        {
            if (isInit)
                return;
            isInit = true;


            #region Login


            //TcpDataHandler.AddType(CmdNo.CmdNo_ServerMsgID, typeof(X2XServerMsg));
            //TcpDataHandler.AddType(CmdNo.CmdNo_StartServer, typeof(StartServer));



            TcpDataHandler.AddType(CmdNo.CmdNo_Gateway_Req, typeof(GatewayReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_Gateway_Rsp, typeof(GatewayRsp));

            TcpDataHandler.AddType(CmdNo.CmdNo_Login_Reg_Req, typeof(RegisterReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_Login_Reg_Rsp, typeof(RegisterRsp));

            TcpDataHandler.AddType(CmdNo.CmdNo_Login_Auth_Req, typeof(LoginReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_Login_Auth_Rsp, typeof(LoginRsp));

            TcpDataHandler.AddType(CmdNo.CmdNo_Login_Enter_Req, typeof(EnterGameReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_Login_Enter_Rsp, typeof(EnterGameRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_Login_UserData_Notify, typeof(UserDataNotify));
            #endregion

            #region 好友
            TcpDataHandler.AddType(CmdNo.CmdNo_MyFriends_Req, typeof(MyFriendsListReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_MyFirends_Rsp, typeof(MyFriendsListRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_AddFriend_Req, typeof(AddFriendReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_AddFriend_Rsp, typeof(AddFriendRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_DelFriend_Req, typeof(DelFriendReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_DelFriend_Rsp, typeof(DelFriendRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_ApplyFriend_Req, typeof(ApplyFriendReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_ApplyFriend_Rsp, typeof(ApplyFriendRsp));

            TcpDataHandler.AddType(CmdNo.CmdNo_UpdateFriend_Notify, typeof(UpdateFriendStatus));
            //邀请好友进桌
            TcpDataHandler.AddType(CmdNo.CmdNo_InviteGame_Req, typeof(InviteFriendGameReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_InviteGame_Rsp, typeof(InviteFriendGameRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_InviteGame_Notify, typeof(InviteFriendGameNotify));
            TcpDataHandler.AddType(CmdNo.CmdNo_RefuseInvite_Req, typeof(FefuseFriendReq));

            //好友申请进桌
            TcpDataHandler.AddType(CmdNo.CmdNo_JoinFriendMjDesk_Req, typeof(JoinFriendMjDeskReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_JoinFriendMjDesk_Rsp, typeof(JoinFriendMjDeskRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_JoinFriendMjDesk_Notify, typeof(JoinFriendMjDeskNotify));
            TcpDataHandler.AddType(CmdNo.CmdNo_RecvDeskJoin_Req, typeof(RecvDeskJoinReq));

            //删除好友
            TcpDataHandler.AddType(CmdNo.CmdNo_DelFriend_Notify, typeof(DelFriendNotify));

            //申请加好友的反馈 我被拒绝了还是同意了
            TcpDataHandler.AddType(CmdNo.CmdNo_IsRefuseApply_Rsp, typeof(IsRefuseApplyRsp));

            #endregion

            #region 大厅



            //其他玩家信息(基本用在好友详情)
            TcpDataHandler.AddType(CmdNo.CmdNo_OtherPlayer_Rsp, typeof(OtherPlayerInfoRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_OtherPlayer_Req, typeof(OtherPlayerInfoReq));

            TcpDataHandler.AddType(CmdNo.CmdNo_ModifyAccount_Req, typeof(ModifyAccountReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_ModifyAccount_Rsp, typeof(ModifyAccountRsp));

            //邮件消息
            TcpDataHandler.AddType(CmdNo.CmdNo_MailList_Req, typeof(MyMailListReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_MailList_Rsp, typeof(MyMailListRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_ReadMail_Req, typeof(MailReadReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_ReadMail_Rsp, typeof(MailReadRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_PickMail_Req, typeof(MailPickupAttachReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_PickMail_Rsp, typeof(MailPickupAttachRsp));

            //背包
            TcpDataHandler.AddType(CmdNo.CmdNo_BagItemList_Req, typeof(MyBagItemListReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_BagItemList_Rsp, typeof(MyBagItemListRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_SyncBagItem_Notify, typeof(SyncMyBagItemNotify));


            TcpDataHandler.AddType(CmdNo.CmdNo_VerifyCode_Req, typeof(GetVerifyCodeReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_BindPhone_Req, typeof(BindPhoneReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_BindPhone_Rsp, typeof(BindPhoneRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_Certification_Req, typeof(CertificationReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_Certification_Rsp, typeof(CertificationRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_UserRegionSetup_Req, typeof(UserRegionSetupReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_UserRegionSetup_Rsp, typeof(UserRegionSetupRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_SystemNotice_Notify, typeof(SystemNotice));
            TcpDataHandler.AddType(CmdNo.CmdNo_WeiXin_Sign_Req, typeof(WeiXinSignReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_WeiXin_Sign_Rsp, typeof(WeiXinSignRsp));

            //麻将玩法

            TcpDataHandler.AddType(CmdNo.MjCmd_RulerConfig_Req, typeof(MjRulerConfigReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_RulerConfig_Rsp, typeof(MjRulerConfigRsp));

            //获取麻将战绩信息
            TcpDataHandler.AddType(CmdNo.MjCmd_ViewMyRecord_Req, typeof(FMjRoomMyRecordReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_ViewMyRecord_Rsp, typeof(FMjRoomMyRecordRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_MyRecordUpdate_Notify, typeof(FMjRoomMyRecordNotify)); // 有新的战绩更新，不用多次请求
            TcpDataHandler.AddType(CmdNo.MjCmd_MyRecordOp_Req, typeof(FMjRoomMyRecordOpReq));         // 跟服务器发送战绩记录操作：已读，删除
            TcpDataHandler.AddType(CmdNo.MjCmd_MyRecordOp_Rsp, typeof(FMjRoomMyRecordOpRsp));         // 服务器返回战绩记录操作

            //活动中心消息
            TcpDataHandler.AddType(CmdNo.CmdNo_Activity_Notify, typeof(SyncActivityListNotify));

            //抽奖数据信息
            TcpDataHandler.AddType(CmdNo.CmdNo_AwardConfig_Req, typeof(GetAwardConfigReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_AwardConfig_Rsp, typeof(GetAwardConfigRsp));

            //抽奖
            TcpDataHandler.AddType(CmdNo.CmdNo_Lottery_Req, typeof(LotteryReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_Lottery_Rsp, typeof(LotteryRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_PickupAward_Req, typeof(PickupAwardReq));
            TcpDataHandler.AddType(CmdNo.CmdNO_PickupAward_Rsp, typeof(PickupAwardRsp));

            //加好友有礼
            TcpDataHandler.AddType(CmdNo.CmdNo_GetInviteeList_Req, typeof(GetInviteeListReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_GetInviteeList_Rsp, typeof(GetInviteeListRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_BindInviteCode_Req, typeof(BindInviteCodeReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_BindInviteCode_Rsp, typeof(BindInviteCodeRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_PickInviteAward_Req, typeof(PickInviteAwardReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_PickInviteAward_Rsp, typeof(PickInviteAwardRsp));

            //聊天
            TcpDataHandler.AddType(CmdNo.MjCmd_DeskChat_Req, typeof(ChatReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_DeskChat_Notify, typeof(ChatNotify));
            //礼包
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomPushGift_Req, typeof(FRoomPushGiftReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomPushGift_Rsp, typeof(FRoomPushGiftRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomReciveGift_Req, typeof(FRoomReciveGiftReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomReciveGift_Rsp, typeof(FRoomRecveGiftRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomPushGfit_Notify, typeof(FRoomPushGiftNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomReciviGfit_Notify, typeof(FRoomRecvieGiftNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_PushMoneyBag_Req, typeof(PushMoneyBagReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_PushMoneyBag_Rsp, typeof(PushMoneyBagRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_PushMoneyBagResult_Req, typeof(PushMoneyBagResultReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_PushMoneyBagResult_Rsp, typeof(PushMoneyBagResultRsp));

            //馆长麻将馆桌卡销售记录
            TcpDataHandler.AddType(CmdNo.MjCmd_QuerySaleData_Req, typeof(FRoomQuerySaleDataReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_QuerySaleData_Rsp, typeof(FRoomQuerySaleDataRsp));

            //购买桌卡
            TcpDataHandler.AddType(CmdNo.CmdNo_GoodsList_Req, typeof(GetGoodsListReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_GoodsList_Rsp, typeof(GetGoodsListRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_BuyGoods_Req, typeof(BuyGoodsReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_BuyGoods_Rsp, typeof(BuyGoodsRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_PayFinish_Notify, typeof(PayFinishNotify));
            TcpDataHandler.AddType(CmdNo.CmdNo_PayStart_Notify, typeof(PayStartNotify));

            //首页官方试玩群
            TcpDataHandler.AddType(CmdNo.MjCmd_OfficialGroup_Req, typeof(GetOfficialGroupReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_OfficialGroup_Rsp, typeof(GetOfficialGroupRsp));


            //公告消息接收
            TcpDataHandler.AddType(CmdNo.CmdNo_GetBannerList_Req, typeof(GetBannerListReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_GetBannerList_Rsp, typeof(GetBannerListRsp));
            #endregion

            #region 麻将馆
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomPlayerInfo_Req, typeof(FMjRoomPlayerInfoReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomPlayerInfo_Rsp, typeof(FMjRoomPlayerInfoRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomInfo_Req, typeof(FMjRoomDescInfoReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomInfo_Rsp, typeof(FMjRoomDescInfoRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomSearch_Req, typeof(FMjRoomSearchReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomSearch_Rsp, typeof(FMjRoomSearchRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomBind_Req, typeof(FMjRoomBindReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomBind_Rsp, typeof(FMjRoomBindRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomUnBind_Req, typeof(FMjRoomUnBindReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomUnBind_Rsp, typeof(FMjRoomUnBindRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_FMjRoomUnBind_Notify, typeof(FMjRoomUnBindNotify));


            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomTuijian_Req, typeof(FMjRoomTuijianReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomTuijian_Rsp, typeof(FMjRoomTuijianRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomEnter_Req, typeof(FMjRoomEnterReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomEnter_Rsp, typeof(FMjRoomEnterRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomLeave_Req, typeof(FMjRoomLeaveReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomLeave_Rsp, typeof(FMjRoomLeaveRsp));

            TcpDataHandler.AddType(CmdNo.CmdNo_SyncMoney_Notify, typeof(SyncMoney));

            //切换牌匾
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomChangeBoard_Req, typeof(FMjRoomChangeBoardReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomChangeBoard_Rsp, typeof(FMjRoomChangeBoardRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomChangeBoard_Notify, typeof(FMjRoomChangeBoardNotify));

            //馆主查看麻将馆 
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomMyRoom_Req, typeof(MyFMjRoomReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomMyRoom_Rsp, typeof(MjFMjRoomRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomUpdate_Notify, typeof(MyFMjRoomUpdateNotify));
            //查看战绩记录
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomDeskRecord_Req, typeof(FMjRoomViewDeskRecordReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomDeskRecord_Rsp, typeof(FMjRoomViewDeskRecordRsp));
            //馆长申请完成通知客户端
            TcpDataHandler.AddType(CmdNo.CmdNo_RoomApplyFinish_Notify, typeof(RoomApplyFinishNotify));
            //作弊码
            TcpDataHandler.AddType(CmdNo.MjCmd_TestReq, typeof(MjTestReq));
            //关联的客户信息
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomPlayers_Req, typeof(RoomPlayersReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_FRoomPlayers_Rsp, typeof(RoomPlayersRsp));

            #endregion

            #region Mahjong Cmd
            TcpDataHandler.AddType(CmdNo.MjCmd_NewDesk_Req, typeof(MjNewDeskReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_NewDesk_Rsp, typeof(MjNewDeskRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_JoinDesk_Req, typeof(MjJoinDeskReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_JoinDesk_Rsp, typeof(MjJoinDeskRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_DeskUsers_Notify, typeof(MjDeskUsersNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_OpAction_Req, typeof(MjOpActionReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_OpAction_Rsp, typeof(MjOpActionRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_OpAction_Notify, typeof(MjOpActionNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_OpPutMjNotify, typeof(MjOpPutMjNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_StartGame_Notify, typeof(MjRoomGameStartNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_InitMjList_Notify, typeof(MjGameInitMjListNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_HuPai_Notify, typeof(MjHuPaiNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_PutDownHandsCodeNotify, typeof(MjPutDownHandsCodeNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_BalanceNotify, typeof(MjBalanceNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_GameOverNotify, typeof(MjGameOverNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_DeskAction_Req, typeof(MjDeskActionReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_DeskAction_Rsp, typeof(MjDeskActionRsp));
            TcpDataHandler.AddType(CmdNo.MjCmd_DeskAction_Notify, typeof(MjDeskActionNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_SyncPlayerState_Notify, typeof(MjSyncPlayerStateNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_DealerBegin_Notify, typeof(MjDealerBeginNotify));

            TcpDataHandler.AddType(CmdNo.MjCmd_BeforeDispatch, typeof(MjBeforeDispatch));
            TcpDataHandler.AddType(CmdNo.MjCmd_StandingPlates, typeof(MjStandingPlates));

            TcpDataHandler.AddType(CmdNo.MjCmd_AskReqChangeThree, typeof(MjAskReqChangeThree));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqChangeThree, typeof(MjReqChangeThree));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspChangeThree, typeof(MjRspChangeThree));
            TcpDataHandler.AddType(CmdNo.MjCmd_ChangeThreeNotify, typeof(MjChangeThreeNotify));

            TcpDataHandler.AddType(CmdNo.MjCmd_AskReqConfirm, typeof(MjAskReqConfirm));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqConfirm, typeof(MjReqConfirm));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspConfirm, typeof(MjRspConfirm));
            TcpDataHandler.AddType(CmdNo.MjCmd_ConfirmNotify, typeof(MjConfirmNotify));

            TcpDataHandler.AddType(CmdNo.MjCmd_AskReqPao, typeof(MjAskReqPao));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqPao, typeof(MjReqPao));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspPao, typeof(MjRspPao));
            TcpDataHandler.AddType(CmdNo.MjCmd_PaoNotify, typeof(MjPaoNotify));

            TcpDataHandler.AddType(CmdNo.MjCmd_Obligate, typeof(MjObligate));
            TcpDataHandler.AddType(CmdNo.MjCmd_BuyHorseCountNotify, typeof(MjBuyHorseCountNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_BuyHorseNotify, typeof(MjBuyHorseNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_GetFlowerNotify, typeof(MjGetFlowerNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_BalanceNewNotify, typeof(MjBalanceNewNotify));


            //明楼
            TcpDataHandler.AddType(CmdNo.MjCmd_GetMingLouReq, typeof(MjGetMingLouReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_GetMingLouNotify, typeof(MjGetMingLouNotify));

            //下炮子
            TcpDataHandler.AddType(CmdNo.MjCmd_AskReqPaoZi, typeof(MjAskReqPaoZi));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqPaoZi, typeof(MjReqPaoZi));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspPaoZi, typeof(MjRspPaoZi));
            TcpDataHandler.AddType(CmdNo.MjCmd_PaoZiNotify, typeof(MjPaoZiNotify));

            //下鱼
            TcpDataHandler.AddType(CmdNo.MjCmd_AskReq258TiaoYu, typeof(MjAskReq258TiaoYu));
            TcpDataHandler.AddType(CmdNo.MjCmd_Req258TiaoYu, typeof(MjReq258TiaoYu));
            TcpDataHandler.AddType(CmdNo.MjCmd_Rsp258TiaoYu, typeof(MjRsp258TiaoYu));
            TcpDataHandler.AddType(CmdNo.MjCmd_258TiaoYuNotify, typeof(Mj258TiaoYuNotify));

            TcpDataHandler.AddType(CmdNo.MjCmd_ScoreChange_Notify, typeof(MjScoreChangeNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_FollowDealer, typeof(MjFollowDealer));
            TcpDataHandler.AddType(CmdNo.MjCmd_Time_Notify, typeof(MjTimeNotify));

            TcpDataHandler.AddType(CmdNo.MjCmd_OnLineOffLine, typeof(MjOnLineOffLine));

            TcpDataHandler.AddType(CmdNo.MjCmd_DeskReconect_Req, typeof(MjDeskReconectReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_DeskReconect_Rsp, typeof(MjDeskReconectRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_BalanceReq, typeof(MjBalanceReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_BalanceRsp, typeof(MjBalanceRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_BiaoYanNotify, typeof(MjBiaoYanNotify));

            #region 坎牌 闹庄 末留
            //闹庄 坎牌 末留
            TcpDataHandler.AddType(CmdNo.MjCmd_NaoZhuangMoLiuKanPaiNotify, typeof(MjNaoZhuangMoLiuKanPaiNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspNaoZhuangMoLiuKanPai, typeof(MjRspNaoZhuangMoLiuKanPai));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqNaoZhuangMoLiuKanPai, typeof(MjReqNaoZhuangMoLiuKanPai));
            #endregion

            #region 飘金 飘素
            TcpDataHandler.AddType(CmdNo.MjCmd_PiaoJinPaioSuNotify, typeof(MjPiaoJinPaioSuNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqGameingRulerSet, typeof(MjReqGameingRulerSet));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspGameingRulerSet, typeof(MjRspGameingRulerSet));
            #endregion

            //明楼听口通知
            TcpDataHandler.AddType(CmdNo.MjCmd_TingKouInfoList, typeof(TingKouInfoList));

            //大结算 打开与关闭 消息（用来让服务器知晓客户端是否真正退出了麻将）
            TcpDataHandler.AddType(CmdNo.MjCmd_GameResultBoardState_Req, typeof(GameResultBoardStateReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_GameResultBoardState_Rsp, typeof(GameResultBoardStateRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_ChangeRulerNotify, typeof(MjRulerChangeNotify));


            #region 杠后拿吃
            TcpDataHandler.AddType(CmdNo.MjCmd_GangHouNaChiNotify, typeof(MjGangHouNaChiNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqGameingNaChi, typeof(MjReqGameingNaChi));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspGameingNaChi, typeof(MjRspGameingNaChi));
            #endregion

            TcpDataHandler.AddType(CmdNo.MjCmd_TipInfoNotify, typeof(MjTipInfoNotify));

            #region 亮一张
            //亮一张
            TcpDataHandler.AddType(CmdNo.MjCmd_LiangYiZhangNotify, typeof(MjLiangYiZhangNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqLiangYiZhang, typeof(MjReqLiangYiZhang));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspLiangYiZhang, typeof(MjRspLiangYiZhang));

            #endregion


            #region 漂胡 跟漂
            TcpDataHandler.AddType(CmdNo.MjCmd_PiaoHuNotify, typeof(MjPiaoHuNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqPiaoHu, typeof(MjReqPiaoHu));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspPiaoHu, typeof(MjRspPiaoHu));
            #endregion


            #region 亮四打一 
            TcpDataHandler.AddType(CmdNo.MjCmd_LiangSiDaYiNotify, typeof(MjLiangSiDaYiNotify));
            #endregion

            #region 补花
            TcpDataHandler.AddType(CmdNo.MjCmd_BuHuaNotify, typeof(MjBuHuaNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_SelfBuHuaNotify, typeof(MjSelfBuHuaNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqBuHuaZhang, typeof(MjReqBuHuaZhang));
            #endregion

            #region 房卡赢家扣卡
            TcpDataHandler.AddType(CmdNo.CmdNo_WinnerCostCard_Rsp, typeof(MjWinnerCostCardInfoRsp));
            #endregion

            #endregion

            #region 其他
            //消息接收
            TcpDataHandler.AddType(CmdNo.CmdNo_MsgBoard_Notify, typeof(MsgBoardNotify));
            //被踢掉线
            TcpDataHandler.AddType(CmdNo.CmdNo_PlayerKickNotify, typeof(PlayerKickNotify));
            //服务器主动Ping
            TcpDataHandler.AddType(CmdNo.CmdNo_AskClient_Notify, typeof(AskClientNotify));
            //服务器主动让客户端断开连接
            TcpDataHandler.AddType(CmdNo.CmdNo_NetErrBreak_Notify, typeof(NetErrBreakNotify));

            TcpDataHandler.AddType(CmdNo.CmdNo_SyncUserState_Notify, typeof(SyncUserStateNotify));
            //



            #endregion

            TcpDataHandler.AddType(CmdNo.CmdNo_ServerMsgID, typeof(X2XServerMsg));
            TcpDataHandler.AddType(CmdNo.CmdNo_StartServer, typeof(StartServer));
            TcpDataHandler.AddType(CmdNo.CmdNo_PingReq, typeof(PingCmd));
            TcpDataHandler.AddType(CmdNo.CmdNo_ClientReport_Req, typeof(ClientStatReport));


            TcpDataHandler.AddType(CmdNo.MjCmd_ReportGPSInfo_Req, typeof(ReportGPSInfoReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReportGPSInfo_Rsp, typeof(ReportGPSInfoRsp));

            TcpDataHandler.AddType(CmdNo.CmdNo_ReportGPSInfo_Req, typeof(UserGPSInfoReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_ReportGPSInfo_Rsp, typeof(UserGPSInfoRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_PlayerWarning_Notify, typeof(PlayerWarningNotify));

            TcpDataHandler.AddType(CmdNo.MjCmd_AskDeskData_Req, typeof(AskDeskDataReq));
            TcpDataHandler.AddType(CmdNo.MjCmd_AskDeskData_Rsp, typeof(AskDeskDataRsp));

            TcpDataHandler.AddType(CmdNo.MjCmd_DuAnGangNotify, typeof(MjDuAnGangNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_ReqDuAnGang, typeof(MjReqDuAnGang));
            TcpDataHandler.AddType(CmdNo.MjCmd_RspDuAnGang, typeof(MjRspDuAnGang));

            TcpDataHandler.AddType(CmdNo.MjCmd_NiuPaiNotify, typeof(MjNiuPaiNotify));
            TcpDataHandler.AddType(CmdNo.MjCmd_NiuPai_Req, typeof(MjReqNiuPai));
            TcpDataHandler.AddType(CmdNo.MjCmd_NiuPai_Rsp, typeof(MjRspNiuPai));

            //回放
            TcpDataHandler.AddType(CmdNo.CmdNo_DeskInfo_Req, typeof(DeskInfoReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_DeskInfo_Rsp, typeof(DeskInfoRsp));
            TcpDataHandler.AddType(CmdNo.CmdNo_DeskRecord_Req, typeof(DeskRecordReq));
            TcpDataHandler.AddType(CmdNo.CmdNo_DeskRecord_Rsp, typeof(DeskRecordRsp));
            //回放专属消息 带有其他3个玩家的数据
            TcpDataHandler.AddType(CmdNo.CmdNo_ShowDeskCard_Rsp, typeof(ShowDeskCard));

            //换3张专属数据协议
            TcpDataHandler.AddType(CmdNo.CmdNo_ChangeThreeeCard_Notify, typeof(ChangeThreeCard));


            #region 出门断
            TcpDataHandler.AddType(CmdNo.MjCmd_PutLimitNotify, typeof(MjPutLimitNotify));
            #endregion
            #region 亮杠头
            TcpDataHandler.AddType(CmdNo.MjCmd_LiangGangTouNotify, typeof(MjLiangGangTouNotify));
            #endregion

            #region 四家买马
            TcpDataHandler.AddType(CmdNo.MjCmd_SiJiaMaiMaNotify, typeof(MjSiJiaMaiMaNotify));
            #endregion

            #region 风圈
            TcpDataHandler.AddType(CmdNo.MjCmd_FengQuanNotify, typeof(MjFengQuanNotify));
            #endregion

        }
    }

}