using UnityEngine;
using System.Collections;


public class GEnum
{
    public enum ID
    {

    }

    public enum NamedEvent
    {
        ENoName = 1,


        EGetGameData = 999, //获取网络模块数据

        EPublicEvent = 1000, //公共广播信息

        //充值
        EChargeResult1, //充值完成

        //统一处理状态
        ESyncUserMoney, //同步用户的金钱
        ESyncUserCreditsData, //同步玩家各游戏积分数据

        ERecivePing, //接收到服务器Ping

        ENetError,  //网络错误开始
        ENetErrorReconnect, //网络错误,重连网络
        ENetErrorReLogin, //网络错误,重新登陆
        ENetErrorEnd,  //网络错误结束
        //2017-05-05 YQC加入网络断线消息
        ENet_Connected,//连接成功
        ENetError_Kick,//被踢下线
        ENetError_Broken,//服务器断开连接
        ENetError_Fail,//未连接到服务器
        ENetError_TimeOut,//超时
        ENetError_Client,//客户端主动发起请求重连

        EPublicEventEnd = 1999,

        EBeforGame = 2000,//网关服务器
        ELoginServer,
        ELoginResult, //登陆结果
        ERegistResult, //注册结果
        EEnterGameResult,//进入游戏结果
        EReciveedUserData,//接收用户数据
        ELoginWXResult, //微信登陆结果
        EWXInstalledResult, //有无微信结果返回
        EModificationAccountResult,//修改帐号返回结果
        EBeforGameEnd = 2999,

        EGameHall = 3000,
        EGHJoinRoundResult, //加入游戏结果
        EGHCreditsRankDataResult, //请求积分排行结果
        EGameHallEnd = 3999,

        EUserCenter = 4000,
        EUCChangeUserInfoResult,//修改用户信息结果
        EUCBuyVIPResult,//购买VIP结果

        ERCGetRankDataRsp,//获取到排行信息

        ERCGetSignResult,//获取签到信息

        ERCCloseingAndRelushUI, //更新面板，并关闭面板的Loading

        EUserCenterEnd = 4999,


        EMahjongLoadSceneSuccess = 5000,       //麻将加载场景
        EMahjongJoinDesk = 5001,        //玩家自己加入desk
        EMahjongDeskPlayer = 5002,      //加入desk的玩家信息(也包括我自己)
        EMahjongReady = 5003,           //有人准备
        EMahjongUnReady = 5004,         //有人取消准备
        EMahjongQuit = 5005,            //有人退出房间             
        EMahjongGameStart = 5006,       //游戏开始
        EMahjongClose = 5007,           //房间解散 
        EMahjongContinue = 5008,        //游戏继续
        EMahjongNewDesk = 5009,         //创建房间
        EMahjongOutScene,               //麻将离开场景

        EYuyinState,                    //语音SDK状态初始化

        EMjNewDeskRsp,                 //创建游戏桌
        EMjJoinDeskRsp,                 //加入游戏桌
        EMjDeskUsersNotify,             //加入房间通知
        EMjOpActionRsp,
        EMjOpActionNotify,              //麻将行为通知
        EMjDeskActionReady,             //麻将有人准备
        EMjOpactionPutNotify,           //麻将出牌
        EMjRoomGameStartNotify,         //游戏开始通知
        EMjGameInitMjListNotify,        //游戏发牌通知
        EMjHuPaiNotify,                 //游戏胡牌通知
        EMjDeskActionNotify,            //桌上行为通知
        EMjDeskActionRsp,               //桌上行为结果
        EMjSyncPlayerStateNotify,       //麻将发牌通知
        EMjDealerBeginNotify,           //庄家第一轮开始
        EMjChangeFlowerWhenDeal,        //发牌后补花
        EMjChangeFlowerWhenPut,         //手动打出花

        EMjBeforeDispatch,              //游戏开始前的状态

        EMjAskReqChangeThree,           //通知进行换三张 
        EMjRspChangeThree,              //个人换三张结果
        EMjChangeThreeNotify,           //所有人换三张结果

        EMjAskReqConfirm,               //通知客户端进行定缺
        EMjRspConfirm,                  //个人定缺结果
        EMjConfirmNotify,              //所有人定缺结果

        EMjAskReqPao,                   //通知客户端进行下跑
        EMjRspPao,                      //个人下跑结果
        EMjPaoNotify,                   //所有人下跑结果

