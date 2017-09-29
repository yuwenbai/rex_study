/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:CEPH
 *	创建时间：01/18/2016
 *	文件名：  NetWorkerImpl.cs @ hc151228
 *	文件功能描述：
 *  创建标识：ceph.01/18/2016
 *	创建描述：
 *
 *  修改标识：
 *  修改描述：
 *
 *
 *
 *****************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;


using Msg;
namespace projectQ {

	/// <summary>
	/// 网络状态
	/// </summary>
	public enum NetState {
		NetNone = -1,
		NetConnecting = 1,

		NetConnected = 2,

		NetError = 100,
		NetClose = 101,
		NetFail = 102,
		NetBroken = 103,
		NetTimeOut = 104,
	}


	public class NetWorkerImpl : MonoBehaviour {

		public static void LOG(LogType t , string p , params object[] v) {
			QLoger.LOG (typeof(NetWorkerImpl), t, p, v);
		}

        public static bool UserLogicNetCanSend = false;
        public static bool UserLogicNetCanRecive = false;


        #region 静态引用
        private static NetWorkerImpl _instance;
		public static NetWorkerImpl Instance {
			get { return _instance; }
		}
		#endregion

		#region 网络状态通知
		public System.Action NetBrokenNotice { get; set; }
		public System.Action NetTimeOutNotice { get; set; }
		public System.Action NetFailNotice { get; set; }
		public System.Action NetConnectedNotice { get; set; }

		public delegate void MessageHandle(ProtoMessage msg);
		public MessageHandle dataHandle = null;
        public bool canSendAndAnalysisProto = true ;

        #endregion

        #region 网络状态参数
        private NetState netWorkState = NetState.NetNone;
		public NetState state { get { return netWorkState; } }

		#endregion

		#region Socket网络连接
		/// <summary>
		/// 网络数据管理实例
		/// </summary>
		private TcpDataHandler p_net_datahandle = new TcpDataHandler();

		/// <summary>
		/// 网络连接管理实例
		/// </summary>
		private TcpClienter p_net_clienter;

		/// <summary>
		/// 异步线程消息队列
		/// </summary>
		private Queue<Action> actionQueue = new Queue<Action>();

		#endregion

		#region 网络回传数据处理

		//每Update 进行检测网络回传数据
		private void handleMessage() {
			if (dataHandle != null && canSendAndAnalysisProto) {
				//每帧处理一个消息
				/*while*/if (!this.p_net_datahandle.messQueue.IsEmpty) {
					dataHandle(this.p_net_datahandle.messQueue.Dequeue());
				}
                ReplayMessage();
            }
		}
//rextest
        public void ReplayMessage()
        {
            //处理回放消息
            if (FakeReplayManager.Instance.CanReplay())
            {
                ProtoMessage pro = FakeReplayManager.Instance.GetMessage();
                if (pro != null)
                {
                    dataHandle(pro);
                }
            }
        }
		#endregion

		#region 网络链接管理

		public void makeClienter(string ip, int port) {
			MemoryData.GameStateData.IsInitiativeCloseConnect = true;
			//Close first
			this.closeConnection();


			MemoryData.GameStateData.IsInitiativeCloseConnect = false;

			//Make client
			this.startConnection(ip, port);
		}

		/// <summary>
		/// 开启网络连接
		/// </summary>
		/// <param name="ip">Ip.</param>
		/// <param name="port">Port.</param>
		private void startConnection(string ip, int port) {

			try {
				this.netWorkState = NetState.NetConnecting;

                this.clearReciveBuffer();

				this.p_net_clienter = new TcpClienter(ip, (ushort)port); //New a TcpClienter

				this.p_net_clienter.pd_reciveDataHandle = this.p_net_datahandle.Receive;  // data handle
				this.p_net_clienter.connectBrokenHandle = this.connectBroken; //net broken handle
				this.p_net_clienter.connectFailHandle = this.connectFail; //net fail handle
				this.p_net_clienter.connectSuccessHandle = this.connectSuccess; // net connect success notice
				this.p_net_clienter.connectingTimeOutHandle = this.connectingTimeout; // net timeout handle
                
				this.p_net_clienter.connect(); // action to connect

			} catch (System.Exception ex) {
				LOG(LogType.EError,"[NetWorkerImpl] {0}", ex);
				this.netWorkState = NetState.NetError;
			}

		}

