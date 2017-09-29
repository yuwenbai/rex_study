/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class GatewayServerData
    {
        public string Ip;
        public int Port;

        public GatewayServerData(string ip,int port)
        {
            this.Ip = ip;
            this.Port = port;
        }
    }
    /// <summary>
    /// 1. 请求网关
    /// 2. 接收到网关信息后 通过IP 连接新地址(游戏服务器)
    /// ------已经创建好连接
    /// 1. 注册或登录
    /// 2. 接收到进入游戏消息
    /// 3. 如果地址改变了再次连接  连接后发送 进入游戏请求 EnterGameReq(null);
    /// 
    /// 连接
    /// 1.网关
    /// 2.注册登录验证 服务器
    /// 3.进入游戏 服务器
    /// 
    /// 消息
    /// 关注
    /// 1.登录回复
    /// 2.进入游戏回复
    /// 发送
    /// 1.进入游戏
    /// </summary>
	public class LoginEnterNetWork : SingletonTamplate<LoginEnterNetWork>
    {
        public enum LinkServerType
        {
            /// <summary>
            /// 网关服务器
            /// </summary>
            GatewayServer,

            /// <summary>
            /// 登录服务器
            /// </summary>
            LoginServer,

            /// <summary>
            /// 游戏服务器
            /// </summary>
            EnterGameServer,

            /// <summary>
            /// 隐藏式连接
            /// </summary>
            HideLink,
        }
       
        public List<GatewayServerData> ServerList = null;

        //断线重连最大次数
        public int ReconnectCountMax = 3;
        public string GatewayServerIp = "127.0.0.1";
        public int GatewayServerPort = 8050;

        //private bool isConnect = false;
        //public bool IsConnect
        //{
        //    get { return NetWorkerImpl.Instance.state == NetState.NetConnecting && isConnect; }
        //}


        /// <summary>
        /// 登录成功后的回调函数
        /// </summary>
        private System.Action<bool> LoginSuccessConnCallBack;


        #region API
        /// <summary>
        /// 连接
        /// </summary>
        public void Connect(System.Action<bool> callBack)
        {
            NetWorkMessageManager.ClearMatchedRecord();
            //isConnect = false;
            this.LoginSuccessConnCallBack = callBack;

            /************************************************************************/
            /* 如果在登录前的话 则
             * 1.随机选择网关服务器
             * 2.连接网关服务器
             * 3.连接登录服务器
             * 如果断开
             * 则 回到1
             *                                                                      */
            /************************************************************************/
            if(IsLoginPre())
            {
#if __DEBUG_SERVER_LIST
                //1. 随机得到网关服务器
                var serverData = GetServerList();
                if (serverData == null) {
                    WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "当前网络状态不佳，请重启游戏", new string[] { "确认" }, (index) =>
                    {
                        Application.Quit();
                    });
                    return;
                } 
                GatewayServerIp = serverData.Ip;
                GatewayServerPort = serverData.Port;
#endif
            }
            //2.连接网关服务器
            GatewayConnect();
        }

        /// <summary>
        /// 重连 
        /// 游戏登录之后走的是这个主动请求重连 登录之前都是自动重连
        /// </summary>
        public void Reconnect(bool isFoceReconn = false)
        {
            UserActionManager.AddLocalTypeLog("Log1", "网络重连 Reconnect 重连1");

            NetWorkMessageManager.ClearMatchedRecord();

            if(!isFoceReconn)
            {
                if (NetWorkerImpl.Instance.state == NetState.NetConnecting || NetWorkerImpl.Instance.state == NetState.NetConnected) return;
            }
            if (IsLoginPre() || ((MemoryData.GameStateData.CurrGameNetWorkStatus == SysGameStateData.EnumGameNetWorkStatus.InitLink 
                || MemoryData.GameStateData.CurrGameNetWorkStatus == SysGameStateData.EnumGameNetWorkStatus.ReconnectLink) 
                && MemoryData.GameStateData.IsInitiativeCloseConnect)) return;

            UserActionManager.AddLocalTypeLog("Log1", "网络重连 Reconnect 重连2");

            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Close, "Reconnect");

            //MemoryData.GameStateData.IsConnecting = true;
            MemoryData.GameStateData.CurrGameNetWorkStatus = SysGameStateData.EnumGameNetWorkStatus.ReconnectLink;
            MemoryData.GameStateData.CurrReconnectState = SysGameStateData.EnumNetReconnectState.None;
            //得到重连次数
            int reconnectCount = MemoryData.GameStateData.ReconnectCount;

            //隐藏式连接
            if(reconnectCount < MemoryData.GameStateData.HideReconnectCount)
            {
                MemoryData.GameStateData.CurrReconnectState = SysGameStateData.EnumNetReconnectState.Hide;
                _R.Instance.StartCoroutine(NetReconnectHide());
            }
            else if (reconnectCount >= ReconnectCountMax) //大于重连次数就是登录
            {
                MemoryData.GameStateData.ReconnectCount = 0;
                this.NetWorkDisconnectTip(EnumNetWorkTipAction.OpenLogin);
            }
            else
            {
                this.NetWorkDisconnectTip(IsLoginPre() ? EnumNetWorkTipAction.OpenLogin : EnumNetWorkTipAction.Reconnection);
            }
        }
        private void RealReconnect(bool isHide = false)
        {
            UserActionManager.AddLocalTypeLog("Log1", "网络重连 RealReconnect 真正重连了");
            MemoryData.GameStateData.CurrGameNetWorkStatus = SysGameStateData.EnumGameNetWorkStatus.ReconnectLink;
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Open, "Reconnect");
            DebugPro.Log(DebugPro.EnumLog.NetWork, "重试_RealReconnect");

            //this.isConnect = false;
            this.CurrIp = "";
            this.CurrPort = 0;
            MemoryData.GameStateData.ReconnectCount++;
            //if (isHide)
            //    ReconnectGameConnect();
            //else
            this.Connect(LoginReq);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            MemoryData.GameStateData.CurrGameNetWorkStatus = SysGameStateData.EnumGameNetWorkStatus.NotLink;
            UserActionManager.AddLocalTypeLog("Log1", "LoginEnterNetWork.Disconnect 主动调用关闭网络连接");

            NetWorkMessageManager.ClearMatchedRecord();
            DebugPro.Log(DebugPro.EnumLog.NetWork, "断开连接_Disconnect");

            MemoryData.GameStateData.IsInitiativeCloseConnect = true;
            QLoger.ERROR("断开连接");
            //注销ping方法
            //MemoryData.OtherData.UnRegistPing();
            //this.isConnect = false;
            this.CurrIp = "";
            this.CurrPort = 0;
            NetWorkerImpl.Instance.CloseConnection();
        }
        #endregion
        //隐藏式的重新连接
        private IEnumerator NetReconnectHide()
        {
            DebugPro.Log(DebugPro.EnumLog.NetWork, "隐藏式的重新连接_NetReconnectHide");

            //等待1秒隐藏式重新连接
            yield return new WaitForSeconds(1f);
            RealReconnect(false);
        }

        /// <summary>
        /// 是否是登录之前
        /// </summary>
        /// <returns></returns>
        private bool IsLoginPre()
        {
            return MemoryData.GameStateData.CurrUISign < SysGameStateData.EnumUISign.LoginSucceed;
        }

        /// <summary>
        /// 服务器连接回调
        /// </summary>
        /// <param name="state"></param>
        /// <param name="data"></param>
        private void ConnectServerCallBack(LinkServerType serverType, NetState state)
        {
            bool isConnected = state == NetState.NetConnected;

            DebugPro.Log(DebugPro.EnumLog.NetWork, "连接回调__ConnectServerCallBack","连接是否成功", isConnected);
            if(!isConnected) //连接失败
            {
                MemoryData.GameStateData.CurrGameNetWorkStatus = SysGameStateData.EnumGameNetWorkStatus.NotLink;
                //MemoryData.GameStateData.IsConnecting = false; 
                if (IsLoginPre())
                {
#if __DEBUG_SERVER_LIST
                    //删除网关服务器
                    RemoveServerList(GatewayServerIp, GatewayServerPort);
                    this.Connect(LoginSuccessConnCallBack);
#else
                    this.NetWorkDisconnectTip(EnumNetWorkTipAction.OpenLogin);
                    if(this.LoginSuccessConnCallBack != null)
                    {
                        this.LoginSuccessConnCallBack(false);
                    }
#endif
                }
                else
                {
                    this.Reconnect();
                }
                return;
            }

            switch (serverType)
            {
                case LinkServerType.GatewayServer:
                    //连接完成后切入到开启游戏请求
                    ModelNetWorker.Instance.GatewayReq(null);
                    break;
                case LinkServerType.LoginServer:
                    {
                        //真正的登录成功了 回调这个函数
                        //this.isConnect = true;
                        LoginSuccessConnCallBack(true);
                    }
                    break;
                case LinkServerType.EnterGameServer:
                    //发送进入游戏请求
                    ModelNetWorker.Instance.EnterGameReq(null);
                    //MemoryData.GameStateData.IsConnecting = false;
                    break;
                case LinkServerType.HideLink:
                    break;
            }
        }


        #region 网关服务器连接
        /// <summary>
        /// 网关连接  之后会发送消息请求IP地址
        /// </summary>
        private void GatewayConnect()
        {
            DebugPro.Log(DebugPro.EnumLog.NetWork, "网关连接__GatewayConnect");
            if (MemoryData.GameStateData.CurrGameNetWorkStatus != SysGameStateData.EnumGameNetWorkStatus.ReconnectLink)
                MemoryData.GameStateData.CurrGameNetWorkStatus = SysGameStateData.EnumGameNetWorkStatus.InitLink;

            //MemoryData.GameStateData.IsConnecting = true;
            this.InitEvent();
            
            MemoryData.Set(MKey.AS_IPADDR, GatewayServerIp);
            MemoryData.Set(MKey.AS_PORT, GatewayServerPort);

            this.MakeClienter(GatewayServerIp, GatewayServerPort, LinkServerType.GatewayServer); 
        }

        /// <summary>
        /// 开启游戏消息接收
        /// </summary>
        /// <param name="data"></param>
        private void GatewayCallback(object[] data)
        {
            LoginGameConnect();
        }
        #endregion

        #region 登录服务器连接
        private void LoginGameConnect()
        {
            string ip = MemoryData.Get<string>(MKey.AS_IPADDR);
            int port = MemoryData.Get<int>(MKey.AS_PORT);
            this.MakeClienter(ip, port, LinkServerType.LoginServer);
        }
        private void LoginGameCallback(object[] data)
        {
            int resultCode = (int)data[1];
            if (resultCode == 0)
            {
                MemoryData.GameStateData.ReconnectCount = 0;
                LoginEnterNetWork.Instance.EnterGameConnect();
            }
            else if (resultCode == (int)Msg.ErrorCode.ErrCode_Version_Error)
            {
                ResourceLoadMain.AppDownPrompt();
            }
        }