        EMjServerYu,                    //通知客户端下鱼逻辑

        EMjAskReqPaoZi,                   //通知客户端进行下跑
        EMjRspPaoZi,                      //个人下跑结果
        EMjPaoNotifyZi,                   //所有人下跑结果

        // 坎牌 闹庄 末留 
        EMjKanNaoMoNotify,                //状态通知 
        EMjKanNaoMoRsp,                   //状态回执
        EMjKanNaoMoData,                  //数据同步

        EMjObligate,                    //通知客户端预留牌
        EMjBuyHorseCountNotify,         //通知客户端买马
        //EMjBuyHorseNotify,              //通知客户端买马结果
        EMjOpenMaNotify,                //扎马，一码全中，抓鸟
        EMjGetFlowerNotify,             //通知客户端补花 
        EMjMinglouNotify,               //通知客户端明楼

        EMjPerformanceNotify,           //通知客户端表演 

        EMjScoreChangeNotify,           //桌上积分变化
        EMjFollowDealer,                //是否产生跟庄 

        EMjTimeNotify,                  //客户端时间通知 
        EMjOnlineStateNotify,           //在线状态notify
        EMjBalanceDataRsp,              //小结算数据推送

        EMjPutDownNotify,               //游戏结束，推牌
        EMjBalanceNotify,               //客户端小结算通知
        EMjBalanceNewNotify,            //客户端新的小结算通知 
        EMjGameOverNotify,              //客户端大结算通知 

        EMjReconned,                    //客户端重连

        EMjGameAnimOverNotify,            //扎马特效播放完毕
        UIChooseItemClickNotify,            //玩法选择界面按钮点击
        UIPlayingTypeChooseOver,           //玩法类型选择结束


        EMjXuanpiaoNotify,                  //选飘网络消息
        EMjXuanPiaoDataNotify,              //选飘数据
        UINumberFlyingSubmit,               //选飘界面发送数据
        UINumberFlyingChoose,               //选飘界面单项数据修改通知

        UIMinglouTingInfo,                  //明楼听牌的信息

        GameStateChange,                    //游戏状态改变

        #region mj ui controller
        EMjControlSetUIClose,           //麻将操作，设置麻将相关UI自己关闭逻辑
        EMjControlClickCloseResult,     //麻将操作，关闭战绩
        EMjControlClickCard,            //麻将操作，点击一张牌
        EMjControlClickCardShowTing,    //麻将操作，点一张牌展示ting状态      
        EMjControlClickChangeSort,      //麻将操作，点击转换手牌排序方式按钮
        EMjControlClickOpaction,        //麻将操作，点击opaction按钮    
        EMjControlClickTingTip,         //麻将操作，点击听口提示
        EMjControlClickMinglou,         //麻将操作，点击明楼
        EMjControlClickFangMao,         //麻将操作，点击放毛按钮
        EMjControlClickChooseBtn,       //麻将操作，下跑下炮点击按钮
        EMjControlClickChooseQue,       //麻将操作，定缺点击按钮
        EMjControlClickBuBtn,           //麻将操作，点击补牌查看按钮
        EMjControlClickHeighLight,      //麻将操作，浮起一张牌之后同牌高亮
        EMjControlClickCloseBalance,    //麻将操作，关闭小结算面板


        EMjControlSendChangeThree,      //麻将发送，请求发送换三张
        EMjControlSendMinglou,          //麻将发送，请求明楼
        EMjControlSendClose,            //麻将发送，请求发送解散桌子
        EMjControlSendCloseAnwser,      //麻将发送，解散桌子选择
        EMjControlSendOpaction,         //麻将发送，请求发送opaction   
        EMjControlSendFangmao,          //麻将发送，请求放毛
        EMjControlSendChooseBtn,        //麻将发送，下跑下炮发送点击结果
        EMjControlSendChooseQue,        //麻将发送，发送定缺结果
        EMjControlSendCloseBalance,     //麻将发送，请求下一局
        EMjControlSendGetBalanceNew,    //麻将发送，请求获取战绩
        EMjControlSendKanNaoMo,         //麻将发送，坎牌闹庄末留
        EMjControlSendXuanPiao,         //麻将发送，选飘结果

