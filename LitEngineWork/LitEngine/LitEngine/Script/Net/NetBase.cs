using UnityEngine;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
namespace LitEngine
{
    namespace NetTool
    {

        #region 回调消息
        public enum MSG_RECALL
        {
            MSG_RECALL_SOCKET_CREATED = 1,//建立socket
            MSG_RECALL_SOCKET_CONNECT_FINISHED,//连接并建立发送接收逻辑完成
            MSG_RECALL_SOCKET_CONNECT_ERRO,//连接出现错误
            MSG_RECALL_SOCKET_RECERRO,//接收出现错误
            MSG_RECALL_SOCKET_SENDERRO,//发送出现错误
            
            MSG_RECALL_SOCKET_P2PFINSHED,//p2p连接成功
            MSG_RECALL_SOCKET_P2PERRO,//p2p连接失败
        }
        #endregion

        #region 回调对象
        public class MSG_RECALL_DATA
        {
            public MSG_RECALL mCmd;
            public string mMsg;

            public MSG_RECALL_DATA(MSG_RECALL _cmd, string _msg)
            {
                mCmd = _cmd;
                mMsg = _msg;
            }
        }

        #endregion

        #region Net基类
        public class NetBase : MonoBehaviour
        {
            #region socket属性
            public enum IPTYPE
            {
                IPVNONE = 0,
                IPV4ONLY,
                IPV6ONLY,
                IPVALL,
            }

            protected Thread mSendThread;
            protected Thread mRecThread;
           // protected ManualResetEvent mWaitObject = new ManualResetEvent(false);

            protected Socket mSocket = null;
            protected string mHostName;//服务器地址
            protected int mPort;
            protected int mRecTimeOut = 0;
            protected int mSendTimeout = 0;
            protected int mReceiveBufferSize = 1024;
            protected int mSendBufferSize = 1024;

            protected string mNetTag = "";
            #endregion

            #region 数据
            protected int mReadyRecDataCount = 100;
            protected int mUseIndex = 0;
            protected ReceiveData[] mReceiveDataList;//data可用列表

            protected const int mReadMaxLen = 2048;
            protected byte[] mRecbuffer = new byte[mReadMaxLen];

            protected BufferBase mBufferData = new BufferBase(2048);
            protected SafeQueue<SendData> mSendDataList = new SafeQueue<SendData>();//发送数据队列
            protected SafeQueue<ReceiveData> mResultDataList = new SafeQueue<ReceiveData>();//已接收的消息队列             
            #endregion
            #region 分发
            static public int OneFixedUpdateChoseCount = 5;
            protected SafeMap<int, System.Action<ReceiveData>> mMsgHandlerList = new SafeMap<int, System.Action<ReceiveData>>();//消息注册列表
            protected SafeQueue<MSG_RECALL_DATA> mToMainThreadMsgList = new SafeQueue<MSG_RECALL_DATA>();//给主线程发送通知
            #endregion
            #region 日志
            public bool IsShowDebugLog = false;
            #endregion

            #region 回调
            protected System.Action<MSG_RECALL_DATA> mReCallDelgate = null;
            protected int mMainThreadMsgReCallCount = 0;
            #endregion

            #region 线程控制
            //protected ManualResetEvent mWaitObject = new ManualResetEvent(false);
            protected bool mConnected = false;
            protected bool mStartThread = false; //线程开关
            protected bool mConnecting = false;
            protected bool mDisConnecting = false;
            #endregion

            public NetBase()
            {
                mReceiveDataList = new ReceiveData[mReadyRecDataCount];
                for (int i = 0; i < mReadyRecDataCount; i++)
                {
                    mReceiveDataList[i] = new ReceiveData(1024);
                    mReceiveDataList[i].ArrayIndex = i;
                }
            }

            virtual public void InitSocket(string _hostname, int _port, System.Action<MSG_RECALL_DATA> _ReCallDelegate = null)
            {

                mHostName = _hostname;
                mPort = _port;
                ReCallDelgate = _ReCallDelegate;
                CreatUnityObject();
            }