#endregion

        #region 进入游戏连接
        private void EnterGameConnect()
        {
            string ip = MemoryData.Get<string>(MKey.AS_IPADDR);
            int port = MemoryData.Get<int>(MKey.AS_PORT);

            this.MakeClienter(ip, port, LinkServerType.EnterGameServer);
        }
        #endregion

        #region 重连游戏连接
        private void ReconnectGameConnect()
        {
            string ip = MemoryData.Get<string>(MKey.AS_IPADDR);
            int port = MemoryData.Get<int>(MKey.AS_PORT);

            this.MakeClienter(ip, port, LinkServerType.HideLink);
        }
        #endregion

        #region 辅助方法
        private string CurrIp;
        private int CurrPort;
        private void MakeClienter(string ip, int port, LinkServerType type/*, System.Action<NetState, object[]> finish*/)
        {
            if (CurrIp != ip || CurrPort != port || NetWorkerImpl.Instance.state != NetState.NetConnected)
            {
                CurrIp = ip;
                CurrPort = port;
                NetWorkerImpl.Instance.makeClienter(ip, port);

                _R.Instance.StartCoroutine(CheckNetWorkerState(type));
            }
            else
            {
                ConnectServerCallBack(type,NetState.NetConnected);
            }

        }
        private IEnumerator CheckNetWorkerState(/*System.Action<NetState, object[]> finish,*/LinkServerType type)
        {
            do
            {
                yield return new WaitForEndOfFrame();
            } while (NetWorkerImpl.Instance.state == NetState.NetConnecting);

            ConnectServerCallBack(type, NetWorkerImpl.Instance.state);
        }
     