        EMjControlSetTipPos,            //麻将状态，牌落位后设置tip位置
        EMjControlSetTipTime,           //麻将状态，设置时间
        EMjControlSetRestCount,         //麻将状态，当前剩余牌数变化
        EMjControlSetTingRest,          //麻将状态，听口状态变化
        EMjControlLoseCard,             //麻将状态，某张牌减少n张
        EMjControlStateChangeThree,     //麻将状态，换三张按钮状态
        EMjControlStataChooseOne,       //麻将状态，亮一张按钮状态
        EMjControlStateFangMao,         //麻将状态，放毛按钮状态
        EMjControlCloseAsk,             //麻将状态，通知解散桌子
        EMjControlCloseState,           //麻将状态，解散请求的状态变更
        EMjControlCloseMaskUI,          //麻将状态，关闭遮挡牌区的UI
        EMjControlOnlineState,          //麻将状态，在线离线通知
        EMjControlSetSlefSeatID,        //麻将状态，设置自己的座位号
        EMjControlCloseStateEnd,        //麻将状态，解散面板关闭
        EMjControlCloseStateUpdate,     //麻将状态，解散面板更新
        EMjControlDismissTipsUpdate,    //麻将状态，解散请求面板更新（新UI）
        EMjControlDismissTipsClose,     //麻将状态，解散请求面板关闭（新UI）
        EMjControlClearPutMessage,      //麻将状态，清理发送出牌状态

        EMjControlAnimGameStart,        //临时状态，麻将开始
        EMjControlAnimPeng,             //临时状态，麻将碰牌
        EMjControlAnimTing,             //临时状态，麻将听牌
        EMjControlAnimGang,             //临时状态，麻将杠牌 
        EMjControlAnimMingLou,          //麻将明楼
        EMjControlAnimHu,               //麻将胡牌
        EMjControlAnimMingLouTing,      //麻将明楼，听口同步
        EMjControlAnimChi,              //临时状态，麻将吃牌
        EMjControlAnimFangmao,          //淋湿状态，放毛
        EMjControlAnimPass,             //临时状态，过（抢杠）
        EMjControlAnimPutTanban,        //临时状态，出牌时展示弹板
        EMjControlAnimTurn,             //临时状态，麻将摸牌
        EMjControlAnimShowControl,      //临时状态，展示弹板
        EMjControlTurnPlayer,
        EMjControlAnimPutCard,          //麻将出牌

        EMjFxHu,                        //麻将特效，胡牌
        EMjFxGuafeng,                   //麻将特效,刮风
        EMjFxXiayu,                     //麻将下雨
        EMJFxHua,                       //麻将补牌
        EMjFxPengGang,                  //麻将碰杠烟雾
        EMjFxShadow,                    //麻将阴影
        EMjControlReconnedFresh,        //麻将状态，重连之后刷新UI数据
        EMjMusicSound,                  //麻将状态，播放音效

        #endregion

        /// <summary>
        /// GPS数据刷新回复 1.bool刷新是否成功
        /// </summary>
        SysGPS_DataUpdate_Rsp,

        /// <summary>
        /// 系统登录请求 参数
        /// </summary>
        SysLoginRequest,

        /// <summary>
        /// 微信回来接收到数据
        /// </summary>
        WeiXinDataResponse,

        //UI所在阶段通知
        SysGame_UI_Sign_Update,
        //当前麻将馆变化
        SysGame_RoomId_Update,
        //当前是否在麻将游戏中
        SysGame_MahjongGameIn_Update,

        /// <summary>
        /// android 返回按键点击
        /// </summary>
        SysAndroid_GoBack_Button_Click,
        //系统数据====================================
        /// <summary>
        /// 重连数据刷新
        /// </summary>
        SysData_ReconnectData_Rsp,