        /// <summary>
        /// 关闭链接，且关闭会发送关闭原因
        /// </summary>
        public bool CloseConnection() {
            var rs = (this.state == NetState.NetConnected) || (this.state == NetState.NetConnecting);
            this.closeConnection(true);
            return rs;
        }


        /// <summary>
        /// 关闭链接，且关闭会屏蔽关闭原因
        /// </summary>
        public bool CloseConnectionWithoutMessage() {
            var rs = (this.state == NetState.NetConnected) || (this.state == NetState.NetConnecting);
            this.closeConnection();
            return rs;
        }


        private void clearEventDelegate() {
            this.p_net_clienter.pd_reciveDataHandle = null;  // data handle
            this.p_net_clienter.connectBrokenHandle = null; //net broken handle
            this.p_net_clienter.connectFailHandle = null; //net fail handle
            this.p_net_clienter.connectSuccessHandle = null; // net connect success notice
            this.p_net_clienter.connectingTimeOutHandle = null; // net timeout handle
        }
		/// <summary>
		/// 关闭网络连接
		/// </summary>
		private void closeConnection(bool needMessage = false) {
            UserActionManager.AddLocalTypeLog("Log1", "closeConnection 主动调用关闭网络连接");
            if (this.p_net_clienter != null) {
				try {
                    if (!needMessage) {
                        clearEventDelegate();
                    }
					this.p_net_clienter.disconnect();
                    
                } catch (System.Exception ex) {
					LOG(LogType.EError,"[NetWorkerImpl] : {0}", ex);
				} finally {
					this.p_net_clienter = null;
				}
			}
			this.netWorkState = NetState.NetClose;

		}


		private void connectSuccess() {
			LOG(LogType.ELog,"[NetWorkerImpl] connectSuccess 网络连接创建成功");
			this.netWorkState = NetState.NetConnected;
			if (this.NetConnectedNotice != null) {
				lock (actionQueue) {
					actionQueue.Enqueue(this.NetConnectedNotice);
				}
			}
		}

		private void connectFail() {
			LOG(LogType.ELog,"[NetWorkerImpl] connectFail 网络连接失败");
			this.netWorkState = NetState.NetFail;

            this.clearEventDelegate();

            if (this.NetFailNotice != null) {
				lock (actionQueue) {
					actionQueue.Enqueue(this.NetFailNotice);
				}
			}
		}

		private void connectBroken() {
			LOG(LogType.ELog,"[NetWorkerImpl] connectBroken 网络连接断开");
			this.netWorkState = NetState.NetBroken;

            this.clearEventDelegate();

            if (this.NetBrokenNotice != null) {
				lock (actionQueue) {
					actionQueue.Enqueue(this.NetBrokenNotice);
				}
			}
		}

		private void connectingTimeout() {
			LOG(LogType.ELog,"[NetWorkerImpl] connectingTimeout 网络连接超时");
			this.netWorkState = NetState.NetTimeOut;

            this.clearEventDelegate();

            if (this.NetTimeOutNotice != null) {
				lock (actionQueue) {
					actionQueue.Enqueue(this.NetTimeOutNotice);
				}
			}
		}

        public void clearReciveBuffer() {
            if (this.p_net_datahandle != null) {
                this.p_net_datahandle.messQueue.Clear();
            }
        }

		#endregion

		#region 发送数据

		/// <summary>
		/// 发送数据
		/// </summary>
		/// <param name="msg"></param>

		static public void Send(
			ProtoBuf.IExtensible msg
		) {
			try {
				Instance.send(msg);
			} catch (System.Exception ex) {
				//LOG(LogType.EError,"发送数据错误{0}", ex.ToString());
				LOG(LogType.EError,"发送数据错误{0}", ex!=null?ex.ToString():"exception is null");
			}
		}