#endregion

#region 自动操作
        /// <summary>
        /// 自动登录
        /// </summary>
        public void AutoLogin()
        {
            //this.isConnect = false;
            this.CurrIp = "";
            this.CurrPort = 0;

            this.Connect(LoginReq);
        }

        /// <summary>
        /// 登录请求
        /// </summary>
        private void LoginReq(bool isSuccess)
        {
            if(isSuccess)
            {
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysLoginRequest, SysLoginData.EnumLoginType.AutoLogin);
                //ModelNetWorker.Instance.LoginReq(null, null);
            }
        }
#endregion
#if __DEBUG_SERVER_LIST
#region 服务器操作
        private GatewayServerData GetServerList()
        {
            if (ServerList == null)
            {
                Init();
            }
            GatewayServerData result = null;
            if (ServerList != null && ServerList.Count > 0)
            {
                int index = Random.Range(0, ServerList.Count - 1);
                result = ServerList[index];
                QLoger.ERROR("连接 " + result.Ip + " __" + result.Port);
            }
            return result;
        }

        private void RemoveServerList(string ip, int port)
        {
            if (ServerList == null) return;

            for (int i=0; i<ServerList.Count; ++i)
            {
                if(ServerList[i].Ip == ip && ServerList[i].Port == port)
                {
                    ServerList.RemoveAt(i--);
                }
            }
        }