        /// <summary>
        /// 用户数据刷新
        /// </summary>
        SysData_PlayerData_Update,
        //桌卡数据更新
        SysData_User_RoomCard_Update,
        //用户麻将馆信息更新  UserId
        SysData_User_MjHallInfoUpdate,
        //麻将馆介绍信息通知--------------------
        SysData_MjHall_DescUpdate,
        //麻将馆信息 查询结果
        SysData_MjHall_SearchResultRsp,
        //麻将馆信息 信息查询结束
        SysData_MjJall_SearchOver,
        //推荐麻将馆列表
        SysData_MjHall_RecommendResultRsp,
        //关联麻将馆信息变更
        SysData_MjHall_BindUpdate,
        //进入麻将馆
        SysData_MjHall_JoinMjHall,
        //进入麻将馆
        SysData_MjHall_ExitMjHall,
        //查看麻将馆信息
        SysData_MjHall_CheckMjHall,
        //麻将馆信息变更
        SysData_MjHall_MjHallUpdate,
        //查看战绩刷新
        SysData_MjHall_MjDeskViewRecord,
        //麻将馆牌匾切换
        SysData_MjHall_BoardUpdata,
        //广播数据
        SysData_Broadcast_Update,
        //地区
        SysData_AreaList_Update,
        //自己的地区修改
        SysData_Region_Update,
        //玩法数据更新
        SysData_MJPlayData_Update,
        //馆长申请完成通知客户端
        SysData_RoomApplyFinishNotify,
        //好友=============================================
        //好友列表数据刷新
        SysData_FriendListData_Updata,
        /// <summary>
        /// 新好友添加 参数1 UserId
        /// </summary>
        SysData_FriendListData_NewAdd,
        //增加好友的反馈
        //SysData_Friend_AddRsp,
        //邀请好友进桌的反馈
        SysData_Friend_InviteFriendGame,
        //被邀请进桌的接收
        SysData_Friend_BeInviteGame,
        SysStagingData_Friend_BeInviteGame,
        //申请进入好友桌子的接收
        SysData_Friend_JoinFriendDeskRsp,
        //好友申请进入我桌子的消息
        SysData_Friend_BeJoinFriendDeskNotify,
        //请求好友结果接受 到底是请求同意了还是拒绝了
        SysData_Friend_RefuseApplyRsp,
        //好友状态更新
        SysData_FriendStatus_Updata,

        //打开好友列表
        SysUI_FriendList_OpenClose_Req,
        //打开好友列表回复
        SysUI_FriendList_OpenClose_Rsp,

        //关闭聊天
        SysUI_Chat_CloseUI,



        //安全中心========================================
        //实名认证
        SysData_Certification_Rsp,
        //手机号绑定回复
        SysData_PhoneNoBind_Rsp,
        //其他数据=========================================
        SysData_Other_Msg_Notify,
        //奖品数据接收
        SysData_Other_AwardConfig_Rsp,
        //活动数据接收
        SysData_Other_Activity_Rsp,
        //摇奖获得的道具接收 //todo 未做 弹两秒提示
        SysData_Other_Lottery_Rsp, //带着 data[0]:AwardID
        //接收抽奖的结果
        SysData_Other_PickupAward_Rsp,
        //接收到麻将桌内的聊天消息
        EMjChatNotify,
        ////发送礼包的结果
        //SysData_Other_PushGift_Rsp,
        ////发送礼包通知
        //SysData_Other_PushGift_Notify,
        ////回应收到礼包的结果
        //SysData_Other_ReceiveGift_Rsp,
        ////回应收到礼包的通知
        //SysData_Other_ReceiveGift_Notify,
        //发送红包结果
        SysData_Other_PushMoney_Rsp,
        //馆主查询麻将馆的桌卡销售数据
        SysData_Other_QuerySaleData_Rsp,

        //小loading 消息
        /// <summary>
        /// 小loading 打开 带string
        /// </summary>
        SysUI_SmallLoading_Open,

        /// <summary>
        /// 小loading 关闭 带string
        /// </summary>
        SysUI_SmallLoading_Close,

        /// <summary>
        /// 小loading 强制关闭
        /// </summary>
        SysUI_SmallLoading_Kill,
        /// <summary>
        /// 小Loading 内容设置 参数1 string
        /// </summary>
        SysUI_SmallLoading_SetContent,


        //普通loading 消息
        SysUI_Loading_Change,   //改变Loading的值 参数0 (float)秒数 参数1 (float)0-100百分比

        //UI打开 消息
        SysUI_StateUpdate_Open,
        //UI关闭 消息
        SysUI_StateUpdate_CloseOrHide,

        //场景打开 消息
        SysScene_Open,
        //场景关闭 消息
        SysScene_Close,

        //弹出所有系统消息
        SysUI_MsgPopAll,
        //=====================关键的UI打开
        //打开公告面板
        SysUI_OpenBanner,

        //打开公告面板请求
        SysUI_OpenBannerReq,
        //打开登录
        SysUI_OpenLogin,
        //打开PrepareGame
        SysUI_OpenPrepareGame,
        //打开Main
        SysUI_OpenMain,

