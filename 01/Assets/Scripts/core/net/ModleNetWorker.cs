using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Msg;
using System.ComponentModel;

namespace projectQ
{

    public partial class ModelNetWorker : SingletonTamplate<ModelNetWorker>
    {

        public ModelNetWorker()
        {
            m_netReciveActions = new Dictionary<System.Type, System.Action<object>>();

        }

        /// <summary>
        /// 注册网络消息处理函数
        /// </summary>
        public void init()
        {
            NetWorkerImpl.Instance.NetBrokenNotice = Broken;
            NetWorkerImpl.Instance.NetTimeOutNotice = TimeOut;
            NetWorkerImpl.Instance.NetFailNotice = Fail;
            NetWorkerImpl.Instance.NetConnectedNotice = Connected;

            this.initDefaultHandle();

            NetWorkerImpl.Instance.dataHandle = handle;
        }

        private void initDefaultHandle()
        {
            var mos = this.GetType().GetMethods();
            for (int i = 0; i < mos.Length; i++)
            {
                if (mos[i].Name.StartsWith("initDefaultHandleOf")
                    && mos[i].GetParameters().Length == 0)
                {
                    mos[i].Invoke(this, new System.Type[] { });
                }
            }
        }
    }



    public partial class ModelNetWorker
    {

        void Connected()
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENet_Connected);
        }

        void Fail()
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError_Fail);
        }

        void Broken()
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError_Broken);
        }

        void TimeOut()
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError_TimeOut);
        }

        static public IEnumerator CheckNetWorkerState(System.Action<NetState, object[]> finish, params object[] var)
        {
            do
            {
                yield return new WaitForEndOfFrame();
            } while (NetWorkerImpl.Instance.state == NetState.NetConnecting);

            finish(NetWorkerImpl.Instance.state, var);
        }
    }

    /// <summary>
    /// 发送接收数据
    /// </summary>
    public partial class ModelNetWorker
    {
        Dictionary<System.Type, System.Action<object>> m_netReciveActions;

        public static void Regiest<T>(System.Action<object> action)
        {
            System.Type type = typeof(T);
            Instance.m_netReciveActions[type] = action;
        }

        public static void UnRegiest<T>(System.Action<object> action)
        {
            System.Type type = typeof(T);
            if (Instance.m_netReciveActions.ContainsKey(type))
            {
                Instance.m_netReciveActions.Remove(type);
            }
        }

        static public void Send(
            ProtoBuf.IExtensible msg
        )
        {
            Instance.send(msg);
        }


        /// <summary>
        /// 是否开始解析网络数据,这里不是处理接收数据，而是处理是否解析数据
        /// </summary>
        /// <param name="yes">开始解析网络数据变量</param>
        static public void OpenReciveAndSend(bool yes = true) {
            if(!yes)
            {
                NetWorkMessageManager.ClearMatchedRecord();
            }
            NetWorkerImpl.Instance.canSendAndAnalysisProto = yes;
        }

        /// <summary>
        /// 清除网络接收BUFF
        /// </summary>
        static public void ClearReciveBuffer() {
            NetWorkerImpl.Instance.clearReciveBuffer();
        }


        protected void handle(ProtoMessage msg)
        {
           
            var type = TcpDataHandler.GetProtobufType(msg.m_cmd_no);
            if (type == null)
            {
                QLoger.ERROR("[ModleNetWorker] 无法解析数据{0},无法找到相应的指令对象", msg.m_cmd_no.ToString());
                return;
            }

            var protodata = TcpDataHandler.DeSerializeProtoData(msg.m_data, type);

#if !__DISABLE_LOG
            if (protodata.GetType ().Equals (typeof(PingCmd)) ||
				protodata.GetType ().Equals (typeof(SystemNotice)) ||
				protodata.GetType ().Equals (typeof(AskClientNotify)) ||
			    protodata.GetType ().Equals (typeof(MyFMjRoomUpdateNotify))
                ) {

			} else {

                string msg_info = CommonTools.ReflactionObject(protodata);

                int sidx = 0;
				int step = 0 ;
				do { 
					int show = 1024 ;
					if(msg_info.Length - sidx < show) {
						show = msg_info.Length - sidx ;
					}
					QLoger.LOG(msg_info.Substring(sidx,show));
					sidx += show ;
					step++ ;
				} while(msg_info.Length > sidx);

                UserActionManager.AddLocalTypeLog("NetR", protodata.GetType().ToString() + ":" + CommonTools.ReflactionObject(protodata));
                //QLoger.LOG(CommonTools.ReflactionObject(protodata));
            }

#endif


            if (!NetWorkerImpl.UserLogicNetCanRecive) {

                switch (msg.m_cmd_no) {
                    case CmdNo.CmdNo_Gateway_Rsp:
                        break;
                    case CmdNo.CmdNo_Login_Reg_Rsp:
                        break;
                    case CmdNo.CmdNo_Login_Auth_Rsp:
                        break;
                    case CmdNo.CmdNo_Login_Enter_Rsp:
                        break;
                    case CmdNo.CmdNo_PingReq:
                        break;
                    case CmdNo.MjCmd_DeskReconect_Rsp:
                        NetWorkerImpl.UserLogicNetCanRecive = true;
                        break;
                    case CmdNo.CmdNo_Activity_Notify:
                        break;
                    case CmdNo.CmdNo_SystemNotice_Notify:
                        break;
                    case CmdNo.CmdNo_UpdateFriend_Notify:
                        break;
                    case CmdNo.CmdNo_Login_UserData_Notify:
                        break;
                    case CmdNo.MjCmd_FRoomPlayerInfo_Rsp:
                        break;
                    case CmdNo.MjCmd_DeskAction_Notify:
                        break;
                    case CmdNo.CmdNo_JoinFriendMjDesk_Notify:
                        break;
                    case CmdNo.CmdNo_InviteGame_Notify:
                        break;
                    default: {        
                            QLoger.ERROR("当前无法接受逻辑指令，只能接受登陆指令" + msg.m_cmd_no.ToString());
                            return ;// 这里不报错，提示发送成功
                        }
                }
            }


            System.Action<object> action = null;
            if (m_netReciveActions.TryGetValue(type, out action)) {
                action(protodata);
            } else {
                QLoger.ERROR("[ModleNetWorker] 没有找到指令{0}的解析方法<{1}>", type, protodata);
            }
            
        }


        protected void send(
            ProtoBuf.IExtensible msg
        )
        {
            NetWorkerImpl.Send(msg);
        }

    }


    /// <summary>
    /// 基础网络数据处理
    /// </summary>
    public partial class ModelNetWorker
    {

        public void initDefaultHandleOfGenerial()
        {
            ModelNetWorker.Regiest<X2XServerMsg>(X2XServerMsg);
			//ModelNetWorker.Regiest<StartServer>(StartServer);
			ModelNetWorker.Regiest<PingCmd>(PingCmd);
			ModelNetWorker.Regiest<AskClientNotify>(AskClientNotify);

            ModelNetWorker.Regiest<SyncUserStateNotify>(SyncUserStateNotify);
        }

        public void X2XServerMsg(object obj)
        {
            var req = obj as X2XServerMsg;
            QLoger.LOG(req.MsgBody);
            QLoger.LOG(req.MsgID);
            QLoger.LOG(req.UserID);
            return;
        }

        /// <summary>
        /// Ping
        /// </summary>
        public void PingCmd()
        {
            var proto = new PingCmd();
            proto.server_id = MemoryData.Get<int>(MKey.AS_SERVER_ID);
            this.send(proto);
        }

        /// <summary>
        /// Ping Push
        /// </summary>
        /// <param name="rsp"></param>
        private void PingCmd(object rsp)
        {
            //ping数清零
            PingManager.Instance.PingRsp();

            var proto = rsp as PingCmd;
            MemoryData.Set(MKey.AS_SERVER_ID, proto.server_id);
            //EventDispatcher.FireEvent(GEnum.NamedEvent.ERecivePing, proto);
            //MemoryData.OtherData.UpdateNetDelay();
        }

		/// <summary>
		/// 收到AskPing消息后 不做处理 现在先不主动Ping一次
		/// </summary>
		/// <param name="rsp">Rsp.</param>
		public void AskClientNotify(object rsp){
			//var proto = rsp as AskClientNotify;
			//this.PingCmd ();
		}

        public void SyncUserStateNotify(object push) {
            var prsp = push as SyncUserStateNotify;

            if (prsp != null) {
                int st = prsp.State;
                MemoryData.UserGameState.MainState = st;

            }
        }
    }

     
}
