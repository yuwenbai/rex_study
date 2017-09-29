/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace projectQ
{
    public partial class UIDefaultModel : UIModelBase
    {
        private UIDefault UI
        {
            get { return _ui as UIDefault; }
        }

        public void UISignUpdate()
        {
            switch (MemoryData.GameStateData.CurrUISign)
            {
                case SysGameStateData.EnumUISign.MainIn:
                    MainIn();
                    break;
                case SysGameStateData.EnumUISign.LoginSucceed:
                    LoginSucceed();
                    break;
            }
            if(MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.PrepareGame)
            {
                //当我处在准备界面和牌桌中时 清除微信进桌的数据
                if (MemoryData.InitData.GetKey() == WXOpenParaEnum.SHARE_INVITE_PLAY)
                {
                    MemoryData.InitData.Clear();
                }
            }
        }
        private void MainIn()
        {
            //好友邀请
            var messageList = MessageStagingManager.Instance.PopAll("Friend_BeInviteGame");
            if (messageList != null && messageList.Count > 0)
            {
                for (int i=0; i< messageList.Count; ++i)
                {
                    var message = messageList[i];
                    if(message != null && message.data != null)
                    {
                        object[] arr = (object[])message.data;
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Friend_BeInviteGame, arr);
                    }
                }
            }
        }

        /// <summary>
        /// 延迟发送
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IEnumerator DelayedSend(string key)
        {
            yield return new WaitForSeconds(1.0f);
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, key);
        }

        /// <summary>
        /// 登陆成功
        /// </summary>
        private void LoginSucceed()
        {
            //发送GPS信息
            ModelNetWorker.Instance.UserGPSInfoReq(GPSManager.Instance.GetNewGpsData());

            _R.flow.SetQueue(QFlowManager.FlowType.LoginEnd, MemoryData.GameStateData.CurrGameNetWorkStatus == SysGameStateData.EnumGameNetWorkStatus.ReconnectLink);
            MemoryData.GameStateData.CurrReconnectState = SysGameStateData.EnumNetReconnectState.None;
            MemoryData.GameStateData.CurrGameNetWorkStatus = SysGameStateData.EnumGameNetWorkStatus.LinkOk;
        }

        #region 登录成功后的请求
        List<string> LoginSouccessReqList;
        /// <summary>
        /// 登陆成功后的请求
        /// </summary>
        public void InitRequestSend(bool isSendAskDeskReq = false)
        {
            LoginSouccessReqList = new List<string>();
            //牌匾数据
            MemoryData.OtherData.InitBoard();
            //请求配置信息列表
            MemoryData.MahjongPlayData.XMLToData();

            if(isSendAskDeskReq)
            {
                DebugPro.Log("[00FF0F]流程 InitRequestSend 调用重连请求AskDeskDataReq[-]");
                ModelNetWorker.Instance.AskDeskDataReq();
            }

            LoginSouccessReqList.Add("MjDeskReconect");
            ////已经改为服务器主动推送所以不请求进桌
            //if(!MemoryData.GameStateData.IsJoinMahjongSceneNew)
            //{
            //    LoginSouccessReqList.Add("MjDeskReconect");
            //    //请求中途进桌数据
            //    ModelNetWorker.Instance.MjDeskReconectReq();
            //}
        }
        //接收
        public void LoginSouccessRsp(string key)
        {
            if (_R.flow.CurrExecute != null && _R.flow.CurrExecute.Key == QFlowManager.EnumFlowKey.InitRequestSend)
            {
                LoginSouccessReqList.Remove(key);

                if (LoginSouccessReqList.Count == 0)
                {
                    //StartCoroutine(DelayedSend("InitRequestSend"));
                }
            }
        }

        #endregion

        #region 消息弹框接收
        private Queue<string> MsgQueue = new Queue<string>();
        private void AddMsg(string msg)
        {
            if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.MainIn
                && MemoryData.GameStateData.CurrUISign < SysGameStateData.EnumUISign.PrepareGame)
            {
                OpenPop(msg, null);
            }
            else
            {
                MsgQueue.Enqueue(msg);
            }
        }
        private void OpenPop(string content, System.Action<int> func)
        {
            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", content, new string[] { "确定" }, func);
        }

        public void SystemMsgPopAll(Action<int> popEndCallBack)
        {
            if (MsgQueue.Count == 0)
            {
                popEndCallBack(0);
            }
            else
            {
                while (MsgQueue.Count > 0)
                {
                    if (MsgQueue.Count == 1)
                    {
                        OpenPop(MsgQueue.Dequeue(), popEndCallBack);
                    }
                    else
                    {
                        OpenPop(MsgQueue.Dequeue(), null);
                    }
                }
            }

        }

        #endregion

        #region 网络连接断开


        /// <summary>
        /// 网络重连
        /// </summary>
        private void NetReconnect(bool isFoceReconn = false)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
            MemoryData.GameStateData.CurrGameNetWorkStatus = SysGameStateData.EnumGameNetWorkStatus.NotLink;
            UserActionManager.AddLocalTypeLog("Log1", "网络重连 NetReconnect 进入方法");
            ////注销ping方法
            //MemoryData.OtherData.UnRegistPing();

            if (MemoryData.GameStateData.CurrReconnectState != SysGameStateData.EnumNetReconnectState.Kick
                && MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.LoginSucceed
                && !MemoryData.GameStateData.IsPause //游戏在前台的时候才会有重连这个东西
                )
            {
                UserActionManager.AddLocalTypeLog("Log1", "网络重连 NetReconnect 重连了");
                LoginEnterNetWork.Instance.Reconnect(isFoceReconn);
            }
            else
            {
                string error = "原因:";
                if(MemoryData.GameStateData.CurrReconnectState == SysGameStateData.EnumNetReconnectState.Kick)
                {
                    error += " 踢下线";
                }
                if (MemoryData.GameStateData.CurrUISign < SysGameStateData.EnumUISign.LoginSucceed)
                {
                    error += " 不在登录后";
                }
                if(MemoryData.GameStateData.IsPause)
                {
                    error += " 游戏在后台";
                }
                UserActionManager.AddLocalTypeLog("Log1", "网络重连 NetReconnect 没重连"+error);
            }
        }
        //被踢下线
        private void NetKick()
        {
            NetWorkMessageManager.ClearMatchedRecord();
            MemoryData.GameStateData.CurrReconnectState = SysGameStateData.EnumNetReconnectState.Kick;
            MemoryData.GameStateData.CurrGameNetWorkStatus = SysGameStateData.EnumGameNetWorkStatus.NotLink;
            WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "您的账号已经在其他设备登录", new string[] { "确认" }, delegate (int index)
            {
                UI.OpenLogin(true);
            });
        }

        /// <summary>
        /// 发送数据失败
        /// </summary>
        private void NetSendError()
        {
            UserActionManager.AddLocalTypeLog("Log1", "[00FF00]网络重连 NetSendError[-]");
            NetReconnect();
        }

        /// <summary>
        /// 未连接到服务器
        /// </summary>
        private void NetFail()
        {
            UserActionManager.AddLocalTypeLog("Log1", "[00FF00]网络重连 NetFail[-]");

            NetReconnect();
        }

        /// <summary>
        /// 服务器断开连接
        /// </summary>
        private void NetBroken()
        {
            UserActionManager.AddLocalTypeLog("Log1", "[00FF00]网络重连 NetBroken[-]");

            NetReconnect();
        }

        /// <summary>
        /// 连接超时
        /// </summary>
        private void NetTimeOut()
        {
            UserActionManager.AddLocalTypeLog("Log1", "[00FF00]网络重连 NetTimeOut[-]");
            NetReconnect();
        }

        private void NetClientReconnect()
        {
            UserActionManager.AddLocalTypeLog("Log1", "[00FF00]网络重连 NetClientReconnect[-]");
            NetReconnect(true);
        }

        /// <summary>
        /// 连接成功
        /// </summary>
        private void NetConnected()
        {
        }
        #endregion

        #region 打开创建牌桌界面
        /// <summary>
        /// 打开选择玩法界面
        /// EventDispatcher.FireSysEvent(GEnum.NamedEvent.OpenUI_UICreateRoom);
        /// </summary>
        public void OpenCreateRoom()
        {
            int onlyPlayId = MemoryData.MahjongPlayData.GetMahjongPlayOnlyConfigId();
            if (onlyPlayId == 0)
            {
                UI.LoadUIMain("UICreateRoom");
            }
            else
            {
                UI.LoadUIMain("UICreateRoomRule", onlyPlayId);
            }
        }
        #endregion

        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                GEnum.NamedEvent.EMahjongJoinDesk,
                GEnum.NamedEvent.EEnterGameResult,
                GEnum.NamedEvent.SysGame_UI_Sign_Update,
                GEnum.NamedEvent.SysData_Other_Msg_Notify,
                GEnum.NamedEvent.SysUI_StateUpdate_Open,
                GEnum.NamedEvent.SysUI_StateUpdate_CloseOrHide,
                GEnum.NamedEvent.EMahjongNewDesk,

                //登录请求的接收
                GEnum.NamedEvent.LoginSouccessRsp,
                GEnum.NamedEvent.LoadMahjongScene,

                //网络连接断开
                GEnum.NamedEvent.ENet_Connected,  //服务器断开连接
                GEnum.NamedEvent.ENetError_Broken,  //服务器断开连接
                GEnum.NamedEvent.ENetError_Fail,    //未连接到服务器
                GEnum.NamedEvent.ENetError_TimeOut, //超时
                GEnum.NamedEvent.ENetError_Kick,    //被踢下线
				GEnum.NamedEvent.ENetError , // 发送网络数据失败，网络链接异常
                GEnum.NamedEvent.ENetError_Client, //客户端主动断开
                GEnum.NamedEvent.SysUI_OpenPrepareGame,
                GEnum.NamedEvent.SysUI_OpenMain,
                GEnum.NamedEvent.SysUI_OpenLogin,


                GEnum.NamedEvent.SysUI_MsgPopAll,//弹出系统消息


                GEnum.NamedEvent.SysScene_Open,     //场景打开
                GEnum.NamedEvent.SysScene_Close,    //场景关闭


                GEnum.NamedEvent.SysLoginRequest,

                GEnum.NamedEvent.SysUI_StarWXPay, //微信支付

				GEnum.NamedEvent.EGetGameData,    //获取网络模块数据

			
				GEnum.NamedEvent.SysData_RoomApplyFinishNotify,    //馆长申请完成通知客户端

                GEnum.NamedEvent.WeiXinDataResponse,//微信回来接收到数据

                //GEnum.NamedEvent.SysData_ReconnectData_Rsp,//重连数据

                GEnum.NamedEvent.SysUI_ParlorWindow_Rsp,//分享链接返回呼出关联麻将馆界面


                GEnum.NamedEvent.SysGPS_DataUpdate_Rsp,//Gps数据回复

                GEnum.NamedEvent.NetWork_CloseConnection,


                GEnum.NamedEvent.SysAndroid_GoBack_Button_Click,

                GEnum.NamedEvent.ELoginResult,
                GEnum.NamedEvent.SysStagingData_Friend_BeInviteGame,

                GEnum.NamedEvent.SysData_Friend_RefuseApplyRsp,
                GEnum.NamedEvent.OpenUI_UICreateRoom,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.EGetGameData:
                    {
                        object[] v = null;
                        if (data.Length > 1)
                        {
                            v = new object[data.Length - 1];
                            Array.Copy(data, 1, v, 0, data.Length - 1);
                        }
                        this.EGetGameData((GEnum.NamedGameData)data[0], v);
                    }
                    break;
                case GEnum.NamedEvent.SysUI_StarWXPay:
                    {
                        string json = data[0] as string;
                        //唤醒微信支付，并关闭H5
                        //SDKManager.Instance.HiddenWebView ();
                        SDKManager.Instance.SDKFunction(6, json);
                    }
                    break;

                case GEnum.NamedEvent.EMahjongJoinDesk:
                    {
                        int resultCode = (int)data[0];
                        if (resultCode == 0)
                        {
                            ModelNetWorker.Instance.ReportGPSInfoReq(MjDataManager.Instance.MjData.curUserData.selfDeskID, GPSManager.Instance.GetNewGpsData());
                            UI.JoinDesk();
                            if(data.Length >= 2)
                            {
                                MjDeskInfo deskInfo = (MjDeskInfo)data[1];
                                MemoryData.MahjongPlayData.SavePlaySet(deskInfo.mjGameConfigId, deskInfo.viewScore, null, deskInfo.mjRulerSelected);
                                MemoryData.MahjongPlayData.AddPlayTime(deskInfo.mjGameConfigId);
                            }
                        }
                        else
                        {
                            if(MemoryData.GameStateData.IsExternalLinkJoinDesk || resultCode == (int)Msg.ErrorCode.ErrCode_FRoom_Ticket_NotEnough)
                            {
                                UI.LoadError(resultCode,null);
                            }
                            else
                            {
                                UI.LoadTip(resultCode);
                            }
                        }
                        MemoryData.GameStateData.IsExternalLinkJoinDesk = false;
                    }
                    break;
                case GEnum.NamedEvent.EEnterGameResult:
                    {
                        int resultCode = (int)data[0];
                        if (resultCode == 0)
                        {
                            MemoryData.NamedGameDataCnf.InitNamedGameDataCnf();
                            ////注册ping方法
                            //MemoryData.OtherData.RegistPing();
                            MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.LoginSucceed);
                        }
                        else
                        {
                            QLoger.LOG("EEnterGameResult 进入游戏失败 关闭小Loading 并断网 ResultCode=" + resultCode);
                            NetWorkerImpl.Instance.CloseConnectionWithoutMessage();
                            DebugPro.LogError("进入游戏错误" + resultCode);

                            if (_R.flow.CurrFlow != QFlowManager.FlowType.InitLoginNew)
                            {
                                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
                                UI.LoadError(resultCode, (index) =>
                                {
                                    UI.OpenLogin(true);
                                });
                            }
                        }
                    }
                    break;
                case GEnum.NamedEvent.ELoginResult:
                    {
                        int resultCode = (int)data[0];
                        if (resultCode == 0)
                        {
                            //登录成功后计时器开始
                            MemoryData.Set(MKey.USER_WALLOW_TIME, Time.realtimeSinceStartup);
                            //SaveUserNamePwd();
                        }
                        if (resultCode != 0)
                        {
                            QLoger.LOG("ELoginResult 登录失败关闭小Loading 并断网 ResultCode=" + resultCode);
                            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
                            NetWorkerImpl.Instance.CloseConnectionWithoutMessage();
                            UI.OpenLogin(true);
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysGame_UI_Sign_Update:
                    UISignUpdate();
                    break;
                case GEnum.NamedEvent.SysData_Other_Msg_Notify://收到消息数据暂存
                    AddMsg(data[0] as string);
                    break;
                case GEnum.NamedEvent.SysUI_StateUpdate_Open:
                    {
                        string uiname = data[0] as string;
                        switch (uiname)
                        {
                            case "UIMain":
                                {
                                    if (MemoryData.GameStateData.CurrUISign > SysGameStateData.EnumUISign.MainIn)
                                    {
                                        MemoryData.GameStateData.SetCurrUISign(SysGameStateData.EnumUISign.MainIn);
                                    }
                                }
                                break;
                        }
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "Open_" + data[0] as string);
                    }
                    break;
                case GEnum.NamedEvent.SysUI_StateUpdate_CloseOrHide:
                    {
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "Close_" + data[0] as string);
                    }
                    break;
                case GEnum.NamedEvent.LoginSouccessRsp:
                    {
                        this.LoginSouccessRsp(data[0] as string);
                    }
                    break;
                case GEnum.NamedEvent.LoadMahjongScene:
                    {
                        //MemoryData.GameStateData.IsJoinMahjongSceneNew = true;
                        MemoryData.GameStateData.IsMahjongGameIn = data.Length > 1 ? (bool)data[1] : false;
                        MemoryData.GameStateData.IsLoadMahjongScene = (bool)data[0] ;

                        //UserActionManager.AddLocalTypeLog("Log1", "重连麻将消息=========IsJoinMahjongSceneNew=" + MemoryData.GameStateData.IsJoinMahjongSceneNew );
                        UserActionManager.AddLocalTypeLog("Log1", "重连麻将消息=========IsMahjongGameIn=" + MemoryData.GameStateData.IsMahjongGameIn);
                        UserActionManager.AddLocalTypeLog("Log1", "重连麻将消息=========IsLoadMahjongScene=" + MemoryData.GameStateData.IsLoadMahjongScene);
                        if (_R.flow.CurrExecute != null && _R.flow.CurrExecute.Key == QFlowManager.EnumFlowKey.InitRequestSend)
                        {
                            UserActionManager.AddLocalTypeLog("Log1", "重连麻将消息===============1");
                            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, QFlowManager.EnumFlowKey.InitRequestSend.ToString(), "LoadMahjongScene", data[0], data.Length > 1 ? data[1] : false);
                        }
                        else if (_R.flow.CurrExecute != null && _R.flow.CurrExecute.Key == QFlowManager.EnumFlowKey.Branch_LoginEnd)
                        {
                            UserActionManager.AddLocalTypeLog("Log1", "重连麻将消息===============2");
                            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, QFlowManager.EnumFlowKey.Branch_LoginEnd.ToString(), "LoadMahjongScene", data[0], data.Length > 1 ? data[1] : false);
                        }
                        else
                        {
                            UserActionManager.AddLocalTypeLog("Log1", "重连麻将消息===============3");
                            _R.flow.SetQueueForce(QFlowManager.FlowType.MahjongReconnect);
                        }

                        //获取好友列表 --
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SFriendList);
                    }
                    break;
                //服务器断开连接
                case GEnum.NamedEvent.ENetError_Broken:
                    this.NetBroken();
                    break;
                //未连接到服务器
                case GEnum.NamedEvent.ENetError_Fail:
                    this.NetFail();
                    break;
                //超时
                case GEnum.NamedEvent.ENetError_TimeOut:
                    this.NetTimeOut();
                    break;
                case GEnum.NamedEvent.ENetError_Client:
                    this.NetClientReconnect();
                    break;
                case GEnum.NamedEvent.ENet_Connected:
                    this.NetConnected();
                    break;
                case GEnum.NamedEvent.ENetError_Kick:
                    this.NetKick();
                    break;
                case GEnum.NamedEvent.ENetError:
                    if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.MainIn)
                    {
                        this.NetSendError();
                    }
                    break;
                case GEnum.NamedEvent.EMahjongNewDesk://创建牌桌后的结果
                    {
                        int resultCode = (int)data[0];
                        if (resultCode != 0)
                        {
                            if (resultCode == (int)Msg.ErrorCode.ErrCode_FRoom_Ticket_OwnnerNotEnough)
                            {
                                UI.LoadError(resultCode, null);
                            }
                            else
                            {
                                UI.LoadTip(resultCode);
                            }
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysUI_OpenPrepareGame: //打开游戏准备界面
                    {
                        UI.JoinDesk();
                    }
                    break;
                case GEnum.NamedEvent.SysUI_OpenMain://打开主页面
                    {
                        UI.JoinMain(data == null || data.Length == 0 ? EnumChangeSceneType.GamePrepare_To_Main : (EnumChangeSceneType)data[0]);
                    }
                    break;
                case GEnum.NamedEvent.SysUI_OpenLogin:
                    {
                        bool isForce = false;
                        if(data.Length > 0)
                        {
                            isForce = (bool)data[0];
                        }
                        UI.OpenLogin(isForce);
                    }
                    break;
                case GEnum.NamedEvent.SysUI_MsgPopAll://弹出系统消息
                    {
                        SystemMsgPopAll(data[0] as Action<int>);
                    }
                    break;

                case GEnum.NamedEvent.SysScene_Open:    //场景打开
                    {
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "OpenScene_" + data[0] as string);
                    }
                    break;
                case GEnum.NamedEvent.SysScene_Close:   //场景关闭
                    {
                        EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysFlow_Flow_CallBack, "CloseScene_" + data[0] as string);
                    }
                    break;
                case GEnum.NamedEvent.SysData_RoomApplyFinishNotify: //馆长申请完成通知客户端
                    {
                        QLoger.LOG("接收到馆长申请完成通知");
                        int resultCode = (int)data[0];
                        long userId = (long)data[1];
                        //SDKManager.Instance.HiddenWebView();

                        UI.LoadPop(WindowUIType.SystemPopupWindow, "提示"
                            , resultCode == 0 ? "恭喜您！申请馆长成功" : ("申请馆长失败" + resultCode)
                            , new string[] { "确认" }
                            , (index) =>
                            {
                                if (resultCode == 0)
                                {
                                    _R.flow.SetQueue(QFlowManager.FlowType.ChangeScene, EnumChangeSceneType.Main_To_Login);
                                }
                            }
                            );

                        QLoger.LOG("已经弹出馆长申请完成通知");

                    }
                    break;
                case GEnum.NamedEvent.SysLoginRequest:
                    ModelNetWorker.Instance.LoginReq(MemoryData.LoginDataMgr.GeLoginReqData((SysLoginData.EnumLoginType)data[0]));
                    break;

                case GEnum.NamedEvent.WeiXinDataResponse:
                    {
                        var key = MemoryData.InitData.GetKey();
                        if (key == WXOpenParaEnum.SHARE_INVITE_PLAY 
                            || key == WXOpenParaEnum.SHARE_INVITE_FRIEND_GO_MYMUSEUM
                            || key == WXOpenParaEnum.SHARE_INVITE_FRIEND_TO_GET_REWARD
                            )
                        {
                            //再次判断是否可以执行
                            if (MemoryData.GameStateData.CurrUISign == SysGameStateData.EnumUISign.MainIn)
                            {
                                //_R.flow.SetQueue(QFlowManager.FlowType.InitDataExecute);
                            }
                        }
                    }
                    break;
                //case GEnum.NamedEvent.SysData_ReconnectData_Rsp:
                //    {
                //        //连数据接收
                //        bool isOk = (bool)data[0];
                //        if (isOk)
                //        {

                //        }
                //        else
                //        {

                //        }
                //    }
                //    break;
                case GEnum.NamedEvent.SysUI_ParlorWindow_Rsp:
                    {
                        if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.IsHaveMjRoom())
                            return;

                        //分享链接返回呼出关联麻将馆界面
                        string roomId = (string)data[0];
                        try
                        {
                            int id = int.Parse(roomId);
                            _R.ui.OpenUI("UIParlorInfo", roomId);
                        }
                        catch
                        {
                            QLoger.LOG(string.Format("ID无法转换：{0}", roomId));
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysGPS_DataUpdate_Rsp:
                    {
                        DebugPro.Log(DebugPro.EnumLog.System, "GPS数据更新", "是否有更新", data[0]);

                        //发送gps位置   暂时不实施上报    
                        //TODO 2017/07/19 取消距离远近检测
                        if (/* (bool)data[0] && */ MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.PrepareGame
                            && NetWorkerImpl.Instance.state == NetState.NetConnected
                            )
                        {
                            ModelNetWorker.Instance.ReportGPSInfoReq(MjDataManager.Instance.MjData.curUserData.selfDeskID, GPSManager.Instance.GetNewGpsData());
                        }

                        //保存到自己的数据里
                        if (MemoryData.GameStateData.CurrUISign > SysGameStateData.EnumUISign.LoginSucceed)
                        {
                            var myPlayData = MemoryData.PlayerData.MyPlayerModel;
                            var gpsData = GPSManager.Instance.GetNewGpsData();
                            if (myPlayData != null)
                            {
                                myPlayData.PlayerDataBase.Latitude = gpsData == null ? 0f : gpsData.Latitude;
                                myPlayData.PlayerDataBase.Longitude = gpsData == null ? 0f : gpsData.Longitude;
                            }
                            ModelNetWorker.Instance.UserGPSInfoReq(GPSManager.Instance.GetNewGpsData());
                        }
                    }
                    return;
                case GEnum.NamedEvent.SysData_AreaList_Update:
                    break;
                case GEnum.NamedEvent.NetWork_CloseConnection:
                    UserActionManager.AddLocalTypeLog("Log1", "NetWork_CloseConnection 主动调用关闭网络连接");
                    NetWorkerImpl.Instance.CloseConnection();
                    break;
                case GEnum.NamedEvent.SysAndroid_GoBack_Button_Click:
                    if(MemoryData.GameStateData.IsOpenWebView)
                    {
                        //StartCoroutine(UITools.WaitExcution(() =>
                        //{
                        //    if (AndroidManager.Instance.IsOpenWebView())
                        //    {
                        //        AndroidManager.Instance.BackWebView();
                        //    }
                        //    else
                        //    {
                        //        AndroidManager.Instance.HiddenWebView();
                        //    }
                        //}, 1));
                    }
                    else
                    {
                        _R.ui.OnAndroidGoBackButton();
                    }
                    break;
                case GEnum.NamedEvent.SysStagingData_Friend_BeInviteGame:
                    {
                        if (MemoryData.GameStateData.CurrUISign >= SysGameStateData.EnumUISign.MainIn && MemoryData.GameStateData.CurrUISign < SysGameStateData.EnumUISign.PrepareGame 
                            )
                        {
                            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Friend_BeInviteGame,data);
                        }
                        else
                        {
                            var msd = new MessageStagingData();
                            msd.key = "Friend_BeInviteGame";
                            msd.data = data;
                            MessageStagingManager.Instance.Add(msd);
                        }
                    }
                    break;
                case GEnum.NamedEvent.SysData_Friend_RefuseApplyRsp:
                    {
                        string name = data[0] as string;
                        //true 为拒绝 false 同意
                        bool isRefuse = (bool)data[1];
                        if(!isRefuse)
                        {
                            WindowUIManager.Instance.CreateTip(name + " 通过了您的好友申请", 2f);
                        }
                        //2017-09-25 应加好友反馈方案 去除拒绝提示
                        //else
                        //{
                        //    UI.LoadTip(name+ " 拒绝添加你为好友");
                        //}
                    }
                    break;
                case GEnum.NamedEvent.OpenUI_UICreateRoom:
                    this.OpenCreateRoom();
                    break;
            }
        }
#endregion

#if UNITY_EDITOR
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysAndroid_GoBack_Button_Click);
            }
        }
#endif
    }
    
}