        OpenUI_UICreateRoom,
        //=====================关键的UI打开 End
        //系统流程回调  string key object[] data
        SysFlow_Flow_CallBack,
        //登录成功后的请求=============================================
        LoginSouccessRsp,
        //登录后是否加载场景
        LoadMahjongScene,
        //请求桌卡列表，服务器返回消息
        SysUI_RoomCardList_Succ,
        //购买桌卡成功，服务器返回消息
        SysUI_RoomCardBuy_Succ,
        //走完h5界面之后完成支付消息通知
        SysUI_PayFinish_Succ,
        //走完h5界面微信支付流程后开始走客户端微信支付流程
        SysUI_StarWXPay,
        //语音失败
        SysUI_Record_Err,
        //初始化语音SDK
        SysUI_Record_InitBegin,
        //录音中
        SysUI_Record_ing,
        //录音时间过长或过短
        SysUI_Record_LongORShort,
        //战绩数据删除成功，返回消息刷新界面
        SysUI_MessageBattleDelete_Succ,
        //填写邀请我的人奖励返回
        SysUI_FriendInvite_CodeAward_Get,
        //领取好友邀请奖励
        SysUI_FriendInvite_PickUpAward_Get,
        //长按发送消息
        SysUI_FriendAward_OnPress,
        //消息数据收到刷新面板
        SysUI_Message_Succ,
        //战绩数据收到刷新面板
        SysUI_MessageBattle_Succ,
        //分享链接返回呼出关联麻将馆界面
        SysUI_ParlorWindow_Rsp,
        //ScrollView 滑动到最底层的时候的发送事件出去
        SysUI_ScrollView_DragFinish,

        //获取关联客户信息结束
        SysUI_RoomPlayers_Over,

        /// <summary>
        /// 准备界面 刷新准备UI
        /// </summary>
        SysUI_PrepareGame_RefreshPrepare,

        /// <summary>
        /// 清空准备UI
        /// </summary>
        SysUI_PrepareGame_ClearPrepare,

        /// <summary>
        /// WebView关闭消息
        /// </summary>
        SysWebView_Close,
        //========================================
        /// <summary>
        /// 作弊信息更新
        /// </summary>
        SysData_CheatInfo_Open,
        SysData_CheatInfo_Update,
        SysData_CheatInfo_Hide,

        /// <summary>
        /// 播放登录结束动画
        /// </summary>
        SysUI_PlayUILoginEndTween,
        /// <summary>
        /// 播放主页开启动画
        /// </summary>
        SysUI_PlayUIMainStartTween,
        /// <summary>
        /// 主界面打开地图界面选择区域
        /// </summary>
        SysUI_UIMap_Open,

        /// <summary>
        /// 断网消息
        /// </summary>
        NetWork_CloseConnection,
        /// <summary>
        /// 关闭发送消息Loading条
        /// </summary>
        LoadingSendClose_Event,
        /// <summary>
        /// 请求个人回放数据
        /// </summary>
        SysPlayerReplayData,
        /// <summary>
        /// 请求个人单局数回放数据
        /// </summary>
        SysRoundReplayData,
        //回放录像播放结束
        SysUI_UIReplayCtrl_Play_Over,

        //回放录像重播初始化界面
        SysUI_UIReplayCtrl_Play_Init,

        //回放速度变化
        SysUI_UIReplayCtrl_Play_Speed,

        //回放返回之后打开战绩
        SysUI_UIReplayCtrl_Play_ViewRecord,

        /// <summary>
        /// 测试码数据变更
        /// </summary>
        SysUI_CheatKeyDataUpdata,
        SysUI_CheatKeySelectDataUpdata,

        /// <summary>
        /// 功能按钮屏蔽
        /// </summary>
        SysTools_FunctionShow,
        /// <summary>
        /// 功能按钮上的红点刷新
        /// </summary>
        SysTools_FunctionPromptRefresh,
    }