            protected IPTYPE GetSelfIPType()
            {
                IPTYPE ret = IPTYPE.IPV4ONLY;
                try
                {
                    bool ishavepiv4 = false;
                    bool ishavepiv6 = false;

                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                    foreach (NetworkInterface adapter in nics)
                    {
                        if (adapter.OperationalStatus != OperationalStatus.Up) continue;                       
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback || adapter.NetworkInterfaceType == NetworkInterfaceType.Tunnel) continue;
                        DLog.LOG(DLogType.Log, "连接状态的网络名称-" + adapter.Description);                     
                        IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                        UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                        foreach (UnicastIPAddressInformation uni in uniCast)
                        {
                            DLog.LOG(DLogType.Log, "获取到的IP:" + uni.Address.ToString());
                            if (ishavepiv4 && ishavepiv6)
                                break;
                            if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                                ishavepiv4 = true;
                            else if (uni.Address.AddressFamily == AddressFamily.InterNetworkV6)
                                ishavepiv6 = true;
                        }
                    }
                    if (ishavepiv4 && ishavepiv6)
                        ret = IPTYPE.IPVALL;
                    else if (ishavepiv4 && !ishavepiv6)
                        ret = IPTYPE.IPV4ONLY;
                    else if (!ishavepiv4 && ishavepiv6)
                        ret = IPTYPE.IPV6ONLY;
                    if (!ishavepiv4 && !ishavepiv6)
                        DLog.LOG(DLogType.Error, "没有发现匹配的协议,默认进行IPV4连接!");
                }
                catch (Exception err)
                {
                    DLog.LOG(DLogType.Error,"取得本地ip错误" + err.ToString());
                }

                return ret;
            }
            protected List<IPAddress> GetServerIpAddress(string _hostname)
            {
                List<IPAddress> ret = new List<IPAddress>();
                try {
                    IPAddress[] tips = Dns.GetHostAddresses(mHostName);
                    DLog.LOG(DLogType.Log, "HostName: " + mHostName + " Length:" + tips.Length);
                    for (int i = 0; i < tips.Length; i++)
                    {
                        DLog.LOG(DLogType.Log, "IpAddress: " + tips[i].ToString() + " AddressFamily:" + tips[i].AddressFamily.ToString());

                        if (tips[i].AddressFamily == AddressFamily.InterNetwork)
                            ret.Insert(0, tips[i]);
                        else
                            ret.Add(tips[i]);
                    }
                }
                catch (Exception e)
                {
                    DLog.LOG(DLogType.Error, "获取IPAddress失败:" + " HostName:" + mHostName + " IP:" + ret.ToString() + " 错误信息:" + e.ToString());
                }
                return ret;
            }

            #region 属性

            public System.Action<MSG_RECALL_DATA> ReCallDelgate
            {
                get
                {
                    return mReCallDelgate;
                }
                set
                {
                    mReCallDelgate = value;
                }
            }

            #endregion
            #region unity对象
            virtual protected void CreatUnityObject()
            {
                gameObject.name = mNetTag + "-Server:" + mHostName;
            }
            #endregion

            #region 建立Socket

           

            virtual public void ConnectToServer()
            {

            }
            virtual protected void SetTimerOutAndBuffSize(int _rec, int _send, int _recsize, int _sendsize)
            {
                mRecTimeOut = _rec;
                mSendTimeout = _send;
                mReceiveBufferSize = _recsize;
                mSendBufferSize = _sendsize;
                ChoseSocketTimeOutAndBuffer();
            }

            virtual protected void ChoseSocketTimeOutAndBuffer()
            {
                if (mSocket == null) return;
                mSocket.ReceiveTimeout = mRecTimeOut;
                mSocket.SendTimeout = mSendTimeout;
                mSocket.ReceiveBufferSize = mReceiveBufferSize;
                mSocket.SendBufferSize = mSendBufferSize;
            }

            #endregion

            #region 断开管理
            virtual protected bool IsCOrD()
            {
                if (mDisConnecting || mConnecting) return true;
                return false;
            }

            virtual protected void CloseSRThread()
            {
                mStartThread = false;
                mConnected = false;
            }
            virtual protected void ClearQueue()
            {
                mSendDataList.Clear();
                mBufferData.Clear();
                mResultDataList.Clear();
            }

            virtual protected void OnDestroy()
            {
                mMsgHandlerList.Clear();
                CloseSocketStart();
            }
            

            virtual protected void WaitThreadJoin(Thread _thread)
            {
                if (_thread == null) return;
                _thread.Join();
            }

            virtual protected void CloseSocket()
            {
                try
                {
                    //需要注意释放顺序
                    CloseSRThread();
                    if (mSocket != null)
                    {
                        if (mSocket.ProtocolType == ProtocolType.Tcp && mSocket.Connected)
                            mSocket.Shutdown(SocketShutdown.Both);
                        mSocket.Close();
                        mSocket = null;
                    }
                    ClearQueue();
                }
                catch (Exception err)
                {
                    DLog.LOG(DLogType.Error,mNetTag + "-socket的关闭时出现异常:" + err);
                }
                
            }
            virtual protected void CloseSocketStart()
            {
                //需要注意重复调用
                mDisConnecting = true;
                Thread tcreatthread = new Thread(CloseSocket);
                tcreatthread.IsBackground = true;
                tcreatthread.Start();
                try
                {
                    WaitThreadJoin(tcreatthread);
                    WaitThreadJoin(mSendThread);
                    WaitThreadJoin(mRecThread);
                    DLog.LOG(DLogType.Log, mNetTag + ":socket is closed!");
                }
                catch (Exception err)
                {
                    DLog.LOG(DLogType.Error,mNetTag + ":Disconnect - " + err);
                }
                mDisConnecting = false;
            }
            virtual public void DisConnect()
            {
                if (IsCOrD())
                {
                    DLog.LOG(DLogType.Error,mNetTag + "-DisConnect->连接中或关闭中，不可进行DisConnect操作！");
                    return;
                }
                CloseSocketStart();  
            }
            