		/// <summary>
		/// 发数据
		/// </summary>
		/// <param name="cmdNumber"></param>
		/// <param name="cmd"></param>
		protected void send(
			ProtoBuf.IExtensible msg
		) {
			CmdNo no = TcpDataHandler.GetProtobufCmdNo(msg.GetType());
			if (no != CmdNo.CmdNo_PingReq &&
				no != CmdNo.CmdNo_ClientReport_Req) {
				LOG(LogType.ELog,"[NetWorkerImpl] 发送消息 {0}",CommonTools.ReflactionObject(msg));
				if (no != CmdNo.CmdNo_None) {
					UserActionManager.AddLocalTypeLog("NetR", "发送" + no.ToString () +":"+ CommonTools.ReflactionObject (msg));
				}

			}

			if (this.p_net_clienter != null &&
				this.p_net_clienter.isConnected()) {
				byte[] data = TcpDataHandler.SerializeProtoData (msg);
				if (data == null) {
					data=new byte[0];
				}

				var sendmsg = new ProtoMessage(no, data);
				bool isSend = this.send(sendmsg);

                if (!isSend) {//发送失败抛出事件
                    LOG(LogType.ELog, "[NetWorkerImpl] 发送消息 {0} 失败,发送失败", no);
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError, msg);
                }

                
			} else {
				LOG(LogType.ELog,"[NetWorkerImpl] 发送消息 {0} 失败,没有网络连接", no);
				EventDispatcher.FireSysEvent(GEnum.NamedEvent.ENetError, msg);
			}
		}

		/// <summary>
		/// 发数据
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		protected bool send(ProtoMessage message) {

			#if !__DISABLE_LOG
			if (message.m_cmd_no != CmdNo.CmdNo_PingReq) {
				LOG(LogType.ELog,"[NetWorkerImpl] 发送消息 {0}", CommonTools.ReflactionObject(message));
			}
			#endif

            
            {//如果是发送登陆指令
                if (CmdNo.CmdNo_Gateway_Req == message.m_cmd_no) {
                    NetWorkerImpl.UserLogicNetCanSend = false;
                    NetWorkerImpl.UserLogicNetCanRecive = false;
                }



                //如果不能发送逻辑数据
                if (!NetWorkerImpl.UserLogicNetCanSend) {
                    switch (message.m_cmd_no) {
                        case CmdNo.CmdNo_Gateway_Req:
                            break;
                        case CmdNo.CmdNo_Login_Reg_Req:
                            break;
                        case CmdNo.CmdNo_Login_Auth_Req:
                            break;
                        default: {
                                QLoger.ERROR("当前无法发送逻辑指令，只能发送登陆指令->" + message.m_cmd_no.ToString());
                                return true;// 这里不报错，提示发送成功
                            }
                    }
                }

                if (!NetWorkerImpl.Instance.canSendAndAnalysisProto) {
                    QLoger.ERROR("当前为后台屏蔽收发，不能发送数据->" + message.m_cmd_no.ToString());
                    return true;// 这里不报错，提示发送成功
                }

            }

            NetWorkMessageManager.MatchedReq(message.m_cmd_no); //添加指令配对序列
			return this.p_net_clienter.send(TcpDataHandler.PackMessage(message));
		}

		#endregion

		#region WWW网络数据处理


		public IEnumerator wwwEnumertor(string uri, System.Action<WWW> finish) {
			yield return new WaitForEndOfFrame();
			WWW www = new WWW(uri);
			yield return www;
			finish(www);
		}
		#endregion

		#region lificycle
		// Use this for initialization

		void Awake() {
			_instance = this;
            //设置匹配消息返回
            this.p_net_datahandle._recive_action = NetWorkMessageManager.MatchedRsp;
		}

		void Start() {

			//mgr self looping
			//this.InvokeRepeating("UpdatePerSecond", 1, 1);
		}

		// Update is called once per frame
		void Update() {
			this.handleMessage();

			//这里是为了分发线程中的消息。当获得到消息后下一帧或者同一帧时处理逻辑
			while (actionQueue.Count > 0) {
				actionQueue.Dequeue()();
			}
		}

		void UpdatePerSecond() {
			//网络连接超时检测
			if (this.p_net_clienter != null &&
				this.p_net_clienter.p_is_connectting) {
				this.p_net_clienter.updateTcpConnectingTimeout();
			}
		}


		void OnDestroy() {
			this.CancelInvoke();
			this.closeConnection();
			_instance = null;
		}

		#endregion
	}

}