    public enum NameAnim
    {
        #region Mahjong
        EMjGameStart,
        EMjCreateCode,
        EMjDeal,
        EMjPut,
        EMjFlip,
        EMjTurnGetCode,
        EMjPeng,
        EMjGang,
        EMjTing,
        EMjChi,
        EMjHu,
        EMjControlAnimHu,
        EMjFangMao,
        EMjPass,
        EMjChangeThreeNotify,
        EMjChangeThreeUpdate,
        EMjChangeThreeLose,
        EMjChangeThreeGet,
        EMjDingQueNotify,
        EMjDingQueUpdate,
        EMjDingQueResult,
        EMjXiaPaoStart,
        EMjXiaPao,
        EMjXiaPaoUpdate,
        EMjXiaPaoZiStart,
        EMjXiaPaoZiUpdate,
        EMjXiaPaoZi,
        EMjBuyHorse,
        EMjBuyHorseResult,
        EMjDealerBegin,
        EMjFollowDealer,
        EMjChangeFlowerLose,
        EMjChangeFlowrGet,
        EMjMinglou,
        EMjScoreChangeNotify,

        EMjRemovePutCard,           //删除某人面前某张牌 

        EMjBalancePutdown,          //推牌
        EMjBalanceHuaJiao,          //查花猪查大叫
        EMjBalanceTuishui,          //退税
        EMjBalanceNotify,
        EMjBalanceNewNotify,        //小结算新
        EMjGameOverNotify,

        EMjAnimJiange,              //作为动画间隔预留
        EMjAnimXiayu,               //下鱼相关
        EMjAnimXiayuUpdate,

        EMjAnimOpenMaStepShow,         //扎马，抓鸟，第一段动画
        EMjAnimOpenMaStepFly,         //扎马，抓鸟，第二段动画
        EMjAnimPerformance,             //表演

        EMjAnimKanNaoMoNotify,          //开始坎 闹 末留
        EMjAnimKanNaoMoUpdate,          //状态更新 
        EMjAnimKanNaoMoResult,          //结果

        EMjAnimXuanPiao,                //选飘的通知

        EMjAnimSiDunSiDing,             //死蹲死顶的通知
        EMjAnimDuAnGang            //赌暗杠通知

        #endregion
    }

    //预定义的模块数据名称
    public enum NamedGameData
    {
        ENone = 0,
        /// <summary>
        /// 桌卡数据请求
        /// </summary>
        EC2SRoomCardList = 1,
        /// <summary>
        /// 消息数据请求
        /// </summary>
        EC2SMessageList = 2,
        /// <summary>
        /// 战绩数据请求
        /// </summary>
        EC2SMessageBattleList = 3,
        /// <summary>
        /// 游戏记录数据请求
        /// </summary>
        EC2SMessageGameHistory = 4,
        /// <summary>
        /// 麻将馆介绍
        /// </summary>
        EC2SMessageRoomDesc = 5,
        /// <summary>
        /// 麻将馆搜索
        /// </summary>
        EC2SMessageRoomSearch = 6,
        /// <summary>
        /// 用户麻将馆消息
        /// </summary>
        EC2SMessageRoomPlayerInfo = 7,
        /// <summary>
        /// 请求抽奖配置信息
        /// </summary>
        EC2SMessageAwardConfig = 8,
        /// <summary>
        /// 好友列表
        /// </summary>
        EC2SFriendList = 9,
        /// <summary>
        /// 官方麻将试玩群
        /// </summary>
        EC2SOfficialGroupList = 10,
        /// <summary>
        /// 麻将游戏请求
        /// </summary>
        EC2SMahjongOpAction = 11,
    }

    #region ---音效枚举---------------------------

    public enum SoundEnum
    {
        btn_null = -1,
        btn_select1 = 1,
        btn_select2 = 2,
        btn_select3 = 3,
        desk_fangka = 4,
        desk_kaiju = 5,
        desk_shaizi = 6,
        desk_chapai = 7,
        desk_dapai = 8,
        desk_gang = 9,
        desk_qiepai = 10,
        desk_hupai = 11,
        desk_time = 12,
        mall_choujiang = 13,
        desk_tuipai = 14,
        desk_gskh = 15,
        desk_ypdx = 16,
        desk_qgh = 17,
        desk_hdly = 18,
        desk_teshu = 19,
        desk_biaoqing_fanqie = 20,
        desk_biaoqing_hecha = 21,
        desk_biaoqing_qiaozhuo = 22,
        desk_biaoqing_dianzan = 23,
        desk_guafeng = 24,
        desk_xiayu = 25,
        desk_hushandian = 26,
        desk_hunqian = 27,
        desk_hunhou = 28,
        desk_kaima = 29,
        desk_dingque = 30,
        btn_select4 = 31,
        btn_tipai = 32,
    }

    #endregion------------------------------------
    