#endregion
#endif

#region 初始化
        //注册事件
        private void InitEvent()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.EBeforGame, GatewayCallback);
            EventDispatcher.AddEvent(GEnum.NamedEvent.ELoginResult, LoginGameCallback);
        }
        //初始化服务器列表
        private void InitServerList()
        {
            ServerList = new List<GatewayServerData>();

            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["ServerList"];

            int sType = 0;
#if __BUNDLE_OUTER_SERVER
            sType = 1;
#elif __BUNDLE_PRE_OUTER_SERVER
            sType = 2;
#elif __BUNDLE_IOS_SERVER
            sType = 1;
#endif
            foreach (BaseXmlBuild build in buildList)
            {
                ServerList info = (ServerList)build;
                if (sType == 0 || //开发模式 JEFF Modify : （int.Parse(info.sType) >= sType） 这里得判断会读取到预发布服务器
                    int.Parse(info.sType) == sType)
                {
                    ServerList.Add(new GatewayServerData(info.Ip, int.Parse(info.Port)));
                }                    
            }
        }

        public void Init()
        {
            InitServerList();
        }
        #endregion


        #region 重连提示框
        public enum EnumNetWorkTipAction
        {
            None = 0,
            Reconnection = 1,
            OpenLogin = 2,
        }
        private List<EnumNetWorkTipAction> DisconnectTipList = new List<EnumNetWorkTipAction>();
        /// <summary>
        /// 网络断开提示
        /// </summary>
        private void NetWorkDisconnectTip(EnumNetWorkTipAction actionTip)
        {
            //如果当前已经连接上了
            if (NetWorkerImpl.Instance.state == NetState.NetConnecting || NetWorkerImpl.Instance.state == NetState.NetConnected) return;
            DisconnectTipList.Add(actionTip);

            if (DisconnectTipList.Count == 1 || WindowUIManager.Instance.curOpenWindow == null) //如果他的提示框数量为1的情况下
            {
                // = actionTip == EnumNetWorkTipAction.OpenLogin ? "您与服务器已经断开链接，请重新登录" : "您的网络状况不佳，请重试";
                string contentStr = "您的网络状况不佳，请重试";
                string buttonStr = actionTip == EnumNetWorkTipAction.OpenLogin ? "确认" : "重试";
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_SmallLoading_Kill);
                WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", contentStr, new string[] { buttonStr }, delegate (int index)
                {
                    EnumNetWorkTipAction temp = EnumNetWorkTipAction.None;
                    if (DisconnectTipList.Count > 0)
                    {
                        for (int i = 0; i < DisconnectTipList.Count; ++i)
                        {
                            if(temp < DisconnectTipList[i])
                            {
                                temp = DisconnectTipList[i];
                            }
                        }
                    }
                   
                    DisconnectTipList.Clear();
                    switch (temp)
                    {
                        case EnumNetWorkTipAction.OpenLogin:
                            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenLogin,true);
                            break;
                        case EnumNetWorkTipAction.Reconnection:
                            RealReconnect();
                            break;
                    }
                }, WindowUIRank.Special);
            }
            QLoger.LOG("断线提示框=====================================================");
            for (int i = 0; i < DisconnectTipList.Count; ++i)
            {
                QLoger.LOG(DisconnectTipList[i].ToString());
            }
            QLoger.LOG("=====================================================断线提示框End");
        }
        #endregion
    }
}