            virtual public void ClearSocket()
            {
                if (IsCOrD())
                {
                    DLog.LOG(DLogType.Error,mNetTag + "-ClearSocket->连接中或关闭中，不可进行ClearSocket操作！");
                    return;
                }
                mMsgHandlerList.Clear();
                DisConnect();
            }
            #endregion


            #region 通知类

            virtual protected MSG_RECALL_DATA GetMsgReCallData(MSG_RECALL _cmd, string _msg = "")
            {
                return new MSG_RECALL_DATA(_cmd, _msg);
            }

            virtual protected void AddMainThreadMsgReCall(MSG_RECALL_DATA _recall)
            {
                if (mReCallDelgate == null) return;
                mToMainThreadMsgList.Enqueue(_recall);
                mMainThreadMsgReCallCount++;

            }

            #endregion

            #region 消息注册与分发

            virtual public void Reg(int msgid, System.Action<ReceiveData> func)
            {
                System.Action<ReceiveData> action = null;

                if (mMsgHandlerList.ContainsKey(msgid))
                {
                    action = mMsgHandlerList[msgid];
                    action += func;
                    mMsgHandlerList[msgid] = action;
                }
                else
                {
                    mMsgHandlerList.Add(msgid, func);
                }
            }
            virtual public void UnReg(int msgid, System.Action<ReceiveData> func)
            {
                System.Action<ReceiveData> action = null;

                if (mMsgHandlerList.ContainsKey(msgid))
                {
                    action = mMsgHandlerList[msgid];
                    action -= func;
                    mMsgHandlerList[msgid] = action;
                }
            }

            virtual public void Call(int _msgid, ReceiveData _msg)
            {
                System.Action<ReceiveData> action = null;

                if (mMsgHandlerList.ContainsKey(_msgid))
                {
                    action = mMsgHandlerList[_msgid];
                    if(action != null)
                        action(_msg);
                }

            }

            #endregion

            #region 主线程逻辑

            virtual protected void UpdateReCalledMsg()
            {
                if (mMainThreadMsgReCallCount == 0 || mReCallDelgate == null) return;
                mReCallDelgate(mToMainThreadMsgList.Dequeue());
                mMainThreadMsgReCallCount--;
            }

            virtual public void UpdateRecMsg()
            {
                short i = 0;
                int tcount = mResultDataList.Count;
                while (tcount > 0)
                {
                    ReceiveData trecdata = mResultDataList.Dequeue();
                    Call(trecdata.Cmd, trecdata);
                    i++;
                    tcount--;
                    if (i > OneFixedUpdateChoseCount)
                        return;
                }

            }
            #endregion

            #region 处理接收到的数据
            virtual protected void Processingdata(int _len, byte[] _buffer)
            {
                DebugMsg(0, _buffer, 0, _len, "接收");
            }
            #endregion

            #region 发送和接收
            virtual protected void DebugMsg(int _cmd, byte[] _buffer, int offset, int _len, string _title)
            {
                if (IsShowDebugLog)
                {
                    System.Text.StringBuilder bufferstr = new System.Text.StringBuilder();
                    bufferstr.Append("{");
                    for (int i = offset; i < _len; i++)
                    {
                        if (i != offset)
                            bufferstr.Append(",");
                        bufferstr.Append(_buffer[i]);
                    }
                    bufferstr.Append("}");
                    string tmsg = string.Format("{0}-cmd:{1} title:{2}  长度:{3}  内容:{4}", mNetTag, _cmd, _title, _len, bufferstr);
                    DLog.LOG(DLogType.Log,tmsg);
                }
            }
            virtual protected SocketAsyncEventArgs GetAsyncEventArgs(EventHandler<SocketAsyncEventArgs> _delgate, SocketFlags _flags)
            {
                SocketAsyncEventArgs ret = new SocketAsyncEventArgs();
                ret.Completed += _delgate;
                ret.AcceptSocket = mSocket;
                ret.SocketFlags = _flags;
                return ret;
            }

            #region 发送
            virtual public void AddSend(SendData _data)
            {

            }
            #endregion


            #endregion

        }
        #endregion
    }
}