    public enum InGameModelEvents
    {
        IGM_JoinTable,
        IGM_SetPaiKou,
        ClearUI
    }

    public enum Table3DEvents
    {
        Table3D_PrepareData,
        Table3D_ShowInfo
    }

    public enum WindowUIEvents
    {
        WUI_CloseTop,
        WUI_CloseAll,
        WUI_ErrorWindow,
        WUI_TimeWindow
    }

    public enum MahjongUI3DEvents
    {
        ClickMahjong
    }

    public enum MJUIManagerEvents
    {
        MJUIM_PengGangTingChi
    }




    /// <summary>
    /// 包次提示
    /// </summary>
    public enum EnumMjOprateBaoci
    {
        BC_LogicShow,          //Data向逻辑层发送
        BC_AnimShow,           //Logic向UI层发送
    }

    public enum EnumWifiAndBattery
    {
        Data_NetType,                    //网络类型数据
        Data_Battery,                    //电量数据
        Data_Wifi,                       //wifi数据
        Refresh_NetType,                 //网络类型刷新显示
        Refresh_Battery,                 //电量刷新显示
        Refresh_Wifi,                    //wifi刷新显示
    }

    #region ---功能按钮枚举-----------------------

    public enum FunctionIconEnum
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,
        /// <summary>
        /// 登录界面试玩群
        /// </summary>
        Function_1001 = 1001,
        /// <summary>
        /// 登录界面右侧邀好友有礼
        /// </summary>
        Function_1002 = 1002,
        /// <summary>
        /// 登录界面右侧关联棋牌室
        /// </summary>
        Function_1003 = 1003,
        /// <summary>
        /// 登录界面右侧代理申请
        /// </summary>
        Function_1004 = 1004,
        /// <summary>
        /// 登录界面右侧我要当馆长
        /// </summary>
        Function_1005 = 1005,
        /// <summary>
        /// 大厅界面用户信息
        /// </summary>
        Function_1006 = 1006,
        /// <summary>
        /// 大厅界面用户信息加号按钮
        /// </summary>
        Function_1007 = 1007,
        /// <summary>
        /// 大厅界面试玩群
        /// </summary>
        Function_1008 = 1008,
        /// <summary>
        /// 大厅界面右侧邀好友有礼
        /// </summary>
        Function_1009 = 1009,
        /// <summary>
        /// 大厅界面右侧关联棋牌室
        /// </summary>
        Function_1010 = 1010,
        /// <summary>
        /// 大厅界面右侧代理申请
        /// </summary>
        Function_1011 = 1011,
        /// <summary>
        /// 大厅界面右侧我要当馆长
        /// </summary>
        Function_1012 = 1012,
        /// <summary>
        /// 大厅界面下方购卡按钮
        /// </summary>
        Function_1013 = 1013,
        /// <summary>
        /// 登录界面右侧客服
        /// </summary>
        Function_1014 = 1014,
        /// <summary>
        /// 登录界面右侧设置
        /// </summary>
        Function_1015 = 1015,
        /// <summary>
        /// 登录界面下方功能区
        /// </summary>
        Function_1016 = 1016,
        /// <summary>
        /// 大厅界面右侧客服
        /// </summary>
        Function_1017 = 1017,
        /// <summary>
        /// 大厅界面右侧设置
        /// </summary>
        Function_1018 = 1018,
        /// <summary>
        /// 大厅界面下方功能区
        /// </summary>
        Function_1019 = 1019,
        /// <summary>
        /// 大厅界面地区选择按钮
        /// </summary>
        Function_1020 = 1020,
        /// <summary>
        /// 好友界面邀请微信好友按钮
        /// </summary>
        Function_1021 = 1021,
        /// <summary>
        /// 牌桌界面邀请微信好友按钮
        /// </summary>
        Function_1022 = 1022,
        /// <summary>
        /// 战绩界面分享战绩按钮
        /// </summary>
        Function_1023 = 1023,
        /// <summary>
        /// 开桌准备界面重设规则按钮
        /// </summary>
        Function_1024 = 1024,
        /// <summary>
        /// 牌桌界面右上角规则按钮
        /// </summary>
        Function_1025 = 1025,
        /// <summary>
        /// 游客登录按钮是否显示
        /// </summary>
        Function_2000 = 2000,
        /// <summary>
        /// 转盘活动按钮是否显示
        /// </summary>
        Function_2001 = 2001,
    }

    #endregion------------------------------------
}